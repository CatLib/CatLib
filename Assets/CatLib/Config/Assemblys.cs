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
    /// 框架会自动添加的程序集自动加载方案
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Assemblys
    {
        /// <summary>
        /// 框架会自动添加的程序集自动加载方案
        /// </summary>
        public static IDictionary<string, int> Assembly
        {
            get
            {
                var result = new Dictionary<string, int>
                {
                };
                Dict.AddRange(result, Game.Assemblys.Assembly);
                return result;
            }
        }
    }
}
