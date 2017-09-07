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
using CatLib.API.Routing;
using UnityEngine;
using ILogger = CatLib.API.Debugger.ILogger;

namespace CatLib.Debugger.WebMonitorContent.Controller
{
    /// <summary>
    /// 指令
    /// </summary>
    [Routed("debug://command", Group = "Debugger.MainThreadCallWithContext")]
    [ExcludeFromCodeCoverage]
    public sealed class Command
    {
        /// <summary>
        /// 陀螺仪命令
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="logger">日志</param>
        [Routed("input-gyro-enable/{enable}")]
        public void GyroEnable(IRequest request, IResponse response, ILogger logger)
        {
            var mainThread = (Action<Action>)request.GetContext();

            mainThread.Invoke(() =>
            {
                Input.gyro.enabled = request.GetBoolean("enable");
                logger.Debug("Input.gyro.enabled = " + Input.gyro.enabled);
            });

            response.SetContext(true);
        }

        /// <summary>
        /// 罗盘命令
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="logger">日志</param>
        [Routed("input-compass-enable/{enable}")]
        public void CompassEnable(IRequest request, IResponse response, ILogger logger)
        {
            var mainThread = (Action<Action>)request.GetContext();

            mainThread.Invoke(() =>
            {
                Input.compass.enabled = request.GetBoolean("enable");
                logger.Debug("Input.compass.enabled = " + Input.compass.enabled);
            });

            response.SetContext(true);
        }

        /// <summary>
        /// 定位器命令
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="response">响应</param>
        /// <param name="logger">日志</param>
        [Routed("input-location-enable/{enable}")]
        public void LocationEnable(IRequest request, IResponse response, ILogger logger)
        {
            var mainThread = (Action<Action>)request.GetContext();

            mainThread.Invoke(() =>
            {
                if (request.GetBoolean("enable"))
                {
                    logger.Debug("Input.location.Start()");
                    Input.location.Start();
                }
                else
                {
                    logger.Debug("Input.location.Stop()");
                    Input.location.Stop();
                }
            });

            response.SetContext(true);
        }
    }
}
