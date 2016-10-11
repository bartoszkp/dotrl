using System;

namespace Core.Reporting
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ReportedValueAttribute : Attribute
    {
    }
}
