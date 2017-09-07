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

using CatLib.Debugger.WebMonitor.Handler;
using UnityEngine;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 陀螺仪相关监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InputGyroscopeMonitor
    {
        /// <summary>
        /// 陀螺仪相关监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public InputGyroscopeMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("Input.gyro.enabled@cmd", string.Empty, new[] { "tag@Input.gyro" },
                () =>
                {
                    if (!Input.gyro.enabled)
                    {
                        return "[command.help.clickStart](debug://command/input-gyro-enable/true)";
                    }
                    return "[command.help.clickStop](debug://command/input-gyro-enable/false)";
                }));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.gyro.enabled", string.Empty, new[] { "tag@Input.gyro" },
                () => Input.gyro.enabled));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.gyro.updateInterval", string.Empty, new[] { "tag@Input.gyro" },
                () => Input.gyro.updateInterval));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.gyro.attitude.eulerAngles", string.Empty, new[] { "tag@Input.gyro" },
                () => Input.gyro.attitude.eulerAngles));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.gyro.gravity", string.Empty, new[] { "tag@Input.gyro" },
                () => Input.gyro.gravity));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.gyro.rotationRate", string.Empty, new[] { "tag@Input.gyro" },
                () => Input.gyro.rotationRate));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.gyro.rotationRateUnbiased", string.Empty, new[] { "tag@Input.gyro" },
                () => Input.gyro.rotationRateUnbiased));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.gyro.userAcceleration", string.Empty, new[] { "tag@Input.gyro" },
                () => Input.gyro.userAcceleration));
        }
    }
}
