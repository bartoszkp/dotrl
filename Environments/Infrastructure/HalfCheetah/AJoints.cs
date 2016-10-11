using System;
using System.Collections;
using System.Diagnostics.Contracts;
using Core;
using System.Collections.Generic;

namespace Environments.Infrastructure.HalfCheetah
{
    public class ALink
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public double l;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D r;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D L;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public ALink(double l)
        {
            this.l = l;
            r = L = new AVector2D(0, 0);
        }
    }

    #region AJoint - a basic joint and the ancestor for all the others
    public class AJoint
    {
        #region Fields
        private List<System.Func<bool, bool>> delegatesBackwards;
        private List<System.Func<bool>> delegatesForward;
        private List<System.Action> delegatesPostCalculate;

        // calculation 
        public double NumericError;
        public double CompensationTime;

        // configuration 
        public ALink PrevLink;
        public ALink NextLink;
        public AJoint PrevJoint;
        public AJoint NextJoint;

        // constant properies
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public double m;
        public AVector2D aW;

        // state
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D x;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D v;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D Fe;
        public AVector2D Fd;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D a;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AMatrix2x2 A;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AMatrix2x2 C;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D b;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D d;
        public string Variant;
        #endregion

        #region Properties
        public bool IsLed
        {
            get { return (aW != null); }
        }

        public bool IsBeginning
        {
            get { return (PrevJoint == null); }
        }

        public bool IsEnding
        {
            get { return (NextJoint == null); }
        }

        virtual public bool IsActive
        {
            get { return false; }
        }

        virtual public bool IsActivable
        {
            get { return false; }
        }

        public void BecomeLed(AVector2D aW)
        {
            this.aW = aW;
        }

        public void BecomeNoLed()
        {
            aW = null;
        }
        #endregion

        #region Construction
        public AJoint()
        {
            PrevLink = NextLink = null;
            PrevJoint = NextJoint = null;
            InitJoint();
        }

        public AJoint(ALink prev_link, AJoint prev_joint)
        {
            PrevLink = prev_link;
            PrevJoint = prev_joint;
            NextLink = null;
            NextJoint = null;
            InitJoint();
        }

        public void RegisterDelegates(
            System.Func<bool, bool> calculate_backwards,
            System.Func<bool> calculate_forward,
            System.Action post_calculate)
        {
            if (calculate_backwards != null)
            {
                delegatesBackwards.Insert(0, calculate_backwards);
            }

            if (calculate_forward != null)
            {
                delegatesForward.Insert(0, calculate_forward);
            }

            if (post_calculate != null)
            {
                delegatesPostCalculate.Add(post_calculate);
            }
        }

        public void SetNumericError(double numeric_error)
        {
            Contract.Requires(numeric_error >= 0);
            NumericError = numeric_error;
        }

        public void SetCompensationTime(double compensation_time)
        {
            CompensationTime = compensation_time;
        }
        #endregion

        #region State
        public double GetLinkArcVelocity()
        {
            return (NextJoint.v - v) * NextLink.L;
        }

        public double GetLinkArcAcceleration()
        {
            return (NextJoint.x - x) * NextLink.L;
        }

        public double GetArc()
        {
            return PrevLink.r.AngleTo(NextLink.r);
        }

        public double GetArcVelocity()
        {
            return (NextJoint.v - v) * NextLink.L - (v - PrevJoint.v) * PrevLink.L;
        }

        public double GetArcAcceleration()
        {
            return (NextJoint.a - a) * NextLink.L - (a - PrevJoint.a) * PrevLink.L;
        }
        #endregion

        #region Calculations
        public void InitFe()
        {
            Fe.x = Fe.y = 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public void UpdateFeGravitation(double g)
        {
            Fe.y -= m * g;
        }

        public void UpdateFeTorch(double tau)
        {
            if (PrevJoint != null)
            {
                PrevJoint.Fe += PrevLink.r.TurnedLeft() * (tau / PrevLink.l);
                Fe += PrevLink.r.TurnedRight() * (tau / PrevLink.l);
            }

            if (NextJoint != null)
            {
                Fe += NextLink.r.TurnedRight() * (tau / NextLink.l);
                NextJoint.Fe += NextLink.r.TurnedLeft() * (tau / NextLink.l);
            }
        }

        public void CalculateBackwards(bool impact)
        {
            foreach (var calculate_backwards in delegatesBackwards)
            {
                if (calculate_backwards(impact))
                {
                    return;
                }
            }
        }

        public void CalculateForward()
        {
            foreach (var calculate_forward in delegatesForward)
            {
                if (calculate_forward())
                {
                    return;
                }
            }
        }

        public void PostCalculate()
        {
            foreach (var post_calculate in delegatesPostCalculate)
            {
                post_calculate();
            }
        }

        #endregion

        #region Auxiliary functions
        protected static double StandarizedArc(double arc)
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
        #endregion

        #region Protected calculations

        protected AVector2D GetLeadingAcc()
        {
            return (aW);
        }

        protected double GetCentripetalAcc(bool impact)
        {
            if (impact)
            {
                return 0;
            }
            else
            {
                double e = (NextJoint.v - v) * (NextJoint.v - v) / NextLink.l;
                double compens = ((NextJoint.x - x) * NextLink.r - NextLink.l) / CompensationTime.Squared()
                    + (NextLink.r * (NextJoint.v - v)) * 2 / CompensationTime;
                return e + compens;
            }
        }
        #endregion
  
        private void InitJoint()
        {
            delegatesBackwards = new List<Func<bool, bool>>();
            delegatesForward = new List<Func<bool>>();
            delegatesPostCalculate = new List<Action>();
            RegisterDelegates(
                (bool impact) => PrvCalculateBackwards(impact),
                () => PrvCalculateForward(),
                () => PrvPostCalculate());
            NumericError = 0.00003;
            CompensationTime = 0.01;
            m = 0;
            aW = null;
            x = new AVector2D();
            v = new AVector2D();
            Fe = new AVector2D();
            Fd = new AVector2D();
            a = new AVector2D();
            A = new AMatrix2x2();
            b = new AVector2D();
            C = new AMatrix2x2();
            d = new AVector2D();
        }

        #region Private calculations
        private bool PrvCalculateBackwards(bool impact)
        {
            if (IsLed && (IsEnding || NextJoint.IsLed))
            {
                Variant = "1.1";
                A.FillWith(0);
                b = GetLeadingAcc();
                C.FillWith(0);
                d.FillWith(0);
            }
            else
            {
                if (IsLed)
                {
                    Variant = "1.2";
                    double e = GetCentripetalAcc(impact);
                    AVector2D r = NextLink.r;
                    A.FillWith(0);
                    b = GetLeadingAcc();

                    double scale = r * (NextJoint.A * r);
                    C.FillWith(0);
                    if (scale.Squared() > NumericError.Squared())
                    {
                        Variant = "1.2.1";
                        d = r * ((r * GetLeadingAcc() - r * NextJoint.b - e) / scale);
                    }
                    else
                    {
                        Variant = "1.2.2";
                        d.FillWith(0);
                    }
                }
                else
                {
                    if (IsEnding)
                    //// ruchome zlacze na koncu ukladu
                    {
                        Variant = "1.3";
                        A = AMatrix2x2.I() / m;
                        b = Fe / m;
                        C.FillWith(0);
                        d.FillWith(0);
                    }
                    else
                    //// zywkle ruchome zlacze w srodku ukladu
                    {
                        Variant = "1.4";
                        double e = GetCentripetalAcc(impact);
                        AVector2D r = NextLink.r;
                        AMatrix2x2 rrT = (r | r);
                        double scala = 1.0 + m * r * NextJoint.A * r;
                        A = AMatrix2x2.I() / m - rrT / (m * scala);
                        b = A * Fe
                            + r * ((r * NextJoint.b + e) / scala);
                        C = rrT / scala;
                        d = C * Fe
                            - r * (m * (r * NextJoint.b + e) / scala);
                    }
                }
            }

            return (true);
        }

        private bool PrvCalculateForward()
        {
            if (PrevJoint == null)
            {
                Fd.FillWith(0);
            }

            a = (A * Fd) + b;
            if (NextJoint != null)
            {
                NextJoint.Fd = (C * Fd) + d;
            }

            return (true);
        }

        private void PrvPostCalculate()
        {
            if (NextJoint != null)
            {
                AVector2D rl = NextJoint.x - x;
                NextLink.r = rl / rl.Length;
                NextLink.L = rl.TurnedLeft() / rl.LengthSq;
            }
        }
        #endregion
    }
    #endregion

    #region AJointActive - an active joint, controlling its anuglar acceleration
    public class AJointActive : AJoint
    {
        #region Fields
        // define properties 
        public double AlphaW { get; set; }

        public double ArcW { get; set; }

        // dynamics 
        public AVector2D Bw { get; set; }

        public AVector2D Dw { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D P { get; set; }

        public double Qw { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public double Q { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D G { get; set; }

        public double Hw { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public double H { get; set; }

        // calculated 
        public double TauW { get; set; }
        #endregion

        #region Properties
        public override bool IsActivable
        {
            get
            {
                return (true);
            }
        }

        override public bool IsActive
        {
            get { return (IsStiff || IsOfConstArcAcceleration); }
        }

        public bool IsOfConstArcAcceleration
        {
            get { return !double.IsNaN(AlphaW); }
        }

        public bool IsStiff
        {
            get { return (!double.IsNaN(ArcW)); }
        }

        // Modifications
        public void BecomeOfConstArcAcceleration(double arc_acceleration)
        {
            BecomeNoStiff();
            AlphaW = arc_acceleration;
        }

        public void BecomeNoOfConstArcAcceleration()
        {
            AlphaW = double.NaN;
        }

        public void BecomeStiff(double arcW)
        {
            BecomeNoOfConstArcAcceleration();
            this.ArcW = arcW;
        }

        public void BecomeNoStiff()
        {
            ArcW = double.NaN;
        }
        #endregion

        #region Construction
        public AJointActive()
            : base()
        {
            InitJointActive();
        }

        public AJointActive(ALink prev_link, AJoint prev_joint)
            : base(prev_link, prev_joint)
        {
            InitJointActive();
        }
        #endregion

        #region Calculations
        public double GetProperArcAcceleration(bool impact)
        {
            Contract.Requires(IsStiff || IsOfConstArcAcceleration, "The joint is not active");

            if (impact)
            {
                if (IsStiff)
                {
                    return (0);
                }
                else
                {
                    // if (IsOfConstArcAcceleration)
                    return (AlphaW);
                }
            }
            else
            {
                if (IsStiff)
                {
                    double e_arc = StandarizedArc(GetArc() - ArcW);
                    double e_darc = GetArcVelocity();
                    double acc = -e_arc / CompensationTime.Squared() - e_darc * 2 / CompensationTime;
                    return (acc);
                }
                else
                ////if (IsOfConstArcAcceleration)
                {
                    return (AlphaW);
                }
            }
        }

        private bool PrvCalculateBackwards(bool impact) // prawda jesli przejmuje odpowiedzialnosc za obliczenia. 
        {
            Bw.FillWith(0);
            Dw.FillWith(0);
            P.FillWith(0);
            G.FillWith(0);
            Qw = Q = Hw = H = 0;

            AJointActive NJ = (!IsEnding && NextJoint.IsActivable) ? (AJointActive)NextJoint : null;

            if (IsEnding || (!IsActive && !NJ.IsActive))
            {
                return (false);
            }
            else
                if (IsLed && NextJoint.IsActive && !NextJoint.IsEnding)
                {
                    #region złącze prowadzone i następne aktywne
                    Variant = "3.1";
                    double e = GetCentripetalAcc(impact);
                    AVector2D r = NextLink.r;
                    AVector2D L = NextLink.L;
                    AMatrix2x2 A_1 = NextJoint.A;
                    AVector2D b_1 = NextJoint.b;

                    double k0 = -r * (A_1 * r);
                    double k1 = r * (A_1 * L) - r * NJ.Bw;
                    double k3 = -r * (A_1 * L);
                    double k4 = r * GetLeadingAcc() - r * b_1 - e;

                    double k5 = NJ.P * r - L * (A_1 * r);
                    double k6 = -NJ.P * L + NJ.Qw + L * (A_1 * L) - L * NJ.Bw;
                    double k8 = NJ.P * L - L * (A_1 * L);
                    double k9 = NJ.Q - L * b_1 + L * GetLeadingAcc() - NJ.GetProperArcAcceleration(impact);

                    double k11 = k5 * k1 - k0 * k6;
                    if (Math.Abs(k11) < NumericError * 2)
                    {
                        // it is impossible to determine both force and torch
                        Variant = "3.1.1";
                        double eq1 = Math.Abs(k0) + Math.Abs(k1);
                        double eq2 = Math.Abs(k5) + Math.Abs(k6);
                        if (eq1 < NumericError || eq1 < eq2)
                        {
                            Variant = "3.1.1.1";
                            k0 = 1;
                            k1 = k3 = k4 = 0;
                        }

                        if (eq2 < NumericError || eq2 <= eq1)
                        {
                            Variant = "3.1.1.2";
                            k6 = 1;
                            k5 = k8 = k9 = 0;
                        }

                        k11 = k5 * k1 - k0 * k6;
                    }

                    double k14 = (k6 * k3 - k1 * k8) / k11;
                    double k15 = (k0 * k8 - k5 * k3) / k11;
                    double k16 = (k4 * k6 - k1 * k9) / k11;
                    double k17 = (k0 * k9 - k5 * k4) / k11;

                    G.FillWith(0);
                    Hw = k15;
                    H = k17;

                    C.FillWith(0);
                    Dw = r * k14 + L - L * k15;
                    d = r * k16 - L * k17;

                    A.FillWith(0);
                    Bw.FillWith(0);
                    b = GetLeadingAcc();

                    P.FillWith(0);
                    Qw = L * A_1 * Dw + L * NJ.Bw * Hw;
                    Q = L * A_1 * d + L * NJ.Bw * H + L * NJ.b - L * b;
                    #endregion
                }
                else
                    if (IsLed)
                    {
                        #region złącze prowadzone i następne nieaktywne
                        Variant = "3.2";
                        double e = GetCentripetalAcc(impact);
                        AVector2D r = NextLink.r;
                        AVector2D L = NextLink.L;
                        AMatrix2x2 A_1 = NextJoint.A;
                        AVector2D b_1 = NextJoint.b;

                        double scala = r * (A_1 * r);

                        if (Math.Abs(scala) > NumericError)
                        {   // jeśli się da, to wywrzej wpływ na i+1-sze złącze
                            Variant = "3.2.1";
                            C.FillWith(0);
                            Dw = L - (r * (r * (A_1 * L) / scala));
                            d = r * (r * (GetLeadingAcc() - b_1) - e) / scala;
                        }
                        else
                        {   // ale jeśli się nie da to odpuść. 
                            Variant = "3.2.2";
                            C.FillWith(0);
                            Dw.FillWith(0);
                            d.FillWith(0);
                        }

                        A.FillWith(0);
                        Bw.FillWith(0);
                        b = GetLeadingAcc();

                        P.FillWith(0);
                        Qw = L * A_1 * Dw - L * Bw;
                        Q = L * A_1 * d + L * b_1 - L * b;
                        #endregion
                    }
                    else
                        if (!NextJoint.IsActive || NextJoint.IsEnding)
                        {
                            #region złącze nieprowadzone i następne nieaktywne
                            Variant = "3.3";
                            double e = GetCentripetalAcc(impact);
                            AVector2D r = NextLink.r;
                            AVector2D L = NextLink.L;
                            AMatrix2x2 A_1 = NextJoint.A;
                            AVector2D b_1 = NextJoint.b;

                            double scala = r * (A_1 * r) * m + 1;

                            C = r | r / scala;
                            Dw = L - r * (r * (A_1 * L) * m / scala);
                            d = r * (r * Fe - (r * b_1 + e) * m) / scala;

                            A = (AMatrix2x2.I() - C) / m;
                            Bw = Dw / -m;
                            b = (Fe - d) / m;

                            P = L * A_1 * C - L * A;
                            Qw = L * A_1 * Dw - L * Bw;
                            Q = L * A_1 * d + L * b_1 - L * b;
                            #endregion
                        }
                        else
                        {
                            #region aktywne zlacze w srodku ukladu
                            Variant = "3.4";
                            double e = GetCentripetalAcc(impact);
                            AVector2D r = NextLink.r;
                            AVector2D L = NextLink.L;
                            AMatrix2x2 A_1 = NextJoint.A;
                            AVector2D b_1 = NextJoint.b;

                            double k0 = 1.0 / -m - r * (A_1 * r);
                            double k1 = r * (A_1 * L) - r * NJ.Bw;
                            AVector2D k2 = r / m;
                            double k3 = -r * (A_1 * L);
                            double k4 = r * Fe / m - r * b_1 - e;

                            double k5 = NJ.P * r - L * (A_1 * r);
                            double k6 = -NJ.P * L + NJ.Qw + L * (A_1 * L) - L * NJ.Bw + L.LengthSq / m;
                            AVector2D k7 = L / m;
                            double k8 = NJ.P * L - L * (A_1 * L) - L.LengthSq / m;
                            double k9 = NJ.Q - L * b_1 + L * Fe / m - NJ.GetProperArcAcceleration(impact);

                            double k11 = k5 * k1 - k0 * k6;
                            AVector2D k12 = (k6 * k2 - k1 * k7) / k11;
                            AVector2D k13 = (k0 * k7 - k5 * k2) / k11;
                            double k14 = (k6 * k3 - k1 * k8) / k11;
                            double k15 = (k0 * k8 - k5 * k3) / k11;
                            double k16 = (k4 * k6 - k1 * k9) / k11;
                            double k17 = (k0 * k9 - k5 * k4) / k11;

                            G = k13;
                            Hw = k15;
                            H = k17;

                            C = (r | k12) - (L | k13);
                            Dw = r * k14 + L - L * k15;
                            d = r * k16 - L * k17;

                            A = (AMatrix2x2.I() - C) / m;
                            Bw = Dw / -m;
                            b = (Fe - d) / m;

                            P = L * A_1 * C + L * NJ.Bw * G - L * A;
                            Qw = L * A_1 * Dw + L * NJ.Bw * Hw - L * Bw;
                            Q = L * A_1 * d + L * NJ.Bw * H + L * NJ.b - L * b;
                            #endregion
                        }

            return (true);
        }

        private bool PrvCalculateForward()
        {
            if (this.PrevJoint == null)
            {
                this.Fd.FillWith(0);
                this.TauW = 0;
            }

            this.a = this.A * this.Fd + this.Bw * this.TauW + this.b;

            if (this.NextJoint != null)
            {
                this.NextJoint.Fd = this.C * this.Fd + this.Dw * this.TauW + this.d;
                if (this.NextJoint.IsActivable)
                {
                    ((AJointActive)this.NextJoint).TauW = this.G * this.Fd + this.Hw * this.TauW + this.H;
                }
            }

            return (true);
        }
        #endregion

        private void InitJointActive()
        {
            RegisterDelegates(
                (bool impact) => PrvCalculateBackwards(impact),
                () => PrvCalculateForward(),
                null /*post calculate*/);
            BecomeNoOfConstArcAcceleration();
            BecomeNoStiff();
            Bw = new AVector2D();
            Dw = new AVector2D();
            P = new AVector2D();
            Qw = 0;
            Q = 0;
            G = new AVector2D();
            Hw = 0;
            H = 0;
        }
    }
    #endregion
}