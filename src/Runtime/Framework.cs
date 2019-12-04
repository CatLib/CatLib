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

using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// The catlib for unity framework.
    /// </summary>
    [DisallowMultipleComponent]
    [HelpURL("https://catlib.io/lasted")]
    public abstract class Framework : MonoBehaviour
    {
        public DebugLevel DebugLevel = DebugLevel.Production;
        private Application application;

        /// <summary>
        /// Gets a value represents a application instance.
        /// </summary>
        public IApplication Application
        {
            get { return application; }
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            App.That = application = CreateApplication(DebugLevel);
            BeforeBootstrap(application);
            application.Bootstrap(GetBootstraps());
        }

        protected virtual void Start()
        {
            application.Init();
        }

        /// <summary>
        /// Create a new Application instance.
        /// </summary>
        protected virtual Application CreateApplication(DebugLevel debugLevel)
        {
            return new UnityApplication(this)
            {
                DebugLevel = debugLevel
            };
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
        /// Returns an array representing the bootstrap of the framework.
        /// </summary>
        protected virtual IBootstrap[] GetBootstraps()
        {
            return GetComponents<IBootstrap>();
        }

        /// <summary>
        /// Trigged when the framework will destroy.
        /// </summary>
        protected virtual void OnDestroy()
        {
            application?.Terminate();
        }

        /// <summary>
        /// Triggered when the framework is started
        /// </summary>
        protected abstract void OnStartCompleted(IApplication application, StartCompletedEventArgs args);
    }
}
