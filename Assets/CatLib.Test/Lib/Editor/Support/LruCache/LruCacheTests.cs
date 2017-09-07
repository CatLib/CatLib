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

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Stl
{
    [TestClass]
    public class LruCacheTests
    {
        /// <summary>
        /// 增加测试
        /// </summary>
        [TestMethod]
        public void AddTest()
        {
            var cache = new LruCache<string, string>(5);
            for (var i = 0; i < 5; i++)
            {
                cache.Add(i.ToString(), i.ToString());
            }

            var n = 5;
            foreach (var v in cache)
            {
                Assert.AreEqual((--n).ToString(), v.Value);
            }
        }

        /// <summary>
        /// 获取测试
        /// </summary>
        [TestMethod]
        public void GetTest()
        {
            var cache = new LruCache<string, string>(5);
            for (var i = 0; i < 5; i++)
            {
                cache.Add(i.ToString(), i.ToString());
            }

            var result = cache["0"];
            result = cache["1"];

            if (result == null)
            {
                Assert.Fail();
            }

            foreach (var v in new[] { "1", "0", "4", "3", "2" })
            {
                Assert.AreEqual(v, cache[v]);
            }
        }

        /// <summary>
        /// 覆盖测试
        /// </summary>
        [TestMethod]
        public void ReplaceTest()
        {
            var cache = new LruCache<string, string>(5);
            for (var i = 0; i < 5; i++)
            {
                cache.Add(i.ToString(), i.ToString());
            }

            cache["0"] = "10";
            cache["1"] = "11";

            foreach (var v in new[] { "1", "0", "4", "3", "2" })
            {
                if (v == "0")
                {
                    Assert.AreEqual("10", cache[v]);
                }
                else if (v == "1")
                {
                    Assert.AreEqual("11", cache[v]);
                }
                else
                {
                    Assert.AreEqual(v, cache[v]);
                }
            }
        }

        /// <summary>
        /// 测试移除事件
        /// </summary>
        [TestMethod]
        public void TestRemoveEvent()
        {
            var cache = new LruCache<string, string>(5);
            for (var i = 0; i < 5; i++)
            {
                cache.Add(i.ToString(), i.ToString());
            }

            var callNum = 0;
            cache.OnRemoveLeastUsed += (key, val) =>
            {
                if (callNum++ <= 0)
                {
                    cache.Get(key);
                }
            };
            cache.Add("10", "10");

            Assert.AreEqual(default(string), cache.Get("1"));
            Assert.AreEqual(5, cache.Count);
            Assert.AreEqual(1, callNum);
        }

        /// <summary>
        /// 末尾移除测试
        /// </summary>
        [TestMethod]
        public void RemoveLastedTest()
        {
            var cache = new LruCache<string, string>(5);
            for (var i = 0; i < 5; i++)
            {
                cache.Add(i.ToString(), i.ToString());
            }
            cache["0"] = "0";
            cache["1"] = "1";
            cache.Add("999", "999");

            Assert.AreEqual(5, cache.Count);
            foreach (var v in new[] { "999", "1", "0", "4", "3" })
            {
                Assert.AreEqual(v, cache[v]);
            }
        }

        /// <summary>
        /// 获取不存在元素的测试
        /// </summary>
        [TestMethod]
        public void GetNotExistsKey()
        {
            var cache = new LruCache<string, string>(5);
            Assert.AreEqual(null, cache["123"]);
            Assert.AreEqual("notExists", cache.Get("111", "notExists"));
        }

        /// <summary>
        /// 移除测试
        /// </summary>
        [TestMethod]
        public void RemoveTest()
        {
            var cache = new LruCache<string, string>(5);
            for (var i = 0; i < 5; i++)
            {
                cache.Add(i.ToString(), i.ToString());
            }

            foreach (var v in new[] { "2", "1", "3" })
            {
                cache.Remove(v);
            }

            foreach (var v in new[] { "4", "0" })
            {
                Assert.AreEqual(v, cache[v]);
            }

            Assert.AreEqual(2, cache.Count);
        }

        /// <summary>
        /// 在头部和尾部移除
        /// </summary>
        [TestMethod]
        public void RemoveWithHeaderAndTail()
        {
            var cache = new LruCache<string, string>(5);
            for (var i = 0; i < 5; i++)
            {
                cache.Add(i.ToString(), i.ToString());
            }

            foreach (var v in new[] { "0", "2", "4" })
            {
                cache.Remove(v);
            }

            foreach (var v in new[] { "3", "1" })
            {
                Assert.AreEqual(v, cache[v]);
            }

            Assert.AreEqual(2, cache.Count);
        }

        /// <summary>
        /// 移除不存在的元素
        /// </summary>
        [TestMethod]
        public void RemoveNotExists()
        {
            var cache = new LruCache<string, string>(5);
            cache.Remove("999");
        }

        [TestMethod]
        public void TestGet()
        {
            var cache = new LruCache<string, string>(5);
            cache.Add("10", "5");

            string val;
            Assert.AreEqual(true, cache.Get("10", out val, "100"));
            Assert.AreEqual("5", val);

            Assert.AreEqual(false, cache.Get("11", out val, "100"));
            Assert.AreEqual("100", val);
        }
    }
}
