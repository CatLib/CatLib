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

using CatLib;
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

namespace Game
{
    /// <summary>
    /// 项目注册的服务提供者
    /// </summary>
    public class Providers
    {
        /// <summary>
        /// 项目注册的服务提供者
        /// </summary>
        public static IServiceProvider[] ServiceProviders
        {
            get
            {
                return new IServiceProvider[]
                {
                    // 由框架提供的服务（这些不是必须的）
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

                    // todo: 在此处增加您项目的服务提供者
                };
            }
        }
    }
}