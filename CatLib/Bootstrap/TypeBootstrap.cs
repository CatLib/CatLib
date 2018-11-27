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
        private readonly IDictionary<string, int> assembly;

        /// <summary>
        /// 构建一个类型查询器引导
        /// </summary>
        /// <param name="assembly">需要添加的程序集</param>
        public TypeBootstrap(IDictionary<string, int> assembly = null)
        {
            this.assembly = new Dictionary<string, int>();
            Dict.AddRange(this.assembly, Assemblys.Assembly);
            Dict.AddRange(this.assembly, assembly);
        }

        /// <summary>
        /// 引导程序接口
        /// </summary>
        [Priority(10)]
        public void Bootstrap()
        {
            if (assembly.Count <= 0)
            {
                return;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!this.assembly.TryGetValue(assembly.GetName().Name, out int sort))
                {
                    continue;
                }

                var localAssembly = assembly;
                App.OnFindType((finder) => localAssembly.GetType(finder), sort);
            }
        }
    }
}
