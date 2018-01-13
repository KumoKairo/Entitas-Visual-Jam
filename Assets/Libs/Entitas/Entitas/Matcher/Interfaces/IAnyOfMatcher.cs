using System;

namespace Entitas
{

    public interface IAnyOfMatcher : INoneOfMatcher
    {
        INoneOfMatcher NoneOf(params Type[] indices);
        INoneOfMatcher NoneOf(params IMatcher[] matchers);
    }
}
