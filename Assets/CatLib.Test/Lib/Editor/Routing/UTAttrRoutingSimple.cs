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

using CatLib.API.Routing;

namespace CatLib.Tests.Routing
{
    // 路由的最简使用方式
    // 如果类或者方法没有给定名字那么会自动使用类名或者方法名作为路由路径
    // 如下面的路由会被默认给定名字：utattr-routing-simple/call 
    [Routed]
    public class UTAttrRoutingSimple
    {
        [Routed]
        public void Call(IRequest request, IResponse response)
        {
            response.SetContext("UTAttrRoutingSimple.Call");
        }

        //连续的大写会被视作一个整体最终的路由路径就是：catlib://utattr-routing-simple/call-mtest
        [Routed]
        public void CallMTest(IRequest request, IResponse response)
        {
            response.SetContext("UTAttrRoutingSimple.CallMTest");
        }
    }
}
