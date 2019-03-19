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

namespace Demo
{
    /// <summary>
    /// 项目自动添加的程序集自动加载方案
    /// </summary>
    public static class Assemblys
    {
        /// <summary>
        /// 项目自动添加的程序集自动加载方案
        /// <para>分数值越小越优先</para>
        /// </summary>
        public static IDictionary<string, int> Assembly
        {
            get
            {
                return new Dictionary<string, int>
                {
                    //{ "Assembly-CSharp" , 0 },
                };
            }
        }
    }
}
