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
    // CatLib 路由系统允许添加多个路由名
    [Routed] // catlib://utmult-attr-routing-simple
    [Routed("uthello-world")] // catlib://hello-world
    [Routed("cat://utmult-attr-routing-simple")] // cat://mult-attr-routing-simple
    public class UTMultAttrRoutingSimple
    {
        // catlib://mult-attr-routing-simple/call
        // catlib://hello-world/call
        // cat://mult-attr-routing-simple/call
        [Routed]
        // catlib://mult-attr-routing-simple/my-hello
        // catlib://hello-world/my-hello
        // cat://mult-attr-routing-simple/my-hello
        [Routed("my-hello")]
        // dog://myname/call
        [Routed("dog://utmyname/call")]
        public void Call(IRequest request, IResponse response)
        {
            response.SetContext("UTMultAttrRoutingSimple.Call");
        }
    }
}
