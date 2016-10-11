using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using DotRLGlueCodec.Types;

namespace Application.Integration.RLGlue
{
    public static class ConversionExtensions
    {
        public static State<TStateSpaceType> ToDotRL<TStateSpaceType>(this Observation observation)
        {
            return new State<TStateSpaceType>(
                getter[typeof(TStateSpaceType)](observation).Cast<TStateSpaceType>());
        }

        public static Action<TActionSpaceType> ToDotRL<TActionSpaceType>(this Action action)
        {
            return new Action<TActionSpaceType>(
                getter[typeof(TActionSpaceType)](action).Cast<TActionSpaceType>());
        }

        public static Observation ToRLGlue<TStateSpaceType>(this State<TStateSpaceType> state)
        {
            if (state.IsTerminal)
            {
                return null;
            }

            var result = new Observation();

            setter[typeof(TStateSpaceType)](result, state.StateVector);

            return result;
        }

        public static Action ToRLGlue<TActionSpaceType>(this Action<TActionSpaceType> action)
        {
            var result = new Action();

            setter[typeof(TActionSpaceType)](result, action.ActionVector);

            return result;
        }

        private static Dictionary<System.Type, System.Func<RLAbstractType, IEnumerable>> getter
            = new Dictionary<System.Type, System.Func<RLAbstractType, IEnumerable>>()
            {
            { typeof(int), t => t.IntArray },
            { typeof(double), t => t.DoubleArray }
            };

        private static Dictionary<System.Type, System.Action<RLAbstractType, IEnumerable>> setter
            = new Dictionary<System.Type, System.Action<RLAbstractType, IEnumerable>>()
            { 
            { typeof(int), (t, a) => t.SetIntArray(a.Cast<int>().ToArray()) },
            { typeof(double), (t, a) => t.SetDoubleArray(a.Cast<double>().ToArray()) } 
            };
    }
}
