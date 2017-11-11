/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 doLittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace doLittle.Applications
{
    /// <summary>
    /// Represents an implementation of <see cref="IApplicationBuilder"/>
    /// </summary>
    public class ApplicationBuilder : IApplicationBuilder
    {
        readonly ApplicationName _name;
        readonly IEnumerable<IApplicationLocationFragment> _prefixes;
        readonly IApplicationStructureBuilder _applicationStructureBuilder;

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationBuilder"/>
        /// </summary>
        public ApplicationBuilder(ApplicationName name) 
            : this(
                name, 
                new IApplicationLocationFragment[0], 
                new NullApplicationStructureBuilder()
            )
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationBuilder"/>
        /// </summary>
        /// <param name="name">Name of the <see cref="ApplicationName"/> <see cref="IApplication"/>></param>
        /// <param name="prefixes"><see cref="IApplicationLocationFragment">Application location fragments</see> to be as prefixes</param>
        /// <param name="applicationStructureBuilder">The <see cref="IApplicationStructureBuilder"/> for building the structure</param>
        public ApplicationBuilder(
            ApplicationName name,
            IEnumerable<IApplicationLocationFragment> prefixes,
            IApplicationStructureBuilder applicationStructureBuilder)
        {
            _name = name;
            _prefixes = prefixes;
            _applicationStructureBuilder = applicationStructureBuilder;
        }


        /// <inheritdoc/>
        public IApplicationBuilder PrefixedWith(params IApplicationLocationFragment[] prefixes)
        {
            return new ApplicationBuilder(_name, prefixes, _applicationStructureBuilder);
        }

        /// <inheritdoc/>
        public IApplicationBuilder WithStructureStartingWith<TFragment>(TFragment fragment, Action<IApplicationStructureBuilder> structureBuilderCallback)
            where TFragment: IApplicationStructureFragment
        {
            var applicationStructureBuilder = ApplicationStructureBuilder.WithRoot(fragment);
            structureBuilderCallback(applicationStructureBuilder);
            return new ApplicationBuilder(_name, _prefixes, applicationStructureBuilder);
        }

        /// <inheritdoc/>
        public IApplication Build()
        {
            var applicationStructure = _applicationStructureBuilder.Build();
            var application = new Application(_name, applicationStructure);
            return application;
        }
    }
}