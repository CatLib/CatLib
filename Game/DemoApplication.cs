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
using CatLib;
using UnityEngine;

namespace Demo
{
    public class DemoApplication : UnityApplication
    {
        public DemoApplication(MonoBehaviour behaviour)
            : base(behaviour)
        {

        }

        public IBindData GetBindDataWithInstance(object instance)
        {
            return GetBindFillable(GetServiceWithInstanceObject(instance));
        }
    }

    public static class ExtendDemoApplication
    {
        public static void Extend<TConcrete>(this IContainer container, Func<IBindData, TConcrete, object> closure)
        {
            container.Extend(null, (instance, _) =>
            {
                if (!(instance is TConcrete))
                {
                    return instance;
                }

                if (App.Handler is DemoApplication)
                {
                    var application = (DemoApplication)App.Handler;
                    return closure(application.GetBindDataWithInstance(instance), (TConcrete)instance);
                }

                return closure(null, (TConcrete)instance);
            });
        }
    }

}