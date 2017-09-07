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

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 路径相关监控
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class PathMonitor
    {
        /// <summary>
        /// 构建路径相关监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public PathMonitor([Inject(Required = true)]IMonitor monitor)
        {
            monitor.Monitor(new OnceRecordMonitorHandler("Application.dataPath", string.Empty, new[] { "tag@Path" },
                () => UnityEngine.Application.dataPath));
            monitor.Monitor(new OnceRecordMonitorHandler("Application.persistentDataPath", string.Empty, new[] { "tag@Path" },
                () => UnityEngine.Application.persistentDataPath));
            monitor.Monitor(new OnceRecordMonitorHandler("Application.streamingAssetsPath", string.Empty, new[] { "tag@Path" },
                () => UnityEngine.Application.streamingAssetsPath));
            monitor.Monitor(new OnceRecordMonitorHandler("Application.temporaryCachePath", string.Empty, new[] { "tag@Path" },
                () => UnityEngine.Application.temporaryCachePath));
        }
    }
}
