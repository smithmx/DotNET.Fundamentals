/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;

namespace Dolittle.Assemblies
{
    /// <summary>
    /// Represents a <see cref="ICompilationAssemblyResolver"/> that tries to resolve from the package runtime store
    /// </summary>
    /// <remarks>
    /// Read more here : https://docs.microsoft.com/en-us/dotnet/core/deploying/runtime-store
    /// Linux / macOS : /usr/local/share/dotnet/store/{CPU}/{targetFramework e.g. netcoreapp2.0}/{package path}
    /// Windows       : C:/Program Files/dotnet/store/{CPU}/{targetFramework e.g. netcoreapp2.0}/{package path} 
    /// </remarks>
    public class PackageRuntimeStoreAssemblyResolver : ICompilationAssemblyResolver
    {
        /// <inheritdoc/>
        public bool TryResolveAssemblyPaths(CompilationLibrary library, List<string> assemblies)
        {
            var basePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)?
                            @"c:\Program Files\dotnet\store":
                            "/usr/local/share/dotnet/store";

            var cpuBasePath = Path.Combine(basePath,RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant());
            if (!Directory.Exists(cpuBasePath)) return false;
            
            var found = false;

            foreach( var targetFrameworkBasePath in Directory.GetDirectories(cpuBasePath))
            {
                var libraryBasePath = Path.Combine(targetFrameworkBasePath,library.Path);
                foreach( var assembly in library.Assemblies )
                {
                    var assemblyPath = Path.Combine(libraryBasePath, assembly);
                    if( File.Exists(assemblyPath))
                    {
                        assemblies.Add(assemblyPath);
                        found = true;
                    }
                }
            }

            return found;
        }
    }    

}