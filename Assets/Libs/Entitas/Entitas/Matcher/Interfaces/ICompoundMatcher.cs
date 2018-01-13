using System;

namespace Entitas {

    public interface ICompoundMatcher : IMatcher {

        Type[] allOfIndices { get; }
        Type[] anyOfIndices { get; }
        Type[] noneOfIndices { get; }
    }
}
