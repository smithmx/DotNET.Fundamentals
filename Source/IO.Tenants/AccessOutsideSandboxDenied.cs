/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Dolittle.IO.Tenants
{
    /// <summary>
    /// Exception that gets thrown when a file/path is trying to access outside the tenant sandbox
    /// </summary>
    public class AccessOutsideSandboxDenied : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AccessOutsideSandboxDenied"/>
        /// </summary>
        /// <param name="path">The path that was attempted</param>
        public AccessOutsideSandboxDenied(string path) : base($"Access outside tenant sandbox denied when trying to access '{path}'") {}
    }
}