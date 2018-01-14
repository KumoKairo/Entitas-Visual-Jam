namespace Entitas.Visual.Utils
{
    public struct Tuple<T1, T2>
    {
        public T1 First;
        public T2 Second;

        public Tuple(T1 t1, T2 t2)
        {
            First = t1;
            Second = t2;
        }
    }

    public struct Tuple<T1, T2, T3>
    {
        public T1 First;
        public T2 Second;
        public T3 Third;

        public Tuple(T1 t1, T2 t2, T3 t3)
        {
            First = t1;
            Second = t2;
            Third = t3;
        }
    }
}
