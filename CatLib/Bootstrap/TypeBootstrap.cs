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
using System.Collections.Generic;

namespace CatLib
{
    /// <summary>
    /// 类型查询器引导
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class TypeBootstrap : IBootstrap
    {
        /// <summary>
        /// 程序集列表
        /// </summary>
        private readonly IDictionary<string, int> assemblies;

        /// <summary>
        /// 构建一个类型查询器引导
        /// </summary>
        /// <param name="assembly">需要添加的程序集</param>
        public TypeBootstrap(IDictionary<string, int> assembly = null)
        {
            assemblies = new Dictionary<string, int>();
            Dict.AddRange(assemblies, Assemblys.Assembly);
            Dict.AddRange(assemblies, assembly);
        }
         
        /// <summary>
        /// 引导程序接口
        /// </summary>
        [Priority(10)]
        public void Bootstrap()
        {
            if (assemblies.Count <= 0)
            {
                return;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                int sort;
                if (!assemblies.TryGetValue(assembly.GetName().Name, out sort))
                {
                    continue;
                }

                var localAssembly = assembly;
                App.OnFindType((finder) => localAssembly.GetType(finder), sort);
            }
        }
    }
}
