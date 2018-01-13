using System;

namespace Entitas
{
    public partial class Matcher : IAllOfMatcher
    {
        public Type[] componentTypes
        {
            get
            {
                if (_indices == null)
                {
                    _indices = mergeIndices(_allOfIndices, _anyOfIndices, _noneOfIndices);
                }
                return _indices;
            }
        }

        public Type[] allOfIndices { get { return _allOfIndices; } }
        public Type[] anyOfIndices { get { return _anyOfIndices; } }
        public Type[] noneOfIndices { get { return _noneOfIndices; } }

        public string[] componentNames { get; set; }

        Type[] _indices;
        Type[] _allOfIndices;
        Type[] _anyOfIndices;
        Type[] _noneOfIndices;

        Matcher()
        {
        }

        IAnyOfMatcher IAllOfMatcher.AnyOf(params Type[] componentTypes)
        {
            _anyOfIndices = distinctIndices(componentTypes);
            _indices = null;
            _isHashCached = false;
            return this;
        }

        IAnyOfMatcher IAllOfMatcher.AnyOf(params IMatcher[] matchers)
        {
            return ((IAllOfMatcher)this).AnyOf(mergeIndices(matchers));
        }

        public INoneOfMatcher NoneOf(params Type[] componentTypes)
        {
            _noneOfIndices = distinctIndices(componentTypes);
            _indices = null;
            _isHashCached = false;
            return this;
        }

        public INoneOfMatcher NoneOf(params IMatcher[] matchers)
        {
            return NoneOf(mergeIndices(matchers));
        }

        public bool Matches(IEntity entity)
        {
            return (_allOfIndices == null || entity.HasComponents(_allOfIndices))
                && (_anyOfIndices == null || entity.HasAnyComponent(_anyOfIndices))
                && (_noneOfIndices == null || !entity.HasAnyComponent(_noneOfIndices));
        }
    }
}
