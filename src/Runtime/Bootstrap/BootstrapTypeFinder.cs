/*
 * This file is part of the CatLib package.
 *
 * (c) CatLib <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.Collections.Generic;

namespace CatLib
{
    /// <summary>
    /// Represents a type querier registrar.
    /// </summary>
    public sealed class BootstrapTypeFinder : IBootstrap
    {
        private readonly IDictionary<string, int> assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapTypeFinder"/> class.
        /// </summary>
        public BootstrapTypeFinder(IDictionary<string, int> assemblies = null)
        {
            this.assemblies = new Dictionary<string, int>(assemblies);
        }

        /// <inheritdoc />
        public void Bootstrap()
        {
            if (assemblies.Count <= 0)
            {
                return;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assemblies.TryGetValue(assembly.GetName().Name, out int sort))
                {
                    continue;
                }

                var localAssembly = assembly;
                App.OnFindType((finder) => localAssembly.GetType(finder), sort);
            }
        }
    }
}
