/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 项目入口
    /// </summary>
    [DisallowMultipleComponent]
    public class Main : Framework, IBootstrap
    {
        /// <summary>
        /// 项目入口
        /// </summary>
        [Priority]
        public void Bootstrap()
        {
            App.On(ApplicationEvents.OnStartCompleted, () =>
            {
                // called this function after, use App.Make function to get service
                // ex: App.Make<ILogger>().Debug("hello world");
                // all can make service see : http://catlib.io/v1/guide/can-make.html

                Debug.Log("Hello CatLib");
            });
        }
    }
}
