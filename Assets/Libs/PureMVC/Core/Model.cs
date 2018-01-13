﻿//
//  PureMVC C# Multicore
//
//  Copyright(c) 2017 Saad Shams <saad.shams@puremvc.org>
//  Your reuse is governed by the Creative Commons Attribution 3.0 License
//

using System;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Utils;

namespace PureMVC.Core
{
    /// <summary>
    /// A Multiton <c>IModel</c> implementation
    /// </summary>
    /// <remarks>
    ///     <para>In PureMVC, the <c>Model</c> class provides access to model objects (Proxies) by named lookup</para>
    ///     <para>The <c>Model</c> assumes these responsibilities:</para>
    ///     <list type="bullet">
    ///         <item>Maintain a cache of <c>IProxy</c> instances</item>
    ///         <item>Provide methods for registering, retrieving, and removing <c>IProxy</c> instances</item>
    ///     </list>
    ///     <para>
    ///         Your application must register <c>IProxy</c> instances
    ///         with the <c>Model</c>. Typically, you use an 
    ///         <c>ICommand</c> to create and register <c>IProxy</c> 
    ///         instances once the <c>Facade</c> has initialized the Core actors
    ///     </para>
    /// </remarks>
    /// <seealso cref="PureMVC.Patterns.Proxy.Proxy"/>
    /// <seealso cref="PureMVC.Interfaces.IProxy" />
    public class Model: IModel
    {
        /// <summary>
        /// Constructs and initializes a new model
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This <c>IModel</c> implementation is a Multiton, 
        ///         so you should not call the constructor 
        ///         directly, but instead call the static Multiton 
        ///         Factory method <c>Model.getInstance(multitonKey, () => new Model(multitonKey))</c>
        ///     </para>
        /// </remarks>
        /// <param name="key">Key of model</param>
        /// <exception cref="System.Exception">Thrown if instance for this Multiton key has already been constructed</exception>
        public Model(string key)
        {
            if (instanceMap.ContainsKey(key) && multitonKey != null) throw new Exception(MULTITON_MSG);
            multitonKey = key;
            instanceMap.TryAdd(key, new Lazy<IModel>(() => this));
            proxyMap = new Dictionary<string, IProxy>();
            InitializeModel();
        }

        /// <summary>
        /// Initialize the Multiton <c>Model</c> instance.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Called automatically by the constructor, this 
        ///         is your opportunity to initialize the Multiton 
        ///         instance in your subclass without overriding the 
        ///         constructor
        ///     </para>
        /// </remarks>
        protected virtual void InitializeModel()
        {
        }

        /// <summary>
        /// <c>Model</c> Multiton Factory method. 
        /// </summary>
        /// <param name="key">Key of model</param>
        /// <param name="modelClassRef">the <c>FuncDelegate</c> of the <c>IModel</c></param>
        /// <returns>the instance for this Multiton key </returns>
        public static IModel GetInstance(string key, Func<IModel> modelClassRef)
        {
            return instanceMap.GetOrAdd(key, new Lazy<IModel>(modelClassRef)).Value;
        }

        /// <summary>
        /// Register an <c>IProxy</c> with the <c>Model</c>.
        /// </summary>
        /// <param name="proxy">proxy an <c>IProxy</c> to be held by the <c>Model</c>.</param>
        public virtual void RegisterProxy(IProxy proxy)
        {
            proxy.InitializeNotifier(multitonKey);
            proxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
        }

        /// <summary>
        /// Retrieve an <c>IProxy</c> from the <c>Model</c>.
        /// </summary>
        /// <param name="proxyName"></param>
        /// <returns>the <c>IProxy</c> instance previously registered with the given <c>proxyName</c>.</returns>
        public virtual IProxy RetrieveProxy(string proxyName)
        {
            IProxy proxy;
            return proxyMap.TryGetValue(proxyName, out proxy) ? proxy : null;
        }

        /// <summary>
        /// Remove an <c>IProxy</c> from the <c>Model</c>.
        /// </summary>
        /// <param name="proxyName">proxyName name of the <c>IProxy</c> instance to be removed.</param>
        /// <returns>the <c>IProxy</c> that was removed from the <c>Model</c></returns>
        public virtual IProxy RemoveProxy(string proxyName)
        {
            IProxy proxy;
            if (proxyMap.TryRemove(proxyName, out proxy))
            {
                proxy.OnRemove();
            }
            return proxy;
        }

        /// <summary>
        /// Check if a Proxy is registered
        /// </summary>
        /// <param name="proxyName"></param>
        /// <returns>whether a Proxy is currently registered with the given <c>proxyName</c>.</returns>
        public virtual bool HasProxy(string proxyName)
        {
            return proxyMap.ContainsKey(proxyName);
        }

        /// <summary>
        /// Remove an IModel instance
        /// </summary>
        /// <param name="key">multitonKey of IModel instance to remove</param>
        public static void RemoveModel(string key)
        {
            instanceMap.TryRemove(key);
        }

        /// <summary>The Multiton Key for this Core</summary>
        protected string multitonKey;

        /// <summary>Mapping of proxyNames to IProxy instances</summary>
        protected readonly Dictionary<string, IProxy> proxyMap;

        /// <summary>The Multiton Model instanceMap.</summary>
        protected static readonly Dictionary<string, Lazy<IModel>> instanceMap = new Dictionary<string, Lazy<IModel>>();

        /// <summary>Message Constants</summary>
        protected const string MULTITON_MSG = "Model instance for this Multiton key already constructed!";
    }
}
