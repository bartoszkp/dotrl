using System;

namespace Core
{
    public class ComponentType
    {
        public Type StateSpaceType { get; private set; }

        public Type ActionSpaceType { get; private set; }

        public ComponentType(Type stateSpaceType, Type actionSpaceType)
        {
            this.StateSpaceType = stateSpaceType;
            this.ActionSpaceType = actionSpaceType;
        }

        public override bool Equals(object obj)
        {
            ComponentType otherComponentType = obj as ComponentType;

            return otherComponentType != null
                && this.StateSpaceType.Equals(otherComponentType.StateSpaceType)
                && this.ActionSpaceType.Equals(otherComponentType.ActionSpaceType);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
