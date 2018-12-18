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

using UnityEditor;
using UnityEngine;

namespace CatLib.Editor
{
    /// <summary>
    /// 框架入口可视化图形界面
    /// </summary>
    [CustomEditor(typeof(Framework), true)]
    public class FrameworkInspector : UnityEditor.Editor
    {
        /// <summary>
        /// 调试等级
        /// </summary>
        private SerializedProperty debugLevel;

        /// <summary>
        /// 绘图界面时
        /// </summary>
        public override void OnInspectorGUI()
        {
            var framework = (Framework)target;
            serializedObject.Update();
            DrawLogo();
            DrawDebugLevels(framework);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 显示时
        /// </summary>
        public void OnEnable()
        {
            debugLevel = serializedObject.FindProperty("DebugLevel");
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        private static void DrawLogo()
        {
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("CatLib Framework ("+ App.Version + ")", EditorStyles.largeLabel,
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
                (int) (DebugLevels) EditorGUILayout.EnumPopup("Debug Level", (DebugLevels) debugLevel.enumValueIndex,
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
    }
}