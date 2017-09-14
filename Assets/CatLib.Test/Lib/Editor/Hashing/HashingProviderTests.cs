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

using CatLib.API.Hashing;
using CatLib.Events;
using CatLib.Hashing;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Hashing
{
    [TestClass]
    public class HashingProviderTests
    {
        public IApplication MakeEnv()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new EventsProvider());
            app.Register(new HashingProvider());
            app.Init();
            return app;
        }

        [TestMethod]
        public void TestCrc64()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();
            var code0 = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var code = hash.Checksum(new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, Checksums.Crc32);
            var code2 = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Crc32);
            var code3 = hash.Checksum(new byte[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Crc32);
            var code4 = hash.Checksum(System.Text.Encoding.Default.GetBytes("123"), Checksums.Crc32);
            var code5 = hash.Checksum(System.Text.Encoding.Default.GetBytes("123"));

            Assert.AreEqual(code, code0);
            Assert.AreEqual(code , code2);
            Assert.AreNotEqual(code2 , code3);
            Assert.AreEqual(2286445522, code4);
            Assert.AreEqual(2286445522, code5);
        }

        [TestMethod]
        public void TestAdler32()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();
            var code = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Adler32);
            var code2 = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Adler32);
            var code3 = hash.Checksum(new byte[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Adler32);
            var code4 = hash.Checksum(System.Text.Encoding.Default.GetBytes("123"), Checksums.Adler32);

            Assert.AreEqual(code, code2);
            Assert.AreNotEqual(code2, code3);
            Assert.AreEqual(19726487, code4);
        }

        [TestMethod]
        public void TestUndefiendChecksums()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, (Checksums.Crc32 + 9999));
            });
        }

        [TestMethod]
        public void TestHashPassword()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            var pass = hash.HashPassword("helloworld", 10);
            Assert.AreEqual(true, hash.CheckPassword("helloworld", pass));

            var pass2 = hash.HashPassword("helloworld",8);
            Assert.AreEqual(true, hash.CheckPassword("helloworld", pass2));

            var pass3 = hash.HashPassword(string.Empty, 8);
            Assert.AreEqual(true, hash.CheckPassword(string.Empty, pass3));
        }

        [TestMethod]
        public void TestMurmurHash()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            var hash0 = hash.HashString("helloworld");
            var hash1 = hash.HashString("helloworld", Hashes.MurmurHash);
            var hash2 = hash.HashString("helloworld", Hashes.MurmurHash);
            var hash3 = hash.HashString("helloworl", Hashes.MurmurHash);
            var hash4 = hash.HashByte(System.Text.Encoding.Default.GetBytes("helloworl"));

            Assert.AreEqual(hash1, hash0);
            Assert.AreEqual(hash1, hash2);
            Assert.AreNotEqual(hash2, hash3);
            Assert.AreEqual(hash3, hash4);
        }

        [TestMethod]
        public void TestDjbHash()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            var hash1 = hash.HashString("helloworld", Hashes.Djb);
            var hash2 = hash.HashString("helloworld", Hashes.Djb);
            var hash3 = hash.HashString("helloworl", Hashes.Djb);

            Assert.AreEqual(hash1, hash2);
            Assert.AreNotEqual(hash2, hash3);
        }

        [TestMethod]
        public void TestUndefiendHashes()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                hash.HashString("helloworld", (Hashes.Djb + 9999));
            });
        }
    }
}
