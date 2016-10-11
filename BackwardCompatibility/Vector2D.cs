using System;
using System.Collections.Generic;

namespace BackwardCompatibility
{
    public class Vector2D
    {
        public static Vector2D ZERO 
        {
            get
            {
                return new Vector2D(0.0, 0.0);
            }
        }

        public double PositionX 
        { 
            get 
            { 
                return positionX; 
            } 

            set 
            { 
                positionX = value; 
                RefreshNorm(); 
            } 
        }

        public double PositionY 
        { 
            get 
            { 
                return positionY; 
            } 

            set 
            { 
                positionY = value; 
                RefreshNorm(); 
            } 
        }

        public double Norm { get; private set; }

        public static Vector2D FromDuple(IList<double> duple)
        {
            return new Vector2D(duple[0], duple[1]);
        }

        public Vector2D(double positionX, double positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            RefreshNorm();
        }

        public Vector2D Add(Vector2D otherOne)
        {
            return new Vector2D(this.PositionX + otherOne.PositionX, this.PositionY + otherOne.PositionY);
        }

        public Vector2D AddScaled(Vector2D otherOne, double scale)
        {
            return new Vector2D(this.PositionX + scale * otherOne.PositionX, this.PositionY + scale * otherOne.PositionY);
        }

        public Vector2D Subtract(Vector2D otherOne)
        {
            return new Vector2D(this.PositionX - otherOne.PositionX, this.PositionY - otherOne.PositionY);
        }

        public Vector2D Scale(double factor)
        {
            return new Vector2D(factor * PositionX, factor * PositionY);
        }

        public Vector2D Normalize()
        {
            return ScaleTo(1.0);
        }

        public Vector2D ScaleTo(double factor)
        {
            if (Norm > 0.0)
            {
                return Scale(factor / Norm);
            }
            else
            {
                return this;
            }
        }

        public double Dot(Vector2D otherOne)
        {
            return this.PositionX * otherOne.PositionX + this.PositionY * otherOne.PositionY;
        }

        public double CrossMag(Vector2D otherOne)
        {
            return this.PositionX * otherOne.PositionY - otherOne.PositionX * this.PositionY;
        }

        public bool IsLeftFrom(Vector2D otherOne)
        {
            return SignSinDifference(this, otherOne) == 1;
        }

        public bool IsPerpenticularTo(Vector2D otherOne)
        {
            return SignSinDifference(this, otherOne) == 0;
        }

        public bool IsRightFrom(Vector2D otherOne)
        {
            return SignSinDifference(this, otherOne) == -1;
        }

        public double GetDistanceFrom(Vector2D otherOne)
        {
            return GetDistanceFrom(otherOne.PositionX, otherOne.PositionY);
        }

        public double GetDistanceFrom(double positionX, double positionY)
        {
            positionX = this.PositionX - positionX;
            positionY = this.PositionY - positionY;
            return Math.Sqrt(positionX * positionX + positionY * positionY);
        }

        public double AngleTo(Vector2D otherOne) // angle between v and this
        {
            double l_prod = Math.Sqrt((PositionX * PositionX + PositionY * PositionY) * (otherOne.PositionX * otherOne.PositionX + otherOne.PositionY * otherOne.PositionY));
            double sin = (this.PositionX * otherOne.PositionY - this.PositionY * otherOne.PositionX) / l_prod;
            double cos = (this.PositionX * otherOne.PositionX + this.PositionY * otherOne.PositionY) / l_prod;
            if (sin >= 0.7) 
            {
                // 0.7<sqrt(0.5)~=0.707
                return Math.Acos(cos);
            }
            else
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
                            //	if (sin>=0 && cos<=-0.7)
                            return Math.PI - Math.Asin(sin);
                        }
                    }
                }
        }

        /// <summary>
        /// Rotate a vector 90 degrees counterclockwise 
        /// </summary>
        public Vector2D Rotate90()
        {
            return new Vector2D(-PositionY, PositionX);
        }

        /// <summary>
        /// Rotate a vector 270 degrees counterclockwise 
        /// </summary>
        public Vector2D Rotate270()
        {
            return new Vector2D(PositionY, -PositionX);
        }

        public System.Drawing.PointF ToPoint2D()
        {
            return new System.Drawing.PointF((float)PositionX, (float)PositionY);
        }

        public override string ToString()
        {
            object[] objectArray = new object[] { PositionX, PositionY };
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "[{0:F}, {1:F}]", objectArray);
        }

        private double positionX;
        private double positionY;

        private static int SignSinDifference(Vector2D vector1, Vector2D vector2)
        {
            return Math.Sign(vector1.positionY * vector2.positionX - vector1.positionX * vector2.positionY);
        }

        private void RefreshNorm()
        {
            Norm = Math.Sqrt(positionX * positionX + positionY * positionY);
        }
    }
}