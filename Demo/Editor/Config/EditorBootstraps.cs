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

using CatLib;
using UnityEngine;

namespace Demo.Editor
{
    public static class EditorBootstraps
    {
        /// <summary>
        /// Returns an array representing the framework bootstrap.
        /// </summary>
        /// <param name="component">Unity root GameObject object.</param>
        public static IBootstrap[] GetBoostraps(Component component)
        {
            return new IBootstrap[]
            {
                new BootstrapProviderRegister(component, Providers.ServiceProviders),
            };
        }
    }
}
