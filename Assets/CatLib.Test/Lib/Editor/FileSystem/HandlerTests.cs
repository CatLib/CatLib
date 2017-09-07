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
    public class HandlerTests
    {

        private Handler handlerDir;

        private Handler handlerFile;

        private Local local;

        internal class HandlerTest : Handler
        {
            /// <summary>
            /// 文件夹
            /// </summary>
            /// <param name="fileSystem">文件系统</param>
            /// <param name="path">文件夹路径</param>
            public HandlerTest(global::CatLib.FileSystem.FileSystem fileSystem, string path) :
                base(fileSystem, path)
            {
            }
        }

        [TestMethod]
        public void HandlerGetPathTest()
        {
            Env(() =>
            {
                Assert.AreEqual("TestHandler", handlerDir.Path);
                Assert.AreEqual("TestFileHandler", handlerFile.Path);
            });
        }

        [TestMethod]
        public void HandlerCopyTest()
        {
            Env(() =>
            {
                handlerDir.Copy("TestHandler-copy");
                Assert.AreEqual(true, handlerDir.IsExists);
                Assert.AreEqual(true, local.Exists("TestHandler-copy"));
                Assert.AreEqual(true, local.Exists("TestHandler-copy/InFile"));

                handlerFile.Copy("TestFileHandler-copy");
                Assert.AreEqual(true, handlerFile.IsExists);
                Assert.AreEqual(true, local.Exists("TestFileHandler-copy"));
            });
        }

        [TestMethod]
        public void HandlerDeletTest()
        {
            Env(() =>
            {
                handlerDir.Delete();
                handlerFile.Delete();
                Assert.AreEqual(false, local.Exists("TestHandler"));
                Assert.AreEqual(false, local.Exists("TestFileHandler"));
            });
        }

        [TestMethod]
        public void HandlerRenameTest()
        {
            Env(() =>
            {
                handlerDir.Rename("TestHandler-rename");
                handlerFile.Rename("TestFileHandler-rename");

                Assert.AreEqual(true, handlerDir.IsExists);
                Assert.AreEqual(true, handlerFile.IsExists);
                Assert.AreEqual(false, local.Exists("TestHandler"));
                Assert.AreEqual(false, local.Exists("TestFileHandler"));
                Assert.AreEqual("TestHandler-rename", handlerDir.Path);
                Assert.AreEqual("TestFileHandler-rename", handlerFile.Path);
            });
        }

        [TestMethod]
        public void HandlerGetAttributesTest()
        {
            Env(() =>
            {
                Assert.AreEqual(FileAttributes.Directory, handlerDir.GetAttributes() & FileAttributes.Directory);
                Assert.AreEqual((FileAttributes)0, handlerFile.GetAttributes() & FileAttributes.Directory);
            });
        }

        [TestMethod]
        public void HandlerIsDirTest()
        {
            Env(() =>
            {
                Assert.AreEqual(true, handlerDir.IsDir);
                Assert.AreEqual(false, handlerFile.IsDir);
            });
        }

        [TestMethod]
        public void HandlerGetSizeTest()
        {
            Env(() =>
            {
                Assert.AreEqual(11, handlerDir.GetSize());
                Assert.AreEqual(11, handlerFile.GetSize());
                Assert.AreEqual(22, local.GetSize());
            });
        }

        private void Env(Action action)
        {
            var path = Path.Combine(System.Environment.CurrentDirectory, "FileSystemTest");
            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
            SIO.Directory.CreateDirectory(path);

            local = new Local(path);
            local.MakeDir("TestHandler");
            local.Write("TestHandler/InFile", GetByte("hello world"));
            local.Write("TestFileHandler", GetByte("hello world"));
            handlerDir = new HandlerTest(new global::CatLib.FileSystem.FileSystem(local), "TestHandler");
            handlerFile = new HandlerTest(new global::CatLib.FileSystem.FileSystem(local), "TestFileHandler");

            Assert.AreEqual(true, handlerDir.IsExists);
            Assert.AreEqual(true, handlerFile.IsExists);

            action.Invoke();

            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
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
