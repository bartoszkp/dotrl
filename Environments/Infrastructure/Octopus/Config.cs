using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    [XmlType(AnonymousType = true)]
    [XmlRoot("config", Namespace = "", IsNullable = false)]
    public class Config
    {
        [XmlElement("constants")]
        public ConstantSet Constants { get; set; }

        [XmlElement("environment")]
        public EnvSpec Environment { get; set; }
    }

    public class ConstantSet
    {
        [XmlElement("frictionTangential")]
        public double FrictionTangential { get; set; }

        [XmlElement("frictionPerpendicular")]
        public double FrictionPerpendicular { get; set; }

        [XmlElement("buoyancy")]
        public double Buoyancy { get; set; }

        [XmlElement("pressure")]
        public double Pressure { get; set; }

        [XmlElement("gravity")]
        public double Gravity { get; set; }

        [XmlElement("muscleActive")]
        public double MuscleActive { get; set; }

        [XmlElement("musclePassive")]
        public double MusclePassive { get; set; }

        [XmlElement("muscleNormalizedMinLength")]
        public double MuscleNormalizedMinLength { get; set; }

        [XmlElement("muscleDamping")]
        public double MuscleDamping { get; set; }

        [XmlElement("surfaceLevel")]
        public double SurfaceLevel { get; set; }

        [XmlElement("repulsionConstant")]
        public double RepulsionConstant { get; set; }

        [XmlElement("repulsionPower")]
        public double RepulsionPower { get; set; }

        [XmlElement("repulsionThreshold")]
        public double RepulsionThreshold { get; set; }
    }

    public class EnvSpec
    {
        [XmlElement("foodTask", Type = typeof(FoodTaskDef))]
        [XmlElement("targetTask", Type = typeof(TargetTaskDef))]
        [XmlElement("pointTask", Type = typeof(PointTaskDef))]
        public TaskDef Task { get; set; }

        [XmlElement("arm")]
        public ArmSpec Arm { get; set; }
    }

    public class ArmSpec
    {
        [XmlElement("nodePair")]
        public NodePairSpec[] NodePair { get; set; }
    }

    public class MouthSpec
    {
        [XmlAttribute(AttributeName = "x")]
        public double PositionX { get; set; }

        [XmlAttribute(AttributeName = "y")]
        public double PositionY { get; set; }

        [XmlAttribute(AttributeName = "width")]
        public double Width { get; set; }

        [XmlAttribute(AttributeName = "height")]
        public double Height { get; set; }
    }

    [XmlInclude(typeof(TargetTaskDef))]
    [XmlInclude(typeof(FoodTaskDef))]
    [XmlInclude(typeof(PointTaskDef))]
    public abstract class TaskDef
    {
        [XmlAttribute(DataType = "positiveInteger", AttributeName = "timeLimit")]
        public string TimeLimit { get; set; }

        [XmlAttribute(AttributeName = "stepReward")]
        public double StepReward { get; set; }
    }

    [XmlInclude(typeof(FoodSpec))]
    public class NodeSpec
    {
        [XmlAttribute(AttributeName = "mass")]
        public double Mass { get; set; }

        [XmlAttribute(AttributeName = "position")]
        public string PositionString { get; set; }

        [XmlAttribute(AttributeName = "velocity")]
        public string VelocityString { get; set; }

        [XmlIgnore]
        public IList<double> Position
        {
            get
            {
                return PositionString.Split(' ').Select(v => double.Parse(v, CultureInfo.InvariantCulture)).ToList<double>();
            }
        }

        [XmlIgnore]
        public IList<double> Velocity
        {
            get
            {
                if (string.IsNullOrEmpty(VelocityString) || VelocityString == null)
                {
                    return new List<double>(new double[] { 0, 0 });
                }

                return VelocityString.Split(' ').Select(v => double.Parse(v, CultureInfo.InvariantCulture)).ToList<double>();
            }
        }
    }

    public class FoodSpec : NodeSpec
    {
        [XmlAttribute(AttributeName = "reward")]
        public double Reward { get; set; }
    }

    public class NodePairSpec
    {
        [XmlElement("upper")]
        public NodeSpec Upper { get; set; }

        [XmlElement("lower")]
        public NodeSpec Lower { get; set; }
    }

    [XmlRoot("foodTask", Namespace = "", IsNullable = false)]
    public class FoodTaskDef : TaskDef
    {
        [XmlElement("mouth")]
        public MouthSpec Mouth { get; set; }

        [XmlElement("food")]
        public FoodSpec[] Food { get; set; }
    }

    [XmlRoot("pointTask", Namespace = "", IsNullable = false)]
    public class PointTaskDef : TaskDef
    {
        [XmlAttribute(AttributeName = "reward")]
        public double Reward { get; set; }

        [XmlAttribute(AttributeName = "minTargetRadius")]
        public double MinTargetRadius { get; set; }

        [XmlAttribute(AttributeName = "maxTargetRadius")]
        public double MaxTargetRadius { get; set; }
    }

    [XmlRoot("targetTask", Namespace = "", IsNullable = false)]
    public class TargetTaskDef : TaskDef
    {
        [XmlElement("choice", Type = typeof(ChoiceSpec))]
        [XmlElement("sequence", Type = typeof(SequenceSpec))]
        [XmlElement("target", Type = typeof(TargetSpec))]
        public ObjectiveSpec Objective { get; set; }
    }

    [XmlInclude(typeof(TargetSpec))]
    [XmlInclude(typeof(ObjectiveSetSpec))]
    [XmlInclude(typeof(SequenceSpec))]
    [XmlInclude(typeof(ChoiceSpec))]
    public abstract class ObjectiveSpec
    {
    }

    [XmlInclude(typeof(SequenceSpec))]
    [XmlInclude(typeof(ChoiceSpec))]
    public class ObjectiveSetSpec : ObjectiveSpec
    {
        [XmlElement("choice", Type = typeof(ChoiceSpec))]
        [XmlElement("sequence", Type = typeof(SequenceSpec))]
        [XmlElement("target", Type = typeof(TargetSpec))]
        public ObjectiveSpec[] Objective { get; set; }
    }

    [XmlRoot("choice", Namespace = "", IsNullable = false)]
    public class ChoiceSpec : ObjectiveSetSpec
    {
    }

    [XmlRoot("sequence", Namespace = "", IsNullable = false)]
    public class SequenceSpec : ObjectiveSetSpec
    {
    }

    [XmlRoot("target", Namespace = "", IsNullable = false)]
    public class TargetSpec : ObjectiveSpec
    {
        [XmlAttribute(AttributeName = "position")]
        public double[] Position { get; set; }

        [XmlAttribute(AttributeName = "reward")]
        public double Reward { get; set; }
    }
}