﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 doLittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using doLittle.Concepts;

namespace doLittle.Applications
{
    /// <summary>
    /// Represents the name of <see cref="IApplication"/>
    /// </summary>
    public class ApplicationName : ConceptAs<string>
    {
        /// <summary>
        /// Gets the <see cref="ApplicationName"/> representing an undefined name
        /// </summary>
        public static readonly ApplicationName NotSet = "[Not Set]";

        /// <summary>
        /// Implicitly converts from a <see cref="string"/> to a <see cref="ApplicationName"/>
        /// </summary>
        /// <param name="applicationName">Name of the <see cref="ApplicationName"/></param>
        public static implicit operator ApplicationName(string applicationName)
        {
            return new ApplicationName { Value = applicationName };
        }
    }
}