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
using UnityEngine;

namespace CatLib.Encryption
{
    /// <summary>
    /// 加解密服务提供者
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("CatLib/Encryption")]
    public sealed class UnityEncryptionProvider : MonoBehaviour, IServiceProvider, IServiceProviderType
    {
        /// <summary>
        /// 加密类型
        /// </summary>
        public enum CipherTypes
        {
            /// <summary>
            /// AES 128 CBC
            /// </summary>
            Aes128Cbc,

            /// <summary>
            /// AES 256 CBC
            /// </summary>
            Aes256Cbc,
        }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Key = "1234567890123456";

        /// <summary>
        /// 加密类型
        /// </summary>
        public CipherTypes Cipher = CipherTypes.Aes128Cbc;

        /// <summary>
        /// 基础服务提供者
        /// </summary>
        private EncryptionProvider baseProvider;

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
            baseProvider = new EncryptionProvider
            {
                Key = Key,
                Cipher = (Cipher == CipherTypes.Aes128Cbc) ? "AES-128-CBC" : "AES-256-CBC"
            };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            baseProvider.Init();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        public void Register()
        {
            baseProvider.Register();
        }
    }
}