using System;
using System.Text;

namespace Entitas
{
    public partial class Matcher
    {
        string _toStringCache;
        StringBuilder _toStringBuilder;

        public override string ToString()
        {
            if (_toStringCache == null)
            {
                if (_toStringBuilder == null)
                {
                    _toStringBuilder = new StringBuilder();
                }
                _toStringBuilder.Length = 0;
                if (_allOfIndices != null)
                {
                    appendIndices(_toStringBuilder, "AllOf", _allOfIndices);
                }
                if (_anyOfIndices != null)
                {
                    if (_allOfIndices != null)
                    {
                        _toStringBuilder.Append(".");
                    }
                    appendIndices(_toStringBuilder, "AnyOf", _anyOfIndices);
                }
                if (_noneOfIndices != null)
                {
                    appendIndices(_toStringBuilder, ".NoneOf", _noneOfIndices);
                }
                _toStringCache = _toStringBuilder.ToString();
            }

            return _toStringCache;
        }

        static void appendIndices(StringBuilder sb, string prefix, Type[] indexArray)
        {
            const string separator = ", ";
            sb.Append(prefix);
            sb.Append("(");
            var lastSeparator = indexArray.Length - 1;
            for (int i = 0; i < indexArray.Length; i++)
            {
                var componentType = indexArray[i];
                sb.Append(componentType.Name);

                if (i < lastSeparator)
                {
                    sb.Append(separator);
                }
            }
            sb.Append(")");
        }
    }
}
