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
using System.Text;
using CatLib.API.Config;
using CatLib.API.Encryption;
using CatLib.Config;
using CatLib.Converters;
using CatLib.Encryption;
using CatLib.Events;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Encryption
{
    [TestClass]
    public class EncryptionProviderTests
    {
        public IApplication MakeEnv(Action then = null)
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new EventsProvider());
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            app.Register(new EncryptionProvider());

            if (then != null)
            {
                then.Invoke();
            }
            app.Init();
            return app;
        }

        [TestMethod]
        public void TestDefaultEncryption()
        {
            var app = MakeEnv(() =>
            {
                var config = App.Make<IConfig>();
                config.SafeSet("EncryptionProvider.Key", "0123456789123456");
            });

            var encrypter = app.Make<IEncrypter>();

            var code = encrypter.Encrypt(Encoding.Default.GetBytes("helloworld"));
            Assert.AreNotEqual("helloworld", code);
            Assert.AreEqual("helloworld", Encoding.Default.GetString(encrypter.Decrypt(code)));
        }

        [TestMethod]
        public void Test256Encryption()
        {
            var app = MakeEnv(() =>
            {
                var config = App.Make<IConfig>();
                config.SafeSet("EncryptionProvider.Key", "01234567891234560123456789123456");
                config.SafeSet("EncryptionProvider.Cipher", "AES-256-CBC");
            });

            var encrypter = app.Make<IEncrypter>();

            var code = encrypter.Encrypt(Encoding.Default.GetBytes("helloworld"));
            Assert.AreNotEqual("helloworld", code);
            Assert.AreEqual("helloworld", Encoding.Default.GetString(encrypter.Decrypt(code)));
        }

        [TestMethod]
        public void TestEncryptionFaild()
        {
            var app = MakeEnv(() =>
            {
                var config = App.Make<IConfig>();
                config.SafeSet("EncryptionProvider.Key", "0123456789123456");
            });

            var encrypter = app.Make<IEncrypter>();

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                encrypter.Decrypt(null);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                encrypter.Decrypt("");
            });

            ExceptionAssert.Throws<EncryptionException>(() =>
            {
                encrypter.Decrypt("123213");
            });

            ExceptionAssert.Throws<EncryptionException>(() =>
            {
                encrypter.Decrypt("123213:123123,123:123,213:123:123");
            });
        }

        [TestMethod]
        public void TestEmptyKey()
        {
            var app = MakeEnv();
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Make<IEncrypter>();
            });
        }

        [TestMethod]
        public void TestNotSupportedKey()
        {
            var app = MakeEnv(() =>
            {
                var config = App.Make<IConfig>();
                config.SafeSet("EncryptionProvider.Key", "01234567891");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Make<IEncrypter>();
            });
        }

        [TestMethod]
        public void TestModifyData()
        {
            var app = MakeEnv(() =>
            {
                var config = App.Make<IConfig>();
                config.SafeSet("EncryptionProvider.Key", "0123456789123456");
            });

            var encrypter = app.Make<IEncrypter>();

            var code = encrypter.Encrypt(Encoding.Default.GetBytes("helloworld"));
            code = code.Replace("7", "8");
            code = code.Replace("5", "6");
            code = code.Replace("3", "4");
            code = code.Replace("1", "2");
            code = code.Replace("z", "0");
            code = code.Replace("x", "y");
            code = code.Replace("v", "w");
            code = code.Replace("t", "u");
            code = code.Replace("r", "s");
            code = code.Replace("p", "q");
            code = code.Replace("n", "o");
            code = code.Replace("k", "l");
            code = code.Replace("i", "j");
            code = code.Replace("g", "h");
            code = code.Replace("e", "f");
            code = code.Replace("c", "D");
            code = code.Replace("a", "B");
            code = code.Replace("M", "a");
            ExceptionAssert.Throws<EncryptionException>(() =>
            {
                encrypter.Decrypt(code);
            });
        }

        [TestMethod]
        public void TestEmptyByteData()
        {
            var app = MakeEnv(() =>
            {
                var config = App.Make<IConfig>();
                config.SafeSet("EncryptionProvider.Key", "0123456789123456");
            });

            var encrypter = app.Make<IEncrypter>();

            var code = encrypter.Encrypt(new byte[] { });
            Assert.AreEqual(0, encrypter.Decrypt(code).Length);
        }
    }
}
