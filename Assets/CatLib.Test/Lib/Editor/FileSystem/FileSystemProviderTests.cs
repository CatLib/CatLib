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
using System.IO;
using CatLib.API.Config;
using CatLib.API.FileSystem;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Events;
using CatLib.FileSystem;
using CatLib.FileSystem.Adapter;
using SIO = System.IO;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.FileSystem
{
    [TestClass]
    public class FileSystemProviderTests
    {
        /// <summary>
        /// 测试路径
        /// </summary>
        private string path;

        [TestInitialize]
        public void TestInitialize()
        {
            path = Path.Combine(global::System.Environment.CurrentDirectory, "FileSystemTest");
            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
            var app = new Application();
            app.Bootstrap();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Register(new FileSystemProvider
            {
                DefaultPath = path
            });
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            app.Register(new EventsProvider());
            app.Init();
        }

        [TestMethod]
        public void GetDiskTest()
        {
            TestInitialize();
            var storage = App.Make<IFileSystemManager>();
            storage.Disk().Write("GetDisk", GetByte("hello world"));
            Assert.AreEqual(true, storage.Disk().Exists("GetDisk"));
            Assert.AreEqual("hello world", GetString(storage.Disk().Read("GetDisk")));
        }

        [TestMethod]
        public void ExtendExistsTest()
        {
            TestInitialize();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                var storage = App.Make<IFileSystemManager>();
                storage.Extend(() => new global::CatLib.FileSystem.FileSystem(new Local(path)));
            });
        }

        [TestMethod]
        public void DefaultConfigTest()
        {
            TestInitialize();

            var storage = App.Make<IFileSystemManager>();
            storage.Extend(() => new global::CatLib.FileSystem.FileSystem(new Local(Path.Combine(path, "DefaultConfigTest"))), "local-2");

            var config = App.Make<IConfigManager>();
            config.Default.Set("FileSystemProvider.DefaultDevice", "local-2");

            storage.Disk().Write("DefaultConfigTest", GetByte("hello world"));
            Assert.AreEqual(true, storage.Disk("local").Exists("DefaultConfigTest/DefaultConfigTest"));
            Assert.AreEqual("hello world", GetString(storage.Disk("local").Read("DefaultConfigTest/DefaultConfigTest")));
        }

        [TestMethod]
        public void UndefinedResolveTests()
        {
            TestInitialize();
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                var storage = App.Make<IFileSystemManager>();
                storage.Disk("undefined-disk");
            });
        }

        private byte[] GetByte(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }

        private string GetString(byte[] byt)
        {
            return System.Text.Encoding.Default.GetString(byt);
        }
    }
}
