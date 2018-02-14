/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this sender code.
 *
 * Document: http://catlib.io/
 */

using CatLib.API.MonoDriver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace CatLib.MonoDriver
{
    /// <summary>
    /// Mono驱动器
    /// </summary>
    internal sealed class MonoDriver : IMonoDriver
    {
        /// <summary>
        /// 更新
        /// </summary>
        private readonly SortSet<IUpdate, int> update = new SortSet<IUpdate, int>();

        /// <summary>
        /// 延后更新
        /// </summary>
        private readonly SortSet<ILateUpdate, int> lateUpdate = new SortSet<ILateUpdate, int>();

        /// <summary>
        /// 固定更新
        /// </summary>
        private readonly SortSet<IFixedUpdate, int> fixedUpdate = new SortSet<IFixedUpdate, int>();

        /// <summary>
        /// GUI绘制
        /// </summary>
        private readonly SortSet<IOnGUI, int> onGui = new SortSet<IOnGUI, int>();

        /// <summary>
        /// 释放时需要调用的
        /// </summary>
        private readonly SortSet<IOnDestroy, int> destroy = new SortSet<IOnDestroy, int>();

        /// <summary>
        /// 载入结果集
        /// </summary>
        private readonly HashSet<object> loadSet = new HashSet<object>();

        /// <summary>
        /// 应用程序
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// Mono驱动程序
        /// </summary>
        private DriverBehaviour behaviour;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// 主线程调度队列
        /// </summary>
        private readonly Queue<Action> mainThreadDispatcherQueue = new Queue<Action>();

        /// <summary>
        /// 主线程ID
        /// </summary>
        private readonly int mainThreadId;

        /// <summary>
        /// 是否是主线程
        /// </summary>
        public bool IsMainThread
        {
            get
            {
                return mainThreadId == Thread.CurrentThread.ManagedThreadId;
            }
        }

        /// <summary>
        /// 调度器
        /// </summary>
        private readonly IDispatcher dispatcher;

        /// <summary>
        /// 构建一个Mono驱动器
        /// </summary>
        /// <param name="container">容器</param>、
        /// <param name="dispatcher">全局调度器</param>
        /// <param name="component">组件</param>
        public MonoDriver(IContainer container, IDispatcher dispatcher, Component component = null)
        {
            Guard.Requires<ArgumentNullException>(container != null);
            Guard.Requires<ArgumentNullException>(dispatcher != null);
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            this.container = container;
            this.dispatcher = dispatcher;
            container.OnResolving(DefaultOnResolving);
            container.OnRelease(DefaultOnRelease);
            if (component != null)
            {
                InitComponent(component);
            }
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        /// <param name="component">Unity组件</param>
        private void InitComponent(Component component)
        {
            behaviour = component.gameObject.AddComponent<DriverBehaviour>();
            behaviour.SetDriver(this);
        }

        /// <summary>
        /// 默认的解决事件
        /// </summary>
        /// <param name="binder">绑定数据</param>
        /// <param name="obj">对象</param>
        /// <returns>处理后的对象</returns>
        private object DefaultOnResolving(IBindData binder, object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (binder.IsStatic)
            {
                Attach(obj);
            }

            return obj;
        }

        /// <summary>
        /// 默认的释放事件
        /// </summary>
        /// <param name="binder">绑定数据</param>
        /// <param name="obj">对象</param>
        private void DefaultOnRelease(IBindData binder, object obj)
        {
            if (obj == null)
            {
                return;
            }

            if (binder.IsStatic)
            {
                Detach(obj);
            }
        }

        /// <summary>
        /// 从驱动器中卸载对象
        /// 如果对象使用了增强接口，那么卸载对应增强接口
        /// 从驱动器中卸载对象会引发IDestroy增强接口
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="ArgumentNullException">当卸载对象为<c>null</c>时引发</exception>
        public void Detach(object obj)
        {
            Guard.Requires<ArgumentNullException>(obj != null);

            if (!loadSet.Contains(obj))
            {
                return;
            }

            ConvertAndRemove(update, obj);
            ConvertAndRemove(lateUpdate, obj);
            ConvertAndRemove(fixedUpdate, obj);
            ConvertAndRemove(onGui, obj);

            if (ConvertAndRemove(destroy, obj))
            {
                ((IOnDestroy)obj).OnDestroy();
            }

            loadSet.Remove(obj);
        }

        /// <summary>
        /// 如果对象实现了增强接口那么将对象装载进对应驱动器
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="ArgumentNullException">当装载对象为<c>null</c>时引发</exception>
        public void Attach(object obj)
        {
            Guard.Requires<ArgumentNullException>(obj != null);

            if (loadSet.Contains(obj))
            {
                throw new RuntimeException("Object [" + obj + "] is already load.");
            }

            var isLoad = ConvertAndAdd(update, obj, "Update");
            isLoad = ConvertAndAdd(lateUpdate, obj, "LateUpdate") || isLoad;
            isLoad = ConvertAndAdd(fixedUpdate, obj, "FixedUpdate") || isLoad;
            isLoad = ConvertAndAdd(onGui, obj, "OnGUI") || isLoad;
            isLoad = ConvertAndAdd(destroy, obj, "OnDestroy") || isLoad;

            if (isLoad)
            {
                loadSet.Add(obj);
            }
        }

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">代码块执行会处于主线程</param>
        public void MainThread(IEnumerator action)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            if (IsMainThread)
            {
                StartCoroutine(action);
                return;
            }
            lock (syncRoot)
            {
                mainThreadDispatcherQueue.Enqueue(() =>
                {
                    StartCoroutine(action);
                });
            }
        }

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">代码块执行会处于主线程</param>
        public void MainThread(Action action)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            if (IsMainThread)
            {
                action.Invoke();
                return;
            }
            MainThread(ActionWrapper(action));
        }

        /// <summary>
        /// 包装器
        /// </summary>
        /// <param name="action">回调函数</param>
        /// <returns>迭代器</returns>
        private IEnumerator ActionWrapper(Action action)
        {
            action.Invoke();
            yield return null;
        }

        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="routine">协程内容</param>
        /// <returns>协程</returns>
        /// <exception cref="ArgumentNullException">当<paramref name="routine"/>为<c>null</c>时引发</exception>
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            Guard.Requires<ArgumentNullException>(routine != null);
            if (behaviour == null)
            {
                while (routine.MoveNext())
                {
                    var current = routine.Current as IEnumerator;
                    if (current != null)
                    {
                        StartCoroutine(current);
                    }
                }
                return null;
            }
            return behaviour.StartCoroutine(routine);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        /// <exception cref="ArgumentNullException">当<paramref name="routine"/>为<c>null</c>时引发</exception>
        public void StopCoroutine(IEnumerator routine)
        {
            if (behaviour == null)
            {
                return;
            }
            Guard.Requires<ArgumentNullException>(routine != null);
            behaviour.StopCoroutine(routine);
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            foreach (var current in update)
            {
                current.Update();
            }
            lock (syncRoot)
            {
                while (mainThreadDispatcherQueue.Count > 0)
                {
                    mainThreadDispatcherQueue.Dequeue().Invoke();
                }
            }
        }

        /// <summary>
        /// 每帧更新后
        /// </summary>
        public void LateUpdate()
        {
            foreach (var current in lateUpdate)
            {
                current.LateUpdate();
            }
        }

        /// <summary>
        /// 固定刷新
        /// </summary>
        public void FixedUpdate()
        {
            foreach (var current in fixedUpdate)
            {
                current.FixedUpdate();
            }
        }

        /// <summary>
        /// GUI绘制时
        /// </summary>
        public void OnGUI()
        {
            foreach (var current in onGui)
            {
                current.OnGUI();
            }
        }

        /// <summary>
        /// 当释放时
        /// </summary>
        public void OnDestroy()
        {
            dispatcher.Trigger(MonoDriverEvents.OnBeforeDestroy);

            container.Flush();
            foreach (var current in destroy)
            {
                current.OnDestroy();
            }

            update.Clear();
            lateUpdate.Clear();
            fixedUpdate.Clear();
            onGui.Clear();
            destroy.Clear();
            loadSet.Clear();

            dispatcher.Trigger(MonoDriverEvents.OnDestroyed);
        }

        /// <summary>
        /// 转换到指定目标并且删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sortset">有序集</param>
        /// <param name="obj">对象</param>
        /// <returns>是否成功</returns>
        private bool ConvertAndRemove<T>(ISortSet<T, int> sortset, object obj) where T : class
        {
            var ele = obj as T;
            return ele != null && sortset.Remove(ele);
        }

        /// <summary>
        /// 转换到指定目标并且添加
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sortset">有序集</param>
        /// <param name="obj">对象</param>
        /// <param name="function">获取优先级的函数名</param>
        /// <returns>是否成功</returns>
        private bool ConvertAndAdd<T>(ISortSet<T, int> sortset, object obj, string function) where T : class
        {
            var ele = obj as T;
            if (ele == null)
            {
                return false;
            }
            sortset.Add(ele, GetPriorities(obj.GetType(), function));
            return true;
        }

        /// <summary>
        /// 获取优先级
        /// </summary>
        /// <param name="type">识别的类型</param>
        /// <param name="method">识别的方法</param>
        /// <returns>优先级</returns>
        private int GetPriorities(Type type, string method = null)
        {
            return Util.GetPriority(type, method);
        }
    }
}
