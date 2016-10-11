using System;
using System.Reflection;
using Core;

namespace Application.Reporting
{
    public class FakeDataProvider : DataProvider
    {
        public static FakeDataProvider Create(DataProvider etalon)
        {
            if (etalon.Field.GetFieldOrPropertyType().Equals(typeof(int)))
            {
                if (etalon.Field.Name.ToLowerInvariant().Contains("step") || etalon.Field.Name.ToLowerInvariant().Contains("count"))
                {
                    return new FakeDataProvider(GeneratorType.Step, etalon.Object, etalon.Field);
                }

                return new FakeDataProvider(GeneratorType.Int, etalon.Object, etalon.Field);
            }

            return new FakeDataProvider(GeneratorType.Double, etalon.Object, etalon.Field);
        }

        private enum GeneratorType
        {
            Int,
            Step,
            Double
        }

        private FakeDataProvider(GeneratorType generatorType, object @object, MemberInfo field)
            : base(@object, field)
        {
            this.generatorType = generatorType;
            this.random = new Random();
            this.currentValue = 0;
        }

        public override double? GetCurrentValue()
        {
            switch (generatorType)
            {
                case GeneratorType.Int:
                    return random.Next(100);
                case GeneratorType.Step:
                    currentValue += 10;
                    return currentValue;
                case GeneratorType.Double:
                    return (random.NextDouble() - 0.5) * 20;
                default:
                    throw new NotImplementedException();
            }
        }

        private Random random;
        private double currentValue;
        private GeneratorType generatorType;
    }
}
