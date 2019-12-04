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

using CatLib.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CatLib.Editor
{
    /// <summary>
    /// Visual graphical interface for <see cref="Framework"/>
    /// </summary>
    [CustomEditor(typeof(Framework), true)]
    public class InspectorFramework : UnityEditor.Editor
    {
        private SerializedProperty debugLevel;
        private readonly Dictionary<Type, Component> serviceProviders = new Dictionary<Type, Component>();
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

        public override void OnInspectorGUI()
        {
            var framework = (Framework)target;
            serializedObject.Update();
            DrawLogo();
            DrawDebugLevel(framework);
            DrawServiceProvider(framework, serviceProviders);
            serializedObject.ApplyModifiedProperties();
        }

        public void OnEnable()
        {
            debugLevel = serializedObject.FindProperty("DebugLevel");
            RefreshServiceProviders();
        }

        private static void DrawLogo()
        {
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("CatLib Framework (" + Application.Version + ")", EditorStyles.largeLabel,
                    GUILayout.Height(20));
            GUILayout.EndHorizontal();
        }

        private void DrawDebugLevel(Framework framework)
        {
            var old = debugLevel.enumValueIndex;
            GUILayout.BeginHorizontal();
            debugLevel.enumValueIndex =
                (int)(DebugLevel)EditorGUILayout.EnumPopup("Debug Level", (DebugLevel)debugLevel.enumValueIndex,
                    EditorStyles.popup);
            GUILayout.EndHorizontal();

            if (old == debugLevel.enumValueIndex)
            {
                return;
            }

            if (framework.Application != null)
            {
                framework.Application.DebugLevel = (DebugLevel)debugLevel.enumValueIndex;
            }
        }

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
        /// Draw service provider switch
        /// </summary>
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
                "The " + providerType.Name + " will be Deleted" + Environment.NewLine + "Configuration will be lost", "Delete", "Cancel"))
            {
                return true;
            }

            DestroyImmediate(master.gameObject);
            return false;
        }

        private static void CreateServiceProvider(Type providerType, GameObject root)
        {
            var go = new GameObject(providerType.Name);
            go.transform.parent = root.transform;
            go.AddComponent(providerType);
        }

        private void RefreshServiceProviders()
        {
            serviceProviders.Clear();
            LoadAllServiceProviders(serviceProviders);
            LoadInstalledServiceProviders(serviceProviders, (Component)target);
        }

        private static void LoadInstalledServiceProviders(IDictionary<Type, Component> result, Component target)
        {
            foreach (var serviceProvider in target.gameObject.GetComponentsInChildren<IServiceProvider>())
            {
                result[serviceProvider.GetType()] = (Component)serviceProvider;
            }
        }

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