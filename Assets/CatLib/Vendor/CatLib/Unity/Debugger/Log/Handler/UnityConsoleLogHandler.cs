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

using CatLib.API.Debugger;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Debugger.Log.Handler
{
    /// <summary>
    /// Unity控制台日志处理器
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class UnityConsoleLogHandler : ILogHandler
    {
        /// <summary>
        /// 实际处理方法
        /// </summary>
        private readonly Dictionary<LogLevels, Action<object>> mapping;

        /// <summary>
        /// Unity控制台日志处理器
        /// </summary>
        public UnityConsoleLogHandler()
        {
            mapping = new Dictionary<LogLevels, Action<object>>
            {
                {LogLevels.Emergency, Debug.LogError},
                {LogLevels.Alert, Debug.LogError},
                {LogLevels.Critical, Debug.LogError},
                {LogLevels.Error, Debug.LogError},
                {LogLevels.Warning, Debug.LogWarning},
                {LogLevels.Notice, Debug.Log},
                {LogLevels.Info, Debug.Log},
                {LogLevels.Debug, Debug.Log}
            };
        }

        /// <summary>
        /// 日志处理器
        /// </summary>
        /// <param name="log">日志条目</param>
        public void Handler(ILogEntry log)
        {
            Action<object> handler;
            if (mapping != null && mapping.TryGetValue(log.Level, out handler))
            {
                handler.Invoke(log.Message);
            }
        }
    }
}
