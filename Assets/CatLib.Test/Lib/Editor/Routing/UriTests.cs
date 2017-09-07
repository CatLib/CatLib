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

using CatLib.Routing;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Routing
{
    /// <summary>
    /// Uri测试
    /// </summary>
    [TestClass]
    public class UriTests
    {
        [TestMethod]
        public void UriTest()
        {
            var uri = new Uri("catlib://user:pass@hello/world?get=123#nihao");
            Assert.AreEqual("catlib://hello/world", uri.NoParamFullPath);
            int i = 0;
            foreach (var s in new[] { "/", "world" })
            {
                Assert.AreEqual(s, uri.Segments[i++]);
            }
            Assert.AreEqual("user:pass", uri.UserInfo);

            uri = new Uri(new System.Uri("catlib://user:pass@hello/world?get=123#nihao"));
            Assert.AreEqual("catlib://hello/world", uri.NoParamFullPath);
            i = 0;
            foreach (var s in new[] { "/", "world" })
            {
                Assert.AreEqual(s, uri.Segments[i++]);
            }
            Assert.AreEqual("user:pass", uri.UserInfo);
        }
    }
}
