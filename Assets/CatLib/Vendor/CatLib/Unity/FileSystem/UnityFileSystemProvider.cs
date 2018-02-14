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
using System.IO;
using UnityEngine;

namespace CatLib.FileSystem
{
    /// <summary>
    /// Unity 文件系统服务提供者
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("CatLib/FileSystem")]
    public sealed class UnityFileSystemProvider : MonoBehaviour, IServiceProvider, IServiceProviderType
    {
        /// <summary>
        /// 路径类型
        /// </summary>
        public enum PathTypes
        {
            /// <summary>
            /// 数据路径
            /// </summary>
            DataPath,

            /// <summary>
            /// StreamingAssets
            /// </summary>
            StreamingAssets,

            /// <summary>
            /// 可读可写路径
            /// </summary>
            PersistentDataPath,

            /// <summary>
            /// 自动识别
            /// </summary>
            Auto,
        }

        /// <summary>
        /// 默认驱动的名字
        /// </summary>
        public string DefaultDevice = "local";

        /// <summary>
        /// 路径类型
        /// </summary>
        public PathTypes PathType = PathTypes.Auto;

        /// <summary>
        /// 基础服务提供者
        /// </summary>
        private FileSystemProvider baseProvider;

        /// <summary>
        /// 提供者基础类型
        /// </summary>
        public Type BaseType
        {
            get
            {
                return baseProvider.GetType();
            }
        }

        /// <summary>
        /// Unity服务提供者
        /// </summary>
        public void Awake()
        {
            baseProvider = new FileSystemProvider
            {
                DefaultDevice = DefaultDevice,
            };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        [Priority(10)]
        public void Init()
        {
            baseProvider.DefaultPath = GetDefaultPath();
            baseProvider.Init();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        public void Register()
        {
            baseProvider.Register();
        }

        /// <summary>
        /// 默认的路径
        /// <para>不同的调试等级下对应不同的资源路径</para>
        /// <para><c>DebugLevels.Prod</c> : 生产环境下将会为<c>Application.persistentDataPath</c>读写目录</para>
        /// <para><c>DebugLevels.Staging</c> : 仿真环境下将会为<c>StreamingAssets</c>文件夹</para>
        /// <para><c>DebugLevels.Dev</c> : 开发者环境下将会为<c>Application.dataPath</c>数据路径</para>
        /// <para>调试等级无论如何设置，脱离编辑器将自动使用<c>Application.persistentDataPath</c>读写目录</para>
        /// <para>如果开发者有手动设置资源路径，将使用开发者设置的路径</para>
        /// </summary>
        /// <returns>路径</returns>
        private string GetDefaultPath()
        {
            switch (PathType)
            {
                case PathTypes.DataPath:
                    return GetPathWithDebugLevels(DebugLevels.Dev);
                case PathTypes.StreamingAssets:
                    return GetPathWithDebugLevels(DebugLevels.Staging);
                case PathTypes.PersistentDataPath:
                    return GetPathWithDebugLevels(DebugLevels.Prod);
                case PathTypes.Auto:
                default:
                    break;
            }

            if (!UnityEngine.Application.isEditor)
            {
                return UnityEngine.Application.persistentDataPath;
            }

            return GetPathWithDebugLevels(App.Make<DebugLevels>());
        }

        /// <summary>
        /// 根据调试等级获取路径
        /// </summary>
        /// <param name="level">调试等级</param>
        /// <returns>路径</returns>
        private string GetPathWithDebugLevels(DebugLevels level)
        {
            switch (level)
            {
                case DebugLevels.Staging:
                    return Path.Combine(UnityEngine.Application.dataPath, "StreamingAssets");
                case DebugLevels.Dev:
                    return UnityEngine.Application.dataPath;
                case DebugLevels.Prod:
                    return UnityEngine.Application.persistentDataPath;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
