using System;

namespace Entitas {

    public interface IAllOfMatcher : IAnyOfMatcher {

        IAnyOfMatcher AnyOf(params Type[] indices);
        IAnyOfMatcher AnyOf(params IMatcher[] matchers);
    }
}
