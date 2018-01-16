using System;
using System.Collections.Generic;

namespace Entitas {

    public delegate void EntityComponentChanged(
        Entity entity, Type componentType, IComponent component
    );

    public delegate void EntityComponentReplaced(
        Entity entity, Type componentType, IComponent previousComponent, IComponent newComponent
    );

    public delegate void EntityEvent(Entity entity);

    public interface IEntity : IAERC {

        event EntityComponentChanged OnComponentAdded;
        event EntityComponentChanged OnComponentRemoved;
        event EntityComponentReplaced OnComponentReplaced;
        event EntityEvent OnEntityReleased;
        event EntityEvent OnDestroyEntity;

        int totalComponents { get; }
        int creationIndex { get; }
        bool isEnabled { get; }

        Dictionary<Type, Stack<IComponent>> componentPools { get; }
        ContextInfo contextInfo { get; }
        IAERC aerc { get; }

        void Initialize(int creationIndex,
                        int totalComponents,
            Dictionary<Type, Stack<IComponent>> componentPools,
                        ContextInfo contextInfo = null,
                        IAERC aerc = null);

        void Reactivate(int creationIndex);

        void AddComponent<T>(T component) where T : IComponent;
        void RemoveComponent<T>() where T : IComponent;
        void ReplaceComponent<T>(T component) where T : IComponent;

        T GetComponent<T>() where T : IComponent;
        IComponent[] GetComponents();

        bool HasComponent<T>();
        bool HasComponents(Type[] indices);
        bool HasAnyComponent(Type[] indices);

        void RemoveAllComponents();

        Stack<IComponent> GetComponentPool(Type type);
        IComponent CreateComponent(Type type);
        T CreateComponent<T>() where T : new();

        void Destroy();
        void InternalDestroy();
        void RemoveAllOnEntityReleasedHandlers();
    }
}
