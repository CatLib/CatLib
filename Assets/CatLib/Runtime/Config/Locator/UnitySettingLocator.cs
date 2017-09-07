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

using CatLib.API.Config;
using UnityEngine;

namespace CatLib.Config.Locator
{
    /// <summary>
    /// Unity设置定位器
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class UnitySettingLocator : IConfigLocator
    {
        /// <summary>
        /// 设定值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置值</param>
        public void Set(string name, string value)
        {
            Guard.NotNull(name, "name");
            PlayerPrefs.SetString(name, value);
        }

        /// <summary>
        /// 根据配置名获取配置的值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置值</param>
        /// <returns>是否获取到配置</returns>
        public bool TryGetValue(string name, out string value)
        {
            Guard.NotNull(name, "name");

            value = string.Empty;
            if (!PlayerPrefs.HasKey(name))
            {
                return false;
            }
            value = PlayerPrefs.GetString(name);
            return true;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}
