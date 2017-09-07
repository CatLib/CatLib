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
    public class FileTests
    {

        private Local local;

        private File handlerFile;

        [TestMethod]
        public void FileWriteTest()
        {
            Env(() =>
            {
                Assert.AreEqual("hello world", GetString(local.Read("FileTests.TestFileHandler")));
                handlerFile.Write(GetByte("ni hao"));
                Assert.AreEqual("ni hao", GetString(local.Read("FileTests.TestFileHandler")));
                Assert.AreEqual("ni hao", GetString(handlerFile.Read()));
            });
        }

        [TestMethod]
        public void FileReadTest()
        {
            Env(() =>
            {
                Assert.AreEqual("hello world", GetString(local.Read("FileTests.TestFileHandler")));
            });
        }

        private void Env(Action action)
        {
            var path = SIO.Path.Combine(System.Environment.CurrentDirectory, "FileSystemTest");
            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
            SIO.Directory.CreateDirectory(path);

            local = new Local(path);
            local.Write("FileTests.TestFileHandler", GetByte("hello world"));
            handlerFile = new File(new global::CatLib.FileSystem.FileSystem(local), "FileTests.TestFileHandler");

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
