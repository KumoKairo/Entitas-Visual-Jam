using System;
using System.Collections.Generic;

namespace Entitas {

    public class PrimaryEntityIndex<TKey> : AbstractEntityIndex<TKey> {

        readonly Dictionary<TKey, IEntity> _index;

        public PrimaryEntityIndex(string name, IGroup group, Func<IEntity, IComponent, TKey> getKey) : base(name, group, getKey) {
            _index = new Dictionary<TKey, IEntity>();
            Activate();
        }

        public PrimaryEntityIndex(string name, IGroup group, Func<IEntity, IComponent, TKey[]> getKeys) : base(name, group, getKeys) {
            _index = new Dictionary<TKey, IEntity>();
            Activate();
        }

        public PrimaryEntityIndex(string name, IGroup group, Func<IEntity, IComponent, TKey> getKey, IEqualityComparer<TKey> comparer) : base(name, group, getKey) {
            _index = new Dictionary<TKey, IEntity>(comparer);
            Activate();
        }

        public PrimaryEntityIndex(string name, IGroup group, Func<IEntity, IComponent, TKey[]> getKeys, IEqualityComparer<TKey> comparer) : base(name, group, getKeys) {
            _index = new Dictionary<TKey, IEntity>(comparer);
            Activate();
        }

        public override void Activate() {
            base.Activate();
            indexEntities(_group);
        }

        public IEntity GetEntity(TKey key) {
            IEntity entity;
            _index.TryGetValue(key, out entity);
            return entity;
        }

        public override string ToString() {
            return "PrimaryEntityIndex(" + name + ")";
        }

        protected override void clear() {
            foreach (var entity in _index.Values) {
                var safeAerc = entity.aerc as SafeAERC;
                if (safeAerc != null) {
                    if (safeAerc.owners.Contains(this)) {
                        entity.Release(this);
                    }
                } else {
                    entity.Release(this);
                }
            }

            _index.Clear();
        }

        protected override void addEntity(TKey key, IEntity entity) {
            if (_index.ContainsKey(key)) {
                throw new EntityIndexException(
                    "Entity for key '" + key + "' already exists!",
                    "Only one entity for a primary key is allowed.");
            }

            _index.Add(key, entity);

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
            _index.Remove(key);

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
