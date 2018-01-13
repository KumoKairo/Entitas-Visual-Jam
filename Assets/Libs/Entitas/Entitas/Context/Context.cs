﻿using System;
using System.Collections.Generic;
using System.Linq;
using Entitas.Utils;

namespace Entitas
{
    /// A context manages the lifecycle of entities and groups.
    /// You can create and destroy entities and get groups of entities.
    /// The prefered way to create a context is to use the generated methods
    /// from the code generator, e.g. var context = new GameContext();
    public class Context : IContext
    {
        /// Occurs when an entity gets created.
        public event ContextEntityChanged OnEntityCreated;

        /// Occurs when an entity will be destroyed.
        public event ContextEntityChanged OnEntityWillBeDestroyed;

        /// Occurs when an entity got destroyed.
        public event ContextEntityChanged OnEntityDestroyed;

        /// Occurs when a group gets created for the first time.
        public event ContextGroupChanged OnGroupCreated;

        /// The total amount of components an entity can possibly have.
        /// This value is generated by the code generator,
        /// e.g ComponentLookup.TotalComponents.
        public int totalComponents { get { return _totalComponents; } }

        /// Returns all componentPools. componentPools is used to reuse
        /// removed components.
        /// Removed components will be pushed to the componentPool.
        /// Use entity.CreateComponent(index, type) to get a new or reusable
        /// component from the componentPool.
        public Dictionary<Type, Stack<IComponent>> componentPools { get { return _componentPools; } }

        /// The contextInfo contains information about the context.
        /// It's used to provide better error messages.
        public ContextInfo contextInfo { get { return _contextInfo; } }

        /// Returns the number of entities in the context.
        public int count { get { return _entities.Count; } }

        /// Returns the number of entities in the internal ObjectPool
        /// for entities which can be reused.
        public int reusableEntitiesCount { get { return _reusableEntities.Count; } }

        /// Returns the number of entities that are currently retained by
        /// other objects (e.g. Group, Collector, ReactiveSystem).
        public int retainedEntitiesCount { get { return _retainedEntities.Count; } }

        readonly int _totalComponents;

        readonly Dictionary<Type, Stack<IComponent>> _componentPools;
        readonly ContextInfo _contextInfo;
        readonly Func<IEntity, IAERC> _aercFactory;

        readonly HashSet<IEntity> _entities = new HashSet<IEntity>(EntityEqualityComparer<IEntity>.comparer);
        readonly Stack<IEntity> _reusableEntities = new Stack<IEntity>();
        readonly HashSet<IEntity> _retainedEntities = new HashSet<IEntity>(EntityEqualityComparer<IEntity>.comparer);

        readonly Dictionary<IMatcher, IGroup> _groups = new Dictionary<IMatcher, IGroup>();
        readonly Dictionary<Type, List<IGroup>> _groupsForComponentType;
        readonly ObjectPool<List<GroupChanged>> _groupChangedListPool;

        readonly Dictionary<string, IEntityIndex> _entityIndices;

        int _creationIndex;

        Entity[] _entitiesCache;

        // Cache delegates to avoid gc allocations
        EntityComponentChanged _cachedEntityChanged;
        EntityComponentReplaced _cachedComponentReplaced;
        EntityEvent _cachedEntityReleased;
        EntityEvent _cachedDestroyEntity;

        public Context() : this(0, null, null)
        {
        }

        /// The prefered way to create a context is to use the generated methods
        /// from the code generator, e.g. var context = new GameContext();
        public Context(int startCreationIndex, ContextInfo contextInfo, Func<IEntity, IAERC> aercFactory)
        {
            _totalComponents = totalComponents;
            _creationIndex = startCreationIndex;

            if (contextInfo != null)
            {
                _contextInfo = contextInfo;
                if (contextInfo.componentNames.Length != totalComponents)
                {
                    throw new ContextInfoException(this, contextInfo);
                }
            }
            else
            {
                _contextInfo = createDefaultContextInfo();
            }

            _aercFactory = aercFactory == null
                ? (entity) => new SafeAERC(entity)
                : aercFactory;

            _groupsForComponentType = new Dictionary<Type, List<IGroup>>();
            _componentPools = new Dictionary<Type, Stack<IComponent>>();
            _entityIndices = new Dictionary<string, IEntityIndex>();
            _groupChangedListPool = new ObjectPool<List<GroupChanged>>(
                                        () => new List<GroupChanged>(),
                                        list => list.Clear()
                                    );

            // Cache delegates to avoid gc allocations
            _cachedEntityChanged = updateGroupsComponentAddedOrRemoved;
            _cachedComponentReplaced = updateGroupsComponentReplaced;
            _cachedEntityReleased = onEntityReleased;
            _cachedDestroyEntity = onDestroyEntity;
        }

        ContextInfo createDefaultContextInfo()
        {
            var componentNames = new string[_totalComponents];
            const string prefix = "Index ";
            for (int i = 0; i < componentNames.Length; i++)
            {
                componentNames[i] = prefix + i;
            }

            return new ContextInfo("Unnamed Context", componentNames, null);
        }

        /// Creates a new entity or gets a reusable entity from the
        /// internal ObjectPool for entities.
        public IEntity CreateEntity()
        {
            IEntity entity;

            if (_reusableEntities.Count > 0)
            {
                entity = _reusableEntities.Pop();
                entity.Reactivate(_creationIndex++);
            }
            else
            {
                entity = (Entity) Activator.CreateInstance(typeof(Entity));
                entity.Initialize(_creationIndex++, _totalComponents, _componentPools,
                    _contextInfo, _aercFactory(entity));
            }

            _entities.Add(entity);
            entity.Retain(this);
            _entitiesCache = null;
            entity.OnComponentAdded += _cachedEntityChanged;
            entity.OnComponentRemoved += _cachedEntityChanged;
            entity.OnComponentReplaced += _cachedComponentReplaced;
            entity.OnEntityReleased += _cachedEntityReleased;
            entity.OnDestroyEntity += _cachedDestroyEntity;

            if (OnEntityCreated != null)
            {
                OnEntityCreated(this, entity);
            }

            return entity;
        }

        /// Destroys the entity, removes all its components and pushs it back
        /// to the internal ObjectPool for entities.
        // TODO Obsolete since 0.42.0, April 2017
        [Obsolete("Please use entity.Destroy()")]
        public void DestroyEntity(IEntity entity)
        {
            var removed = _entities.Remove(entity);
            if (!removed)
            {
                throw new ContextDoesNotContainEntityException(
                    "'" + this + "' cannot destroy " + entity + "!",
                    "This cannot happen!?!"
                );
            }
            _entitiesCache = null;

            if (OnEntityWillBeDestroyed != null)
            {
                OnEntityWillBeDestroyed(this, entity);
            }

            entity.InternalDestroy();

            if (OnEntityDestroyed != null)
            {
                OnEntityDestroyed(this, entity);
            }

            if (entity.retainCount == 1)
            {
                // Can be released immediately without
                // adding to _retainedEntities
                entity.OnEntityReleased -= _cachedEntityReleased;
                _reusableEntities.Push(entity);
                entity.Release(this);
                entity.RemoveAllOnEntityReleasedHandlers();
            }
            else
            {
                _retainedEntities.Add(entity);
                entity.Release(this);
            }
        }

        /// Destroys all entities in the context.
        /// Throws an exception if there are still retained entities.
        public void DestroyAllEntities()
        {
            var entities = GetEntities();
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i].Destroy();
            }

            _entities.Clear();

            if (_retainedEntities.Count != 0)
            {
                throw new ContextStillHasRetainedEntitiesException(this, _retainedEntities.ToArray());
            }
        }

        /// Determines whether the context has the specified entity.
        public bool HasEntity(IEntity entity)
        {
            return _entities.Contains(entity);
        }

        /// Returns all entities which are currently in the context.
        public IEntity[] GetEntities()
        {
            if (_entitiesCache == null)
            {
                _entitiesCache = new Entity[_entities.Count];
                _entities.CopyTo(_entitiesCache);
            }

            return _entitiesCache;
        }

        /// Returns a group for the specified matcher.
        /// Calling context.GetGroup(matcher) with the same matcher will always
        /// return the same instance of the group.
        public IGroup GetGroup(IMatcher matcher)
        {
            IGroup group;
            if (!_groups.TryGetValue(matcher, out group))
            {
                group = new Group(matcher);
                var entities = GetEntities();
                for (int i = 0; i < entities.Length; i++)
                {
                    group.HandleEntitySilently(entities[i]);
                }
                _groups.Add(matcher, group);

                for (int i = 0; i < matcher.componentTypes.Length; i++)
                {
                    var componentType = matcher.componentTypes[i];
                    if (!_groupsForComponentType.ContainsKey(componentType))
                    {
                        _groupsForComponentType[componentType] = new List<IGroup>();
                    }
                    _groupsForComponentType[componentType].Add(group);
                }

                if (OnGroupCreated != null)
                {
                    OnGroupCreated(this, group);
                }
            }

            return group;
        }

        /// Adds the IEntityIndex for the specified name.
        /// There can only be one IEntityIndex per name.
        public void AddEntityIndex(IEntityIndex entityIndex)
        {
            if (_entityIndices.ContainsKey(entityIndex.name))
            {
                throw new ContextEntityIndexDoesAlreadyExistException(this, entityIndex.name);
            }

            _entityIndices.Add(entityIndex.name, entityIndex);
        }

        /// Gets the IEntityIndex for the specified name.
        public IEntityIndex GetEntityIndex(string name)
        {
            IEntityIndex entityIndex;
            if (!_entityIndices.TryGetValue(name, out entityIndex))
            {
                throw new ContextEntityIndexDoesNotExistException(this, name);
            }

            return entityIndex;
        }

        /// Resets the creationIndex back to 0.
        public void ResetCreationIndex()
        {
            _creationIndex = 0;
        }

        /// Clears the componentPool at the specified index.
        public void ClearComponentPool(Type type)
        {
            var componentPool = _componentPools[type];
            if (componentPool != null)
            {
                componentPool.Clear();
            }
        }

        /// Clears all componentPools.
        public void ClearComponentPools()
        {
            foreach (var componentPool in _componentPools)
            {
                ClearComponentPool(componentPool.Key);
            }
        }

        /// Resets the context (destroys all entities and
        /// resets creationIndex back to 0).
        public void Reset()
        {
            DestroyAllEntities();
            ResetCreationIndex();

            OnEntityCreated = null;
            OnEntityWillBeDestroyed = null;
            OnEntityDestroyed = null;
            OnGroupCreated = null;
        }

        public override string ToString()
        {
            return _contextInfo.name;
        }

        void updateGroupsComponentAddedOrRemoved(Entity entity, Type type, IComponent component)
        {
            if (_groupsForComponentType.ContainsKey(type))
            {
                var groups = _groupsForComponentType[type];
                if (groups != null)
                {
                    var events = _groupChangedListPool.Get();

                    var tEntity = (Entity)entity;

                    for (int i = 0; i < groups.Count; i++)
                    {
                        events.Add(groups[i].HandleEntity(tEntity));
                    }

                    for (int i = 0; i < events.Count; i++)
                    {
                        var groupChangedEvent = events[i];
                        if (groupChangedEvent != null)
                        {
                            groupChangedEvent(
                                groups[i], tEntity, type, component
                            );
                        }
                    }

                    _groupChangedListPool.Push(events);
                }
            }
        }

        void updateGroupsComponentReplaced(Entity entity, Type type, IComponent previousComponent, IComponent newComponent)
        {
            var groups = _groupsForComponentType[type];
            if (groups != null)
            {

                var tEntity = (Entity)entity;

                for (int i = 0; i < groups.Count; i++)
                {
                    groups[i].UpdateEntity(
                        tEntity, type, previousComponent, newComponent
                    );
                }
            }
        }

        void onEntityReleased(Entity entity)
        {
            if (entity.isEnabled)
            {
                throw new EntityIsNotDestroyedException(
                    "Cannot release " + entity + "!"
                );
            }
            var tEntity = (Entity)entity;
            entity.RemoveAllOnEntityReleasedHandlers();
            _retainedEntities.Remove(tEntity);
            _reusableEntities.Push(tEntity);
        }

        void onDestroyEntity(Entity entity)
        {
            DestroyEntity((Entity)entity);
        }
    }
}