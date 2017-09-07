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
    /// 快速列表测试
    /// </summary>
    [TestClass]
    public class QuickListTests
    {
        /// <summary>
        /// 推入数据到尾部测试
        /// </summary>
        [TestMethod]
        public void PushShiftData()
        {
            var num = 500000;
            var lst = new QuickList<int>();
            var rand = new System.Random();
            var lst2 = new List<int>();
            for (var i = 0; i < num; i++)
            {
                var v = rand.Next();
                lst.Push(v);
                lst2.Add(v);
            }
            foreach (var v in lst2)
            {
                Assert.AreEqual(v, lst.Shift());
            }
            Assert.AreEqual(0, lst.Count);
            Assert.AreEqual(0, lst.Length);
        }

        /// <summary>
        /// 头部推入和尾部推出测试
        /// </summary>
        [TestMethod]
        public void UnShiftPopTest()
        {
            var num = 50000;
            var lst = new QuickList<int>();
            var rand = new System.Random();
            var lst2 = new List<int>();
            for (var i = 0; i < num; i++)
            {
                var v = rand.Next();
                lst.UnShift(v);
                lst2.Add(v);
            }
            foreach (var v in lst2)
            {
                Assert.AreEqual(v, lst.Pop());
            }
            Assert.AreEqual(0, lst.Count);
            Assert.AreEqual(0, lst.Length);
        }

        /// <summary>
        /// 在扫描到的第一个元素之后插入
        /// </summary>
        [TestMethod]
        public void InsertAfterTest()
        {
            var lst = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                lst.Push(i);
            }

            lst.InsertAfter(1, 999);
            lst.InsertAfter(3, 999);
            lst.InsertAfter(5, 999);
            lst.InsertAfter(7, 999);
            lst.InsertAfter(9, 999);

            Assert.AreEqual(999, lst[2]);
            Assert.AreEqual(999, lst[5]);
            Assert.AreEqual(999, lst[8]);
            Assert.AreEqual(999, lst[11]);
            Assert.AreEqual(15, lst.Count);
        }

        /// <summary>
        /// 在扫描到的第一个元素之后插入边界测试
        /// </summary>
        [TestMethod]
        public void InsertAfterBound()
        {
            var lst = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                lst.Push(i);
            }

            lst.InsertAfter(4, 999);
            lst.InsertAfter(999, 888);
            lst.InsertAfter(888, 777);
            Assert.AreEqual(999, lst[5]);
            Assert.AreEqual(888, lst[6]);
            Assert.AreEqual(777, lst[7]);
        }

        /// <summary>
        /// 在扫描到的第一个元素之前插入边界测试
        /// </summary>
        [TestMethod]
        public void InsertBeforeBound()
        {
            var lst = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                lst.Push(i);
            }

            lst.InsertBefore(4, 999);
            lst.InsertBefore(999, 888);
            lst.InsertBefore(888, 777);
            Assert.AreEqual(777, lst[4]);
            Assert.AreEqual(888, lst[5]);
            Assert.AreEqual(999, lst[6]);
        }

        /// <summary>
        /// 运行时增加
        /// </summary>
        [TestMethod]
        public void ForeachAdd()
        {
            var master = new QuickList<int>(10);
            for (int i = 0; i < 10; i++)
            {
                master.Push(i);
            }

            int n = 0;
            ExceptionAssert.Throws<InvalidOperationException>(() =>
            {
                foreach (var val in master)
                {
                    if (n < 20)
                    {
                        master.Push(10 + n);
                    }
                    Assert.AreEqual(n++, val);
                }
            });
        }

        /// <summary>
        /// 通过下标搜索
        /// </summary>
        [TestMethod]
        public void FindByIndex()
        {
            var master = new QuickList<int>(256);
            for (var i = 0; i < 5000; i++)
            {
                master.Push(i);
            }

            for (var i = 0; i < 5000; i++)
            {
                Assert.AreEqual(i, master[i]);
            }
        }

        /// <summary>
        /// 负数下标
        /// </summary>
        [TestMethod]
        public void FindByIndexNegativeSubscript()
        {
            var master = new QuickList<int>(256);
            for (var i = 0; i < 5000; i++)
            {
                master.Push(i);
            }
            for (var i = 0; i < 5000; i++)
            {
                Assert.AreEqual(5000 - i - 1, master[-(i + 1)]);
            }
        }

        /// <summary>
        /// 通过下标搜索溢出测试
        /// </summary>
        [TestMethod]
        public void FindByIndexOverflow()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 10; i++)
            {
                master.Push(i);
            }

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var val = master[master.Count];
                if (val < 0)
                {
                }
            });

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var val = master[-(master.Count + 1)];
                if (val < 0)
                {
                }
            });
        }

        /// <summary>
        /// 获取区间测试
        /// </summary>
        [TestMethod]
        public void GetRangeTest()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            var elements = master.GetRange(10, 100);
            Assert.AreEqual(91, elements.Length);
            for (var i = 10; i < 100; i++)
            {
                Assert.AreEqual(i, elements[i - 10]);
            }
        }

        /// <summary>
        /// 获取区间，跳过区块测试
        /// </summary>
        [TestMethod]
        public void GetRangeBlockTest()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            var elements = master.GetRange(100, 200);
            Assert.AreEqual(101, elements.Length);
            for (var i = 100; i < 200; i++)
            {
                Assert.AreEqual(i, elements[i - 100]);
            }
        }

        /// <summary>
        /// 无效的获取区间测试
        /// </summary>
        [TestMethod]
        public void InvalidGetRange()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                master.GetRange(50, 10);
            });
        }

        /// <summary>
        /// 顺序的移除元素测试
        /// </summary>
        [TestMethod]
        public void SequenceRemove()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            master.Remove(5);
            master.Remove(6);

            Assert.AreEqual(7, master[5]);
            Assert.AreEqual(8, master[6]);
            Assert.AreEqual(254, master.Count);
        }

        /// <summary>
        /// 顺序随机的移除元素测试
        /// </summary>
        [TestMethod]
        public void SequenceRemoveRandom()
        {
            var master = new QuickList<int>(8);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            var lst = new List<int>();
            for (var i = 255; i >= 0; i--)
            {
                if (!lst.Contains(i))
                {
                    lst.Add(i);
                    master.Remove(i);
                }
            }

            foreach (var v in master)
            {
                if (lst.Contains(v))
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// 逆序的移除元素测试
        /// </summary>
        [TestMethod]
        public void ReverseRemove()
        {
            var master = new QuickList<int>(20);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            master.Remove(5, -999);
            master.Remove(6, -999);

            Assert.AreEqual(7, master[5]);
            Assert.AreEqual(8, master[6]);
            Assert.AreEqual(254, master.Count);
        }

        /// <summary>
        /// 逆序的随机删除元素
        /// </summary>
        [TestMethod]
        public void ReverseRemoveRandom()
        {
            var master = new QuickList<int>(8);
            for (var i = 0; i < 256; i++)
            {
                master.Push(i);
            }

            var lst = new List<int>();
            for (var i = 255; i >= 0; i--)
            {
                if (!lst.Contains(i))
                {
                    lst.Add(i);
                    master.Remove(i, -999);
                }
            }

            foreach (var v in master)
            {
                if (lst.Contains(v))
                {
                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// 移除返回值测试
        /// </summary>
        [TestMethod]
        public void RemoveReturnNumTest()
        {
            var master = new QuickList<int>(8);
            master.Push(111);
            master.Push(111);
            master.Push(111);
            master.Push(222);
            master.Push(333);
            master.Push(111);
            master.Push(111);
            master.Push(444);
            master.Push(333);

            var removeNum = master.Remove(111);
            Assert.AreEqual(5, removeNum);
        }

        /// <summary>
        /// 移除返回值限制测试
        /// </summary>
        [TestMethod]
        public void RemoveReturnNumLimitTest()
        {
            var master = new QuickList<int>(8);
            master.Push(111);
            master.Push(111);
            master.Push(111);
            master.Push(222);
            master.Push(333);
            master.Push(111);
            master.Push(111);
            master.Push(444);
            master.Push(333);

            var removeNum = master.Remove(111, 3);
            Assert.AreEqual(3, removeNum);
            Assert.AreEqual(222, master[0]);
            Assert.AreEqual(111, master[2]);
        }

        /// <summary>
        /// 逆序的移除返回值限制测试
        /// </summary>
        [TestMethod]
        public void ReverseRemoveReturnNumLimitTest()
        {
            var master = new QuickList<int>(8);
            master.Push(111);
            master.Push(111);
            master.Push(111);
            master.Push(222);
            master.Push(333);
            master.Push(111);
            master.Push(111);
            master.Push(444);
            master.Push(333);

            var removeNum = master.Remove(111, -3);
            Assert.AreEqual(3, removeNum);
            Assert.AreEqual(111, master[0]);
            Assert.AreEqual(111, master[1]);
            Assert.AreEqual(222, master[2]);
            Assert.AreEqual(333, master[3]);
            Assert.AreEqual(444, master[4]);
            Assert.AreEqual(6, master.Count);
        }

        /// <summary>
        /// 合并结点测试
        /// </summary>
        [TestMethod]
        public void MergeNodeTest()
        {
            var master = new QuickList<int>(10);
            //node 1
            master.Push(0);
            master.Push(1);
            master.Push(2);
            master.Push(3);
            master.Push(4);
            master.Push(5);
            master.Push(6);
            master.Push(7);
            master.Push(8);
            master.Push(9);

            //node 2 
            master.Push(10);
            master.Push(11);
            master.Push(12);
            master.Push(13);
            master.Push(14);
            master.Push(15);
            master.Push(16);
            master.Push(17);
            master.Push(18);
            master.Push(19);

            //node 3
            master.Push(20);
            master.Push(21);
            master.Push(22);
            master.Push(23);
            master.Push(24);
            master.Push(25);
            master.Push(26);
            master.Push(27);
            master.Push(28);
            master.Push(29);


            master.InsertAfter(17, 777);
            master.InsertAfter(17, 666);
            master.InsertAfter(17, 555);

            Assert.AreEqual(4, master.Length);

            master.InsertAfter(555, 444);

            Assert.AreEqual(4, master.Length);
        }

        /// <summary>
        /// 合并结点前合并测试
        /// </summary>
        [TestMethod]
        public void MergeNodeBeforeTest()
        {
            var master = new QuickList<int>(10);
            //node 1
            master.Push(0);
            master.Push(1);
            master.Push(2);
            master.Push(3);
            master.Push(4);
            master.Push(5);
            master.Push(6);
            master.Push(7);
            master.Push(8);
            master.Push(9);

            //node 2 
            master.Push(10);
            master.Push(11);
            master.Push(12);
            master.Push(13);
            master.Push(14);
            master.Push(15);
            master.Push(16);
            master.Push(17);
            master.Push(18);
            master.Push(19);

            //node 3
            master.Push(20);
            master.Push(21);
            master.Push(22);
            master.Push(23);
            master.Push(24);
            master.Push(25);
            master.Push(26);
            master.Push(27);
            master.Push(28);
            master.Push(29);


            master.InsertBefore(12, 777);
            master.InsertBefore(12, 666);
            master.InsertBefore(12, 555);

            Assert.AreEqual(4, master.Length);

            master.InsertBefore(555, 444);

            Assert.AreEqual(4, master.Length);
        }

        /// <summary>
        /// Foreach遍历列表
        /// </summary>
        [TestMethod]
        public void ForeachSequenceListTest()
        {
            var master = new QuickList<int>(5);
            master.Push(0);
            master.Push(1);
            master.Push(2);
            master.Push(3);
            master.Push(4);
            master.Push(5);
            master.Push(6);
            master.Push(7);
            master.Push(8);
            master.Push(9);

            var i = 0;
            foreach (var v in master)
            {
                Assert.AreEqual(i, master[i]);
                Assert.AreEqual(i++, v);
                break;
            }
            i = 0;
            foreach (var v in master)
            {
                Assert.AreEqual(i, master[i]);
                Assert.AreEqual(i++, v);
            }
        }

        /// <summary>
        /// 反向遍历列表
        /// </summary>
        [TestMethod]
        public void ForeachReverseListTest()
        {
            var master = new QuickList<int>(5);
            master.Push(0);
            master.Push(1);
            master.Push(2);
            master.Push(3);
            master.Push(4);
            master.Push(5);
            master.Push(6);
            master.Push(7);
            master.Push(8);
            master.Push(9);

            master.ReverseIterator();

            var i = 0;
            foreach (var v in master)
            {
                Assert.AreEqual(9 - i++, v);
                break;
            }
            i = 0;
            foreach (var v in master)
            {
                Assert.AreEqual(9 - i++, v);
            }
        }

        /// <summary>
        /// 空列表遍历
        /// </summary>
        [TestMethod]
        public void EmptyListForeach()
        {
            var master = new QuickList<int>(5);
            foreach (var v in master)
            {
                Assert.Fail();
                if (v < 0)
                {
                }
            }

            master.ReverseIterator();
            foreach (var v in master)
            {
                Assert.Fail();
                if (v < 0)
                {
                }
            }
        }

        /// <summary>
        /// 裁剪测试
        /// </summary>
        [TestMethod]
        public void TrimTest()
        {
            foreach (var cap in new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })
            {
                var master = new QuickList<int>(cap);
                master.Push(0);
                master.Push(1);
                master.Push(2);
                master.Push(3);
                master.Push(4);
                master.Push(5);
                master.Push(6);
                master.Push(7);
                master.Push(8);
                master.Push(9);
                master.Push(10);
                master.Push(11);
                master.Push(12);

                Assert.AreEqual(8, master.Trim(4, 8));
                var i = 4;
                foreach (var v in master)
                {
                    Assert.AreEqual(i++, v);
                }
                Assert.AreEqual(5, master.Count);
                Assert.AreEqual(9, i);
            }
        }

        /// <summary>
        /// 裁切的起始元素和终止元素一致测试
        /// </summary>
        [TestMethod]
        public void TrimToOneTest()
        {
            foreach (var cap in new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })
            {
                var master = new QuickList<int>(cap);
                master.Push(0);
                master.Push(1);
                master.Push(2);
                master.Push(3);
                master.Push(4);
                master.Push(5);
                master.Push(6);
                master.Push(7);
                master.Push(8);
                master.Push(9);
                master.Push(10);
                master.Push(11);
                master.Push(12);

                Assert.AreEqual(12, master.Trim(4, 4));
                var i = 4;
                foreach (var v in master)
                {
                    Assert.AreEqual(i++, v);
                }
                Assert.AreEqual(1, master.Count);
                Assert.AreEqual(5, i);
            }
        }

        /// <summary>
        /// 裁剪边界测试
        /// </summary>
        [TestMethod]
        public void TrimBoundTest()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            Assert.AreEqual(250, master.Trim(250, 999));
            Assert.AreEqual(250, master[0]);
            Assert.AreEqual(251, master[1]);
            Assert.AreEqual(252, master[2]);
            Assert.AreEqual(253, master[3]);
            Assert.AreEqual(254, master[4]);

            master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }
        }

        [TestMethod]
        public void FindNotExistsNode()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            master.InsertAfter(888, 888);
            Assert.AreEqual(255, master.Count);
        }

        [TestMethod]
        public void FindOutArgsIndexNode()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var v = master[256];
                if (v < 0)
                {
                }
            });

            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var v = master[-256];
                if (v < 0)
                {
                }
            });
        }

        /// <summary>
        /// 尾部插入测试
        /// </summary>
        [TestMethod]
        public void InsertNodeAtTailTest()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            master.Remove(5);
            master.InsertAfter(4, 999);

            Assert.AreEqual(999, master[5]);
        }

        /// <summary>
        /// 头部插入测试
        /// </summary>
        [TestMethod]
        public void InsertNodeAtHeaderTest()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            master.Remove(4);
            master.InsertBefore(5, 999);

            Assert.AreEqual(999, master[4]);
        }

        /// <summary>
        /// 结点是满的然后插入
        /// </summary>
        [TestMethod]
        public void FullNodeAndInsert()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }
            master.InsertBefore(5, 999);
            Assert.AreEqual(999, master[5]);

            master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }
            master.InsertAfter(4, 999);
            Assert.AreEqual(999, master[5]);
        }

        /// <summary>
        /// 空内容Pop
        /// </summary>
        [TestMethod]
        public void EmptyListPop()
        {
            var master = new QuickList<object>();

            ExceptionAssert.Throws<InvalidOperationException>(() =>
            {
                master.Pop();
            });
        }

        /// <summary>
        /// 空内容Pop
        /// </summary>
        [TestMethod]
        public void EmptyListShift()
        {
            var master = new QuickList<object>();
            ExceptionAssert.Throws<InvalidOperationException>(() =>
            {
                master.Shift();
            });
        }

        /// <summary>
        /// 找不到元素时的插入测试
        /// </summary>
        [TestMethod]
        public void NotFindToInsterBefore()
        {
            var master = new QuickList<int>();
            master.InsertBefore(999, 199);
            Assert.AreEqual(0, master.Count);
        }

        /// <summary>
        /// 根据下标设定值
        /// </summary>
        [TestMethod]
        public void SetListValueByIndex()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            master[0] = 999;
            master[128] = 999;
            master[254] = 999;

            Assert.AreEqual(999, master[0]);
            Assert.AreEqual(999, master[128]);
            Assert.AreEqual(999, master[254]);
        }

        /// <summary>
        /// 从尾部开始移除指定数量的元素
        /// </summary>
        [TestMethod]
        public void RemovWithCountByTail()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            master.Push(255);
            master.Push(255);
            master.Push(255);
            master.Push(255);

            master.Remove(255, -3);

            Assert.AreEqual(255, master[255]);
            Assert.AreEqual(256, master.Count);
        }

        /// <summary>
        /// 获取同步
        /// </summary>
        [TestMethod]
        public void GetSyncRootTest()
        {
            var master1 = new QuickList<int>();
            var master2 = new QuickList<int>();

            Assert.AreNotSame(master1, master2);
        }

        /// <summary>
        /// 空的列表测试
        /// </summary>
        [TestMethod]
        public void GetRangeEmptyListTest()
        {
            var master = new QuickList<int>(5);
            Assert.AreEqual(0, master.GetRange(-1, 100).Length);
        }

        /// <summary>
        /// 清空测试
        /// </summary>
        [TestMethod]
        public void ClearTest()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            master.Clear();

            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            Assert.AreEqual(255 / 5, master.Length);
            Assert.AreEqual(255, master.Count);
            for (var i = 0; i < 255; i++)
            {
                Assert.AreEqual(i, master[i]);
            }
        }

        /// <summary>
        /// 边界测试
        /// </summary>
        [TestMethod]
        public void BoundFirstTest()
        {
            var master = new QuickList<int>(5);
            ExceptionAssert.Throws<InvalidOperationException>(() =>
            {
                master.First();
            });
        }

        /// <summary>
        /// 边界测试
        /// </summary>
        [TestMethod]
        public void BoundLastTest()
        {
            var master = new QuickList<int>(5);
            ExceptionAssert.Throws<InvalidOperationException>(() =>
            {
                master.Last();
            });
        }

        /// <summary>
        /// 头尾测试
        /// </summary>
        [TestMethod]
        public void FirstLastTest()
        {
            var master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            Assert.AreEqual(0, master.First());
            Assert.AreEqual(254, master.Last());

            for (var i = 0; i < 255; i++)
            {
                master.Shift();
                if (i < 254)
                {
                    Assert.AreEqual(i + 1, master.First());
                }
            }

            Assert.AreEqual(0, master.Count);

            master = new QuickList<int>(5);
            for (var i = 0; i < 255; i++)
            {
                master.Push(i);
            }

            for (var i = 0; i < 255; i++)
            {
                if (i < 254)
                {
                    Assert.AreEqual(254 - i, master.Last());
                }
                Assert.AreEqual(254 - i, master.Pop());
            }
        }

        [TestMethod]
        public void NullElementPushTest()
        {
            var master = new QuickList<object>(3);
            var result = new List<object> { null, 1, 2, null, 3 };

            foreach (var r in result)
            {
                master.Push(r);
            }

            var index = 0;
            foreach (var r in master)
            {
                Assert.AreEqual(result[index++], r);
            }
        }

        [TestMethod]
        public void NullElementUnShiftTest()
        {
            var master = new QuickList<object>(3);
            var result = new List<object> { null, 1, 2, null, 3 };

            foreach (var r in result)
            {
                master.UnShift(r);
            }

            result.Reverse();
            var index = 0;
            foreach (var r in master)
            {
                Assert.AreEqual(result[index++], r);
            }
        }

        [TestMethod]
        public void NullElementPopTest()
        {
            var master = new QuickList<object>(3);
            var result = new List<object> { null, 1, 2, null, 3 };

            foreach (var r in result)
            {
                master.Push(r);
            }

            result.Reverse();
            foreach (var r in result)
            {
                Assert.AreEqual(r, master.Pop());
            }
        }

        [TestMethod]
        public void NullElementShiftTest()
        {
            var master = new QuickList<object>(3);
            var result = new List<object> { null, 1, 2, null, 3 };

            foreach (var r in result)
            {
                master.Push(r);
            }

            foreach (var r in result)
            {
                Assert.AreEqual(r, master.Shift());
            }
        }

        [TestMethod]
        public void NullElementRemoveOne()
        {
            var master = new QuickList<object>(3);
            var result = new List<object> { null, 1, 2, null, 3 };

            foreach (var r in result)
            {
                master.Add(r); // alias name of Push
            }

            master.Remove(null, 1);

            Assert.AreEqual(4, master.Count);

            var index = 0;
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }

            Assert.AreEqual(0, master.Count);
            Assert.AreEqual(1, index);
        }

        [TestMethod]
        public void NullElementRemoveAll()
        {
            var master = new QuickList<object>(3);
            var result = new List<object> { null, 1, 2, null, 3 };

            foreach (var r in result)
            {
                master.Push(r);
            }

            master.Remove(null);

            Assert.AreEqual(3, master.Count);

            var index = 0;
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }

            Assert.AreEqual(0, master.Count);
            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void ElementRemoveNotNullElementTest()
        {
            var master = new QuickList<object>(3);
            var result = new List<object> { null, 1, 2, null, 3 };
            foreach (var r in result)
            {
                master.Push(r);
            }
            master.Remove(1);
            Assert.AreEqual(4, master.Count);
            var index = 0;
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }

            Assert.AreEqual(0, master.Count);
            Assert.AreEqual(2, index);
        }

        [TestMethod]
        public void NullElementRemoveAllWithNum()
        {
            var master = new QuickList<object>(3);
            var result = new List<object> { null, 1, 2, null, 3 };

            foreach (var r in result)
            {
                master.Push(r);
            }

            master.Remove(null, 5);

            Assert.AreEqual(3, master.Count);

            var index = 0;
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }
            if (master.Pop() == null) { index++; }

            Assert.AreEqual(0, master.Count);
            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void EmptyForeachTest()
        {
            var master = new QuickList<object>(3);

            foreach (var r in master)
            {
                Assert.Fail();
                if (r == null)
                {
                }
            }
        }
    }
}
