namespace Entitas
{

    public static class ContextExtension
    {
        /// Returns all entities matching the specified matcher.
        public static IEntity[] GetEntities(this IContext context, IMatcher matcher)
        {
            return context.GetGroup(matcher).GetEntities();
        }
    }
}
