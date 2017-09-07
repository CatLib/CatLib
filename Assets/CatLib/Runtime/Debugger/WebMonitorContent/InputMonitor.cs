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
    /// 输入触摸相关监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InputMonitor
    {
        /// <summary>
        /// 输入触摸相关监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public InputMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("Input.touchSupported", string.Empty, new[] { "tag@Input" },
                () => Input.touchSupported));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.touchPressureSupported", string.Empty, new[] { "tag@Input" },
                () => Input.touchPressureSupported));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.stylusTouchSupported", string.Empty, new[] { "tag@Input" },
                () => Input.stylusTouchSupported));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.simulateMouseWithTouches", string.Empty, new[] { "tag@Input" },
                () => Input.simulateMouseWithTouches));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.multiTouchEnabled", string.Empty, new[] { "tag@Input" },
                () => Input.multiTouchEnabled));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.touchCount", string.Empty, new[] { "tag@Input" },
                () => Input.touchCount));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.touches", string.Empty, new[] { "tag@Input" },
                () =>
                {
                    var touches = Input.touches;
                    var touchStrings = new string[touches.Length];
                    for (var i = 0; i < touches.Length; i++)
                    {
                        touchStrings[i] = string.Format("pos {0}, delta pos {1}, raw pos {2}, pressure {3}, {4}", 
                                                            touches[i].position, 
                                                            touches[i].deltaPosition, 
                                                            touches[i].rawPosition, 
                                                            touches[i].pressure, 
                                                            touches[i].phase);
                    }
                    return string.Join("; ", touchStrings);
                }));

            monitor.Monitor(new OnceRecordMonitorHandler("Input.backButtonLeavesApp", string.Empty, new[] { "tag@Input" },
                () => Input.backButtonLeavesApp));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.deviceOrientation", string.Empty, new[] { "tag@Input" },
                () => Input.deviceOrientation));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.mousePresent", string.Empty, new[] { "tag@Input" },
                () => Input.mousePresent));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.mousePosition", string.Empty, new[] { "tag@Input" },
                () => Input.mousePosition));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.mouseScrollDelta", string.Empty, new[] { "tag@Input" },
                () => Input.mouseScrollDelta));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.anyKey", string.Empty, new[] { "tag@Input" },
                () => Input.anyKey));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.imeIsSelected", string.Empty, new[] { "tag@Input" },
                () => Input.imeIsSelected));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.imeCompositionMode", string.Empty, new[] { "tag@Input" },
                () => Input.imeCompositionMode));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compensateSensors", string.Empty, new[] { "tag@Input" },
                () => Input.compensateSensors));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compositionCursorPos", string.Empty, new[] { "tag@Input" },
                () => Input.compositionCursorPos));
            monitor.Monitor(new OnceRecordMonitorHandler("Input.compositionString", string.Empty, new[] { "tag@Input" },
                () => Input.compositionString));
        }
    }
}
