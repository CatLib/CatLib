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
using CatLib.API.Config;
using CatLib.Config;
using CatLib.Config.Locator;
using CatLib.Converters;
using CatLib.Converters.Plan;
using CatLib.Events;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Config
{
    [TestClass]
    public class ConfigProviderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            var app = new Application();
            app.Bootstrap();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            app.Register(new EventsProvider());
            app.Init();
        }

        [TestMethod]
        public void SetDefault()
        {
            var configManager = App.Make<IConfigManager>();

            configManager.SetDefault("catlib");
            configManager.Extend(() =>
            {
                return new global::CatLib.Config.Config(new global::CatLib.Converters.Converters(), new CodeConfigLocator());
            });

            Assert.AreEqual(typeof(global::CatLib.Config.Config), configManager.Get().GetType());
            Assert.AreEqual(typeof(global::CatLib.Config.Config), configManager.Get("default").GetType());
            Assert.AreNotSame(configManager.Get(), configManager["default"]);

            configManager.SetDefault(string.Empty);
            Assert.AreSame(configManager.Get(), configManager["default"]);
        }

        [TestMethod]
        public void WatchTest()
        {
            var configManager = App.Make<IConfigManager>();
            configManager.SetDefault("catlib");
            configManager.Extend(() =>
            {
                var convert = new global::CatLib.Converters.Converters();
                convert.AddConverter(new StringStringConverter());
                return new global::CatLib.Config.Config(convert, new CodeConfigLocator());
            });

            var watchValue = string.Empty;
            configManager.Default.Watch("watch", (value) =>
           {
               watchValue = value.ToString();
           });

            configManager.Default.Set("watch", "123");
            configManager.Default.Set("nowatch", "333");

            Assert.AreEqual("123", watchValue);
        }
    }
}
