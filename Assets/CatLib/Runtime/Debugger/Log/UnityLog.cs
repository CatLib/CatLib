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
using UnityEngine;

namespace CatLib.Debugger.Log
{
    /// <summary>
    /// Unity Log
    /// </summary>
    internal sealed class UnityLog : IOnDestroy
    {
        /// <summary>
        /// 基础日志工具
        /// </summary>
        private readonly Logger baseLogger;

        /// <summary>
        /// Unity Log
        /// </summary>
        /// <param name="baseLogger">基础日志工具</param>
        public UnityLog([Inject(Required = true)]Logger baseLogger)
        {
            Guard.Requires<ArgumentNullException>(baseLogger != null);
            this.baseLogger = baseLogger;
            UnityEngine.Application.logMessageReceived += Log;
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            UnityEngine.Application.logMessageReceived -= Log;
        }

        /// <summary>
        /// 输出Unity日志
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="type">日志类型</param>
        private void Log(string message, string stackTrace, LogType type)
        {
            baseLogger.Log(new UnityLogEntry(message, stackTrace, type));
        }
    }
}
