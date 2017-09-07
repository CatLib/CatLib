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

using System.Collections.Generic;
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
    /// 请求测试
    /// </summary>
    [TestClass]
    public class RequestTests
    {
        /// <summary>
        /// 请求测试
        /// </summary>
        [TestMethod]
        public void RequestTest()
        {
            var obj = new object();
            var request = new Request("catlib://routing/world", obj);

            request.SetParameters(new Dictionary<string, string>
            {
                {"1", "1"},
                {"2", "2"},
                {"3", "3"},
                {"a", "b"},
                {"5.5", "5.5"},
                {"true", "true"},
            });

            request.SetRoute(null);

            Assert.AreEqual(obj, request.GetContext());
            Assert.AreEqual(null, request.Route);

            Assert.AreEqual("catlib", request.RouteUri.Scheme);

            Assert.AreEqual("1", request["1"]);
            Assert.AreEqual("999", request.GetString("999", "999"));

            Assert.AreEqual(2, request.GetInt("2"));
            Assert.AreEqual(2, request.GetInt("a", 2));

            Assert.AreEqual(3, request.GetLong("3"));
            Assert.AreEqual(3, request.GetLong("a", 3));

            Assert.AreEqual(5.5f, request.GetFloat("5.5"));
            Assert.AreEqual(5.5f, request.GetFloat("a", 5.5f));

            Assert.AreEqual(false, request.GetBoolean("1", false));
            Assert.AreEqual(true, request.GetBoolean("true"));
            Assert.AreEqual(false, request.GetBoolean("a", false));

            Assert.AreEqual(1, request.GetShort("1"));
            Assert.AreEqual(1, request.GetShort("a", 1));

            Assert.AreEqual("1", request.GetString("1"));
            Assert.AreEqual("1", request.Get("1"));

            Assert.AreEqual('1', request.GetChar("1", 'a'));
            Assert.AreEqual('a', request.GetChar("true", 'a'));

            Assert.AreEqual(1, request.GetDouble("1"));
            Assert.AreEqual(1, request.GetDouble("a", 1));

            Assert.AreEqual("/", request.Segment(0));
            Assert.AreEqual("world", request.Segment(1));
            Assert.AreEqual(null, request.Segment(2, null));
        }
    }
}
