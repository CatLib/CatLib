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

namespace Demo
{
    public static class Providers
    {
        /// <summary>
        /// An array represents the list of service providers,
        /// and initialization will proceed in this order.
        /// </summary>
        public static IServiceProvider[] ServiceProviders
        {
            get
            {
                return new IServiceProvider[]
                {
                    // todo: Add a service provider for your project here.
                };
            }
        }
    }
}