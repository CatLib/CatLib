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
    /// 屏幕监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ScreenMonitor
    {
        /// <summary>
        /// 屏幕监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public ScreenMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.dpi", "unit.dpi", new[] { "tag@Screen" },
                () => Screen.dpi));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.height", "unit.px", new[] { "tag@Screen" },
                () => Screen.height));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.width", "unit.px", new[] { "tag@Screen" },
                () => Screen.width));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.orientation", "", new[] { "tag@Screen" },
                () => Screen.orientation.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.autorotateToLandscapeLeft", "", new[] { "tag@Screen" },
                () => Screen.autorotateToLandscapeLeft.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.autorotateToLandscapeRight", "", new[] { "tag@Screen" },
                () => Screen.autorotateToLandscapeRight.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.autorotateToPortrait", "", new[] { "tag@Screen" },
                () => Screen.autorotateToPortrait.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.autorotateToPortraitUpsideDown", "", new[] { "tag@Screen" },
                () => Screen.autorotateToPortraitUpsideDown.ToString()));
            monitor.Monitor(new OnceRecordMonitorHandler("Screen.sleepTimeout", "", new[] { "tag@Screen" },
                () =>
                {
                    switch (Screen.sleepTimeout)
                    {
                        case SleepTimeout.NeverSleep:
                            return "code.SleepTimeout.NeverSleep";
                        case SleepTimeout.SystemSetting:
                            return "code.sleepTimeout.SystemSetting";
                    }
                    return Screen.sleepTimeout.ToString();
                }));
        }
    }
}
