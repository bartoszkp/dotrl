using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Core.Parameters
{
    public static class IParametrizedObjectExtensions
    {
        public static IEnumerable<Parameter> GetParameters(this IParametrizedObject @this)
        {
            return @this
                .GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Concat<MemberInfo>(@this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                .Select(fop => new { FieldOrProperty = fop, Attribute = ProcessAttributes(fop) })
                .Where(d => d.Attribute != null)
                .Select(d => CreateParameter(@this, d.FieldOrProperty, d.Attribute))
                .ToArray();
        }

        public static IEnumerable<InnerParametrizedObject> GetInnerParametrizedObjects(
            this IParametrizedObject @this)
        {
            return @this
                .GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Concat<MemberInfo>(@this.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                .Where(fop => !fop.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
                .Where(fop => typeof(IParametrizedObject).IsAssignableFrom(fop.GetFieldOrPropertyType()))
                .Select(fop => GetInnerParametrizedObject(@this, fop))
                .Where(po => po != null)
                .ToArray();
        }

        public static ParametersSnapshot GetParametersSnapshot(this IParametrizedObject @this)
        {
            return @this.GetParametersSnapshot(string.Empty);
        }

        public static ParametersSnapshot GetParameterSnapshotWithInnerObjects(this IParametrizedObject @this)
        {
            return @this.GetParameterSnapshotWithInnerObjects(string.Empty);
        }

        public static void SetParametersFromSnapshot(this IParametrizedObject @this, ParametersSnapshot snapshot)
        {
            Dictionary<string, string> parameterValues = snapshot.ParameterValues.ToDictionary(pvp => pvp.Name, pvp => pvp.Value);

            foreach (Parameter parameter in @this.GetParameters())
            {
                if (parameterValues.ContainsKey(parameter.Field.Name))
                {
                    if (parameter.IsEnum)
                    {
                        parameter.SetValue(Enum.Parse(parameter.FieldType, parameterValues[parameter.Field.Name]));
                    }
                    else
                    {
                        parameter.SetValue(Convert.ChangeType(parameterValues[parameter.Field.Name], parameter.FieldType, CultureInfo.InvariantCulture));
                    }
                }
            }

            @this.ParametersChanged();
        }

        public static void SetParametersFromSnapshotWithInnerObjects(this IParametrizedObject @this, ParametersSnapshot snapshot)
        {
            @this.SetParametersFromSnapshot(snapshot);

            Dictionary<string, ParametersSnapshot> snapshots = snapshot.InnerSnapshots.ToDictionary(s => s.ParentFieldName, s => s);

            foreach (InnerParametrizedObject io in @this.GetInnerParametrizedObjects())
            {
                if (snapshots.ContainsKey(io.ParentFieldName))
                {
                    io.ParametrizedObject.SetParametersFromSnapshotWithInnerObjects(snapshots[io.ParentFieldName]);
                }
            }

            @this.ParametersChanged();
        }

        public static void CopyParametersFrom(this IParametrizedObject @this, IParametrizedObject other)
        {
            Contract.Requires(@this.GetType().Equals(other.GetType()));

            foreach (var parameterPair
                in Enumerable.Zip(@this.GetParameters(), other.GetParameters(), (p1, p2) => new { P1 = p1, P2 = p2 }))
            {
                parameterPair.P1.SetValue(parameterPair.P2.GetValue());
            }
        }

        public static void CopyParametersFromWithInnerObjects(this IParametrizedObject @this, IParametrizedObject other)
        {
            Contract.Requires(@this.GetType().Equals(other.GetType()));

            @this.CopyParametersFrom(other);

            foreach (var objectPair
                in Enumerable.Zip(@this.GetInnerParametrizedObjects(), other.GetInnerParametrizedObjects(), (o1, o2) => new { O1 = o1, O2 = o2 }))
            {
                objectPair.O1.ParametrizedObject.CopyParametersFromWithInnerObjects(objectPair.O2.ParametrizedObject);
            }

            @this.ParametersChanged();
        }

        private static ParametersSnapshot GetParametersSnapshot(this IParametrizedObject @this, string parentFieldName)
        {
            ParametersSnapshot result = new ParametersSnapshot()
            {
                ParentFieldName = parentFieldName
            };

            result.ParameterValues = @this
                .GetParameters()
                .Select(p => new ParameterValuePair() 
                { 
                    Name = p.Field.Name,
                    Value = (string)Convert.ChangeType(p.GetValue(), typeof(string), CultureInfo.InvariantCulture) })
                .ToArray();

            return result;
        }

        public static ParametersSnapshot GetParameterSnapshotWithInnerObjects(this IParametrizedObject @this, string parentFieldName)
        {
            ParametersSnapshot result = @this.GetParametersSnapshot(parentFieldName);

            result.InnerSnapshots = @this
                .GetInnerParametrizedObjects()
                .Select(io => GetParameterSnapshotWithInnerObjects(io.ParametrizedObject, io.ParentFieldName))
                .ToArray();

            return result;
        }

        private static InnerParametrizedObject GetInnerParametrizedObject(
            IParametrizedObject @this,
            MemberInfo fieldOrProperty)
        {
            FieldInfo fi = fieldOrProperty as FieldInfo;

            InnerParametrizedObject result = new InnerParametrizedObject()
            {
                ParentFieldName = fieldOrProperty.Name
            };

            if (fi != null)
            {
                result.ParametrizedObject = fi.GetValue(@this) as IParametrizedObject;
                return result;
            }

            PropertyInfo pi = fieldOrProperty as PropertyInfo;

            if (pi != null)
            {
                result.ParametrizedObject = pi.GetValue(@this, null) as IParametrizedObject;
                return result;
            }

            return null;
        }

        private static object ProcessAttributes(MemberInfo fieldOrProperty)
        {
            return fieldOrProperty.GetCustomAttributes(typeof(ParameterAttribute), true).SingleOrDefault();
        }

        private static Parameter CreateParameter(IParametrizedObject @this, MemberInfo fieldOrProperty, object attribute)
        {
            Parameter result = null;

            ParameterAttribute parameterAttribute = attribute as ParameterAttribute;

            Type fieldType = fieldOrProperty.GetFieldOrPropertyType();

            string typeDescription = GetFieldOrPropertyTypeDescription(@this, fieldType, fieldOrProperty);

            result = new Parameter(
               @this,
               fieldOrProperty,
               fieldType,
               parameterAttribute.MinimumValue,
               parameterAttribute.MaximumValue,
               parameterAttribute.StringType,
               parameterAttribute.Description,
               typeDescription); 
            
            if (!result.IsString && result.StringType != StringParameterType.Unspecified)
            {
                throw new InvalidOperationException("'StringType' argument can be used only with string fields. Class: " + @this.GetType().FullName + ", field name: " + fieldOrProperty.Name);
            }

            if (!result.IsNumeric && (result.MinimumValue != null || result.MaximumValue != null))
            {
                throw new InvalidOperationException("'MinimumValue' and 'MaximumValue' arguments can be used only with numeric fields. Class: " + @this.GetType().FullName + ", field name: " + fieldOrProperty.Name);
            }

            return result;
        }

        private static string GetFieldOrPropertyTypeDescription(IParametrizedObject @this, Type type, MemberInfo fieldOrProperty)
        {
            if (fieldTypeToDescription.ContainsKey(type))
            {
                return fieldTypeToDescription[type];
            }

            if (type.BaseType != null && fieldTypeToDescription.ContainsKey(type.BaseType))
            {
                return fieldTypeToDescription[type.BaseType];
            }

            throw new InvalidOperationException("Unsupported field type: '" + type.FullName + "' used in class: '" + @this.GetType().FullName + "', field name: '" + fieldOrProperty.Name + "'");
        }

        private static Dictionary<Type, string> fieldTypeToDescription = new Dictionary<Type, string>()
        {
            { typeof(bool), "boolean value" },
            { typeof(int), "integer number" },
            { typeof(long), "long integer number" },
            { typeof(float), "floating point number" },
            { typeof(double), "double precision floating point number" },
            { typeof(decimal), "decimal type floating point number" },
            { typeof(string), "string" },
            { typeof(Enum), "enumeration" }
        };
    }
}
