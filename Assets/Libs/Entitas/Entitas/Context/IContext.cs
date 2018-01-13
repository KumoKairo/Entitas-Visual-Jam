using System;
using System.Collections.Generic;

namespace Entitas
{

    public delegate void ContextEntityChanged(IContext context, IEntity entity);
    public delegate void ContextGroupChanged(IContext context, IGroup group);

    public interface IContext
    {
        event ContextEntityChanged OnEntityCreated;
        event ContextEntityChanged OnEntityWillBeDestroyed;
        event ContextEntityChanged OnEntityDestroyed;

        event ContextGroupChanged OnGroupCreated;

        int totalComponents { get; }

        Dictionary<Type, Stack<IComponent>> componentPools { get; }
        ContextInfo contextInfo { get; }

        int count { get; }
        int reusableEntitiesCount { get; }
        int retainedEntitiesCount { get; }

        void DestroyAllEntities();

        void AddEntityIndex(IEntityIndex entityIndex);
        IEntityIndex GetEntityIndex(string name);

        void ResetCreationIndex();
        void ClearComponentPool(Type type);
        void ClearComponentPools();
        void Reset();

        IEntity CreateEntity();

        // TODO Obsolete since 0.42.0, April 2017
        [Obsolete("Please use entity.Destroy()")]
        void DestroyEntity(IEntity entity);

        bool HasEntity(IEntity entity);
        IEntity[] GetEntities();

        IGroup GetGroup(IMatcher matcher);
    }
}
