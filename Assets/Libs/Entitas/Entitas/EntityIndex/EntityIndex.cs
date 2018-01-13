using System;
using System.Collections.Generic;

namespace Entitas {

    public class EntityIndex<TKey> : AbstractEntityIndex<TKey> {

        readonly Dictionary<TKey, HashSet<IEntity>> _index;

        public EntityIndex(string name, IGroup group, Func<IEntity, IComponent, TKey> getKey) : base(name, group, getKey) {
            _index = new Dictionary<TKey, HashSet<IEntity>>();
            Activate();
        }

        public EntityIndex(string name, IGroup group, Func<IEntity, IComponent, TKey[]> getKeys) : base(name, group, getKeys) {
            _index = new Dictionary<TKey, HashSet<IEntity>>();
            Activate();
        }

        public EntityIndex(string name, IGroup group, Func<IEntity, IComponent, TKey> getKey, IEqualityComparer<TKey> comparer) : base(name, group, getKey) {
            _index = new Dictionary<TKey, HashSet<IEntity>>(comparer);
            Activate();
        }

        public EntityIndex(string name, IGroup group, Func<IEntity, IComponent, TKey[]> getKeys, IEqualityComparer<TKey> comparer) : base(name, group, getKeys) {
            _index = new Dictionary<TKey, HashSet<IEntity>>(comparer);
            Activate();
        }

        public override void Activate() {
            base.Activate();
            indexEntities(_group);
        }

        public HashSet<IEntity> GetEntities(TKey key) {
            HashSet<IEntity> entities;
            if (!_index.TryGetValue(key, out entities)) {
                entities = new HashSet<IEntity>(EntityEqualityComparer<IEntity>.comparer);
                _index.Add(key, entities);
            }

            return entities;
        }

        public override string ToString() {
            return "EntityIndex(" + name + ")";
        }

        protected override void clear() {
            foreach (var entities in _index.Values) {
                foreach (var entity in entities) {
                    var safeAerc = entity.aerc as SafeAERC;
                    if (safeAerc != null) {
                        if (safeAerc.owners.Contains(this)) {
                            entity.Release(this);
                        }
                    } else {
                        entity.Release(this);
                    }
                }
            }

            _index.Clear();
        }

        protected override void addEntity(TKey key, IEntity entity) {
            GetEntities(key).Add(entity);

            var safeAerc = entity.aerc as SafeAERC;
            if (safeAerc != null) {
                if (!safeAerc.owners.Contains(this)) {
                    entity.Retain(this);
                }
            } else {
                entity.Retain(this);
            }
        }

        protected override void removeEntity(TKey key, IEntity entity) {
            GetEntities(key).Remove(entity);

            var safeAerc = entity.aerc as SafeAERC;
            if (safeAerc != null) {
                if (safeAerc.owners.Contains(this)) {
                    entity.Release(this);
                }
            } else {
                entity.Release(this);
            }
        }
    }
}
