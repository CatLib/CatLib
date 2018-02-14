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

using UnityEngine;

namespace CatLib.MonoDriver
{
    /// <summary>
    /// 驱动脚本
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class DriverBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 驱动器
        /// </summary>
        private MonoDriver driver;

        /// <summary>
        /// Awake
        /// </summary>
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 设定驱动器
        /// </summary>
        /// <param name="driver">驱动器</param>
        public void SetDriver(MonoDriver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// 每帧更新时
        /// </summary>
        public void Update()
        {
            if (driver != null)
            {
                driver.Update();
            }
        }

        /// <summary>
        /// 在每帧更新时之后
        /// </summary>
        public void LateUpdate()
        {
            if (driver != null)
            {
                driver.LateUpdate();
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            if (driver != null)
            {
                driver.OnDestroy();
            }
        }

        /// <summary>
        /// 固定刷新
        /// </summary>
        public void FixedUpdate()
        {
            if (driver != null)
            {
                driver.FixedUpdate();
            }
        }

        /// <summary>
        /// 当绘制GUI时
        /// </summary>
        public void OnGUI()
        {
            if (driver != null)
            {
                driver.OnGUI();
            }
        }
    }
}