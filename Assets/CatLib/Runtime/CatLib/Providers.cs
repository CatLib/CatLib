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

using CatLib.Compress;
using CatLib.Debugger;
using CatLib.Encryption;
using CatLib.FileSystem;
using CatLib.Hashing;
using CatLib.Json;
using CatLib.MonoDriver;
using CatLib.Network;
using CatLib.Random;
using CatLib.Routing;
using CatLib.Socket;
using CatLib.Tick;
using CatLib.Time;
using CatLib.Timer;
using CatLib.Translation;

namespace CatLib
{
    /// <summary>
    /// 框架默认的服务提供者
    /// <para>这里的提供者在框架启动时必定会被加载</para>
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Providers
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        public static IServiceProvider[] ServiceProviders
        {
            get
            {
                return new IServiceProvider[]
                {
                    new CompressProvider(),
                    new DebuggerProvider(),
                    new EncryptionProvider(),
                    new FileSystemProvider(),
                    new HashingProvider(),
                    new JsonProvider(),
                    new MonoDriverProvider(),
                    new RoutingProvider(),
                    new TimeProvider(),
                    new TimerProvider(),
                    new TranslationProvider(),
                    new RandomProvider(), 
                    new NetworkProvider(), 
                    new SocketProvider(), 
                    new TickProvider(), 
                };
            }
        }
    }
}
