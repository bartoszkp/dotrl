using System;
using System.Globalization;
using System.Reflection;
using Core;

namespace Application.Reporting
{
    public class DataProvider
    {
        public object Object
        {
            get
            {
                return this.@object;
            }
        }

        public MemberInfo Field
        {
            get
            {
                return this.field;
            }
        }

        public string ProposedColumnHeaderText
        {
            get 
            {
                return field.Name;
            }
        }

        public DataProvider()
        {
        }

        public DataProvider(object @object, MemberInfo field)
        {
            this.@object = @object;
            this.field = field;
        }

        public virtual double? GetCurrentValue()
        {
            object result = null;

            if (field is FieldInfo)
            {
                result = ((FieldInfo)field).GetValue(this.@object);
            }
            else if (field is PropertyInfo)
            {
                result = ((PropertyInfo)field).GetValue(this.@object, null);
            }
            else
            {
                throw new InvalidOperationException();
            }

            if (result == null)
            {
                return null;
            }

            return (double)Convert.ChangeType(result, typeof(double), CultureInfo.InvariantCulture);
        }

        public DataProvider CloneFor(ExperimentBase experiment)
        {
            return DataSource
                .FindDataSource(experiment, this.@object.GetType().GetDisplayName())
                .ToDataProvider(this.field.Name);
        }

        private object @object;
        private MemberInfo field;
    }
}
