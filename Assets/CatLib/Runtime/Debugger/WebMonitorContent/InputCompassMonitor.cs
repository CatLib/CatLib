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
    /// 罗盘相关监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InputCompassMonitor
    {
        /// <summary>
        /// 罗盘相关监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public InputCompassMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compass.enabled@cmd", string.Empty, new[] { "tag@Input.compass" },
                () =>
                {
                    if (!Input.gyro.enabled)
                    {
                        return "[command.help.clickStart](debug://command/input-compass-enable/true)";
                    }
                    return "[command.help.clickStop](debug://command/input-compass-enable/false)";
                }));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compass.enabled", string.Empty, new[] { "tag@Input.compass" },
                () => Input.compass.enabled));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compass.headingAccuracy", "unit.degree", new[] { "tag@Input.compass" },
                () => Input.compass.headingAccuracy));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compass.magneticHeading", "unit.degree", new[] { "tag@Input.compass" },
                () => Input.compass.magneticHeading));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compass.rawVector", "unit.microteslas", new[] { "tag@Input.compass" },
                () => Input.compass.rawVector));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compass.timestamp", string.Empty, new[] { "tag@Input.compass" },
                () => Input.compass.timestamp));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compass.trueHeading", "unit.degree", new[] { "tag@Input.compass" },
                () => Input.compass.trueHeading));
        }
    }
}
