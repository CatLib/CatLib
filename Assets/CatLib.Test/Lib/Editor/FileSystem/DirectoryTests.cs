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
using System.Collections.Generic;
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
    public class DirectoryTests
    {

        private Local local;

        private Directory handlerDir;

        [TestMethod]
        public void DirectoryGetListTest()
        {
            Env(() =>
            {
                local.Write("DirectoryTests.Directory/helloworld", GetByte("hello world"));
                local.Write("DirectoryTests.Directory/helloworld2", GetByte("hello world2"));
                local.MakeDir("DirectoryTests.Directory/helloworld-dir");

                var handlers = handlerDir.GetList();
                Assert.AreEqual(3, handlers.Length);

                var dict = new Dictionary<string, bool>();
                dict.Add("DirectoryTests.Directory\\helloworld", false);
                dict.Add("DirectoryTests.Directory\\helloworld2", false);
                dict.Add("DirectoryTests.Directory\\helloworld-dir", false);

                foreach (var handler in handlers)
                {
                    dict[handler.Path.Substring(SIO.Path.Combine(System.Environment.CurrentDirectory, "FileSystemTest").Length + 1)] = true;
                }

                foreach (var kv in dict)
                {
                    if (!kv.Value)
                    {
                        Assert.Fail();
                    }
                }
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
            local.MakeDir("DirectoryTests.Directory");
            handlerDir = new Directory(new global::CatLib.FileSystem.FileSystem(local), "DirectoryTests.Directory");

            Assert.AreEqual(true, handlerDir.IsExists);

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
