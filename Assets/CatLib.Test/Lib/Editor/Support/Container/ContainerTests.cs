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
    /// 容器测试用例
    /// </summary>
    [TestClass]
    public class ContainerTest
    {
        #region Tag
        /// <summary>
        /// 是否可以标记服务
        /// </summary>
        [TestMethod]
        public void CanTagService()
        {
            var container = MakeContainer();
            ExceptionAssert.DoesNotThrow(() =>
            {
                container.Tag("TestTag", "service1", "service2");
            });
        }

        /// <summary>
        /// 检测无效的Tag输入
        /// </summary>
        [TestMethod]
        public void CheckIllegalTagInput()
        {
            var container = MakeContainer();
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Tag("TestTag");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Tag("TestTag", null);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Tag(null, "service1", "service2");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Tag(string.Empty, "service1", "service2");
            });
        }

        /// <summary>
        /// 是否可以根据标签生成服务
        /// </summary>
        [TestMethod]
        public void CanMakeWithTaged()
        {
            var container = MakeContainer();
            container.Bind("TestService1", (app, param) => "hello");
            container.Bind("TestService2", (app, param) => "world");

            container.Tag("TestTag", "TestService1", "TestService2");

            ExceptionAssert.DoesNotThrow(() =>
            {
                var obj = container.Tagged("TestTag");
                Assert.AreEqual(2, obj.Length);
                Assert.AreEqual("hello", obj[0]);
                Assert.AreEqual("world", obj[1]);
            });
        }

        /// <summary>
        /// 测试不存在的Tag
        /// </summary>
        [TestMethod]
        public void CheckNotExistTaged()
        {
            var container = MakeContainer();
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Tagged("TestTag");
            });
        }

        /// <summary>
        /// 合并标记
        /// </summary>
        [TestMethod]
        public void MergeTag()
        {
            var container = MakeContainer();
            container.Tag("hello", "world");
            container.Tag("hello", "world2");

            Assert.AreEqual(2, container.Tagged("hello").Length);
        }

        /// <summary>
        /// 空服务测试
        /// </summary>
        [TestMethod]
        public void NullTagService()
        {
            var container = MakeContainer();
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Tag("hello", "world", null);
            });
        }

        #endregion

        #region Bind
        /// <summary>
        /// 是否能够进行如果不存在则绑定的操作
        /// </summary>
        [TestMethod]
        public void CanBindIf()
        {
            var container = MakeContainer();
            var bind = container.BindIf("CanBindIf", (cont, param) => "Hello", true);
            var bind2 = container.BindIf("CanBindIf", (cont, param) => "World", false);

            Assert.AreSame(bind, bind2);
        }

        /// <summary>
        /// 是否能够进行如果不存在则绑定的操作
        /// </summary>
        [TestMethod]
        public void CanBindIfByType()
        {
            var container = MakeContainer();
            var bind = container.BindIf("CanBindIf", typeof(ContainerTest), true);
            var bind2 = container.BindIf("CanBindIf", typeof(ContainerTest), false);

            Assert.AreSame(bind, bind2);
        }

        /// <summary>
        /// 检测无效的绑定
        /// </summary>
        [TestMethod]
        public void CheckIllegalBind()
        {
            var container = MakeContainer();
            container.Bind("CheckIllegalBind", (cont, param) => "HelloWorld", true);

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Bind("CheckIllegalBind", (cont, param) => "Repeat Bind");
            });

            container.Instance("InstanceBind", "hello world");

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Bind("InstanceBind", (cont, param) => "Instance Repeat Bind");
            });

            container.Alias("Hello", "CheckIllegalBind");

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Bind("Hello", (cont, param) => "Alias Repeat Bind");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Bind(string.Empty, (cont, param) => "HelloWorld");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Bind(null, (cont, param) => "HelloWorld");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Bind("NoConcrete", null);
            });
        }

        /// <summary>
        /// 静态绑定方法
        /// </summary>
        [TestMethod]
        public void CanBindFuncStatic()
        {
            var container = MakeContainer();
            container.Bind("CanBindFuncStatic", (cont, param) => "HelloWorld", true);

            var bind = container.GetBind("CanBindFuncStatic");
            var hasBind = container.HasBind("CanBindFuncStatic");
            var obj = container.Make("CanBindFuncStatic");

            Assert.AreNotEqual(null, bind);
            Assert.AreEqual(true, hasBind);
            Assert.AreEqual(true, bind.IsStatic);
            Assert.AreSame("HelloWorld", obj);
        }

        /// <summary>
        /// 非静态绑定
        /// </summary>
        [TestMethod]
        public void CanBindFunc()
        {
            var container = MakeContainer();
            container.Bind("CanBindFunc", (cont, param) => new List<string>());

            var bind = container.Make("CanBindFunc");
            var bind2 = container.Make("CanBindFunc");

            Assert.AreNotEqual(null, bind);
            Assert.AreNotSame(bind, bind2);
        }

        /// <summary>
        /// 检测获取绑定
        /// </summary>
        [TestMethod]
        public void CanGetBind()
        {
            var container = MakeContainer();
            var bind = container.Bind("CanGetBind", (cont, param) => "hello world");
            var getBind = container.GetBind("CanGetBind");
            Assert.AreSame(bind, getBind);

            var getBindNull = container.GetBind("CanGetBindNull");
            Assert.AreEqual(null, getBindNull);

            bind.Alias("AliasName");
            var aliasBind = container.GetBind("AliasName");
            Assert.AreSame(bind, aliasBind);
        }

        /// <summary>
        /// 检测非法的获取绑定
        /// </summary>
        [TestMethod]
        public void CheckIllegalGetBind()
        {
            var container = MakeContainer();
            container.Bind("CheckIllegalGetBind", (cont, param) => "hello world");

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.GetBind(string.Empty);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.GetBind(null);
            });
        }

        /// <summary>
        /// 检测是否拥有绑定
        /// </summary>
        [TestMethod]
        public void CanHasBind()
        {
            var container = MakeContainer();
            var bind = container.Bind("CanHasBind", (cont, param) => "hello world");
            bind.Alias("AliasName");
            Assert.IsTrue(container.HasBind("CanHasBind"));
            Assert.IsTrue(container.HasBind("AliasName"));
            Assert.IsFalse(container.HasBind(container.Type2Service(typeof(ContainerTest))));
            bind.Alias<ContainerTest>();
            Assert.IsTrue(container.HasBind(container.Type2Service(typeof(ContainerTest))));
        }

        /// <summary>
        /// 检查是否是静态的函数
        /// </summary>
        [TestMethod]
        public void CanIsStatic()
        {
            var container = MakeContainer();
            var bind = container.Bind("CanIsStatic", (cont, param) => "hello world", true);
            container.Bind("CanIsStaticNotStatic", (cont, param) => "hello world not static");

            bind.Alias("AliasName");
            Assert.IsTrue(container.IsStatic("CanIsStatic"));
            Assert.IsTrue(container.IsStatic("AliasName"));
            Assert.IsFalse(container.IsStatic("NoAliasName"));
            Assert.IsFalse(container.IsStatic("CanIsStaticNotStatic"));
            Assert.IsTrue(container.HasBind("CanIsStaticNotStatic"));
        }
        #endregion

        #region Alias
        /// <summary>
        /// 正常的设定别名
        /// </summary>
        [TestMethod]
        public void CheckNormalAlias()
        {
            var container = MakeContainer();
            container.Bind("CheckNormalAlias", (cont, param) => "hello world");

            container.Instance("StaticService", "hello");
            ExceptionAssert.DoesNotThrow(() =>
            {
                container.Alias("AliasName1", "CheckNormalAlias");
            });
            ExceptionAssert.DoesNotThrow(() =>
            {
                container.Alias("AliasName2", "StaticService");
            });
        }

        /// <summary>
        /// 检测非法的别名输入
        /// </summary>
        [TestMethod]
        public void CheckIllegalAlias()
        {
            var container = MakeContainer();
            container.Bind("CheckIllegalAlias", (cont, param) => "hello world");
            container.Alias("AliasName", "CheckIllegalAlias");

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Alias("AliasName", "CheckIllegalAlias");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Alias("AliasNameOther", "CheckNormalAliasNotExist");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Alias(string.Empty, "CheckIllegalAlias");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Alias(null, "CheckIllegalAlias");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Alias("AliasNameOther2", string.Empty);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Alias("AliasNameOther3", null);
            });
        }
        #endregion

        #region Call
        /// <summary>
        /// 被注入的测试类
        /// </summary>
        public class CallTestClassInject
        {
            public object GetNumber()
            {
                return 2;
            }
        }
        /// <summary>
        /// 调用测试类
        /// </summary>
        public class CallTestClass
        {
            public object GetNumber(CallTestClassInject cls)
            {
                return cls != null ? cls.GetNumber() : 1;
            }

            public object GetNumberNoParam()
            {
                return 1;
            }
        }

        /// <summary>
        /// 调用测试类
        /// </summary>
        public class CallTestClassLoopDependency
        {
            public object GetNumber(LoopDependencyClass cls)
            {
                return 1;
            }
        }

        public class LoopDependencyClass
        {
            public LoopDependencyClass(LoopDependencyClass2 cls)
            {

            }
        }

        public class LoopDependencyClass2
        {
            public LoopDependencyClass2(LoopDependencyClass cls)
            {

            }
        }

        /// <summary>
        /// 循环依赖测试
        /// </summary>
        [TestMethod]
        public void CheckLoopDependency()
        {
            var container = MakeContainer();
            container.Bind<LoopDependencyClass>();
            container.Bind<LoopDependencyClass2>();

            var cls = new CallTestClassLoopDependency();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Call(cls, "GetNumber");
            });
        }

        /// <summary>
        /// 可以调用方法
        /// </summary>
        [TestMethod]
        public void CanCallMethod()
        {
            var container = MakeContainer();
            container.Bind<CallTestClassInject>();
            var cls = new CallTestClass();

            var result = container.Call(cls, "GetNumber");
            Assert.AreEqual(2, result);

            var results = container.Call(cls, "GetNumberNoParam");
            Assert.AreEqual(1, results);
        }

        /// <summary>
        /// 无参数调用函数
        /// </summary>
        [TestMethod]
        public void CanCallMethodNoParam()
        {
            var container = MakeContainer();
            container.Bind<CallTestClassInject>();
            var cls = new CallTestClass();

            var result = container.Call(cls, "GetNumber", null);
            Assert.AreEqual(2, result);
        }

        /// <summary>
        /// 测试无效的调用方法
        /// </summary>
        [TestMethod]
        public void CheckIllegalCallMethod()
        {
            var container = MakeContainer();
            container.Bind<CallTestClassInject>();
            var cls = new CallTestClass();

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Call(null, "GetNumber");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Call(cls, "GetNumberIllegal");
            });
        }

        /// <summary>
        /// 测试无效的传入参数
        /// </summary>
        [TestMethod]
        public void CheckIllegalCallMethodParam()
        {
            var container = MakeContainer();
            container.Bind<CallTestClassInject>();
            var cls = new CallTestClass();

            var result = container.Call(cls, "GetNumber", "illegal param");
            Assert.AreEqual(2, result);

            result = container.Call(cls, "GetNumber", null);
            Assert.AreEqual(2, result);
        }
        #endregion

        #region Make

        public class MakeTestClass
        {
            private readonly MakeTestClassDependency dependency;

            [Inject]
            public MakeTestClassDependency Dependency { get; set; }

            [Inject(Required = true)]
            public MakeTestClassDependency DependencyRequired { get; set; }

            [Inject("AliasName")]
            public MakeTestClassDependency2 DependencyAlias { get; set; }

            [Inject("AliasNameRequired", Required = true)]
            public MakeTestClassDependency DependencyAliasRequired { get; set; }

            public MakeTestClass(MakeTestClassDependency dependency)
            {
                this.dependency = dependency;
            }

            public string GetMsg()
            {
                return dependency.GetMsg();
            }
        }

        public class MakeTestNoParamClass
        {
            public int I { get; set; }

            public MakeTestClassDependency Dependency { get; set; }

            public MakeTestNoParamClass(int i, MakeTestClassDependency dependency)
            {
                I = i;
                Dependency = dependency;
            }
        }

        public interface IMsg
        {
            string GetMsg();
        }

        public class MakeTestClassDependency : IMsg
        {
            public string GetMsg()
            {
                return "hello";
            }
        }

        public class MakeTestClassDependency2 : IMsg
        {
            public string GetMsg()
            {
                return "world";
            }
        }

        public class NoClassAttrInject
        {
            [Inject]
            public int Time { get; set; }
        }

        /// <summary>
        /// 非类的属性注入
        /// </summary>
        [TestMethod]
        public void MakeNoClassAttrInject()
        {
            var container = MakeContainer();
            container.Bind<NoClassAttrInject>();

            var result = container.Make<NoClassAttrInject>();
            Assert.AreEqual(0, result.Time);
        }

        /// <summary>
        /// 跨域生成没有绑定的服务
        /// </summary>
        [TestMethod]
        public void MakeNoBindType()
        {
            var container = MakeContainer();

            //container.OnFindType(Type.GetType); 不要使用这种写法否则域将不是这个程序集
            container.OnFindType((str) =>
            {
                return Type.GetType(str);
            });

            container.Bind<MakeTestClassDependency>().Alias("AliasNameRequired");
            var result = container.Make<MakeTestClass>();

            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        public void MakeWithNoParam()
        {
            var container = MakeContainer();
            container.Bind<MakeTestClassDependency>();
            var result = container.Make(container.Type2Service(typeof(MakeTestClassDependency)));
            Assert.AreNotEqual(null, result);
        }

        /// <summary>
        /// 无参构造函数的类进行make
        /// </summary>
        [TestMethod]
        public void MakeNoParamConstructor()
        {
            var container = MakeContainer();
            container.Bind<MakeTestClassDependency2>();
            var result = container.Make<MakeTestClassDependency2>();
            Assert.AreNotEqual(null, result);
        }

        /// <summary>
        /// 注入非类类型参数的构造函数
        /// </summary>
        [TestMethod]
        public void MakeNotClassConstructor()
        {
            var container = MakeContainer();
            container.Bind<MakeTestNoParamClass>();
            container.Bind<MakeTestClassDependency>();
            var result = container.Make<MakeTestNoParamClass>();
            Assert.AreEqual(0, result.I);
            Assert.AreNotEqual(null, result.Dependency);

            var result2 = container.MakeWith<MakeTestNoParamClass>(100);
            Assert.AreEqual(100, result2.I);
            Assert.AreNotEqual(null, result2.Dependency);
        }

        /// <summary>
        /// 是否能正常生成服务
        /// </summary>
        [TestMethod]
        public void CanMake()
        {
            var container = MakeContainer();
            container.Bind<MakeTestClass>();
            container.Bind<MakeTestClassDependency>().Alias("AliasNameRequired");

            var result = container.Make<MakeTestClass>();
            Assert.AreEqual(typeof(MakeTestClass), result.GetType());

            var dep = new MakeTestClassDependency();
            var result2 = container.MakeWith<MakeTestClass>(dep);
            Assert.AreEqual(typeof(MakeTestClass), result2.GetType());

            var result3 = container[container.Type2Service(typeof(MakeTestClass))] as MakeTestClass;
            Assert.AreEqual(typeof(MakeTestClass), result3.GetType());
        }

        /// <summary>
        /// 引发一个类型不一致的异常
        /// </summary>
        [TestMethod]
        public void CheckIllegalMakeTypeIsNotSame()
        {
            var container = MakeContainer();
            container.Singleton<MakeTestClass>();
            container.Singleton<MakeTestClassDependency2>().Alias("AliasNameRequired");
            container.Singleton<MakeTestClassDependency>().Alias("AliasName");

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Make<MakeTestClass>();
            });
        }

        /// <summary>
        /// 可以生成静态的对象
        /// </summary>
        [TestMethod]
        public void CanMakeStaticAlias()
        {
            var container = MakeContainer();
            container.Singleton<MakeTestClass>();
            container.Singleton<MakeTestClassDependency2>().Alias("AliasName");
            container.Singleton<MakeTestClassDependency>().Alias("AliasNameRequired");

            var result1 = container.Make<MakeTestClass>();
            var result2 = container.Make<MakeTestClass>();

            Assert.AreSame(result1, result2);
            Assert.AreSame(result1.DependencyAliasRequired, result2.DependencyAliasRequired);
            Assert.AreNotSame(result1.DependencyAlias, result2.DependencyAliasRequired);
        }

        /// <summary>
        /// 可以根据别名来生成对应不同的实例
        /// </summary>
        [TestMethod]
        public void CanMakeWithAlias()
        {
            var container = MakeContainer();
            container.Bind<MakeTestClass>();
            container.Bind<MakeTestClassDependency2>().Alias("AliasName");
            container.Bind<MakeTestClassDependency>().Alias("AliasNameRequired");

            var result = container.Make<MakeTestClass>();

            Assert.AreEqual("world", result.DependencyAlias.GetMsg());
            Assert.AreEqual("hello", result.DependencyAliasRequired.GetMsg());
        }

        /// <summary>
        /// 能够生成常规绑定
        /// </summary>
        [TestMethod]
        public void CanMakeNormalBind()
        {
            var container = MakeContainer();
            container.Bind<MakeTestClass>();
            container.Bind<MakeTestClassDependency>().Alias("AliasNameRequired");

            var result1 = container.Make<MakeTestClass>();
            var result2 = container.Make<MakeTestClass>();

            Assert.AreNotSame(result1, result2);
            Assert.AreNotSame(result1.Dependency, result1.DependencyRequired);
            Assert.AreNotSame(null, result1.DependencyRequired);
            Assert.AreNotSame(null, result1.DependencyAliasRequired);
            Assert.AreSame(null, result1.DependencyAlias);
        }

        /// <summary>
        /// 必须参数约束
        /// </summary>
        [TestMethod]
        public void CheckMakeRequired()
        {
            var container = MakeContainer();
            container.Bind<MakeTestClass>();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Make<MakeTestClass>();
            });
        }

        /// <summary>
        /// 无效的生成服务
        /// </summary>
        [TestMethod]
        public void CheckIllegalMake()
        {
            var container = MakeContainer();
            container.Bind<MakeTestClass>();

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                container.Make(string.Empty);
            });
        }

        /// <summary>
        /// 解决器是否有效
        /// </summary>
        [TestMethod]
        public void CanMakeWithResolve()
        {
            var container = MakeContainer();
            var bind = container.Bind<MakeTestClassDependency>();

            bind.OnResolving((bindData, obj) => "local resolve");
            container.OnResolving((bindData, obj) => obj + " global resolve");

            var result = container.Make(container.Type2Service(typeof(MakeTestClassDependency)));

            Assert.AreEqual("local resolve global resolve", result);
        }

        /// <summary>
        /// 给与了错误的解决器,导致不正确的返回值
        /// </summary>
        [TestMethod]
        public void CheckMakeWithErrorResolve()
        {
            var container = MakeContainer();
            var bind = container.Bind<MakeTestClass>();
            container.Bind<MakeTestClassDependency2>().Alias("AliasName");
            var bind2 = container.Bind<MakeTestClassDependency>().Alias("AliasNameRequired");

            bind.OnResolving((bindData, obj) => "local resolve");
            container.OnResolving((bindData, obj) => obj + " global resolve");
            bind2.OnResolving((bindData, obj) => "bind2");

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Make(container.Type2Service(typeof(MakeTestClass)));
            });
        }

        /// <summary>
        /// 参数注入标记测试类
        /// </summary>
        public class TestMakeParamInjectAttrClass
        {
            private IMsg msg;
            public TestMakeParamInjectAttrClass(
                [Inject("AliasName")]IMsg msg)
            {
                this.msg = msg;
            }

            public string GetMsg()
            {
                return msg.GetMsg();
            }
        }

        /// <summary>
        /// 参数可以使用注入标记
        /// </summary>
        [TestMethod]
        public void CanParamUseInjectAttr()
        {
            var container = MakeContainer();
            var bind = container.Bind<TestMakeParamInjectAttrClass>();
            container.Bind<MakeTestClassDependency>();
            var subBind = container.Bind<MakeTestClassDependency2>().Alias("AliasName");

            bind.Needs<IMsg>().Given<MakeTestClassDependency>();
            var cls = container.Make<TestMakeParamInjectAttrClass>();
            Assert.AreEqual("world", cls.GetMsg());

            bind.Needs("AliasName").Given<MakeTestClassDependency>();
            cls = container.Make<TestMakeParamInjectAttrClass>();
            Assert.AreEqual("hello", cls.GetMsg());

            subBind.UnBind();
            cls = container.Make<TestMakeParamInjectAttrClass>();
            Assert.AreEqual("hello", cls.GetMsg());
        }


        /// <summary>
        /// 参数注入是必须的
        /// </summary>
        public class TestMakeParamInjectAttrRequiredClass
        {
            private IMsg msg;
            public TestMakeParamInjectAttrRequiredClass(
                [Inject(Required = true)]IMsg msg)
            {
                this.msg = msg;
            }

            public string GetMsg()
            {
                return msg.GetMsg();
            }
        }

        /// <summary>
        /// 参数可以使用注入标记
        /// </summary>
        [TestMethod]
        public void CanParamUseInjectAttrRequired()
        {
            var container = MakeContainer();
            container.Bind<TestMakeParamInjectAttrRequiredClass>();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Make<TestMakeParamInjectAttrRequiredClass>();
            });

            container.Bind<IMsg, MakeTestClassDependency>();
            var result = container.Make<TestMakeParamInjectAttrRequiredClass>();
            Assert.AreEqual("hello", result.GetMsg());
        }

        public struct TestStruct
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// 测试结构体注入
        /// </summary>
        class TestMakeStructInject
        {
            [Inject]
            public TestStruct Struct { get; set; }

            [Inject]
            public MakeTestClassDependency Dependency { get; set; }
        }

        /// <summary>
        /// 可以进行结构体注入
        /// </summary>
        [TestMethod]
        public void CanMakeStructInject()
        {
            var container = MakeContainer();
            container.OnFindType((str) =>
            {
                return Type.GetType(str);
            });

            var result = container.Make<TestMakeStructInject>();
            Assert.AreNotEqual(null, result.Struct);
            Assert.AreNotEqual(null, result.Dependency);
        }

        class GenericClass<T>
        {
            public string GetMsg()
            {
                return typeof(T).ToString();
            }
        }

        /// <summary>
        /// 测试泛型注入
        /// </summary>
        class TestMakeGenericInject
        {
            [Inject]
            public GenericClass<string> Cls { get; set; }
        }

        /// <summary>
        /// 可以进行泛型注入
        /// </summary>
        [TestMethod]
        public void CanMakeGenericInject()
        {
            var container = MakeContainer();
            container.OnFindType((str) =>
            {
                return Type.GetType(str);
            });

            var result = container.Make<TestMakeGenericInject>();
            Assert.AreNotEqual(null, result.Cls);
            Assert.AreEqual(typeof(string).ToString(), result.Cls.GetMsg());

            container.Bind<GenericClass<string>>((app, param) => null);
            result = container.Make<TestMakeGenericInject>();
            Assert.AreEqual(null, result.Cls);

        }

        class TestOptionalInject
        {
            [Inject]
            public GenericClass<string> Cls { get; set; }

            public GenericClass<string> ClsNull { get; set; }
        }

        /// <summary>
        /// 可以进行泛型注入
        /// </summary>
        [TestMethod]
        public void OptionalInject()
        {
            var container = MakeContainer();
            container.OnFindType((str) =>
            {
                return Type.GetType(str);
            });

            var result = container.Make<TestOptionalInject>();
            Assert.AreNotEqual(null, result.Cls);
            Assert.AreEqual(null, result.ClsNull);
        }

        abstract class TestInjectBase
        {
            [Inject]
            public virtual GenericClass<string> Base { get; set; }

            public virtual GenericClass<string> Base2 { get; set; }

            [Inject]
            public virtual GenericClass<string> Base3 { get; set; }

            [Inject]
            public virtual GenericClass<string> Base4 { get; set; }

            [Inject]
            public abstract GenericClass<string> Base5 { get; set; }
        }

        class TestInject : TestInjectBase
        {
            [Inject]
            public override GenericClass<string> Base { get; set; }

            [Inject]
            public override GenericClass<string> Base2 { get; set; }

            public override GenericClass<string> Base3 { get; set; }

            public override GenericClass<string> Base5 { get; set; }
        }

        /// <summary>
        /// 实例一个无效的类
        /// </summary>
        [TestMethod]
        public void InvalidClassNew()
        {
            var container = MakeContainer();
            container.OnFindType((str) =>
            {
                return Type.GetType(str);
            });
            var result = container.Make<TestInjectBase>();
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// 继承注入
        /// </summary>
        [TestMethod]
        public void InheritanceInject()
        {
            var container = MakeContainer();
            container.OnFindType((str) =>
            {
                return Type.GetType(str);
            });

            var result = container.Make<TestInject>();

            Assert.AreNotEqual(null, result.Base);
            Assert.AreNotEqual(null, result.Base2);
            Assert.AreEqual(null, result.Base3);
            Assert.AreNotEqual(null, result.Base4);
            Assert.AreEqual(null, result.Base5);
        }

        interface IComplexInterface
        {
            string Message();
        }

        class ComplexClass
        {
            [Inject]
            public IComplexInterface Msg { get; set; }
        }

        class ComplexInjectClass1 : IComplexInterface
        {
            public string Message()
            {
                return "ComplexInjectClass1";
            }
        }

        class ComplexInjectClass2 : IComplexInterface
        {
            public string Message()
            {
                return "ComplexInjectClass2";
            }
        }

        /// <summary>
        /// 复杂的上下文关系测试
        /// </summary>
        [TestMethod]
        public void ComplexContextualRelationshipTest1()
        {
            var container = MakeContainer();
            container.Bind<ComplexClass>().Needs<IComplexInterface>().Given("IComplexInterface.alias");
            container.Bind<ComplexInjectClass1>().Alias<IComplexInterface>();
            container.Bind<ComplexInjectClass2>().Alias("IComplexInterface.alias");

            var cls = container.Make<ComplexClass>();
            Assert.AreEqual("ComplexInjectClass2", cls.Msg.Message());
        }

        /// <summary>
        /// 复杂的上下文关系测试
        /// </summary>
        [TestMethod]
        public void ComplexContextualRelationshipTest2()
        {
            var container = MakeContainer();
            container.Bind<ComplexClass>();
            container.Bind<ComplexInjectClass1>().Alias<IComplexInterface>();
            container.Bind<ComplexInjectClass2>().Alias("IComplexInterface.alias");

            var cls = container.Make<ComplexClass>();
            Assert.AreEqual("ComplexInjectClass1", cls.Msg.Message());
        }

        class ComplexClassAlias
        {
            [Inject("IComplexInterface.alias")]
            public IComplexInterface Msg { get; set; }
        }

        /// <summary>
        /// 复杂的上下文关系测试
        /// </summary>
        [TestMethod]
        public void ComplexContextualRelationshipTest3()
        {
            var container = MakeContainer();
            container.Bind<ComplexClassAlias>();
            container.Bind<ComplexInjectClass1>().Alias<IComplexInterface>();
            container.Bind<ComplexInjectClass2>().Alias("IComplexInterface.alias");

            var cls = container.Make<ComplexClassAlias>();
            Assert.AreEqual("ComplexInjectClass2", cls.Msg.Message());
        }

        /// <summary>
        /// 复杂的上下文关系测试
        /// </summary>
        [TestMethod]
        public void ComplexContextualRelationshipTest4()
        {
            var container = MakeContainer();
            container.Bind<ComplexClassAlias>().Needs("IComplexInterface.alias").Given<IComplexInterface>();
            container.Bind<ComplexInjectClass1>().Alias<IComplexInterface>();
            container.Bind<ComplexInjectClass2>().Alias("IComplexInterface.alias");

            var cls = container.Make<ComplexClassAlias>();
            Assert.AreEqual("ComplexInjectClass1", cls.Msg.Message());
        }
        #endregion

        #region Instance
        /// <summary>
        /// 可以正确的给定静态实例
        /// </summary>
        [TestMethod]
        public void CanInstance()
        {
            var container = MakeContainer();
            var data = new List<string> { "hello world" };
            var data2 = new List<string> { "hello world" };
            container.Instance("TestInstance", data);
            var result = container.Make("TestInstance");

            Assert.AreSame(data, result);
            Assert.AreNotSame(data2, result);
        }

        /// <summary>
        /// 测试无效的实例
        /// </summary>
        [TestMethod]
        public void CheckIllegalInstance()
        {
            var container = MakeContainer();
            container.Bind("TestInstance", (app, param) => "hello world", false);
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                container.Instance("TestInstance", "online");
            });
        }

        /// <summary>
        /// 能够通过release
        /// </summary>
        [TestMethod]
        public void CanInstanceWithRelease()
        {
            var container = MakeContainer();
            var bindData = container.Bind("TestInstance", (app, param) => string.Empty, true);

            bool isComplete = false, isComplete2 = false;
            bindData.OnRelease((bind, obj) =>
            {
                Assert.AreEqual("hello world", obj);
                Assert.AreSame(bindData, bind);
                isComplete = true;
            });

            container.OnRelease((bind, obj) =>
            {
                Assert.AreEqual("hello world", obj);
                Assert.AreSame(bindData, bind);
                isComplete2 = true;
            });
            container.Instance("TestInstance", "hello world");
            container.Release("TestInstance");

            if (isComplete && isComplete2)
            {
                return;
            }
            Assert.Fail();
        }
        #endregion

        /// <summary>
        /// 已存在的静态对象在注册新的OnResolving时会自动触发
        /// </summary>
        [TestMethod]
        public void OnResolvingExistsObject()
        {
            var container = MakeContainer();
            var data = new List<string> { "hello world" };
            container.Instance("TestInstance", data);

            var isCall = false;
            container.OnResolving((bind, obj) =>
            {
                isCall = true;
                Assert.AreSame(data, obj);
                return obj;
            });

            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 测试释放所有静态服务
        /// </summary>
        [TestMethod]
        public void TestReleaseAllStaticService()
        {
            var container = MakeContainer();
            var data = new List<string> { "hello world" };
            var isCallTest = false;
            container.Singleton("Test", (c, p) => { return "Test1"; }).OnRelease((bind, o) => { isCallTest = true; });
            container.Instance("TestInstance2", data);

            Assert.AreEqual("Test1", container.Make("Test"));

            container.Flush();

            Assert.AreEqual(true, isCallTest);
            Assert.AreEqual(null, container.Make("TestInstance2"));
            Assert.AreEqual(null, container.Make("Test"));
        }

        public class TestParamsMakeClass
        {
            public TestParamsMakeClass()
            {
            }
        }

        [TestMethod]
        public void TestMakeWithParams()
        {
            var container = MakeContainer();
            container.Bind<TestParamsMakeClass>();
            Assert.AreEqual(typeof(TestParamsMakeClass), container.MakeWith<TestParamsMakeClass>(null).GetType());
        }

        /// <summary>
        /// 生成容器
        /// </summary>
        /// <returns>容器</returns>
        private Container MakeContainer()
        {
            var container = new Container();
            return container;
        }
    }
}