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

using CatLib.API.Config;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Debugger;
using CatLib.Events;
using CatLib.Json;
using CatLib.Routing;
using System;

namespace CatLib.Tests.Debugger
{
    public static class DebuggerHelper
    {
        public static Application GetApplication(bool enableWebConsole = true)
        {
            var app = new Application();
            app.OnFindType((str) => { return Type.GetType(str);});
            app.Bootstrap();
            app.Register(new RoutingProvider());
            app.Register(new JsonProvider());
            app.Register(new DebuggerProvider());
            app.Register(new ConfigProvider());
            app.Register(new EventsProvider());
            app.Register(new ConvertersProvider());
            var config = app.Make<IConfigManager>().Default;
            config.Set("DebuggerProvider.WebConsoleEnable", enableWebConsole);
            config.Set("UnityDebuggerProvider.UnityConsoleLoggerHandler", false);
            config.Set("UnityDebuggerProvider.MonitorPerformance", false);
            config.Set("UnityDebuggerProvider.MonitorScreen", false);
            config.Set("UnityDebuggerProvider.MonitorScene", false);
            config.Set("UnityDebuggerProvider.MonitorSystemInfo", false);
            config.Set("UnityDebuggerProvider.MonitorPath", false);
            config.Set("UnityDebuggerProvider.MonitorInput", false);
            config.Set("UnityDebuggerProvider.MonitorInputLocation", false);
            config.Set("UnityDebuggerProvider.MonitorInputGyroscope", false);
            config.Set("UnityDebuggerProvider.MonitorInputCompass", false);
            config.Set("debugger.monitor.graphics", false);
            app.Init();
            return app;
        }
    }
}
