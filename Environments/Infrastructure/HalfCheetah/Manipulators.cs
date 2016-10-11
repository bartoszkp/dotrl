using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Environments.Infrastructure.HalfCheetah
{
    #region Basic manipulator
    public class A2D
    {
        private AJoint[] joints;

        public IList<AJoint> Joints
        {
            get
            {
                return new List<AJoint>(joints);
            }
        }

        #region Construction
        public A2D()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", Justification = "Code contract ensures no overflow will occur.")]
        public void Build(int count)
        {
            Contract.Requires(count > 0);

            joints = Enumerable
                .Range(0, count)
                .Select(i => new AJointActive(null, null))
                .ToArray();

            for (int i = 0; i < count - 1; i++)
            {
                joints[i].NextJoint = joints[i + 1];
                joints[i].NextLink = joints[i + 1].PrevLink = new ALink(1);
                joints[i + 1].PrevJoint = joints[i];
            }
        }

        public void SetCompensationTime(double compensationTime)
        {
            foreach (AJoint joint in joints)
            {
                joint.SetCompensationTime(compensationTime);
            }
        }

        public void SetLengths(double[] lengths)
        {
            Contract.Requires(lengths.Length == JointNo - 1);
            for (int i = 0; i < JointNo - 1; i++)
            {
                joints[i].NextLink.l = lengths[i];
            }
        }

        public void SetMass(int index, double mass)
        {
            joints[index].m = mass;
        }

        public void SetMasses(double[] masses)
        {
            Contract.Requires(masses.Length == JointNo);
            for (int i = 0; i < JointNo; i++)
            {
                joints[i].m = masses[i];
            }
        }

        public void SetLead(int jointIndex, AVector2D a_w)
        {
            joints[jointIndex].BecomeLed(a_w);
        }

        public void SetArcAcceleration(int jointIndex, double arcAcc)
        {
            AJointActive joint = (AJointActive)joints[jointIndex];
            joint.BecomeOfConstArcAcceleration(arcAcc);
        }

        public void SetStiff(int jointIndex, double arc)
        {
            AJointActive joint = (AJointActive)joints[jointIndex];
            joint.BecomeStiff(arc);
        }
        #endregion

        #region Properties
        protected AJoint First
        {
            get { return joints[0]; }
        }

        protected AJoint Last
        {
            get { return joints[joints.Length - 1]; }
        }

        protected int JointNo
        {
            get { return joints.Length; }
        }
        #endregion

        #region State
        public void SetVelocities(AVector2D[] vels)
        {
            Contract.Requires(vels.Length == JointNo);
            int i = 0;
            for (AJoint joint = First; joint != null; joint = joint.NextJoint)
            {
                joint.v = vels[i++];
            }
        }

        public AVector2D GetPosition(int jointIndex)
        {
            return joints[jointIndex].x;
        }

        public AVector2D GetVelocity(int jointIndex)
        {
            return joints[jointIndex].v;
        }

        public AVector2D GetAcceleration(int jointIndex)
        {
            return joints[jointIndex].a;
        }

        public AVector2D[] GetPositions()
        {
            AVector2D[] positions = new AVector2D[JointNo];
            int i = 0;
            for (AJoint joint = First; joint != null; joint = joint.NextJoint)
            {
                positions[i++] = joint.x;
            }

            return positions;
        }

        public AVector2D[] GetVelocities()
        {
            AVector2D[] velocities = new AVector2D[JointNo];
            int i = 0;
            for (AJoint joint = First; joint != null; joint = joint.NextJoint)
            {
                velocities[i++] = joint.v;
            }

            return velocities;
        }

        public AVector2D[] GetAccelerations()
        {
            AVector2D[] accelerations = new AVector2D[JointNo];
            int i = 0;
            for (AJoint joint = First; joint != null; joint = joint.NextJoint)
            {
                accelerations[i++] = joint.a;
            }

            return accelerations;
        }

        public double GetArc(int jointIndex)
        {
            return joints[jointIndex].PrevLink.r.AngleTo(joints[jointIndex].NextLink.r);
        }

        public double GetArcVelocity(int jointIndex)
        {
            AJoint joint = joints[jointIndex];
            return (joint.NextLink.L * (joint.NextJoint.v - joint.v) / joint.NextLink.l
                - joint.PrevLink.L * (joint.v - joint.PrevJoint.v) / joint.PrevLink.l);
        }

        public double GetArcAcceleration(int jointIndex)
        {
            AJoint joint = joints[jointIndex];
            return (joint.NextLink.L * (joint.NextJoint.a - joint.a) / joint.NextLink.l
                - joint.PrevLink.L * (joint.a - joint.PrevJoint.a) / joint.PrevLink.l);
        }

        public double[] GetLinkArcs()
        {
            double[] arcs = new double[JointNo - 1];
            int i = 0;
            AVector2D reference = new AVector2D(1, 0);
            for (AJoint joint = First; joint.NextJoint != null; joint = joint.NextJoint)
            {
                arcs[i++] = reference.AngleTo(joint.NextLink.r);
            }

            return arcs;
        }

        public double[] GetLinkArcVelocities()
        {
            double[] linkVels = new double[JointNo - 1];
            int i = 0;
            for (AJoint joint = First; joint.NextJoint != null; joint = joint.NextJoint)
            {
                linkVels[i++] = joint.NextLink.r.TurnedLeft() * (joint.NextJoint.v - joint.v) / joint.NextLink.l;
            }

            return linkVels;
        }

        public double[] GetLinkArcAccelerations()
        {
            double[] linkAccs = new double[JointNo - 1];
            int i = 0;
            for (AJoint joint = First; joint.NextJoint != null; joint = joint.NextJoint)
            {
                linkAccs[i++] = joint.NextLink.r.TurnedLeft() * (joint.NextJoint.a - joint.a) / joint.NextLink.l;
            }

            return linkAccs;
        }

        #endregion

        #region Calculations in time
        public void Calculate2ndDerivatives(bool gravitation_on, double[] taus, bool impact)
        {
            if (taus != null)
            {
                Contract.Requires(taus.Length == JointNo);
            }

            AJoint joint;
            for (joint = First; joint != null; joint = joint.NextJoint)
            {
                joint.InitFe();
            }

            if (gravitation_on)
            {
                for (int i = 0; i < JointNo; i++)
                {
                    joints[i].UpdateFeGravitation(9.81);
                }
            }

            if (taus != null)
            {
                for (int i = 0; i < JointNo; i++)
                {
                    joints[i].UpdateFeTorch(taus[i]);
                }
            }

            for (joint = Last; joint != null; joint = joint.PrevJoint)
            {
                joint.CalculateBackwards(impact);
            }

            for (joint = First; joint != null; joint = joint.NextJoint)
            {
                joint.CalculateForward();
            }
        }

        public void AStepForward(double dt)
        {
            AJoint joint;
            for (joint = First; joint != null; joint = joint.NextJoint)
            {
                joint.x += joint.v * dt + 0.5 * joint.a * dt * dt;
                joint.v += joint.a * dt;
            }

            for (joint = First; joint != null; joint = joint.NextJoint)
            {
                joint.PostCalculate();
            }
        }

        public void GoAhead(double[] taus, double dt)
        {
            Calculate2ndDerivatives(true, taus, false);
            AStepForward(dt);
        }
        #endregion
    }
    #endregion

    #region Various descriptions of a joint
    public class JointLinkConstants
    {
        public bool IsAlwaysStiff { get; set; }

        public double MinArc { get; set; }

        public double MaxArc { get; set; }

        public double Mass { get; set; }

        public double LinkLength { get; set; }

        public JointLinkConstants()
        {
        }

        public JointLinkConstants(
            bool is_always_stiff,
            double min_arc, 
            double max_arc,
            double mass, 
            double link_length)
        {
            IsAlwaysStiff = is_always_stiff;
            MinArc = min_arc;
            MaxArc = max_arc;
            Mass = mass;
            LinkLength = link_length;
            Verify();
        }

        public void Verify()
        {
            Contract.Requires(MinArc <= MaxArc);
        }
    }

    public class OutsideJointImage
    {
        public bool IsStanding { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D X { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D V { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D A { get; set; }

        public double TauW { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D R { get; set; }

        public double LinkArcVelocity { get; set; }

        public int Activity { get; set; }

        public OutsideJointImage()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public void Init(
            bool is_standing,
            AVector2D x,
            AVector2D v,
            AVector2D a,
            double tauW,
            AVector2D r,
            double linkArcVelocity, 
            int activity)
        {
            IsStanding = is_standing;
            this.X = x.Clone();
            this.V = v.Clone();
            this.A = a.Clone();
            this.TauW = tauW;
            this.R = r != null ? r.Clone() : null;
            this.LinkArcVelocity = linkArcVelocity;
            this.Activity = activity;
        }
    }
    #endregion

    #region 2Dimensional Walker
    public class A2DWalker : A2D
    {
        #region Internal image of state
        private class AJointIntImage
        {
            // constants
            public bool IsAlwaysStiff { get; set; }

            public double MinArc { get; set; }

            public double MaxArc { get; set; }

            public double LinkLength { get; set; }

            public bool IsGrounded { get; set; } // złącze jest przy ziemi

            public bool IsStanding { get; set; } // złącze jest prowadzone w związku z pobytem na ziemi

            public int ArcLocation { get; set; } // złącze jest przy ograniczeniu na kąt
            // 1 if Arc is on its UPPER bound
            // -1 if Arc is on its LOWER bound
            // 0 otherwise
            public int ArcActivity { get; set; } // złącze jest sztywne w związku z ograniczeniem na kąt
            // -1 if Arc is on its UPPER bound
            // 1 if Arc is on its LOWER bound
            // 0 otherwise
            public bool IsPlunging { get; set; }  // złącze wbija się w punkt

            public bool IsClenching { get; set; } // złącze zwiera się z swoim ograniczeniem kątowym

            public int NoNotStanding { get; set; } // ile razy złącze przestało stać w ciągu kwantu czasu

            public int NoNotActive { get; set; }   // ile razy złącze przestało być aktywne w ciągu kwantu czasu

            // the joint
            public AJointActive Joint { get; set; }

            // state
            public double Arc { get; set; }

            public double ArcVelocity { get; set; }

            public double ArcAcceleration { get; set; }

            public AJointIntImage()
            {
                IsPlunging = IsClenching = false;
                Arc = ArcVelocity = ArcAcceleration = double.NaN;
            }

            public AJointIntImage(JointLinkConstants jls, AJointActive joint)
                : this()
            {
                jls.Verify();
                IsAlwaysStiff = jls.IsAlwaysStiff;
                MinArc = jls.MinArc;
                MaxArc = jls.MaxArc;
                LinkLength = jls.LinkLength;
                Joint = joint;
            }

            public double GetArcBound()
            {
                Contract.Requires(ArcLocation == -1 || ArcLocation == 1, "Arc bound is undefined");
                if (ArcLocation == -1)
                {
                    return MinArc;
                }
                else
                {
                    return MaxArc;
                }
            }
        }
        #endregion

        #region Fields
        // by a difference of consecutive accelerations
        private double RealTime;

        // auxilaries
        public double ZeroThreshold { get; set; }

        public AVector2D Zero { get; set; }

        // state 
        private AJointIntImage[] jI;
        private bool axSet;          // are present accelerations computed
        private double axMaxDeltaV;  // max discrepancy of speed caused 
        #endregion

        #region Construction
        public A2DWalker()
            : base()
        {
            axMaxDeltaV = 1;
            RealTime = 0;
            ZeroThreshold = 0.01;
            Zero = new AVector2D(0, 0);
        }

        public void Build(JointLinkConstants[] constants)
        {
            double[] lengths = new double[constants.Length - 1];
            double[] masses = new double[constants.Length];
            int i = 0;
            for (i = 0; i < constants.Length - 1; i++)
            {
                lengths[i] = constants[i].LinkLength;
                masses[i] = constants[i].Mass;
            }

            masses[i] = constants[i].Mass;
            Build(constants.Length);
            SetLengths(lengths);
            SetMasses(masses);

            jI = new AJointIntImage[this.JointNo];
            for (i = 0; i < this.JointNo; i++)
            {
                jI[i] = new AJointIntImage(constants[i], (AJointActive)Joints[i]);
            }
        }

        public void SetZeroThreshold(double zero_threshold)
        {
            ZeroThreshold = zero_threshold;
        }
        #endregion

        #region State
        public OutsideJointImage[] GetOutsideJointImages()
        {
            OutsideJointImage[] jointImages = new OutsideJointImage[JointNo];

            int k = 0;
            for (int i = 0; i < JointNo; i++)
            {
                double link_arc_velocity = double.NaN;
                if (i != JointNo - 1)
                {
                    link_arc_velocity = jI[i].Joint.GetLinkArcVelocity();
                }

                if (jointImages[i] == null)
                {
                    jointImages[i] = new OutsideJointImage();
                }

                jointImages[k++].Init(
                    jI[i].IsStanding,
                    jI[i].Joint.x, jI[i].Joint.v, jI[i].Joint.a,
                    (jI[i].ArcActivity != 0) ? jI[i].Joint.TauW : 0,
                    (i != JointNo - 1) ? jI[i].Joint.NextLink.r : null,
                    link_arc_velocity, jI[i].ArcActivity);
            }

            return jointImages;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public void PutInPosition(AVector2D x, AVector2D r1, double[] arcs)
        {
            Contract.Requires(arcs.Length == JointNo - 2);

            Joints[0].x = x;
            Joints[0].v.FillWith(0);
            Joints[0].a.FillWith(0);
            Joints[0].NextLink.r = r1.Direction;
            double arc = (new AVector2D(1, 0)).AngleTo(r1);
            x += Joints[0].NextLink.r * jI[0].LinkLength;

            int i;
            for (i = 1; i < JointNo - 1; i++)
            {
                Joints[i].x = x;
                Joints[i].v.FillWith(0);
                Joints[i].a.FillWith(0);
                arc += Math.Min(Math.Max(jI[i].MinArc, arcs[i - 1]), jI[i].MaxArc);
                Joints[i].NextLink.r.x = Math.Cos(arc);
                Joints[i].NextLink.r.y = Math.Sin(arc);
                x += Joints[i].NextLink.r * jI[i].LinkLength;
            }

            for (i = 1; i < JointNo - 1; i++)
            {
                if (jI[i].IsAlwaysStiff)
                {
                    jI[i].Joint.BecomeStiff(Joints[i].GetArc());
                }
                else
                {
                    jI[i].Joint.BecomeNoStiff();
                }
            }

            Joints[i].x = x;
            Joints[i].v.FillWith(0);
            Joints[i].a.FillWith(0);

            for (i = 0; i < JointNo; i++)
            {
                jI[i].Joint.BecomeNoLed();
                jI[i].Joint.PostCalculate();
                jI[i].ArcActivity = jI[i].ArcLocation = 0;
                jI[i].IsGrounded = jI[i].IsStanding = jI[i].IsPlunging = jI[i].IsClenching = false;
            }

            UpdateInternalImages(true, true, true);

            axSet = false;
        }

        public void Translate(AVector2D delta)
        {
            for (int i = 0; i < JointNo; i++)
            {
                jI[i].Joint.x += delta;
            }
        }

        protected void UpdateInternalImages(bool positions, bool velocities, bool accelerations)
        {
            if (positions)
            {
                for (int i = 1; i < JointNo - 1; i++)
                {
                    jI[i].Arc = jI[i].Joint.GetArc();
                }
            }

            if (velocities)
            {
                for (int i = 1; i < JointNo - 1; i++)
                {
                    jI[i].ArcVelocity = jI[i].Joint.GetArcVelocity();
                }
            }

            if (accelerations)
            {
                for (int i = 1; i < JointNo - 1; i++)
                {
                    jI[i].ArcAcceleration = jI[i].Joint.GetArcAcceleration();
                }
            }
        }

        protected AVector2D[] SaveAx()
        {
            AVector2D[] ret = new AVector2D[JointNo];
            for (int i = 0; i < JointNo; i++)
            {
                ret[i] = Joints[i].a;
            }

            return ret;
        }

        protected void RestoreAx(AVector2D[] ax)
        {
            for (int i = 0; i < JointNo; i++)
            {
                Joints[i].a = ax[i];
            }
        }

        protected double GetMaxTimespan(AVector2D[] ax)
        {
            double max_arc_v = 1e-20;
            for (int i = 0; i < JointNo - 1; i++)
            {
                max_arc_v = Math.Max(max_arc_v, Math.Abs(Joints[i].GetLinkArcVelocity()));
            }

            double max_delta_a = 1e-20;
            for (int i = 0; i < JointNo; i++)
            {
                max_delta_a = Math.Max(max_delta_a, (Joints[i].a - ax[i]).Length);
            }

            return Math.Min(axMaxDeltaV / max_delta_a, 0.05 / max_arc_v);
        }

        #endregion

        #region Calculations in time
        protected void ComputeAccelerations(bool gravitation_on, double[] taus, bool impact)
        {
            Calculate2ndDerivatives(gravitation_on, taus, impact);
            UpdateInternalImages(false, false, true);
            int iteration = 0;
            bool once_again = true;
            while (once_again)
            {
                if (++iteration > 30)
                {
                    throw new InvalidOperationException("Shuffling");
                }

                once_again = false;

                // ewentualnie odrywamy od ziemi i od ograniczeń na kąty
                for (int i = 0; i < JointNo; i++)
                {
                    // weryfikacja, czy nie odrywa sie od gleby
                    if (!jI[i].IsPlunging && jI[i].IsStanding && iteration < JointNo && jI[i].NoNotStanding < 4)
                    {
                        AVector2D Fi = jI[i].Joint.Fe + jI[i].Joint.Fd;
                        if (i != JointNo - 1)
                        {
                            Fi -= Joints[i + 1].Fd;
                        }

                        if (Fi.y > ZeroThreshold)
                        {
                            jI[i].IsStanding = false;
                            jI[i].Joint.BecomeNoLed();
                            Calculate2ndDerivatives(gravitation_on, taus, impact);
                            UpdateInternalImages(false, false, true);
                            if (jI[i].Joint.a.y < 0)
                            {
                                jI[i].IsStanding = true;
                                jI[i].Joint.BecomeLed(Zero);
                                Calculate2ndDerivatives(gravitation_on, taus, impact);
                                UpdateInternalImages(false, false, true);
                            }
                            else
                            {
                                once_again = true;
                                break;
                            }
                        }
                    }

                    // weryfikacja czy nie odrywa się od ograniczenia na kąt
                    if (!jI[i].IsAlwaysStiff && !jI[i].IsClenching && jI[i].ArcActivity != 0 && iteration < JointNo && jI[i].NoNotActive < 4)
                    {
                        AJointActive joint = (AJointActive)Joints[i];
                        if (joint.TauW * jI[i].ArcActivity < -ZeroThreshold)
                        {
                            jI[i].ArcActivity = 0;
                            jI[i].Joint.BecomeNoStiff();
                            Calculate2ndDerivatives(gravitation_on, taus, impact);
                            UpdateInternalImages(false, false, true);
                            if (jI[i].ArcAcceleration * jI[i].ArcLocation > 0)
                            {
                                jI[i].ArcActivity = -jI[i].ArcLocation;
                                jI[i].Joint.BecomeStiff(jI[i].GetArcBound());
                                Calculate2ndDerivatives(gravitation_on, taus, impact);
                                UpdateInternalImages(false, false, true);
                            }
                            else
                            {
                                once_again = true;
                                break;
                            }
                        }
                    }

                    // czy aby jednak nie stoi
                    if (!jI[i].IsPlunging && jI[i].IsGrounded && !jI[i].IsStanding)
                    {
                        if (jI[i].Joint.a.y < 0)
                        {
                            jI[i].IsStanding = true;
                            jI[i].Joint.BecomeLed(new AVector2D(0, 0));
                            Calculate2ndDerivatives(gravitation_on, taus, impact);
                            UpdateInternalImages(false, false, true);
                            once_again = true;
                            break;
                        }
                    }

                    // czy aby ograniczenie nie powinno zadziałać
                    if (!jI[i].IsAlwaysStiff && !jI[i].IsClenching && jI[i].ArcLocation != 0 && jI[i].ArcActivity == 0)
                    {
                        if (jI[i].ArcLocation * jI[i].ArcAcceleration > 0)
                        {
                            jI[i].ArcActivity = -jI[i].ArcLocation;
                            jI[i].Joint.BecomeStiff(jI[i].GetArcBound());
                            Calculate2ndDerivatives(gravitation_on, taus, impact);
                            UpdateInternalImages(false, false, true);
                            once_again = true;
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < JointNo; i++)
            {
                if (jI[i].ArcLocation != 0 && jI[i].ArcActivity == 0)
                {
                    jI[i].ArcLocation = 0;
                    jI[i].NoNotActive++;
                }

                if (jI[i].IsGrounded && !jI[i].IsStanding)
                {
                    jI[i].IsGrounded = false;
                    jI[i].NoNotStanding++;
                }
            }

            axSet = true;
        }

        protected void ImpactCore()
        {
            UpdateInternalImages(false, true, false);
            ComputeAccelerations(false, null, true);

            for (int k = 0; k < JointNo; k++)
            {
                Joints[k].v = (Joints[k].v + Joints[k].a);
            }

            UpdateInternalImages(false, true, true);
            axSet = false;
        }

        protected void StopMovementDown(int jointIndex)
        {
            AJoint ith_joint = (AJoint)Joints[jointIndex];
            jI[jointIndex].IsPlunging = true; // wbicie się w punkt
            ith_joint.BecomeLed(-jI[jointIndex].Joint.v);
            ImpactCore();
            ith_joint.BecomeNoLed();
            jI[jointIndex].IsPlunging = false; // koniec wbijania się w punkt
            Joints[jointIndex].v.FillWith(0);
            ith_joint.BecomeNoLed();
        }

        protected void StopArcMovement(int index)
        {
            AJointActive ith_joint = (AJointActive)Joints[index];
            jI[index].ArcActivity = (int)Math.Sign(-jI[index].ArcVelocity);
            jI[index].IsClenching = true;
            ith_joint.BecomeOfConstArcAcceleration(-jI[index].ArcVelocity);
            ImpactCore();
            ith_joint.BecomeNoOfConstArcAcceleration();
            jI[index].IsClenching = false;
            jI[index].ArcActivity = 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        static public double GetTimeAfterImpact(double y, double v, double a)
        {
            if (y >= 0 || v >= 0)
            {
                return -1;
            }

            double delta = v * v - a * y * 2;
            if (delta < 0)
            {
                delta = 0;
            }

            double epsilon;
            if (a != 0)
            {
                epsilon = (v + Math.Sqrt(delta)) / a;
            }
            else
            {
                epsilon = (y / v);
            }

            return (epsilon);
        }

        new public void GoAhead(double[] taus, double dt)
        {
            for (int i = 0; i < JointNo; i++)
            {
                jI[i].NoNotActive = 0;
                jI[i].NoNotStanding = 0;
            }

            PrvGoAhead(taus, dt, 0);
        }

        private void PrvGoAhead(double[] taus, double dt, int goAheadLevel)
        {
            Contract.Requires(goAheadLevel <= 40, "Zapętlenie");
            ++goAheadLevel;

            if (!axSet)
            {
                ComputeAccelerations(true, taus, false);
            }

            AVector2D[] ax = this.SaveAx();

            AStepForward(dt);
            RealTime += dt;
            UpdateInternalImages(true, true, false);

            ComputeAccelerations(true, taus, false);
            UpdateInternalImages(false, false, true);
            double max_prev_timespan = GetMaxTimespan(ax);
            if (max_prev_timespan < dt)
            {
                RestoreAx(ax);
                AStepForward(-dt);
                RealTime -= dt;
                UpdateInternalImages(true, true, true);
                PrvGoAhead(taus, dt * 0.5, goAheadLevel);
                PrvGoAhead(taus, dt * 0.5, goAheadLevel);
                return;
            }

            int the_i = 0, the_event = 0;
            double the_epsilon = -0.5;
            for (int i = 0; i < JointNo; i++)
            {
                if (!jI[i].IsStanding && jI[i].Joint.x.y < 0 && jI[i].Joint.v.y < 0)
                {
                    double ith_epsilon = GetTimeAfterImpact(jI[i].Joint.x.y, jI[i].Joint.v.y, jI[i].Joint.a.y);
                    if (ith_epsilon > the_epsilon)
                    {
                        the_epsilon = ith_epsilon;
                        the_i = i;
                        the_event = 1;
                    }
                }

                // czy kąt złącza nie stał się większy niż maksymalny
                if (!jI[i].IsAlwaysStiff && jI[i].ArcActivity == 0 && jI[i].Arc > jI[i].MaxArc && jI[i].ArcVelocity > 0)
                {
                    int kkk = i + 1;
                    double ith_epsilon = GetTimeAfterImpact(
                        jI[i].MaxArc - jI[i].Arc,
                        -jI[i].ArcVelocity,
                        -jI[i].ArcAcceleration);
                    if (ith_epsilon > the_epsilon)
                    {
                        the_epsilon = ith_epsilon;
                        the_i = i;
                        the_event = 2;
                    }
                }

                // czy kąt złącza nie stał się mniejszy niż minimalny
                if (!jI[i].IsAlwaysStiff && jI[i].ArcActivity == 0 && jI[i].Arc < jI[i].MinArc && jI[i].ArcVelocity < 0)
                {
                    int kkk = i + 1;
                    double ith_epsilon = GetTimeAfterImpact(
                        jI[i].Arc - jI[i].MinArc,
                        jI[i].ArcVelocity,
                        jI[i].ArcAcceleration);
                    if (ith_epsilon > the_epsilon)
                    {
                        the_epsilon = ith_epsilon;
                        the_i = i;
                        the_event = 3;
                    }
                }
            }

            if (the_epsilon > 0)
            {
                the_epsilon = Math.Min(the_epsilon, dt);

                RestoreAx(ax);
                AStepForward(-the_epsilon);
                RealTime -= the_epsilon;
                UpdateInternalImages(true, true, true);
                switch (the_event)
                {
                    case 1:
                        jI[the_i].IsGrounded = jI[the_i].IsStanding = true;
                        StopMovementDown(the_i);
                        jI[the_i].IsGrounded = jI[the_i].IsStanding = true;
                        Joints[the_i].BecomeLed(new AVector2D(0, 0));
                        break;
                    case 2:
                        jI[the_i].ArcLocation = 1;
                        jI[the_i].ArcActivity = -1;
                        StopArcMovement(the_i);
                        jI[the_i].ArcLocation = 1;
                        jI[the_i].ArcActivity = -1;
                        ((AJointActive)Joints[the_i]).BecomeStiff(jI[the_i].GetArcBound());
                        break;
                    case 3:
                        jI[the_i].ArcLocation = -1;
                        jI[the_i].ArcActivity = 1;
                        StopArcMovement(the_i);
                        jI[the_i].ArcLocation = -1;
                        jI[the_i].ArcActivity = 1;
                        ((AJointActive)Joints[the_i]).BecomeStiff(jI[the_i].GetArcBound());
                        break;
                    default:
                        throw new ArgumentException("Something is going very wrong");
                }

                ComputeAccelerations(true, taus, false);
                UpdateInternalImages(true, true, true);
                PrvGoAhead(taus, the_epsilon, goAheadLevel);
            }
        }
        #endregion
    }
    #endregion
}
