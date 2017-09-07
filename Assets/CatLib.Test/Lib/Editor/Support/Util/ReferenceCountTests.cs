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
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Support.Util
{
    [TestClass]
    public class ReferenceCountTests
    {
        public class TestReferenceClass : IDisposable
        {
            /// <summary>
            /// 回调
            /// </summary>
            private Action callback;

            public TestReferenceClass(Action callback)
            {
                this.callback = callback;
            }

            /// <summary>
            /// 释放
            /// </summary>
            public void Dispose()
            {
                callback.Invoke();
            }
        }

        public class TestInherit : ReferenceCount ,IDisposable
        {
            internal bool IsReleaseCall;

            internal bool IsRetainCall;

            internal bool IsDispose;

            public override void Release()
            {
                IsReleaseCall = true;
                base.Release();
            }

            public override void Retain()
            {
                IsRetainCall = true;
                base.Retain();
            }

            public void Dispose()
            {
                IsDispose = true;
            }
        }

        [TestMethod]
        public void SimpleReferenceCountTest()
        {
            var isCall = false;

            var cls = new TestReferenceClass(() =>
            {
                isCall = true;
            });

            var rc = new ReferenceCount(cls);
            rc.Retain();
            Assert.AreEqual(1, rc.RefCount);
            rc.Release();

            Assert.AreEqual(true, isCall);

            ExceptionAssert.Throws<AssertException>(() =>
            {
                rc.Release();
            });

            ExceptionAssert.Throws<AssertException>(() =>
            {
                rc.Retain();
            });
        }

        [TestMethod]
        public void TestInheritReferenceCount()
        {
            var cls = new TestInherit();

            cls.Retain();
            Assert.AreEqual(1, cls.RefCount);
            cls.Release();

            Assert.AreEqual(true, cls.IsReleaseCall);
            Assert.AreEqual(true, cls.IsRetainCall);
            Assert.AreEqual(true, cls.IsDispose);

            ExceptionAssert.Throws<AssertException>(() =>
            {
                cls.Release();
            });

            ExceptionAssert.Throws<AssertException>(() =>
            {
                cls.Retain();
            });
        }
    }
}
