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
using System.Reflection;
using UnityEditor;

namespace CatLib.Editor
{
    [InitializeOnLoad]
    public class EditorFramework
    {
        private static Application editorApplication;
        private static readonly string[] checkInAssembiles = new string[]
        {
            "*-Editor",
            "*.Editor",
        };

        static EditorFramework()
        {
            GC.Collect();
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                UnityEngine.Application.quitting += Quitted;
                return;
            }
            ApplyEditorApplication();
        }

        ~EditorFramework()
        {
            OnDestroy();
        }

        public IApplication Application
        {
            get { return editorApplication; }
        }

        private static void Quitted()
        {
            ApplyEditorApplication();
        }

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

        private static void ApplyEditorApplication()
        {
            var editorFramework = GetEditorFramework();
            App.That = editorApplication = editorFramework.CreateApplication();
            editorFramework.BeforeBootstrap(editorApplication);
            editorApplication.Bootstrap(editorFramework.GetBootstraps());
            editorApplication.Init();
        }

        /// <summary>
        /// Trigged before booting.
        /// </summary>
        protected virtual void BeforeBootstrap(IApplication application)
        {
            application.GetDispatcher()?.AddListener(ApplicationEvents.OnStartCompleted, (sender, args) =>
            {
                OnStartCompleted((IApplication)sender, (StartCompletedEventArgs)args);
            });
        }

        /// <summary>
        /// Triggered when the framework is started
        /// </summary>
        protected virtual void OnStartCompleted(IApplication application, StartCompletedEventArgs args)
        {
            // noop.
        }

        /// <summary>
        /// Create a new Application instance.
        /// </summary>
        protected virtual Application CreateApplication()
        {
            return new UnityApplication(null);
        }

        /// <summary>
        /// Returns an array representing the bootstrap of the framework.
        /// </summary>
        protected virtual IBootstrap[] GetBootstraps()
        {
            return Array.Empty<IBootstrap>();
        }

        /// <summary>
        /// Trigged when the framework will destroy.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Application != null)
            {
                Application.Terminate();
            }
        }
    }
}
