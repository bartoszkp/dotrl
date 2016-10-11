using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Parameters;
using Environments.Infrastructure.HalfCheetah;

namespace Environments.ContinuousStateContinuousDecision
{
    public class HalfCheetah : Environment<double, double>
    {
        [Parameter]
        private ControlType controlType = ControlType.StabilizedProportionalDerivative;
        [Parameter(0, 10000)]
        private double controlKp = 2;
        [Parameter(0, 10000)]
        private double controlKd = 0.05;
        [Parameter(0, 10000)]
        private double overloadPenalty = 0.1;
        [Parameter(0, 10000)]
        private double touchPenalty = 1;
        [Parameter(0, 10000)]
        private double faceFallPenalty = 0;
        [Parameter(0, 10000)]
        private double trunkWeight = 1;
        [Parameter(0, 10000)]
        private double feetWeight = 1;
        [Parameter(0, 10000)]
        private double maxTau = 50;
        [Parameter(0.00001, 0.5)]
        private double internalDiscretization = 0.002;
        [Parameter(0.001, 0.5)]
        private double externalDiscretization = 0.02;
        [Parameter(0, 10000)]
        private double tauPenalty = 0.05;
        [Parameter(0, 10000)]
        private double crushPenalty = 3;

        public HalfCheetah()
        {
            theObject = new A2DWalker();

            objConsts = new JointLinkConstants[10];
            objConsts[0] = new JointLinkConstants(false, double.NaN, double.NaN, 1, 0.2 / Math.Cos(Math.PI / 12));
            objConsts[1] = new JointLinkConstants(false, -Math.PI * 5 / 6, -Math.PI * 1 / 6, 1, 0.15 / Math.Cos(Math.PI * 4 / 12));
            objConsts[2] = new JointLinkConstants(false, Math.PI * 1 / 4, Math.PI * 3 / 4, 1, 0.25 / Math.Cos(Math.PI * 2 / 12));
            objConsts[3] = new JointLinkConstants(false, -Math.PI * 5 / 6, -Math.PI * 1 / 3, 4, 1);
            objConsts[4] = new JointLinkConstants(true, Math.PI / 6, Math.PI / 6, 1, 0.3);
            objConsts[5] = new JointLinkConstants(true, -Math.PI, -Math.PI, 2, 0.3);
            objConsts[6] = new JointLinkConstants(false, -Math.PI / 18, Math.PI / 2, 1, 0.25 / Math.Cos(Math.PI / 9));
            objConsts[7] = new JointLinkConstants(false, 0, Math.PI * 8 / 9, 1, 0.2 / Math.Cos(Math.PI / 9));
            objConsts[8] = new JointLinkConstants(false, -Math.PI * 2 / 3, Math.PI / 9, 1, 0.15 / Math.Cos(Math.PI / 9));
            objConsts[9] = new JointLinkConstants(false, double.NaN, double.NaN, 1, double.NaN);
            theObject.Build(objConsts);
            nominalArcs
                = new double[] 
                { 
                    -Math.PI * 5 / 12, Math.PI / 2, -Math.PI * 4 / 6, Math.PI / 6, 
                    -Math.PI, Math.PI / 6, Math.PI * 5 / 18, 0 
                };

            p = Enumerable
                .Range(1, objConsts.Length - 2)
                .Where(i => !objConsts[i].IsAlwaysStiff)
                .ToArray();

            maxTaus = new double[] { 60, 90, 120, 90, 60, 30 };

            CurrentState = new MutableState<double>(31);
            UpdateCurrentState();
        }

        public override EnvironmentDescription<double, double> GetEnvironmentDescription()
        {
            double[] average = new double[]
            {
                0.1, 0, 0.5, 0.6, 1, 0, 0, 0.1, 0, 0.5,
                0, 0, 0,
                0, 0, 0,
                0, 0, 0,
                0, 0, 0,
                0, 0, 0,
                0, 0, 0,
                0, 0, 0
            };
            double[] standardDeviation = new double[]
            {
                0.1, 1, 0.5, 0.2, 2, 0.7, 0.7, 0.1, 1, 0.5,
                0.7, 0.7, 6, 
                0.7, 0.7, 6, 
                0.7, 0.7, 6, 
                0.7, 0.7, 6, 
                0.7, 0.7, 6, 
                0.7, 0.7, 6, 
                0.7, 0.7, 6, 
            };
            
            SpaceDescription<double> stateSpaceDescription
                = new SpaceDescription<double>(null, null, average, standardDeviation);

            SpaceDescription<double> actionSpaceDescription
                = new SpaceDescription<double>(Enumerable.Repeat(-1.0, 6).ToArray(), Enumerable.Repeat(1.0, 6).ToArray());

            DimensionDescription<double> reinforcementSpaceDescription
                = new DimensionDescription<double>(-1, 1, 0, 1);

            return new EnvironmentDescription<double, double>(stateSpaceDescription, actionSpaceDescription, reinforcementSpaceDescription, 0.9);
        }

        public override void StartEpisode()
        {
            AVector2D x0 = new AVector2D(0, 0.1);
            AVector2D r0 = new AVector2D(-Math.Tan(Math.PI / 12), 1);
            theObject.PutInPosition(x0, r0, nominalArcs);
            UpdateCurrentState();
        }

        public override Reinforcement PerformAction(Core.Action<double> action)
        {
            double reward = 0;
            double[] taus = Enumerable.Repeat(0.0, 10).ToArray();
            double[] u = new double[6];

            double arc, d_arc;

            for (double t = 0; t < externalDiscretization; t += internalDiscretization)
            {
                double dt = Math.Min(internalDiscretization, externalDiscretization - t);
                double[] arcs = theObject.GetLinkArcs();
                double[] dArcs = theObject.GetLinkArcVelocities();
                for (int i = 0; i < 6; i++)
                {
                    switch (controlType)
                    {
                        case ControlType.Direct:
                            u[i] = Math.Min(Math.Max(-1, action[i]), 1);
                            reward -= Math.Abs(action[i] - u[i]) * tauPenalty * dt / externalDiscretization;
                            taus[p[i]] = u[i] * maxTau;
                            break;
                        case ControlType.ProportionalDerivative:
                            u[i] = Math.Min(Math.Max(-1, action[i]), 1);
                            reward -= Math.Abs(action[i] - u[i]) * tauPenalty * dt / externalDiscretization;
                            arc = arcs[p[i]] - arcs[p[i] - 1];
                            arc -= nominalArcs[i] + u[i] * (objConsts[p[i]].MaxArc - nominalArcs[p[i]]);
                            arc = NormalizedArc(arc);
                            d_arc = dArcs[p[i]] - dArcs[p[i] - 1];
                            taus[p[i]] = Math.Min(Math.Max(-maxTau, -arc * controlKp - d_arc * controlKd), maxTau);
                            break;
                        case ControlType.StabilizedProportionalDerivative:
                            arc = arcs[p[i]] - arcs[p[i] - 1];
                            arc -= nominalArcs[p[i] - 1];
                            arc = NormalizedArc(arc);
                            d_arc = dArcs[p[i]] - dArcs[p[i] - 1];
                            double pd_ctrl = Math.Atan(-arc * controlKp - d_arc * controlKd) * 2 / Math.PI;
                            u[i] = pd_ctrl + action[i];
                            taus[p[i]] = Math.Min(Math.Max(-1, u[i]), 1);
                            reward -= Math.Abs(taus[p[i]] - u[i]) * tauPenalty * dt / externalDiscretization;
                            taus[p[i]] *= maxTaus[i];
                            break;
                        default:
                            throw new InvalidOperationException("Unknown control type");
                    }
                }

                theObject.GoAhead(taus, dt);
            }

            UpdateCurrentState();

            if (jointImages[4].X.x > 6)
            {
                theObject.Translate(new AVector2D(-5, 0));
            }

            // system 6
            double speed = 0.5 * (jointImages[3].V.x + jointImages[4].V.x);
            double back = Math.Max(Math.Min(trunkWeight * jointImages[3].V.y, feetWeight), feetWeight * SoftStep01((jointImages[0].IsStanding ? 0 : 0.5) + jointImages[0].X.y * 5));
            double front = Math.Max(Math.Min(trunkWeight * jointImages[4].V.y, feetWeight), feetWeight * SoftStep01((jointImages[9].IsStanding ? 0 : 0.5) + jointImages[9].X.y * 5));
            double overload = 0;
            for (int i = 0; i < 6; i++)
            {
                overload += Math.Min(Math.Abs(jointImages[p[i]].TauW), maxTau);
            }

            reward += speed;
            reward -= overload * overloadPenalty;
            reward += (Math.Max(back, front) - 1) * (1.0 - SoftStep01(speed));
            reward += faceFallPenalty * (SoftStep01(speed) - 1) *
                 (-jointImages[6].V.y > 0 && jointImages[6].V.x - jointImages[9].V.x > 0 && jointImages[6].X.x > jointImages[9].X.x ? 1 : 0) *
                 (-jointImages[6].V.y + jointImages[6].V.x - jointImages[9].V.x);
            reward += touchPenalty * (SoftStep01(jointImages[1].X.y * 9) - 1);
            reward += touchPenalty * (SoftStep01(jointImages[2].X.y * 5) - 1);
            reward += touchPenalty * (SoftStep01(jointImages[5].X.y * 2) - 1);

            if (!IsStateOK())
            {
                reward -= crushPenalty;
            }

            return reward;
        }

        public bool IsStateOK()
        {
            for (int i = 0; i < CurrentState.StateVector.Length; i++)
            {
                if (double.IsNaN(CurrentState[i])

                    || double.IsInfinity(CurrentState[i])
                    || Math.Abs(CurrentState[i]) > 1e6)
                {
                    return (false);
                }
            }

            return (true);
        }

        public IEnumerable<double> GetJointPositions()
        {
            return Enumerable
                .Range(0, 20)
                .Select(i => (i % 2) == 0 ? jointImages[i / 2].X.x : jointImages[i / 2].X.y)
                .ToArray();
        }

        private static double NormalizedArc(double arc)
        {
            while (arc > Math.PI)
            {
                arc -= Math.PI * 2;
            }

            while (arc < -Math.PI)
            {
                arc += Math.PI * 2;
            }

            return (arc);
        }

        private static double SoftStep01(double x)
        {
            if (x < 0)
            {
                return 0;
            }

            if (x < 0.5)
            {
                return x * x * 2;
            }

            if (x < 1.0)
            {
                return 1.0 - (x - 1) * (x - 1) * 2;
            }

            return 1.0;
        }

        private void UpdateCurrentState()
        {
            jointImages = theObject.GetOutsideJointImages();
            int i = 0;

            CurrentState[i++] = jointImages[0].X.y;
            CurrentState[i++] = jointImages[0].V.y;
            CurrentState[i++] = jointImages[0].IsStanding ? 1 : 0;
            CurrentState[i++] = 0.5 * (jointImages[3].X.y + jointImages[4].X.y);
            CurrentState[i++] = 0.5 * (jointImages[3].V.x + jointImages[4].V.x);
            CurrentState[i++] = jointImages[3].V.y;
            CurrentState[i++] = jointImages[4].V.y;
            CurrentState[i++] = jointImages[9].X.y;
            CurrentState[i++] = jointImages[9].V.y;
            CurrentState[i++] = jointImages[9].IsStanding ? 1 : 0;
            CurrentState[i++] = jointImages[0].R.x;
            CurrentState[i++] = jointImages[0].R.y;
            CurrentState[i++] = jointImages[0].LinkArcVelocity;

            for (int k = 0; k < 6; k++)
            {
                CurrentState[i++] = jointImages[p[k]].R.x;
                CurrentState[i++] = jointImages[p[k]].R.y;
                CurrentState[i++] = jointImages[p[k]].LinkArcVelocity;
            }

            CurrentState.IsTerminal = !IsStateOK();
        }

        private enum ControlType
        {
            Direct,
            ProportionalDerivative,
            StabilizedProportionalDerivative
        }

        private double[] maxTaus;
        private JointLinkConstants[] objConsts;
        private A2DWalker theObject;
        private double[] nominalArcs;
        private int[] p;
        private OutsideJointImage[] jointImages;
    }
}
