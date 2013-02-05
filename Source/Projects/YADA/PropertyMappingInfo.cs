using System;
using System.Reflection;

namespace Yada
{
    internal class PropertyMappingInfo
    {
        public PropertyMappingInfo(MemberInfo memberInfo, string columnName)
        {
            PropertyInformation = memberInfo;
            Name = columnName;
        }

        public PropertyMappingInfo(MemberInfo memberInfo, Type otherMap)
            : this(memberInfo, string.Empty)
        {
            OtherMapType = otherMap;
        }

        public string Name { get; set; }
        public Type OtherMapType { get; set; }
        public MemberInfo PropertyInformation { get; set; }
    }
}