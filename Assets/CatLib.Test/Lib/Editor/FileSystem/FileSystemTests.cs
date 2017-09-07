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
    public class FileSystemTests
    {
        private global::CatLib.FileSystem.FileSystem fileSystem;

        [TestMethod]
        public void FileSystemCreateDirTest()
        {
            Env(() =>
            {
                Assert.AreEqual(false, fileSystem.Exists("FileSystemCreateDirTest-dir"));
                fileSystem.MakeDir("FileSystemCreateDirTest-dir");
                Assert.AreEqual(true, fileSystem.Exists("FileSystemCreateDirTest-dir"));

                Assert.AreEqual(false, fileSystem.Exists("FileSystemCreateDirTest2-dir/hello/world"));
                fileSystem.MakeDir("FileSystemCreateDirTest2-dir/hello/world");
                Assert.AreEqual(true, fileSystem.Exists("FileSystemCreateDirTest2-dir/hello/world"));
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

            var local = new Local(path);
            fileSystem = new global::CatLib.FileSystem.FileSystem(local);

            action.Invoke();

            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
        }
    }
}
