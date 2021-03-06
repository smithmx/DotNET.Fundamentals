/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Machine.Specifications;

namespace Dolittle.DependencyInversion.Autofac.Tenancy.for_InstancesPerTenant.given
{
    public class no_instances : all_dependencies
    {
        protected static InstancesPerTenant instances_per_tenant;

        Establish context = () =>
        {
            instances_per_tenant = new InstancesPerTenant(tenant_key_creator.Object, type_activator.Object);
        };
    }
}