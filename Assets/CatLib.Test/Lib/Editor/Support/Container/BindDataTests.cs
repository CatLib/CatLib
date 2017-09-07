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
    /// 绑定数据测试用例
    /// </summary>
    [TestClass]
    public class BindDataTest
    {
        #region Needs
        /// <summary>
        /// 需要什么样的数据不为空
        /// </summary>
        [TestMethod]
        public void CheckNeedsIsNotNull()
        {
            var container = new Container();
            var bindData = new BindData(container, "NeedsIsNotNull", (app, param) => "hello world", false);

            var needs = bindData.Needs("TestService");
            var needsWithType = bindData.Needs<BindDataTest>();

            Assert.AreNotEqual(null, needs);
            Assert.AreNotEqual(null, needsWithType);
        }

        /// <summary>
        /// 检测当需求什么方法时传入无效参数
        /// </summary>
        [TestMethod]
        public void CheckNeedsIllegalValue()
        {
            var container = new Container();
            var bindData = new BindData(container, "CheckNeedsIllegalValue", (app, param) => "hello world", false);

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                bindData.Needs(null);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                bindData.Needs(string.Empty);
            });
        }

        /// <summary>
        /// 是否可以取得关系上下文
        /// </summary>
        [TestMethod]
        public void CanGetContextual()
        {
            var container = new Container();
            var bindData = new BindData(container, "NeedsIsNotNull", (app, param) => "hello world", false);

            bindData.Needs("need1").Given("abc");
            bindData.Needs("need2").Given<BindDataTest>();

            Assert.AreEqual("abc", bindData.GetContextual("need1"));
            Assert.AreEqual(container.Type2Service(typeof(BindDataTest)), bindData.GetContextual("need2"));
            Assert.AreEqual("empty", bindData.GetContextual("empty"));
        }
        #endregion

        #region Alias
        /// <summary>
        /// 是否能够增加别名
        /// </summary>
        [TestMethod]
        public void CanAddAlias()
        {
            var container = new Container();
            var bindData = container.Bind("CanAddAlias", (app, param) => "hello world", false);

            bindData.Alias("Alias");
            bindData.Alias<BindDataTest>();

            var textAliasGet = container.GetBind("Alias");
            Assert.AreSame(textAliasGet, bindData);

            var classAliasGet = container.GetBind(container.Type2Service(typeof(BindDataTest)));
            Assert.AreSame(bindData, textAliasGet);
            Assert.AreSame(bindData, classAliasGet);
        }

        /// <summary>
        /// 检测无效的别名
        /// </summary>
        [TestMethod]
        public void CheckIllegalAlias()
        {
            var container = new Container();
            var bindData = new BindData(container, "CheckIllegalAlias", (app, param) => "hello world", false);

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                bindData.Alias(null);
            });
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                bindData.Alias(string.Empty);
            });
        }
        #endregion

        #region OnRelease
        /// <summary>
        /// 是否能追加到释放事件
        /// </summary>
        [TestMethod]
        public void CanOnRelease()
        {
            var container = new Container();
            var bindData = new BindData(container, "CanAddOnRelease", (app, param) => "hello world", true);

            bindData.OnRelease((bind, obj) =>
            {
                Assert.AreEqual("Test", obj);
                Assert.AreSame(bindData, bind);
            });

            container.Instance("CanAddOnRelease", "Test");
            container.Release("CanAddOnRelease");
        }
        /// <summary>
        /// 检查无效的解决事件传入参数
        /// </summary>
        [TestMethod]
        public void CheckIllegalRelease()
        {
            var container = new Container();
            var bindData = new BindData(container, "CheckIllegalRelease", (app, param) => "hello world", false);

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                bindData.OnRelease(null);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                bindData.OnRelease((bind, obj) =>
                {
                    Assert.Fail();
                });
                container.Instance("CheckIllegalRelease", "Test");
                container.Release("CheckIllegalRelease");
            });
        }

        #endregion

        #region OnResolving
        /// <summary>
        /// 是否能追加到解决事件
        /// </summary>
        [TestMethod]
        public void CanAddOnResolving()
        {
            var container = new Container();
            var bindData = new BindData(container, "CanAddOnResolving", (app, param) => "hello world", false);

            bindData.OnResolving((bind, obj) => null);

            var data = bindData.ExecResolvingDecorator(new Container());
            Assert.AreEqual(null, data);
        }

        /// <summary>
        /// 检查无效的解决事件传入参数
        /// </summary>
        [TestMethod]
        public void CheckIllegalResolving()
        {
            var container = new Container();
            var bindData = new BindData(container, "CanAddOnResolving", (app, param) => "hello world", false);

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                bindData.OnResolving(null);
            });
        }
        #endregion

        #region UnBind
        /// <summary>
        /// 能够正常解除绑定
        /// </summary>
        [TestMethod]
        public void CanUnBind()
        {
            var container = new Container();
            var bindData = container.Bind("CanUnBind", (app, param) => "hello world", false);

            Assert.AreEqual("hello world", container.Make("CanUnBind").ToString());
            bindData.UnBind();
            Assert.AreEqual(null, container.Make("CanUnBind"));
        }

        /// <summary>
        /// 能够正常解除绑定
        /// </summary>
        [TestMethod]
        public void CheckIllegalUnBindInput()
        {
            var container = new Container();
            var bindData = container.Bind("CanUnBind", (app, param) => "hello world", false);
            bindData.UnBind();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                bindData.Alias("hello");
            });
        }
        #endregion

        #region AddContextual

        /// <summary>
        /// 重复写入上下文
        /// </summary>
        [TestMethod]
        public void AddContextualRepeat()
        {
            var container = new Container();
            var bindData = new BindData(container, "AddContextualRepeat", (app, param) => "hello world", false);

            bindData.AddContextual("service", "service given");
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                bindData.AddContextual("service", "service given");
            });
        }

        #endregion
    }
}