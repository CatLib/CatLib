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

using CatLib.API.Debugger;
using CatLib.Debugger.Log;
using CatLib.Debugger.WebLog;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Debugger.WebLog
{
    [TestClass]
    public class LogStoreTests
    {
        [TestMethod]
        public void TestLogOverflow()
        {
            var store = new LogStore(new Logger());

            store.SetMaxStore(10);

            for (var i = 0; i < 20; i++)
            {
                store.Log(new LogEntry(LogLevels.Info, "hello", 0));
            }

            Assert.AreEqual(10, store.GetUnloadEntrysByClientId("test").Count);
        }
    }
}
