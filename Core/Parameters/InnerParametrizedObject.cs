namespace Core.Parameters
{
    public class InnerParametrizedObject
    {
        public string ParentFieldName { get; set; }

        public IParametrizedObject ParametrizedObject { get; set; }
    }
}
