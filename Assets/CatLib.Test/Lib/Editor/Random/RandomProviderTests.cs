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
using CatLib.API.Random;
using CatLib.Events;
using CatLib.Random;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Random
{
    [TestClass]
    public class RandomProviderTests
    {
        private IContainer MakeEnv()
        {
            var container = new Application();
            container.Bootstrap();
            container.Register(new EventsProvider());
            container.Register(new RandomProvider());
            container.Init();
            return container;
        }

        private void TestRandomNext(RandomTypes type)
        {
            var env = MakeEnv();
            var factory = env.Make<IRandomFactory>();

            var random = factory.Make(type);

            Assert.AreNotEqual(random.Next(), random.Next());
            Assert.AreNotEqual(random.Next(100000), random.Next(100000));

            for (var i = 0; i < 100; i++)
            {
                Assert.AreEqual(true, random.Next(100) <= 100);
            }

            Assert.AreNotEqual(random.Next(100, 200), random.Next(100, 200));
            for (var i = 0; i < 100; i++)
            {
                var v = random.Next(100, 200);
                Assert.AreEqual(true, v >= 100 && v <= 200);
            }

            var b = new byte[10];

            random.NextBytes(b);
            var sum = 0;
            for (var i = 0; i < 10; i++)
            {
                sum += b[i];
            }

            Assert.AreEqual(true , sum > 0);

            Assert.AreNotEqual(random.NextDouble(), random.NextDouble());
            for (var i = 0; i < 100; i++)
            {
                var v = random.NextDouble();
                Assert.AreEqual(true, v >= 0 && v <= 1);
            }
        }

        [TestMethod]
        public void TestFactoryNext()
        {
            var env = MakeEnv();
            var factory = env.Make<IRandomFactory>();

            Assert.AreNotEqual(factory.Next(), factory.Next());
            Assert.AreNotEqual(factory.Next(10000000), factory.Next(10000000));
            Assert.AreNotEqual(factory.Next(0, 10000000), factory.Next(0, 10000000));

            var b = new byte[10];
            factory.NextBytes(b);
            var sum = 0;
            for (var i = 0; i < 10; i++)
            {
                sum += b[i];
            }

            Assert.AreEqual(true, sum > 0);
            Assert.AreNotEqual(factory.NextDouble(), factory.NextDouble());
        }

        [TestMethod]
        public void TestMersenneTwister()
        {
            TestRandomNext(RandomTypes.MersenneTwister);
        }

        [TestMethod]
        public void TestMrg32k3a()
        {
            TestRandomNext(RandomTypes.Mrg32k3a);
        }

        [TestMethod]
        public void TestWH2006()
        {
            TestRandomNext(RandomTypes.WH2006);
        }

        [TestMethod]
        public void TestXorshift()
        {
            TestRandomNext(RandomTypes.Xorshift);
        }

        [TestMethod]
        public void TestMakeDefault()
        {
            var env = MakeEnv();
            var factory = env.Make<IRandomFactory>();

            Assert.AreNotEqual(null, factory.Make());
        }

        [TestMethod]
        public void TestMakeUndefiend()
        {
            var env = MakeEnv();
            var factory = env.Make<IRandomFactory>();

            ExceptionAssert.Throws<NotImplementedException>(() =>
            {
                factory.Make("helloworld");
            });
        }

        [TestMethod]
        public void TestMakeCanNotBuild()
        {
            var env = MakeEnv();
            var factory = env.Make<RandomFactory>();
            factory.RegisterRandom("helloworld2", (seed) => null);
            ExceptionAssert.Throws<NotImplementedException>(() =>
            {
                factory.Make("helloworld2");
            });
        }
    }
}
