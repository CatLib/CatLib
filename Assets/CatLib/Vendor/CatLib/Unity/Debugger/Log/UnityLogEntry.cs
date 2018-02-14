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
using System.Collections.Generic;
using CatLib.API.Debugger;
using CatLib.Debugger.Log.Handler;
using UnityEngine;

namespace CatLib.Debugger.Log
{
    /// <summary>
    /// Unity日志实体
    /// </summary>
    internal sealed class UnityLogEntry : ILogEntry
    {
        /// <summary>
        /// 实际处理方法
        /// </summary>
        private static readonly Dictionary<LogType, LogLevels> mapping = new Dictionary<LogType, LogLevels>
        {
            { LogType.Assert , LogLevels.Emergency },
            { LogType.Error , LogLevels.Error },
            { LogType.Exception , LogLevels.Alert },
            { LogType.Log , LogLevels.Debug },
            { LogType.Warning , LogLevels.Warning },
        };

        /// <summary>
        /// 条目id
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevels Level { get; private set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public long Time { get; private set; }

        /// <summary>
        /// 调用堆栈
        /// </summary>
        private readonly string[] callStack = {};

        /// <summary>
        /// Unity日志实体
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="stackTrace">调用堆栈</param>
        /// <param name="logType">日志类型</param>
        public UnityLogEntry(string message, string stackTrace, LogType logType)
        {
            Message = message ?? string.Empty;
            Id = LogUtil.GetLastId();
            Time = DateTime.Now.Timestamp();

            if (stackTrace != null)
            {
                callStack = stackTrace.Split('\n');
            }

            if (callStack.Length >= 2)
            {
                SetNameSpace(callStack[1]);
            }

            LogLevels level;
            if (!mapping.TryGetValue(logType, out level))
            {
                level = LogLevels.Emergency;
                Message = "[LogType Invalid]" + Message;
            }
            Level = level;
        }

        /// <summary>
        /// 获取调用堆栈
        /// </summary>
        /// <param name="match">是否符合输出条件</param>
        /// <returns>调用堆栈</returns>
        public string[] GetStackTrace(Predicate<string> match = null)
        {
            return callStack;
        }

        /// <summary>
        /// 是否可以被忽略
        /// </summary>
        /// <param name="type">处理器类型</param>
        /// <returns>是否可以忽略这个处理器</returns>
        public bool IsIgnore(Type type)
        {
            return type == typeof(UnityConsoleLogHandler);
        }

        /// <summary>
        /// 设定命名空间
        /// </summary>
        /// <param name="code">代码</param>
        private void SetNameSpace(string code)
        {
            var namespaceClass = code.Split(':')[0];
            var namespaceArr = namespaceClass.Split('.');

            if (namespaceArr.Length >= 2)
            {
                Namespace = string.Join(".", namespaceArr, 0, namespaceArr.Length - 1);
            }
        }
    }
}
