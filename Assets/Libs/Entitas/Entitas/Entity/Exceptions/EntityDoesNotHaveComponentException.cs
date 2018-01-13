namespace Entitas {

    public class EntityDoesNotHaveComponentException<T> : EntitasException {

        public EntityDoesNotHaveComponentException(string message, string hint)
            : base(message + "\nEntity does not have a component at index " + typeof(T).Name + "!", hint) {
        }
    }
}
