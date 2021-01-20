using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using HeaderDetection.Models;

namespace HeaderDetection
{
    public static class Detection
    {
        public static ModelStructure DetectHeader(Type type)
        {
            if (!IsValidType(type))
                throw new ArgumentException($"Type is not valid. ({type.Name} - {type})");

            var name = GetPropertyName(type);
            if (!HasInnerModel(type))
                return new ModelStructure(name, 1, 0, 0, null);

            var outerTypes = new List<Type> {type};
            var innerStructure = GetHeaderStructures(type, 1, outerTypes).ToList();
            var (numOfColumns, depth) = GetDepthAndColumns(innerStructure);
            return new ModelStructure(name, numOfColumns, 0, depth, innerStructure);
        }

        private static IEnumerable<ModelStructure> GetHeaderStructures(Type type, int depth, List<Type> outerTypes) =>
            type.GetProperties().Select(property => GetHeaderStructure(property, depth, outerTypes));

        private static ModelStructure GetHeaderStructure(PropertyInfo property, int currentDepth, List<Type> outerTypes)
        {
            if (!IsValidType(property.PropertyType))
                throw new ArgumentException($"Property type is not valid. ({property.Name} - {property.PropertyType})");
            if (outerTypes.Any(type => type == property.PropertyType))
                throw new ArgumentException($"Recursive detected. Type: {property.PropertyType.FullName}",
                    property.Name);

            var name = GetPropertyName(property);
            if (!HasInnerModel(property.PropertyType))
                return new ModelStructure(name, 1, currentDepth, 0, null);

            outerTypes.Add(property.PropertyType);
            var innerStructure = GetHeaderStructures(
                property.PropertyType, currentDepth + 1, outerTypes).ToList();
            var (numOfColumns, maximumInnerDepth) = GetDepthAndColumns(innerStructure);
            outerTypes.Remove(property.PropertyType);
            return new ModelStructure(name, numOfColumns, currentDepth, maximumInnerDepth, innerStructure);
        }

        private static string GetPropertyName(MemberInfo property)
        {
            var displayAttribute = property.GetCustomAttributes(typeof(DisplayNameAttribute)).FirstOrDefault();
            if (displayAttribute is null)
                return property.Name;

            var name = ((DisplayNameAttribute) displayAttribute).DisplayName;
            return string.IsNullOrEmpty(name) ? property.Name : name;
        }

        private static (int numOfColumns, int depth) GetDepthAndColumns(IEnumerable<ModelStructure> headers)
        {
            var maximumInnerDepth = 0;
            var sum = 0;
            foreach (var header in headers)
            {
                int innerDepth;
                int numOfColumns;
                if (header.InnerProperties is null)
                {
                    numOfColumns = 1;
                    innerDepth = 0;
                }
                else
                {
                    var (columns, depth) = GetDepthAndColumns(header.InnerProperties);
                    numOfColumns = columns;
                    innerDepth = depth;
                }

                sum += numOfColumns;
                innerDepth++;
                if (innerDepth > maximumInnerDepth)
                    maximumInnerDepth = innerDepth;
            }

            return (sum, maximumInnerDepth);
        }

        public static bool IsValidType(Type type) => !type.IsArray && !IsList(type);

        private static bool IsList(Type type) => type.IsGenericType && (
            type.GetGenericTypeDefinition() == typeof(List<>)
            || type.GetGenericTypeDefinition() == typeof(IList<>));

        private static bool HasInnerModel(Type type)
        {
            if (type.Namespace != null && type.Namespace.StartsWith("System"))
                return false;
            return !type.IsArray && !type.IsEnum && !type.IsInterface && !type.IsPointer && type.IsClass;
        }
    }
}