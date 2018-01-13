using System;

namespace Entitas
{
    public interface IMatcher
    {
        Type[] componentTypes { get; }
        bool Matches(IEntity entity);
    }
}
