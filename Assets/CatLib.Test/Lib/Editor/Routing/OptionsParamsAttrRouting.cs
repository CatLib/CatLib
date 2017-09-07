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
    /// <summary>
    /// 可选的参数接受
    /// </summary>
    [Routed]
    public class OptionsParamsAttrRouting
    {
        [Routed]
        public void Call(IRequest request)
        {

        }

        [Routed]
        public void CallNull()
        {

        }

        [Routed]
        public void CallResponseAndApp(IApplication app, IResponse response)
        {
            response.SetContext(app.ToString());
        }

        [Routed]
        public void CallResponse(IResponse response)
        {
            response.SetContext("OptionsParamsAttrRouting.CallResponse");
        }
    }
}
