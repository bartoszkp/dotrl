namespace Core.Parameters
{
    public class ParametersSnapshot
    {
        public string ParentFieldName { get; set; }

        public ParameterValuePair[] ParameterValues { get; set; }

        public ParametersSnapshot[] InnerSnapshots { get; set; }
    }
}
