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
    //1.越接近方法的路由定义的优先级越高
    //2.代码中的参数定义要比组的优先级高,如果违反第1条那么使用第1条规则

    //定义了类的路由约束，如果在方法上没有覆盖这些参数那么将会使用类的路由约束
    [Routed(Defaults = "str=>world,val=>hello", Where = "age=>[0-9]+,val=>[0-9]+")]
    public class ParamsAttrRouting
    {
        //您也可以在路由方法中强制指定scheme，这样她会忽略来自class中定义的scheme。
        //与此同时我们还为age定义了正则约束，如果您填写了age这个参数那么他必须受到正则约束才能匹配
        //age是必须参数所以没有必要定义default值。
        [Routed("catlib://params-attr-routing/params-call/{age}/{val?}/{str?}",
            Defaults = "str=>catlib")]
        public void ParamsCall(IRequest request, IResponse response)
        {
            response.SetContext("ParamsAttrRouting.ParamsCall." + request["age"] + "." + request["val"] + "." + request["str"]);
        }

        [Routed("catlib://params-attr-routing/params-call-with-group/{age}/{val?}/{str?}", Group = "default-group")]
        public void ParamsCallWithGroup(IRequest request, IResponse response)
        {
            response.SetContext("ParamsAttrRouting.ParamsCall." + request["age"] + "." + request["val"] + "." + request["str"]);
        }
    }
}
