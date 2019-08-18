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
using UnityEditor;
using UnityEngine;

namespace CatLib.Editor
{
    public static class InspectorTool
    {
        private static GUIStyle guiStyle;

        public static GUIStyle GUIStyle
        {
            get { return guiStyle = guiStyle ?? GUI.skin.FindStyle("box"); }
            set { guiStyle = value; }
        }

        /// <summary>
        /// Draw a horizontal content.
        /// </summary>
        public static void Horizontal(Action closure)
        {
            EditorGUILayout.BeginHorizontal(GUIStyle);
            try
            {
                closure();
            }
            finally
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Draw a vertical content.
        /// </summary>
        public static void Vertical(Action closure)
        {
            EditorGUILayout.BeginVertical(GUIStyle);
            try
            {
                closure();
            }
            finally
            {
                EditorGUILayout.EndVertical();
            }
        }

        public static void LabelBox(string title, Action codeBlock)
        {
            Vertical(() =>
            {
                GUILayout.Label(title);
                Vertical(codeBlock);
            });
        }

        /// <summary>
        /// Drawing a folding box.
        /// </summary>
        public static bool ToggleBox(bool visiable, string title, Action codeBlock)
        {
            Vertical(() =>
            {
                visiable = GUILayout.Toggle(visiable, title, EditorStyles.foldout);
                if (!visiable)
                {
                    EditorGUILayout.EndVertical();
                }
                Vertical(codeBlock);
            });
            return visiable;
        }

        /// <summary>
        /// Drawing a button.
        /// </summary>
        public static bool Button(string title, string tooltip, bool enabled, float width = -1, float height = -1,
            GUIStyle style = null)
        {
            var widthOptions = (width <= 0) ? GUILayout.ExpandWidth(true) : GUILayout.Width(width);
            var heightOptions = (height <= 0) ? GUILayout.ExpandHeight(true) : GUILayout.Height(height);
            style = style ?? EditorStyles.miniButton;
            if (enabled)
            {
                return GUILayout.Button(new GUIContent(title, tooltip), style, widthOptions, heightOptions);
            }

            return (bool)ApplyColor(
                () => GUILayout.Button(new GUIContent(title, tooltip), style, widthOptions,
                    heightOptions), new Color(1f, 1f, 1f, 0.25f));
        }

        public static object ApplyColor(Func<object> action, Color color)
        {
            return ApplyColor(action, color, GUI.contentColor);
        }

        public static object ApplyColor(Func<object> action, Color color, Color contentColor)
        {
            var backup = GUI.color;
            var backupContent = GUI.contentColor;
            try
            {
                GUI.color = color;
                GUI.contentColor = contentColor;
                return action();
            }
            finally
            {
                GUI.color = backup;
                GUI.contentColor = backupContent;
            }
        }
    }
}