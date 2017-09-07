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
    class RoutingPriortityMiddleware : IMiddleware
    {
        /// <summary>
        /// 中间件
        /// </summary>
        public IFilterChain<IRequest, IResponse> Middleware
        {
            get
            {
                var filter = new FilterChain<IRequest, IResponse>();
                filter.Add((req, res, next) =>
                {
                    next(req, res);
                    res.SetContext(res.GetContext() + "[with middleware 15]");
                }, 15);
                filter.Add((req, res, next) =>
                {
                    next(req, res);
                    res.SetContext(res.GetContext() + "[with middleware 20]");
                }, 20);
                filter.Add((req, res, next) =>
                {
                    next(req, res);
                    res.SetContext(res.GetContext() + "[with middleware 10]");
                }, 10);
                return filter;
            }
        }

        [Routed("call")]
        public void Call(IRequest request, IResponse response)
        {
            response.SetContext("RoutingPriortityMiddleware.Call");
        }
    }
}
