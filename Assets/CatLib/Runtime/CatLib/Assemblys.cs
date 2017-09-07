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

using System.Collections.Generic;

namespace CatLib
{
    /// <summary>
    /// 程序集会自动添加加载方案
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Assemblys
    {
        /// <summary>
        /// 程序集会自动添加加载方案
        /// </summary>
        public static IDictionary<string, int> Assembly
        {
            get
            {
                return new Dictionary<string, int>
                {
                    { "Assembly-CSharp" , 0 },
                    { "CatLib.Unity" , 10 },
                };
            }
        }
    }
}
