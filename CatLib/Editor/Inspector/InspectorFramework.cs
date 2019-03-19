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
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CatLib.Editor
{
    /// <summary>
    /// 框架入口可视化图形界面
    /// </summary>
    [CustomEditor(typeof(Framework), true)]
    public class InspectorFramework : UnityEditor.Editor
    {
        /// <summary>
        /// 调试等级
        /// </summary>
        private SerializedProperty debugLevel;

        /// <summary>
        /// 已经安装了的服务提供者列表
        /// </summary>
        private readonly Dictionary<Type, Component> serviceProviders = new Dictionary<Type, Component>();

        /// <summary>
        /// 绘图界面时
        /// </summary>
        public override void OnInspectorGUI()
        {
            var framework = (Framework)target;
            serializedObject.Update();
            DrawLogo();
            DrawDebugLevels(framework);
            DrawServiceProvider(framework, serviceProviders);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 显示时
        /// </summary>
        public void OnEnable()
        {
            debugLevel = serializedObject.FindProperty("DebugLevel");
            RefreshServiceProviders();
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        private static void DrawLogo()
        {
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("CatLib Framework (" + App.Version + ")", EditorStyles.largeLabel,
                GUILayout.Height(20));
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制调试等级
        /// </summary>
        private void DrawDebugLevels(Framework framework)
        {
            var old = debugLevel.enumValueIndex;
            GUILayout.BeginHorizontal();
            debugLevel.enumValueIndex =
                (int)(DebugLevels)EditorGUILayout.EnumPopup("Debug Level", (DebugLevels)debugLevel.enumValueIndex,
                    EditorStyles.popup);
            GUILayout.EndHorizontal();

            if (old == debugLevel.enumValueIndex)
            {
                return;
            }

            if (framework.Application != null)
            {
                framework.Application.DebugLevel = (DebugLevels)debugLevel.enumValueIndex;
            }
        }

        /// <summary>
        /// 绘制服务提供者
        /// </summary>
        /// <param name="root">根节点信息</param>
        /// <param name="serviceProviders">服务提供者列表</param>
        private void DrawServiceProvider(Component root, IDictionary<Type, Component> serviceProviders)
        {
            GUILayout.Space(5);

            if (EditorApplication.isPlaying)
            {
                InspectorTool.LabelBox("GUI Service Providers:", () =>
                {
                    EditorGUILayout.HelpBox("Only be modified if it stops running", MessageType.Info);
                });
                return;
            }

            var reload = false;
            InspectorTool.LabelBox("GUI Service Providers:", () =>
            {
                if (serviceProviders.Count <= 0)
                {
                    EditorGUILayout.HelpBox("No optional service provider", MessageType.Info);
                    return;
                }

                foreach (var providerAndGameObject in serviceProviders)
                {
                    var enable = providerAndGameObject.Value != null;
                    InspectorTool.Horizontal(() =>
                    {
                        reload = ToggleProvider(root.gameObject, providerAndGameObject.Key,
                                     providerAndGameObject.Value) != enable || reload;

                        if (InspectorTool.Button("Go", "Goto The GameObject", enable, 40,
                                16) && enable)
                        {
                            Selection.activeObject = providerAndGameObject.Value;
                        }
                    });
                }
            });

            if (reload)
            {
                RefreshServiceProviders();
            }
        }

        /// <summary>
        /// 服务提供者开关
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="providerType">服务提供者类型</param>
        /// <param name="master">宿主对象</param>
        private bool ToggleProvider(GameObject root, Type providerType, Component master)
        {
            if (EditorGUILayout.ToggleLeft(providerType.Name, master != null))
            {
                if (master == null)
                {
                    CreateServiceProvider(providerType, root);
                }
                return true;
            }

            if (master == null)
            {
                return false;
            }

            if (root == master.gameObject)
            {
                EditorUtility.DisplayDialog("Failure",
                    providerType.Name + " is located in the main game object, please manually delete the script", "Ok");
                return true;
            }

            if (!EditorUtility.DisplayDialog("Deleting",
                "The " + providerType.Name + " will be Deleted"+ Environment.NewLine + "Configuration will be lost", "Delete", "Cancel"))
            {
                return true;
            }

            DestroyImmediate(master.gameObject);
            return false;
        }

        /// <summary>
        /// 创建服务提供者
        /// </summary>
        /// <param name="root">根节点</param>
        /// <param name="providerType">服务提供者类型</param>
        private static void CreateServiceProvider(Type providerType, GameObject root)
        {
            var go = new GameObject(providerType.Name);
            go.transform.parent = root.transform;
            go.AddComponent(providerType);
        }

        /// <summary>
        /// 刷新服务提供者信息
        /// </summary>
        private void RefreshServiceProviders()
        {
            serviceProviders.Clear();
            LoadAllServiceProviders(serviceProviders);
            LoadInstalledServiceProviders(serviceProviders, (Component)target);
        }

        /// <summary>
        /// 获取已经安装了的服务提供者
        /// </summary>
        /// <param name="result">结果集</param>
        /// <param name="target">扫描的根组件</param>
        /// <returns>安装关系字典</returns>
        private static void LoadInstalledServiceProviders(IDictionary<Type, Component> result, Component target)
        {
            foreach (var serviceProvider in target.gameObject.GetComponentsInChildren<IServiceProvider>())
            {
                result[serviceProvider.GetType()] = (Component)serviceProvider;
            }
        }

        /// <summary>
        /// 获取GUI支持的服务提供者
        /// </summary>
        /// <returns>编辑器框架</returns>
        private static void LoadAllServiceProviders(IDictionary<Type, Component> result)
        {
            var targetProvider = typeof(IServiceProvider);
            var targetComponent = typeof(Component);
            var assembiles = Arr.Filter(AppDomain.CurrentDomain.GetAssemblies(), TestCheckInAssembiles);

            foreach (var assembly in assembiles)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (targetProvider.IsAssignableFrom(type)
                        && targetComponent.IsAssignableFrom(type))
                    {
                        result.Add(type, null);
                    }
                }
            }
        }

        /// <summary>
        /// 需要忽略的程序集
        /// </summary>
        private static readonly string[] ignoreAssembiles = new string[]
        {
            "mscorlib",
            "UnityEngine",
            "UnityEditor",
            "netstandard",
            "nunit.framework",
            "System",
            "ExCSS.Unity",
            "CatLib.Core",
            "UnityEditor.*",
            "UnityEngine.*",
            "*-Editor",
            "*.Editor",
            "System.*",
            "Mono.*",
            "Unity.*",
            "SyntaxTree.*",
            "Microsoft.*",
        };

        /// <summary>
        /// 测试是否处于需要检查的程序集列表
        /// </summary>
        /// <param name="assembly">测试程序集</param>
        /// <returns>是否处于忽略程序集列表</returns>
        private static bool TestCheckInAssembiles(Assembly assembly)
        {
            foreach (var pattern in ignoreAssembiles)
            {
                if (Str.Is(pattern, assembly.GetName().Name))
                {
                    return false;
                }
            }
            return true;
        }
    }
}