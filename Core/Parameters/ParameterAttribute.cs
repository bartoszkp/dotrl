using System;
using System.Globalization;

namespace Core.Parameters
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ParameterAttribute : Attribute
    {
        public object MinimumValue { get; private set; }

        public object MaximumValue { get; private set; }

        public StringParameterType StringType { get; private set; }

        public string Description { get; private set; }

        public ParameterAttribute()
        {
            this.MinimumValue = null;
            this.MaximumValue = null;
            this.StringType = StringParameterType.Unspecified;
            this.Description = string.Empty;
        }

        public ParameterAttribute(string description)
            : this()
        {
            this.Description = description;
        }

        public ParameterAttribute(object minimumValue, object maximumValue)
            : this()
        {
            this.MinimumValue = minimumValue;
            this.MaximumValue = maximumValue;
        }

        public ParameterAttribute(object minimumValue, object maximumValue, string description)
            : this(minimumValue, maximumValue)
        {
            this.Description = description;
        }

        public ParameterAttribute(StringParameterType stringType, string description)
            : this()
        {
            this.StringType = stringType;
            this.Description = description;
        }
    }
}
