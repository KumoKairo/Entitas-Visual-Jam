using System;
using System.Collections.Generic;
using System.Text;
using Entitas.Utils;

namespace Entitas
{
    /// Use context.CreateEntity() to create a new entity and
    /// entity.Destroy() to destroy it.
    /// You can add, replace and remove IComponent to an entity.
    public class Entity : IEntity
    {
        /// Occurs when a component gets added.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityComponentChanged OnComponentAdded;

        /// Occurs when a component gets removed.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityComponentChanged OnComponentRemoved;

        /// Occurs when a component gets replaced.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityComponentReplaced OnComponentReplaced;

        /// Occurs when an entity gets released and is not retained anymore.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityEvent OnEntityReleased;

        /// Occurs when calling entity.Destroy().
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        public event EntityEvent OnDestroyEntity;

        /// The total amount of components an entity can possibly have.
        public int totalComponents { get { return _totalComponents; } }

        /// Each entity has its own unique creationIndex which will be set by
        /// the context when you create the entity.
        public int creationIndex { get { return _creationIndex; } }

        /// The context manages the state of an entity.
        /// Active entities are enabled, destroyed entities are not.
        public bool isEnabled { get { return _isEnabled; } }

        /// componentPools is set by the context which created the entity and
        /// is used to reuse removed components.
        /// Removed components will be pushed to the componentPool.
        /// Use entity.CreateComponent(index, type) to get a new or
        /// reusable component from the componentPool.
        /// Use entity.GetComponentPool(index) to get a componentPool for
        /// a specific component index.
        public Dictionary<Type, Stack<IComponent>> componentPools { get { return _componentPools; } }

        /// The contextInfo is set by the context which created the entity and
        /// contains information about the context.
        /// It's used to provide better error messages.
        public ContextInfo contextInfo { get { return _contextInfo; } }

        /// Automatic Entity Reference Counting (AERC)
        /// is used internally to prevent pooling retained entities.
        /// If you use retain manually you also have to
        /// release it manually at some point.
        public IAERC aerc { get { return _aerc; } }

        int _creationIndex;
        bool _isEnabled;

        int _totalComponents;
        Dictionary<Type, IComponent> _components;
        Dictionary<Type, Stack<IComponent>> _componentPools;
        ContextInfo _contextInfo;
        IAERC _aerc;

        string _toStringCache;
        StringBuilder _toStringBuilder;

        public void Initialize(int creationIndex, 
            int totalComponents, 
            Dictionary<Type, Stack<IComponent>> componentPools, ContextInfo contextInfo = null, IAERC aerc = null)
        {
            Reactivate(creationIndex);

            _totalComponents = totalComponents;
            _components = new Dictionary<Type, IComponent>();
            _componentPools = componentPools;

            _contextInfo = contextInfo ?? createDefaultContextInfo();
            _aerc = aerc ?? new SafeAERC(this);
        }

        ContextInfo createDefaultContextInfo()
        {
            var componentNames = new string[totalComponents];
            for (int i = 0; i < componentNames.Length; i++)
            {
                componentNames[i] = i.ToString();
            }

            return new ContextInfo("No Context", componentNames, null);
        }

        public void Reactivate(int creationIndex)
        {
            _creationIndex = creationIndex;
            _isEnabled = true;
        }

        /// Adds a component at the specified index.
        /// You can only have one component at an index.
        /// Each component type must have its own constant index.
        /// The prefered way is to use the
        /// generated methods from the code generator.
        public void AddComponent<T>(T component) where T : IComponent
        {
            if (!_isEnabled)
            {
                throw new EntityIsNotEnabledException(
                    "Cannot add component '" +
                    typeof(T).Name + "' to " + this + "!"
                );
            }

            if (HasComponent<T>())
            {
                throw new EntityAlreadyHasComponentException<T>(
                    "Cannot add component '" +
                    typeof(T).Name + "' to " + this + "!",
                    "You should check if an entity already has the component " +
                    "before adding it or use entity.ReplaceComponent()."
                );
            }

            _components[typeof(T)] = component;
            _toStringCache = null;
            if (OnComponentAdded != null)
            {
                OnComponentAdded(this, typeof(T), component);
            }
        }

        /// Removes a component at the specified index.
        /// You can only remove a component at an index if it exists.
        /// The prefered way is to use the
        /// generated methods from the code generator.
        public void RemoveComponent<T>() where T : IComponent
        {
            if (!_isEnabled)
            {
                throw new EntityIsNotEnabledException(
                    "Cannot remove component '" +
                    typeof(T) + "' from " + this + "!"
                );
            }

            if (!HasComponent<T>())
            {
                throw new EntityDoesNotHaveComponentException<T>(
                    "Cannot remove component '"
                     + "' from " + this + "!",
                    "You should check if an entity has the component " +
                    "before removing it."
                );
            }

            replaceComponent<T>(null);
        }

        /// Replaces an existing component at the specified index
        /// or adds it if it doesn't exist yet.
        /// The prefered way is to use the
        /// generated methods from the code generator.
        public void ReplaceComponent<T>(T component) where T : IComponent
        {
            if (!_isEnabled)
            {
                throw new EntityIsNotEnabledException(
                    "Cannot replace component '" +
                    typeof(T).Name + "' on " + this + "!"
                );
            }

            if (HasComponent<T>())
            {
                replaceComponent<T>(component);
            }
            else if (component != null)
            {
                AddComponent<T>(component);
            }
        }

        void replaceComponent<T>(IComponent replacement)
        {
            _toStringCache = null;
            var previousComponent = _components[typeof(T)];
            if (replacement != previousComponent)
            {
                _components[typeof(T)] = replacement;
                if (replacement != null)
                {
                    if (OnComponentReplaced != null)
                    {
                        OnComponentReplaced(
                            this, typeof(T), previousComponent, replacement
                        );
                    }
                }
                else
                {
                    if (OnComponentRemoved != null)
                    {
                        OnComponentRemoved(this, typeof(T), previousComponent);
                    }
                }

                GetComponentPool(typeof(T)).Push(previousComponent);
            }
            else
            {
                if (OnComponentReplaced != null)
                {
                    OnComponentReplaced(
                        this, typeof(T), previousComponent, replacement
                    );
                }
            }
        }

        /// Returns a component at the specified index.
        /// You can only get a component at an index if it exists.
        /// The prefered way is to use the
        /// generated methods from the code generator.
        public T GetComponent<T>() where T : IComponent
        {
            if (!HasComponent<T>())
            {
                throw new EntityDoesNotHaveComponentException<T>(
                    "Cannot get component '" +
                    typeof(T).Name + "' from " + this + "!",
                    "You should check if an entity has the component " +
                    "before getting it."
                );
            }

            return (T) _components[typeof(T)];
        }

        /// Returns all added components.
        public IComponent[] GetComponents()
        {
            IComponent[] components = new IComponent[_components.Count];

            int i = 0;
            foreach (var component in _components)
            {
                components[i] = component.Value;
                i++;
            }

            return components;
        }

        /// Determines whether this entity has a component
        public bool HasComponent<T>()
        {
            return HasComponent(typeof(T));
        }

        public bool HasComponent(Type type)
        {
            return _components.ContainsKey(type);
        }

        /// Determines whether this entity has components
        public bool HasComponents(Type[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (!HasComponent(components[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// Determines whether this entity has a component
        /// at any of the specified indices.
        public bool HasAnyComponent(Type[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (HasComponent(components[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// Removes all components.
        public void RemoveAllComponents()
        {
            _toStringCache = null;
            _components.Clear();
        }

        /// Returns the componentPool for the specified component index.
        /// componentPools is set by the context which created the entity and
        /// is used to reuse removed components.
        /// Removed components will be pushed to the componentPool.
        /// Use entity.CreateComponent(index, type) to get a new or
        /// reusable component from the componentPool.
        public Stack<IComponent> GetComponentPool(Type type)
        {
            if (_componentPools.ContainsKey(type))
            {
                return _componentPools[type];
            }

            var componentPool = new Stack<IComponent>();
            _componentPools[type] = componentPool;

            return componentPool;
        }

        /// Returns a new or reusable component from the componentPool
        /// for the specified component index.
        public IComponent CreateComponent(Type type)
        {
            var componentPool = GetComponentPool(type);
            return componentPool.Count > 0
                        ? componentPool.Pop()
                        : (IComponent) Activator.CreateInstance(type);
        }

        /// Returns a new or reusable component from the componentPool
        /// for the specified component index.
        public T CreateComponent<T>() where T : new()
        {
            var componentPool = GetComponentPool(typeof(T));
            return componentPool.Count > 0 ? (T)componentPool.Pop() : new T();
        }

        /// Returns the number of objects that retain this entity.
        public int retainCount { get { return _aerc.retainCount; } }

        /// Retains the entity. An owner can only retain the same entity once.
        /// Retain/Release is part of AERC (Automatic Entity Reference Counting)
        /// and is used internally to prevent pooling retained entities.
        /// If you use retain manually you also have to
        /// release it manually at some point.
        public void Retain(object owner)
        {
            _aerc.Retain(owner);
            _toStringCache = null;
        }

        /// Releases the entity. An owner can only release an entity
        /// if it retains it.
        /// Retain/Release is part of AERC (Automatic Entity Reference Counting)
        /// and is used internally to prevent pooling retained entities.
        /// If you use retain manually you also have to
        /// release it manually at some point.
        public void Release(object owner)
        {
            _aerc.Release(owner);
            _toStringCache = null;

            if (_aerc.retainCount == 0)
            {
                if (OnEntityReleased != null)
                {
                    OnEntityReleased(this);
                }
            }
        }

        // Dispatches OnDestroyEntity which will start the destroy process.
        public void Destroy()
        {
            if (!_isEnabled)
            {
                throw new EntityIsNotEnabledException("Cannot destroy " + this + "!");
            }

            if (OnDestroyEntity != null)
            {
                OnDestroyEntity(this);
            }
        }

        // This method is used internally. Don't call it yourself.
        // Use entity.Destroy();
        public void InternalDestroy()
        {
            _isEnabled = false;
            RemoveAllComponents();
            OnComponentAdded = null;
            OnComponentReplaced = null;
            OnComponentRemoved = null;
            OnDestroyEntity = null;
        }

        // Do not call this method manually. This method is called by the context.
        public void RemoveAllOnEntityReleasedHandlers()
        {
            OnEntityReleased = null;
        }

        /// Returns a cached string to describe the entity
        /// with the following format:
        /// Entity_{creationIndex}(*{retainCount})({list of components})
        public override string ToString()
        {
            if (_toStringCache == null)
            {
                if (_toStringBuilder == null)
                {
                    _toStringBuilder = new StringBuilder();
                }
                _toStringBuilder.Length = 0;
                _toStringBuilder
                    .Append("Entity_")
                    .Append(_creationIndex)
                    .Append("(*")
                    .Append(retainCount)
                    .Append(")")
                    .Append("(");

                const string separator = ", ";
                var components = GetComponents();
                var lastSeparator = components.Length - 1;
                for (int i = 0; i < components.Length; i++)
                {
                    var component = components[i];
                    var type = component.GetType();
                    var implementsToString = type.GetMethod("ToString")
                                                 .DeclaringType.ImplementsInterface<IComponent>();
                    _toStringBuilder.Append(
                        implementsToString
                            ? component.ToString()
                            : type.ToCompilableString().RemoveComponentSuffix()
                    );

                    if (i < lastSeparator)
                    {
                        _toStringBuilder.Append(separator);
                    }
                }

                _toStringBuilder.Append(")");
                _toStringCache = _toStringBuilder.ToString();
            }

            return _toStringCache;
        }
    }
}
