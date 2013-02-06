using System;
using System.Linq.Expressions;

namespace Yada
{
    internal class PropertyMappingInfo
    {
        public PropertyMappingInfo(MemberExpression memberInfo, string columnName)
        {
            MemberExpression = memberInfo;
            Name = columnName;
        }

        public PropertyMappingInfo(MemberExpression memberInfo, Type otherMap)
            : this(memberInfo, string.Empty)
        {
            OtherMapType = otherMap;
        }

        public string Name { get; set; }
        public Type OtherMapType { get; set; }
        public MemberExpression MemberExpression { get; set; }
    }
}