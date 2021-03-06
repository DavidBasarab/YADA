using System;
using System.Linq.Expressions;

namespace Yada
{
    public class MappingExpression<T>
    {
        internal MappingExpression(MappingInfo mappingInfo)
        {
            MappingInfo = mappingInfo;
        }

        private MappingInfo MappingInfo { get; set; }

        public MappingExpression<T> Map(Expression<Func<T, object>> destinationProperty, string name)
        {
            if (MappingInfo.MultiResultSet)
                throw new NotSupportedException("Cannot mix Map with typeOf(OtherMap) and basic structs maps.  All multi result sets should be user types only.");

            var memberInfo = GetDestinationpProperty(destinationProperty);

            var propertyMappingInfo = new PropertyMappingInfo(memberInfo, name);

            MappingInfo.Properties.Add(propertyMappingInfo);

            return this;
        }

        public MappingExpression<T> Map(Expression<Func<T, object>> destinationProperty, Type otherMap)
        {
            var memberInfo = GetDestinationpProperty(destinationProperty);

            var propertyMappingInfo = new PropertyMappingInfo(memberInfo, otherMap);

            MappingInfo.Properties.Add(propertyMappingInfo);
            MappingInfo.MultiResultSet = true;

            return this;
        }

        private MemberExpression GetDestinationpProperty(Expression expression)
        {
            var currentExpression = expression;

            var compelte = false;

            while (!compelte)
            {
                switch (currentExpression.NodeType)
                {
                    case ExpressionType.Convert:
                        currentExpression = ((UnaryExpression)currentExpression).Operand;
                        break;
                    case ExpressionType.Lambda:
                        currentExpression = ((LambdaExpression)currentExpression).Body;
                        break;
                    case ExpressionType.MemberAccess:
                        {
                            return ((MemberExpression)currentExpression);
                        }
                    default:
                        compelte = true;
                        break;
                }
            }

            return null;
        }
    }
}