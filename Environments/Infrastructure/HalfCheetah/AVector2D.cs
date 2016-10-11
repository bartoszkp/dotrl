using System;

namespace Environments.Infrastructure.HalfCheetah
{
    public class AVector2D
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public double x;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public double y;

        #region Operators
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1013:OverloadOperatorEqualsOnOverloadingAddAndSubtract", Justification = "Backwards compatibility code")]
        public static AVector2D operator +(AVector2D aVector1, AVector2D aVector2)
        {
            return new AVector2D(aVector1.x + aVector2.x, aVector1.y + aVector2.y);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1013:OverloadOperatorEqualsOnOverloadingAddAndSubtract", Justification = "Backwards compatibility code")]
        public static AVector2D operator -(AVector2D aVector1, AVector2D aVector2)
        {
            return new AVector2D(aVector1.x - aVector2.x, aVector1.y - aVector2.y);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1013:OverloadOperatorEqualsOnOverloadingAddAndSubtract", Justification = "Backwards compatibility code")]
        public static AVector2D operator -(AVector2D aVector2D)
        {
            return new AVector2D(-aVector2D.x, -aVector2D.y);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AVector2D operator *(AVector2D aVector2D, double number)
        {
            return new AVector2D(aVector2D.x * number, aVector2D.y * number);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AVector2D operator *(double number, AVector2D aVector2D)
        {
            return aVector2D * number;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AVector2D operator /(AVector2D aVector, double number)
        {
            return new AVector2D(aVector.x / number, aVector.y / number);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static double operator *(AVector2D aVector1, AVector2D aVector2)
        {
            return (aVector1.x * aVector2.x + aVector1.y * aVector2.y);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AVector2D operator &(AVector2D aVector1, AVector2D aVector2)
        {
            return new AVector2D(aVector1.x * aVector2.x, aVector1.y * aVector2.y);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 operator |(AVector2D aVector1, AVector2D aVector2)
        {
            return new AMatrix2x2(
                aVector1.x * aVector2.x, 
                aVector1.x * aVector2.y,
                aVector1.y * aVector2.x, 
                aVector1.y * aVector2.y);
        }
        #endregion

        public AVector2D()
        {
            x = y = 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AVector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public AVector2D Clone()
        {
            return new AVector2D(x, y);
        }

        public void FillWith(double value)
        {
            x = y = value;
        }

        public double LengthSq
        {
            get { return (x * x + y * y); }
        }

        public double Length
        {
            get { return Math.Sqrt(x * x + y * y); }
        }

        public AVector2D Direction
        {
            get { return this / Length; }
        }

        #region Geometry
        public double AngleTo(AVector2D vector2D)
        {
            double l_prod = Math.Sqrt((this * this) * (vector2D * vector2D));
            double sin = (this.x * vector2D.y - this.y * vector2D.x) / l_prod;
            double cos = (this.x * vector2D.x + this.y * vector2D.y) / l_prod;
            if (sin >= 0.7) 
            {
                // 0.7<sqrt(0.5)~=0.707
                return Math.Acos(cos);
            }
            else
            {
                if (cos >= 0.7)
                {
                    return Math.Asin(sin);
                }
                else
                {
                    if (sin <= -0.7)
                    {
                        return -Math.Acos(cos);
                    }
                    else
                    {
                        if (sin < 0)
                        {
                            return -Math.PI - Math.Asin(sin);
                        }
                        else
                        {
                            // if (sin>=0 && cos<=-0.7)
                            return Math.PI - Math.Asin(sin);
                        }
                    }
                }
            }
        }

        public AVector2D TurnedLeft()
        {
            return new AVector2D(-y, x);
        }

        public AVector2D TurnedRight()
        {
            return new AVector2D(y, -x);
        }
        #endregion
    }

    public class AMatrix2x2
    {
        public double xx, xy;
        public double yx, yy;

        #region Operators
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1013:OverloadOperatorEqualsOnOverloadingAddAndSubtract", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 operator +(AMatrix2x2 matrix1, AMatrix2x2 matrix2)
        {
            return new AMatrix2x2(
                matrix1.xx + matrix2.xx,
                matrix1.xy + matrix2.xy,
                matrix1.yx + matrix2.yx,
                matrix1.yy + matrix2.yy);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1013:OverloadOperatorEqualsOnOverloadingAddAndSubtract", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 operator -(AMatrix2x2 matrix1, AMatrix2x2 matrix2)
        {
            return new AMatrix2x2(
                matrix1.xx - matrix2.xx,
                matrix1.xy - matrix2.xy,
                matrix1.yx - matrix2.yx,
                matrix1.yy - matrix2.yy);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1013:OverloadOperatorEqualsOnOverloadingAddAndSubtract", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 operator -(AMatrix2x2 matrix)
        {
            return new AMatrix2x2(
                -matrix.xx,
                -matrix.xy,
                -matrix.yx,
                -matrix.yy);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 operator *(AMatrix2x2 matrix1, AMatrix2x2 matrix2)
        {
            return new AMatrix2x2(
                matrix1.xx * matrix2.xx + matrix1.xy * matrix2.yx, 
                matrix1.xx * matrix2.xy + matrix1.xy * matrix2.yy,
                matrix1.yx * matrix2.xx + matrix1.yy * matrix2.yx, 
                matrix1.yx * matrix2.xy + matrix1.yy * matrix2.yy);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AVector2D operator *(AVector2D vector2D, AMatrix2x2 matrix)
        {
            return new AVector2D(
                vector2D.x * matrix.xx + vector2D.y * matrix.yx,
                vector2D.x * matrix.xy + vector2D.y * matrix.yy);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AVector2D operator *(AMatrix2x2 matrix, AVector2D vector2D)
        {
            return new AVector2D(
                matrix.xx * vector2D.x + matrix.xy * vector2D.y,
                matrix.yx * vector2D.x + matrix.yy * vector2D.y);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 operator *(AMatrix2x2 matrix, double factor)
        {
            return new AMatrix2x2(
                matrix.xx * factor,
                matrix.xy * factor,
                matrix.yx * factor,
                matrix.yy * factor);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 operator *(double factor, AMatrix2x2 matrix)
        {
            return matrix * factor;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 operator /(AMatrix2x2 matrix, double factor)
        {
            return new AMatrix2x2(
                matrix.xx / factor,
                matrix.xy / factor,
                matrix.yx / factor,
                matrix.yy / factor);
        }
        #endregion

        #region Construction
        public AMatrix2x2()
        {
            xx = xy = yx = yy = 0;
        }

        public AMatrix2x2(double xx, double xy, double yx, double yy)
        {
            this.xx = xx;
            this.xy = xy;
            this.yx = yx;
            this.yy = yy;
        }

        public AMatrix2x2(AMatrix2x2 matrix)
        {
            xx = matrix.xx; 
            xy = matrix.xy;
            yx = matrix.yx; 
            yy = matrix.yy;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public static AMatrix2x2 I()
        {
            return new AMatrix2x2(1, 0, 0, 1);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Backwards compatibility code")]
        public AMatrix2x2 T
        {
            get
            {
                return new AMatrix2x2(xx, yx, xy, yy);
            }
            set
            {
                xx = value.xx; 
                xy = value.yx;
                yx = value.xy; 
                yy = value.yy;
            }
        }

        public void FillWith(double val)
        {
            xx = xy = yx = yy = val;
        }
        #endregion
    }
}
