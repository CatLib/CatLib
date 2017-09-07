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
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Stl
{
    /// <summary>
    /// 给与数据测试用例
    /// </summary>
    [TestClass]
    public class GivenDataTest
    {
        /// <summary>
        /// 可以给与数据
        /// </summary>
        [TestMethod]
        public void CanGiven()
        {
            var container = new Container();
            var bindData = new BindData(container, "CanGiven", (app, param) => "hello world", false);
            var givenData = new GivenData(container, bindData);
            givenData.Needs("needs1");
            givenData.Given("hello");
            Assert.AreEqual("hello", bindData.GetContextual("needs1"));

            givenData = new GivenData(container, bindData);
            givenData.Needs("needs2");
            givenData.Given<GivenDataTest>();
            Assert.AreEqual(container.Type2Service(typeof(GivenDataTest)), bindData.GetContextual("needs2"));
        }

        /// <summary>
        /// 检查给与的无效值
        /// </summary>
        [TestMethod]
        public void CheckGivenIllegalValue()
        {
            var container = new Container();
            var bindData = new BindData(container, "CanGiven", (app, param) => "hello world", false);
            var givenData = new GivenData(container, bindData);
            givenData.Needs("needs");

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                givenData.Given(null);
            });
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                givenData.Given(string.Empty);
            });
        }
    }
}