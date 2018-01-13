namespace Entitas {

    public class EntityAlreadyHasComponentException<T> : EntitasException {

        public EntityAlreadyHasComponentException(string message, string hint)
            : base(message + "\nEntity already has a component " + typeof(T).Name + "!", hint) {
        }
    }
}
