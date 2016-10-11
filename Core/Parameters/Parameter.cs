using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Core.Parameters
{
    public class Parameter
    {
        public object Owner { get; set; }

        public MemberInfo Field { get; set; }
       
        public Type FieldType { get; set; }

        public object MinimumValue { get; private set; }

        public object MaximumValue { get; private set; }

        public StringParameterType StringType { get; private set; }

        public string Description { get; private set; }

        public string TypeDescription { get; private set; }

        public bool IsNumeric 
        { 
            get { return numericTypes.Contains(this.FieldType); } 
        }

        public bool IsString 
        {
            get { return this.FieldType.Equals(typeof(string)); } 
        }

        public bool IsBoolean
        {
            get { return this.FieldType.Equals(typeof(bool)); } 
        }

        public bool IsEnum 
        { 
            get { return typeof(Enum).IsAssignableFrom(this.FieldType); } 
        }

        public Parameter(
            object owner,
            MemberInfo field,
            Type fieldType,
            object minimumValue,
            object maximumValue,
            StringParameterType stringType,
            string description,
            string typeDescription)
        {
            this.Owner = owner;
            this.Field = field;
            this.FieldType = fieldType;

            if (IsNumeric)
            {
                if (minimumValue != null)
                {
                    this.MinimumValue = Convert.ChangeType(minimumValue, this.FieldType, CultureInfo.InvariantCulture);
                }

                if (maximumValue != null)
                {
                    this.MaximumValue = Convert.ChangeType(maximumValue, this.FieldType, CultureInfo.InvariantCulture);
                }
            }

            this.StringType = stringType;
            this.Description = description;
            this.TypeDescription = typeDescription;
        }

        public void SetValue(object value)
        {
            if (Field is FieldInfo)
            {
                ((FieldInfo)Field).SetValue(Owner, value);
            }
            else if (Field is PropertyInfo)
            {
                ((PropertyInfo)Field).SetValue(Owner, value, null);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public object GetValue()
        {
            if (Field is FieldInfo)
            {
                return ((FieldInfo)Field).GetValue(Owner);
            }
            else if (Field is PropertyInfo)
            {
                return ((PropertyInfo)Field).GetValue(Owner, null);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static Type[] numericTypes = new Type[] { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) };
    }
}
