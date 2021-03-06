﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using Dolittle.Collections;
using Dolittle.Types;

namespace Dolittle.Mapping
{
    /// <summary>
    /// Represents an implementation of <see cref="IMaps"/>
    /// </summary>
    public class Maps : IMaps
    {
        IInstancesOf<IMap> _maps;
        Dictionary<string, IMap> _mapsByKey;


        /// <summary>
        /// Initializes a new instance of <see cref="Maps"/>
        /// </summary>
        /// <param name="maps"><see cref="IInstancesOf{IMap}">Instances of maps</see></param>
        public Maps(IInstancesOf<IMap> maps)
        {
            _maps = maps;
            _mapsByKey = new Dictionary<string, IMap>();
            PopulateMapsBasedOnKeys();
        }

        /// <inheritdoc/>
        public bool HasFor(Type source, Type target)
        {
            var key = GetKeyFor(source, target);
            return _mapsByKey.ContainsKey(key);
        }

        /// <inheritdoc/>
        public IMap GetFor(Type source, Type target)
        {
            ThrowIfMissingMap(source, target);
            var key = GetKeyFor(source,target);
            return _mapsByKey[key];
        }


        string GetKeyFor(Type source, Type target)
        {
            return string.Format("{0}_{1}", source.FullName, target.FullName);
        }

        void PopulateMapsBasedOnKeys()
        {
            _maps.ForEach(map => _mapsByKey[GetKeyFor(map.Source, map.Target)] = map);
        }

        void ThrowIfMissingMap(Type source, Type target)
        {
            var key = GetKeyFor(source, target);
            if (!_mapsByKey.ContainsKey(key)) throw new MissingMapException(source, target);
        }
    }
}
