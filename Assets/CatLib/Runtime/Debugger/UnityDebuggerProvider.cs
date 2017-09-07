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

using CatLib.API.MonoDriver;
using CatLib.API.Routing;
using CatLib.Debugger.Log.Handler;
using CatLib.Debugger.WebMonitorContent;
using System;
using System.Collections.Generic;
using CatLib.Debugger.Log;
using UnityEngine;

namespace CatLib.Debugger
{
    /// <summary>
    /// 调试服务
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("CatLib/Debugger")]
    public sealed class UnityDebuggerProvider : MonoBehaviour, IServiceProvider , IServiceProviderType
    {
        /// <summary>
        /// 是否启用Web控制台
        /// </summary>
        public bool WebConsoleEnable = true;

        /// <summary>
        /// 监听Host
        /// </summary>
        public string WebConsoleHost = "*";

        /// <summary>
        /// 监听端口
        /// </summary>
        public int WebConsolePort = 9478;

        /// <summary>
        /// Unity控制台日志句柄
        /// </summary>
        public bool UnityConsoleLoggerHandler = true;

        /// <summary>
        /// 标准输出控制台日志句柄
        /// </summary>
        public bool StdConsoleLoggerHandler = true;

        /// <summary>
        /// 关联Unity Log
        /// </summary>
        public bool AssociationUnityLog = true;

        /// <summary>
        /// 启动性能监控
        /// </summary>
        public bool MonitorPerformance = true;

        /// <summary>
        /// 启动屏幕监控
        /// </summary>
        public bool MonitorScreen = true;

        /// <summary>
        /// 启动场景监控
        /// </summary>
        public bool MonitorScene = true;

        /// <summary>
        /// 启动系统信息监控
        /// </summary>
        public bool MonitorSystemInfo = true;

        /// <summary>
        /// 启动路径监控
        /// </summary>
        public bool MonitorPath = true;

        /// <summary>
        /// 启动输入监控
        /// </summary>
        public bool MonitorInput = true;

        /// <summary>
        /// 启动定位监控
        /// </summary>
        public bool MonitorInputLocation = true;

        /// <summary>
        /// 启动陀螺仪监控
        /// </summary>
        public bool MonitorInputGyroscope = true;

        /// <summary>
        /// 启动罗盘监控
        /// </summary>
        public bool MonitorInputCompass = true;

        /// <summary>
        /// 启动显卡监控
        /// </summary>
        public bool MonitorGraphics = true;

        /// <summary>
        /// 基础服务提供者
        /// </summary>
        private DebuggerProvider baseProvider;

        /// <summary>
        /// 提供者基础类型
        /// </summary>
        public Type BaseType
        {
            get
            {
                return baseProvider.GetType();
            }
        }

        /// <summary>
        /// Unity服务提供者
        /// </summary>
        public void Awake()
        {
            baseProvider = new DebuggerProvider
            {
                WebConsoleEnable = WebConsoleEnable,
                WebConsoleHost = WebConsoleHost,
                WebConsolePort = WebConsolePort,
                StdConsoleLoggerHandler = StdConsoleLoggerHandler
            };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        [Priority(20)]
        public void Init()
        {
            baseProvider.Init();
            InitMainThreadGroup();

            if (AssociationUnityLog)
            {
                App.Make<UnityLog>();
            }
        }

        /// <summary>
        /// 初始化主线程组
        /// </summary>
        private void InitMainThreadGroup()
        {
            var router = App.Make<IRouter>();
            var driver = App.Make<IMonoDriver>();

            if (driver == null)
            {
                return;
            }

            Action<Action> mainThreadCall = (call) =>
            {
                driver.MainThread(call);
            };

            router.Group("Debugger.MainThreadCallWithContext").Middleware((request, response, next) =>
            {
                request.ReplaceContext(mainThreadCall);
                next(request, response);
            });
        }

        /// <summary>
        /// 注册调试服务
        /// </summary>
        public void Register()
        {
            RegisterLogger();
            RegisterWebUI();
            RegisterWebMonitorContent();
            RegisterMonitors();
            baseProvider.Register();
        }

        /// <summary>
        /// 获取监控
        /// </summary>
        /// <returns></returns>
        private void RegisterMonitors()
        {
            baseProvider.AutoMake = new Dictionary<string, KeyValuePair<Type, bool>>
            {
                { "UnityDebuggerProvider.MonitorPerformance" , new KeyValuePair<Type, bool>(typeof(PerformanceMonitor) , MonitorPerformance) },
                { "UnityDebuggerProvider.MonitorScreen" , new KeyValuePair<Type, bool>(typeof(ScreenMonitor) , MonitorScreen)},
                { "UnityDebuggerProvider.MonitorScene" , new KeyValuePair<Type, bool>(typeof(SceneMonitor) , MonitorScene)},
                { "UnityDebuggerProvider.MonitorSystemInfo" , new KeyValuePair<Type, bool>(typeof(SystemInfoMonitor) , MonitorSystemInfo)},
                { "UnityDebuggerProvider.MonitorPath" , new KeyValuePair<Type, bool>(typeof(PathMonitor) , MonitorPath)},
                { "UnityDebuggerProvider.MonitorInput" , new KeyValuePair<Type, bool>(typeof(InputMonitor) , MonitorInput)},
                { "UnityDebuggerProvider.MonitorInputLocation" , new KeyValuePair<Type, bool>(typeof(InputLocationMonitor) ,MonitorInputLocation)},
                { "UnityDebuggerProvider.MonitorInputGyroscope" , new KeyValuePair<Type, bool>(typeof(InputGyroscopeMonitor) , MonitorInputGyroscope)},
                { "UnityDebuggerProvider.MonitorInputCompass" , new KeyValuePair<Type, bool>(typeof(InputCompassMonitor),MonitorInputCompass)},
                { "UnityDebuggerProvider.MonitorGraphics" , new KeyValuePair<Type, bool>(typeof(GraphicsMonitor) , MonitorGraphics)},
            };
        }

        /// <summary>
        /// 注册日志系统
        /// </summary>
        private void RegisterLogger()
        {
            baseProvider.LogHandlers = new Dictionary<string, KeyValuePair<Type, bool>>
            {
                { "UnityDebuggerProvider.UnityConsoleLoggerHandler" , new KeyValuePair<Type, bool>(typeof(UnityConsoleLogHandler) , UnityConsoleLoggerHandler)},
            };

            App.Singleton<UnityLog>();
        }

        /// <summary>
        /// 注册WebUI
        /// </summary>
        private void RegisterWebUI()
        {
            // 名字为相关监控定义的名字
            baseProvider.IndexMonitor = new List<string>
            {
                "Profiler.GetMonoUsedSize@memory",
                "Profiler.GetTotalAllocatedMemory",
                "fps"
            };
        }

        /// <summary>
        /// 注册Web监控
        /// </summary>
        private void RegisterWebMonitorContent()
        {
            App.Singleton<PerformanceMonitor>();
            App.Singleton<ScreenMonitor>();
            App.Singleton<SceneMonitor>();
            App.Singleton<SystemInfoMonitor>();
            App.Singleton<PathMonitor>();
            App.Singleton<InputMonitor>();
            App.Singleton<InputLocationMonitor>();
            App.Singleton<InputGyroscopeMonitor>();
            App.Singleton<InputCompassMonitor>();
            App.Singleton<GraphicsMonitor>();
        }
    }
}