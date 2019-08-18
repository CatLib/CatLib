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

using CatLib;
using CatLib.Editor;
using CatLib.Util;
using UnityEngine;

namespace Demo.Editor
{
    /// <summary>
    /// Main project entrance for editor.
    /// </summary>
    public class EditorMain : EditorFramework
    {
        /// <inheritdoc />
        protected override void OnStartCompleted(IApplication application, StartCompletedEventArgs args)
        {
            Debug.Log("Hello Editor CatLib.");
            base.OnStartCompleted(application, args);
        }

        /// <inheritdoc />
        protected override IBootstrap[] GetBootstraps()
        {
            return Arr.Merge(base.GetBootstraps(), EditorBootstraps.GetBoostraps(null));
        }
    }
}