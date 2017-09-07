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
    /// 定位相关监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InputLocationMonitor
    {
        /// <summary>
        /// 定位相关监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public InputLocationMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.enabled@cmd", string.Empty, new[] { "tag@Input.location" },
                () =>
                {
                    if (Input.location.status == LocationServiceStatus.Stopped)
                    {
                        return "[command.help.clickStart](debug://command/input-location-enable/true)";
                    }
                    return "[command.help.clickStop](debug://command/input-location-enable/false)";
                }));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.isEnabledByUser", string.Empty, new[] { "tag@Input.location" },
                () => Input.location.isEnabledByUser));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.status", string.Empty, new[] { "tag@Input.location" },
                () => Input.location.status));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.lastData.horizontalAccuracy", string.Empty, new[] { "tag@Input.location" },
                () => Input.location.lastData.horizontalAccuracy));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.lastData.verticalAccuracy", string.Empty, new[] { "tag@Input.location" },
                () => Input.location.lastData.verticalAccuracy));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.lastData.longitude", string.Empty, new[] { "tag@Input.location" },
                () => Input.location.lastData.longitude));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.lastData.latitude", string.Empty, new[] { "tag@Input.location" },
                () => Input.location.lastData.latitude));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.lastData.altitude", string.Empty, new[] { "tag@Input.location" },
                () => Input.location.lastData.altitude));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.location.lastData.timestamp", string.Empty, new[] { "tag@Input.location" },
                () => Input.location.lastData.timestamp));
        }
    }
}
