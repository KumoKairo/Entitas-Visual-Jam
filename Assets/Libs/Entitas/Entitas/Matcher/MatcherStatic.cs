using System;
using System.Collections.Generic;

namespace Entitas
{
    public partial class Matcher
    {
        public static IAllOfMatcher AllOf(params Type[] indices)
        {
            var matcher = new Matcher();
            matcher._allOfIndices = distinctIndices(indices);
            return matcher;
        }

        public static IAllOfMatcher AllOf(params IMatcher[] matchers)
        {
            var allOfMatcher = (Matcher) Matcher.AllOf(mergeIndices(matchers));
            setComponentNames(allOfMatcher, matchers);
            return allOfMatcher;
        }

        public static IAnyOfMatcher AnyOf(params Type[] indices)
        {
            var matcher = new Matcher();
            matcher._anyOfIndices = distinctIndices(indices);
            return matcher;
        }

        public static IAnyOfMatcher AnyOf(params IMatcher[] matchers)
        {
            var anyOfMatcher = (Matcher)Matcher.AnyOf(mergeIndices(matchers));
            setComponentNames(anyOfMatcher, matchers);
            return anyOfMatcher;
        }

        static Type[] mergeIndices(Type[] allOfIndices, Type[] anyOfIndices, Type[] noneOfIndices)
        {
            var indicesList = new List<Type>();

            if (allOfIndices != null)
            {
                indicesList.AddRange(allOfIndices);
            }
            if (anyOfIndices != null)
            {
                indicesList.AddRange(anyOfIndices);
            }
            if (noneOfIndices != null)
            {
                indicesList.AddRange(noneOfIndices);
            }

            var mergedIndices = distinctIndices(indicesList);

            return mergedIndices;
        }

        static Type[] mergeIndices(IMatcher[] matchers)
        {
            var indices = new Type[matchers.Length];
            for (int i = 0; i < matchers.Length; i++)
            {
                var matcher = matchers[i];
                if (matcher.componentTypes.Length != 1)
                {
                    throw new MatcherException(matcher.componentTypes.Length);
                }
                indices[i] = matcher.componentTypes[0];
            }

            return indices;
        }

        static string[] getComponentNames(IMatcher[] matchers)
        {
            for (int i = 0; i < matchers.Length; i++)
            {
                var matcher = matchers[i] as Matcher;
                if (matcher != null && matcher.componentNames != null)
                {
                    return matcher.componentNames;
                }
            }

            return null;
        }

        static void setComponentNames(Matcher matcher, IMatcher[] matchers)
        {
            var componentNames = getComponentNames(matchers);
            if (componentNames != null)
            {
                matcher.componentNames = componentNames;
            }
        }

        static Type[] distinctIndices(IList<Type> indices)
        {
            var indicesSet = new HashSet<Type>();

            foreach (var index in indices)
            {
                indicesSet.Add(index);
            }
            var uniqueIndices = new Type[indicesSet.Count];
            indicesSet.CopyTo(uniqueIndices);

            return uniqueIndices;
        }
    }
}
