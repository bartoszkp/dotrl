namespace Core
{
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    public class Reinforcement
    {
        public double Value { get; private set; }

        public Reinforcement(double value)
        {
            this.Value = value;
        }

        public static double ToDouble(Reinforcement reinforcement)
        {
            return reinforcement.Value;
        }

        public static Reinforcement FromDouble(double value)
        {
            return new Reinforcement(value);
        }

        public static implicit operator double(Reinforcement reinforcement)
        {
            return reinforcement.Value;
        }

        public static implicit operator Reinforcement(double value)
        {
            return new Reinforcement(value);
        }
    }
}
