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

using CatLib.API.Translation;
using CatLib.Translation;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Translation
{
    [TestClass]
    public class SelectorTests
    {
        [TestMethod]
        public void TestBaseSelect()
        {
            var selector = new Selector();
            Assert.AreEqual("hello this is test", selector.Choose("hello this is test", 10, Languages.Chinese));
        }

        [TestMethod]
        public void TestNotDefineStart()
        {
            var selector = new Selector();
            Assert.AreEqual("hello", selector.Choose("hello|[10,20]world|[21,*]", 5, Languages.Chinese));
        }

        [TestMethod]
        public void TestRangSelect()
        {
            var selector = new Selector();
            Assert.AreEqual("world", selector.Choose("[*,9]hello|[10,20]world", 10, Languages.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world", 7, Languages.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world", -1, Languages.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,9}hello|{10,20]world", 7, Languages.Chinese));

            Assert.AreEqual("world", selector.Choose("[*,9]hello|[10,*]world", 30, Languages.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,*]hello|[10,*]world", 30, Languages.Chinese));

            Assert.AreEqual("hello", selector.Choose("{*}hello|[10,*]world", 30, Languages.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*]hello|[10,*]world", 30, Languages.Chinese));

            Assert.AreEqual("catlib", selector.Choose("[*,9]hello|[10,20]world|{21,*}catlib", 30, Languages.Chinese));

            //如果什么都没有则过滤区间范围使用复数规则
            Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world", 30, Languages.Chinese));
            Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world|catlib", 30, Languages.Chinese));
        }

        [TestMethod]
        public void TestNullEmptyLine()
        {
            var selector = new Selector();
            Assert.AreEqual(string.Empty, selector.Choose(null, 30, Languages.Chinese));
            Assert.AreEqual(string.Empty, selector.Choose("", 30, Languages.Chinese));
        }

        [TestMethod]
        public void TestErrorRange()
        {
            var selector = new Selector();
            Assert.AreEqual("world", selector.Choose("[*,9,10]hello|[10,20,3030]world", 10, Languages.Chinese));
            Assert.AreEqual(string.Empty, selector.Choose("[*,9,10]|[19,20,3030]", 10, Languages.Chinese));
            Assert.AreEqual("world", selector.Choose("[*,9,1hello|[10,20,3030]world", 10, Languages.Chinese));
            Assert.AreEqual("[*,9,1hello", selector.Choose("[*,9,1hello|[10,20,3030]world", 2, Languages.Chinese));
        }

        [TestMethod]
        public void TestPluralOfZero()
        {
            var selector = new Selector();

            var langs = new[]
            {
                Languages.Azerbaijani,
                Languages.Tibetan,
                Languages.Bhutani,
                Languages.Indonesian,
                Languages.Japanese,
                Languages.Javanese,
                Languages.Georgian,
                Languages.Cambodian,
                Languages.Kannada,
                Languages.Korean,
                Languages.Malay,
                Languages.Thai,
                Languages.Turkish,
                Languages.Vietnamese,
                Languages.Chinese,
                Languages.ChineseTw,
            };

            foreach (var lang in langs)
            {
                Assert.AreEqual("hello", selector.Choose("[*,9]hello|[10,20]world", 30, lang));
                Assert.AreEqual("hello", selector.Choose("hello|world", 8, lang));
            }
        }

        [TestMethod]
        public void TestPluralMoreOne()
        {
            var selector = new Selector();

            var langs = new[]
            {
                Languages.Afrikaans,
                Languages.Bengali,
                Languages.Bulgarian,
                Languages.Catalan,
                Languages.Danish,
                Languages.German,
                Languages.Greek,
                Languages.English,
                Languages.Esperanto,
                Languages.Spanish,
                Languages.Estonian,
                Languages.Basque,
                Languages.Farsi,
                Languages.Finnish,
                Languages.Faeroese,
                Languages.Frisian,
                Languages.Galician,
                Languages.Gujarati,
                Languages.Hausa,
                Languages.Hebrew,
                Languages.Hungarian,
                Languages.Icelandic,
                Languages.Italian,
                Languages.Kurdish,
                Languages.Malayalam,
                Languages.Mongolian,
                Languages.Marathi,
                Languages.Nepali,
                Languages.Dutch,
                Languages.Norwegian,
                Languages.Oromo,
                Languages.Oriya,
                Languages.Punjabi,
                Languages.Pashto,
                Languages.Portuguese,
                Languages.Somali,
                Languages.Albanian,
                Languages.Swedish,
                Languages.Swahili,
                Languages.Tamil,
                Languages.Telugu,
                Languages.Turkmen,
                Languages.Urdu,
                Languages.Zulu,
            };

            foreach (var lang in langs)
            {
                Assert.AreEqual("world", selector.Choose("[*,9]hello|[10,20]world", 30, lang));
                Assert.AreEqual("world", selector.Choose("hello|world", -10, lang));
                Assert.AreEqual("hello", selector.Choose("hello|world", 1, lang));
            }
        }
    }
}
