/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac.Features.ResolveAnything;
using Dolittle.Assemblies;
using Dolittle.Collections;
using Dolittle.DependencyInversion.Autofac.Tenancy;
using Dolittle.Lifecycle;
using Dolittle.Reflection;

namespace Dolittle.DependencyInversion.Autofac
{
    /// <summary>
    /// Extensions for <see cref="ContainerBuilder"/>
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        
        /// <summary>
        /// Add Dolittle specifics to the <see cref="ContainerBuilder"/>
        /// </summary>
        /// <param name="containerBuilder"><see cref="ContainerBuilder"/> to extend</param>
        /// <param name="assemblies">Discovered <see cref="IAssemblies"/></param>
        /// <param name="bindings"><see cref="IBindingCollection">Bindings</see> to hook up</param>
        public static void AddDolittle(this ContainerBuilder containerBuilder, IAssemblies assemblies, IBindingCollection bindings)
        {
            var allAssemblies = assemblies.GetAll().ToArray();
            containerBuilder.RegisterAssemblyModules(allAssemblies);

            var selfBindingRegistrationSource = new SelfBindingRegistrationSource(type => 
                !type.Namespace.StartsWith("Microsoft") &&
                !type.Namespace.StartsWith("System"));

            selfBindingRegistrationSource.RegistrationConfiguration = HandleLifeCycleFor;

            containerBuilder.AddBindingsPerTenantRegistrationSource();

            containerBuilder.RegisterSource(selfBindingRegistrationSource);
            containerBuilder.RegisterSource(new FactoryForRegistrationSource());
            containerBuilder.RegisterSource(new OpenGenericTypeCallbackRegistrationSource());
            DiscoverAndRegisterRegistrationSources(containerBuilder, allAssemblies);

            RegisterUpBindingsIntoContainerBuilder(bindings, containerBuilder);
        }

        static void HandleLifeCycleFor(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder)
        {
            var service = builder.RegistrationData.Services.First();
            if (service is TypedService)
            {
                var typedService = service as TypedService;
                if (typedService.ServiceType.HasAttribute<SingletonAttribute>())builder.SingleInstance();
            }
        }

        static void RegisterUpBindingsIntoContainerBuilder(IBindingCollection bindings, ContainerBuilder containerBuilder)
        {
            bindings.ForEach(binding =>
            {
                if (binding.Scope is Scopes.SingletonPerTenant) 
                {
                    BindingsPerTenantsRegistrationSource.AddBinding(binding);
                    return;
                }
                if (binding.Service.ContainsGenericParameters)
                {
                    if (binding.Strategy is Strategies.Type)
                    {
                        var registrationBuilder = containerBuilder.RegisterGeneric(((Strategies.Type)binding.Strategy).Target).As(binding.Service);
                        if (binding.Scope is Scopes.Singleton)registrationBuilder = registrationBuilder.SingleInstance();
                    }
                    else if (binding.Strategy is Strategies.TypeCallback)
                    {
                        OpenGenericTypeCallbackRegistrationSource.AddService(new KeyValuePair<Type, Func<Type>>(binding.Service, ((Strategies.TypeCallback)binding.Strategy).Target));
                    }
                }
                else
                {
                    switch(binding.Strategy)
                    {
                        case Strategies.Type type: {
                            var registrationBuilder = containerBuilder.RegisterType(type.Target).As(binding.Service);
                            if (binding.Scope is Scopes.Singleton) registrationBuilder = registrationBuilder.SingleInstance();
                        } break;

                        case Strategies.Constant constant:
                            containerBuilder.RegisterInstance(constant.Target).As(binding.Service);
                            break;

                        case Strategies.Callback callback: {
                            var registrationBuilder = containerBuilder.Register((context)=>callback.Target()).As(binding.Service);
                            if (binding.Scope is Scopes.Singleton) registrationBuilder = registrationBuilder.SingleInstance();
                        } break;

                        case Strategies.TypeCallback typeCallback: {
                            var registrationBuilder = containerBuilder.Register((context) => context.Resolve(typeCallback.Target())).As(binding.Service);
                            if (binding.Scope is Scopes.Singleton) registrationBuilder = registrationBuilder.SingleInstance();
                        } break;
                    }
                }
            });
        }

        static void DiscoverAndRegisterRegistrationSources(ContainerBuilder containerBuilder, IEnumerable<Assembly> allAssemblies)
        {
            allAssemblies.ForEach(assembly =>
            {
                var registrationSourceProviderTypes = assembly.GetTypes().Where(type => type.HasInterface<ICanProvideRegistrationSources>());
                registrationSourceProviderTypes.ForEach(registrationSourceProviderType =>
                {
                    ThrowIfRegistrationSourceProviderTypeIsMissingDefaultConstructor(registrationSourceProviderType);
                    var registrationSourceProvider = Activator.CreateInstance(registrationSourceProviderType)as ICanProvideRegistrationSources;
                    var registrationSources = registrationSourceProvider.Provide();
                    registrationSources.ForEach(containerBuilder.RegisterSource);
                });
            });
        }

        static void ThrowIfRegistrationSourceProviderTypeIsMissingDefaultConstructor(Type type)
        {
            if (!type.HasDefaultConstructor())throw new RegistrationSourceProviderMustHaveADefaultConstructor(type);
        }
    }
}