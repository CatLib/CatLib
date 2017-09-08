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
    /// 系统信息监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SystemInfoMonitor
    {
        /// <summary>
        /// 系统信息监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public SystemInfoMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.deviceUniqueIdentifier", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.deviceUniqueIdentifier));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.deviceName", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.deviceName));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.deviceType", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.deviceType));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.deviceModel", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.deviceModel));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.processorType", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.processorType));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.processorCount", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.processorCount));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.processorFrequency", "MHz",
                new[] { "tag@SystemInfo" },
                () => SystemInfo.processorFrequency));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.systemMemorySize", "MB",
                new[] { "tag@SystemInfo" },
                () => SystemInfo.systemMemorySize));
#if UNITY_5_5_OR_NEWER
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.operatingSystemFamily", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.operatingSystemFamily));
#endif
#if UNITY_5_6_OR_NEWER
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.batteryStatus", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.batteryStatus));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.batteryLevel", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.batteryLevel < 0f ? "code.unavailable" : SystemInfo.batteryLevel.ToString("P0")));
#endif
#if UNITY_5_4_OR_NEWER
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsAudio", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.supportsAudio));
#endif
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsLocationService", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.supportsLocationService));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsAccelerometer", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.supportsAccelerometer));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsGyroscope", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.supportsGyroscope));
            monitor.Monitor(new OnceRecordMonitorHandler("SystemInfo.supportsVibration", string.Empty,
                new[] { "tag@SystemInfo" },
                () => SystemInfo.supportsVibration));
        }
    }
}
