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
using CatLib.API.Time;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Events;
using CatLib.Time;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Time
{
    [TestClass]
    public class TimeProviderTests
    {
        /// <summary>
        /// 这是一个调试的时间
        /// </summary>
        public class TestTime : ITime
        {
            /// <summary>
            /// 从游戏开始到现在所用的时间(秒)
            /// </summary>
            public float Time { get; private set; }

            /// <summary>
            /// 上一帧到当前帧的时间(秒)
            /// </summary>
            public float DeltaTime { get; private set; }

            /// <summary>
            /// 从游戏开始到现在的时间（秒）使用固定时间来更新
            /// </summary>
            public float FixedTime { get; private set; }

            /// <summary>
            /// 从当前scene开始到目前为止的时间(秒)
            /// </summary>
            public float TimeSinceLevelLoad { get; private set; }

            /// <summary>
            /// 固定的上一帧到当前帧的时间(秒)
            /// </summary>
            public float FixedDeltaTime { get; set; }

            /// <summary>
            /// 能获取最大的上一帧到当前帧的时间(秒)
            /// </summary>
            public float MaximumDeltaTime { get; private set; }

            /// <summary>
            /// 平稳的上一帧到当前帧的时间(秒)，根据前N帧的加权平均值
            /// </summary>
            public float SmoothDeltaTime { get; private set; }

            /// <summary>
            /// 时间缩放系数
            /// </summary>
            public float TimeScale { get; set; }

            /// <summary>
            /// 总帧数
            /// </summary>
            public int FrameCount { get; private set; }

            /// <summary>
            /// 自游戏开始后的总时间（暂停也会增加）
            /// </summary>
            public float RealtimeSinceStartup { get; private set; }

            /// <summary>
            /// 每秒的帧率
            /// </summary>
            public int CaptureFramerate { get; set; }

            /// <summary>
            /// 不考虑时间缩放上一帧到当前帧的时间(秒)
            /// </summary>
            public float UnscaledDeltaTime { get; private set; }

            /// <summary>
            /// 不考虑时间缩放的从游戏开始到现在的时间
            /// </summary>
            public float UnscaledTime { get; private set; }

            public TestTime()
            {
                Time = 0;
                DeltaTime = 0;
                FixedTime = 0;
                TimeSinceLevelLoad = 0;
                FixedDeltaTime = 0;
                MaximumDeltaTime = 0;
                SmoothDeltaTime = 0;
                TimeScale = 0;
                FrameCount = 0;
                RealtimeSinceStartup = 0;
                CaptureFramerate = 0;
                UnscaledDeltaTime = 0;
                UnscaledTime = 0;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new TimeProvider());
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            app.Register(new EventsProvider());
            app.Init();

            var timeManager = app.Make<ITimeManager>();
            timeManager.Extend(() => new TestTime(), "test");

            var config = app.Make<IConfigManager>();
            config.Default.Set("TimeProvider.DefaultTime", "test");
        }

        [TestMethod]
        public void TestTimeGetDefault()
        {
            var timeManager = App.Make<ITimeManager>();
            Assert.AreEqual(typeof(TestTime), timeManager.Default.GetType());
        }

        [TestMethod]
        public void TimeGetTest()
        {
            var timeManager = App.Make<ITimeManager>();

            Assert.AreEqual(0, timeManager.CaptureFramerate);
            Assert.AreEqual(0, timeManager.DeltaTime);
            Assert.AreEqual(0, timeManager.FixedDeltaTime);
            Assert.AreEqual(0, timeManager.FixedTime);
            Assert.AreEqual(0, timeManager.FrameCount);
            Assert.AreEqual(0, timeManager.MaximumDeltaTime);
            Assert.AreEqual(0, timeManager.RealtimeSinceStartup);
            Assert.AreEqual(0, timeManager.SmoothDeltaTime);
            Assert.AreEqual(0, timeManager.Time);
            Assert.AreEqual(0, timeManager.TimeScale);
            Assert.AreEqual(0, timeManager.TimeSinceLevelLoad);
            Assert.AreEqual(0, timeManager.UnscaledDeltaTime);
            Assert.AreEqual(0, timeManager.UnscaledTime);
        }

        [TestMethod]
        public void TimeSetTest()
        {
            var timeManager = App.Make<ITimeManager>();

            timeManager.FixedDeltaTime = 1;
            timeManager.TimeScale = 1;
            timeManager.CaptureFramerate = 1;

            Assert.AreEqual(1, timeManager.FixedDeltaTime);
            Assert.AreEqual(1, timeManager.TimeScale);
            Assert.AreEqual(1, timeManager.CaptureFramerate);

            timeManager.FixedDeltaTime = 0;
            timeManager.TimeScale = 0;
            timeManager.CaptureFramerate = 0;
        }
    }
}
