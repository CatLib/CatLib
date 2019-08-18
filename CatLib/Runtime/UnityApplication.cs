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

using CatLib.Container;
using CatLib.Exception;
using System;
using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// CatLib for unity application
    /// </summary>
    public class UnityApplication : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityApplication"/> class.
        /// </summary>
        /// <param name="behaviour">驱动器</param>
        public UnityApplication(MonoBehaviour behaviour)
        {
            if (behaviour == null)
            {
                return;
            }

            this.Singleton<MonoBehaviour>(() => behaviour)
                .Alias<Component>();
        }

        /// <inheritdoc />
        public override void Register(IServiceProvider provider, bool force = false)
        {
            var component = provider as Component;
            if (component != null
                && !component)
            {
                throw new LogicException(
                    "Service providers inherited from MonoBehaviour only be registered mounting on the GameObject.");
            }

            base.Register(provider, force);
        }

        /// <inheritdoc />
        protected override bool IsUnableType(Type type)
        {
            return typeof(Component).IsAssignableFrom(type) || base.IsUnableType(type);
        }
    }
}