using Core.Parameters;

namespace Core
{
    public abstract class Component : IParametrizedObject
    {
        public IUserInitializationActionManager UserInitializationActionManager { get; set; }

        public abstract ComponentType ComponentType { get; }

        public abstract void ParametersChanged();

        public virtual Component Clone()
        {
            var result = this.GetType().InstantiateWithDefaultConstructor<Component>();

            result.CopyParametersFromWithInnerObjects(this);

            return result;
        }
    }
}
