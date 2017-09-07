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

using System;

namespace CatLib
{
    /// <summary>
    /// 默认提供者引导
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TypeFinderBootstrap : IBootstrap
    {
        /// <summary>
        /// 引导程序接口
        /// </summary>
        [Priority(10)]
        public void Bootstrap()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                int sort;
                if (!Assemblys.Assembly.TryGetValue(assembly.GetName().Name, out sort))
                {
                    continue;
                }
                var assemblyData = assembly;
                App.OnFindType((finder) => assemblyData.GetType(finder), sort);
            }
        }
    }
}
