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
    [Routed]
    public class AttrCompilerRouting
    {
        [Routed("routed://first-compiler-then-group/{str?}", Group = "DefaultGroup")]
        public void FirstCompilerThenAddGroup(IRequest request, IResponse response)
        {
            response.SetContext(request["str"]);
        }

        [Routed("routed://use-group-and-local-defaults/{str?}", Group = "DefaultGroup2", Defaults = "str=>hello world")]
        public void UseGroupAndLocalDefaults(IRequest request, IResponse response)
        {
            response.SetContext(request["str"]);
        }
    }
}
