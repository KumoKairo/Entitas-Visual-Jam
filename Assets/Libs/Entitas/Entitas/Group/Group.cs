﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Entitas {

    /// Use context.GetGroup(matcher) to get a group of entities which match
    /// the specified matcher. Calling context.GetGroup(matcher) with the
    /// same matcher will always return the same instance of the group.
    /// The created group is managed by the context and will always be up to date.
    /// It will automatically add entities that match the matcher or
    /// remove entities as soon as they don't match the matcher anymore.
    public class Group : IGroup {

        /// Occurs when an entity gets added.
        public event GroupChanged OnEntityAdded;

        /// Occurs when an entity gets removed.
        public event GroupChanged OnEntityRemoved;

        /// Occurs when a component of an entity in the group gets replaced.
        public event GroupUpdated OnEntityUpdated;

        /// Returns the number of entities in the group.
        public int count { get { return _entities.Count; } }

        /// Returns the matcher which was used to create this group.
        public IMatcher matcher { get { return _matcher; } }

        readonly IMatcher _matcher;

        readonly HashSet<IEntity> _entities = new HashSet<IEntity>(
            EntityEqualityComparer<IEntity>.comparer
        );

        IEntity[] _entitiesCache;
        IEntity _singleEntityCache;
        string _toStringCache;

        /// Use context.GetGroup(matcher) to get a group of entities which match
        /// the specified matcher.
        public Group(IMatcher matcher) {
            _matcher = matcher;
        }

        /// This is used by the context to manage the group.
        public void HandleEntitySilently(IEntity entity) {
            if (_matcher.Matches(entity)) {
                addEntitySilently(entity);
            } else {
                removeEntitySilently(entity);
            }
        }

        /// This is used by the context to manage the group.
        public void HandleEntity(IEntity entity, Type componentType, IComponent component) {
            if (_matcher.Matches(entity)) {
                addEntity(entity, componentType, component);
            } else {
                removeEntity(entity, componentType, component);
            }
        }

        /// This is used by the context to manage the group.
        public void UpdateEntity(IEntity entity, Type componentType, IComponent previousComponent, IComponent newComponent) {
            if (_entities.Contains(entity)) {
                if (OnEntityRemoved != null) {
                    OnEntityRemoved(this, entity, componentType, previousComponent);
                }
                if (OnEntityAdded != null) {
                    OnEntityAdded(this, entity, componentType, newComponent);
                }
                if (OnEntityUpdated != null) {
                    OnEntityUpdated(
                        this, entity, componentType, previousComponent, newComponent
                    );
                }
            }
        }

        /// This is called by context.Reset() to remove all event handlers.
        /// This is useful when you want to soft-restart your application.
        public void RemoveAllEventHandlers() {
            OnEntityAdded = null;
            OnEntityRemoved = null;
            OnEntityUpdated = null;
        }

        public GroupChanged HandleEntity(IEntity entity) {
            return _matcher.Matches(entity)
                       ? (addEntitySilently(entity) ? OnEntityAdded : null)
                       : (removeEntitySilently(entity) ? OnEntityRemoved : null);
        }

        bool addEntitySilently(IEntity entity) {
            if (entity.isEnabled) {
                var added = _entities.Add(entity);
                if (added) {
                    _entitiesCache = null;
                    _singleEntityCache = null;
                    entity.Retain(this);
                }

                return added;
            }

            return false;
        }

        void addEntity(IEntity entity, Type componentType, IComponent component) {
            if (addEntitySilently(entity) && OnEntityAdded != null) {
                OnEntityAdded(this, entity, componentType, component);
            }
        }

        bool removeEntitySilently(IEntity entity) {
            var removed = _entities.Remove(entity);
            if (removed) {
                _entitiesCache = null;
                _singleEntityCache = null;
                entity.Release(this);
            }

            return removed;
        }

        void removeEntity(IEntity entity, Type componentType, IComponent component) {
            var removed = _entities.Remove(entity);
            if (removed) {
                _entitiesCache = null;
                _singleEntityCache = null;
                if (OnEntityRemoved != null) {
                    OnEntityRemoved(this, entity, componentType, component);
                }
                entity.Release(this);
            }
        }

        /// Determines whether this group has the specified entity.
        public bool ContainsEntity(IEntity entity) {
            return _entities.Contains(entity);
        }

        /// Returns all entities which are currently in this group.
        public IEntity[] GetEntities() {
            if (_entitiesCache == null) {
                _entitiesCache = new IEntity[_entities.Count];
                _entities.CopyTo(_entitiesCache);
            }

            return _entitiesCache;
        }

        public IEnumerator<IEntity> GetEnumerator() {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// Returns the only entity in this group. It will return null
        /// if the group is empty. It will throw an exception if the group
        /// has more than one entity.
        public IEntity GetSingleEntity() {
            if (_singleEntityCache == null) {
                var c = _entities.Count;
                if (c == 1) {
                    using (var enumerator = _entities.GetEnumerator()) {
                        enumerator.MoveNext();
                        _singleEntityCache = enumerator.Current;
                    }
                } else if (c == 0) {
                    return null;
                } else {
                    throw new GroupSingleEntityException(this);
                }
            }

            return _singleEntityCache;
        }

        public override string ToString() {
            if (_toStringCache == null) {
                _toStringCache = "Group(" + _matcher + ")";
            }
            return _toStringCache;
        }
    }
}
