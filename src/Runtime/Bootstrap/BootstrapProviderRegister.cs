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

using System.Collections.Generic;
using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// Represents a service provider bootstrap.
    /// </summary>
    public sealed class BootstrapProviderRegister : IBootstrap
    {
        private readonly IServiceProvider[] providers;
        private readonly Component component;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapProviderRegister"/> class.
        /// </summary>
        /// <param name="component">Unity root GameObject object.</param>
        /// <param name="serviceProviders">An array of service providers.</param>
        public BootstrapProviderRegister(Component component, IServiceProvider[] serviceProviders = null)
        {
            providers = serviceProviders;
            this.component = component;
        }

        /// <inheritdoc />
        public void Bootstrap()
        {
            LoadUnityComponentProvider();
            RegisterProviders(providers);
        }

        /// <summary>
        /// Service provider that loads Unity components.
        /// </summary>
        private void LoadUnityComponentProvider()
        {
            if (!component)
            {
                return;
            }

            RegisterProviders(component.GetComponentsInChildren<IServiceProvider>());
        }

        /// <summary>
        /// Register service provider to the framework.
        /// </summary>
        private static void RegisterProviders(IEnumerable<IServiceProvider> providers)
        {
            foreach (var provider in providers)
            {
                if (provider == null)
                {
                    continue;
                }

                if (!App.IsRegistered(provider))
                {
                    App.Register(provider);
                }
            }
        }
    }
}
