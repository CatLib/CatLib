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

using CatLib.API.Compress;
using CatLib.Compress;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Events;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Compress
{
    [TestClass]
    public class CompressProviderTests
    {
        /// <summary>
        /// 准备环境
        /// </summary>
        /// <returns></returns>
        public IApplication MakeEnv()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new ConfigProvider());
            app.Register(new EventsProvider());
            app.Register(new ConvertersProvider());
            app.Register(new CompressProvider());
            app.Init();
            return app;
        }

        [TestMethod]
        public void TestGZipCompress()
        {
            TestCompress("gzip");
        }

        [TestMethod]
        public void TestLzmaCompress()
        {
            TestCompress("lzma");
        }

        private void TestCompress(string name = null)
        {
            var app = MakeEnv();
            var manager = app.Make<ICompressManager>();
            var compress = manager.Compress(System.Text.Encoding.Default.GetBytes("helloworld,helloworld,helloworld,helloworld,helloworld"), name);
            Assert.AreEqual(true, compress.Length < "helloworld,helloworld,helloworld,helloworld,helloworld".Length);
            Assert.AreEqual("helloworld,helloworld,helloworld,helloworld,helloworld", System.Text.Encoding.Default.GetString(manager.Decomporess(compress, name)));
        }

        [TestMethod]
        public void TestMakeFromICompress()
        {
            var app = MakeEnv();
            var manager = app.Make<ICompressManager>();
            Assert.AreSame(manager.Default, app.Make<ICompress>());
        }
    }
}
