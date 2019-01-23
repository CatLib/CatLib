/*
 * This file is part of the CatLib package.
 *
 * (c) CatLib <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.Reflection;
using UnityEditor;

namespace CatLib.Editor
{
    /// <summary>
    /// CatLib For Unity Editor
    /// </summary>
    [InitializeOnLoad]
    public class EditorFramework
    {
        /// <summary>
        /// 编辑器层的框架实例
        /// </summary>
        private static Application editorApplication;

        /// <summary>
        /// 创建一个新的CatLib For Unity Editor实例
        /// </summary>
        static EditorFramework()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                UnityEngine.Application.quitting += Quitted;
                return;
            }

            CreateEditorApplication();
        }

        /// <summary>
        /// 程序需要退出
        /// </summary>
        private static void Quitted()
        {
            CreateEditorApplication();
        }

        /// <summary>
        /// 获取编辑器框架
        /// </summary>
        /// <returns>编辑器框架</returns>
        private static EditorFramework GetEditorFramework()
        {
            var target = typeof(EditorFramework);
            var assembiles = Arr.Filter(AppDomain.CurrentDomain.GetAssemblies(), TestCheckInAssembiles);
            foreach (var assembly in assembiles)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type != target && target.IsAssignableFrom(type)
                        && type.IsClass && !type.IsAbstract)
                    {
                        return (EditorFramework)Activator.CreateInstance(type);
                    }
                }
            }
            return new EditorFramework();
        }

        /// <summary>
        /// 需要检查的程序集
        /// </summary>
        private static readonly string[] checkInAssembiles = new string[]
        {
            "*-Editor",
            "*.Editor",
        };

        /// <summary>
        /// 测试是否处于需要检查的程序集列表
        /// </summary>
        /// <param name="assembly">测试程序集</param>
        /// <returns>是否处于忽略程序集列表</returns>
        private static bool TestCheckInAssembiles(Assembly assembly)
        {
            foreach (var pattern in checkInAssembiles)
            {
                if (Str.Is(pattern, assembly.GetName().Name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建编辑器应用程序
        /// </summary>
        private static void CreateEditorApplication()
        {
            var editorFramework = GetEditorFramework();
            editorApplication = editorFramework.CreateApplication();
            editorApplication.Bootstrap(editorFramework.GetBootstraps());
            editorApplication.Init();
        }

        /// <summary>
        /// CatLib Unity Framework
        /// </summary>
        public IApplication Application
        {
            get { return editorApplication; }
        }

        /// <summary>
        /// 创建框架实例
        /// </summary>
        /// <returns>创建框架实例</returns>
        protected virtual Application CreateApplication()
        {
            return new Application();
        }

        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        protected virtual IBootstrap[] GetBootstraps()
        {
            return Bootstraps.Bootstrap;
        }
    }
}