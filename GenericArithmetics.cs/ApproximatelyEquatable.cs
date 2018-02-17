using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.Extensions
{
    public interface IApproximatelyEquatable<T> : IEquatable<T> where T : IEquatable<T>
    {

        /**
         * <summary>
         *     Defines a generalized method for determining approximate equality of instances.
         *     If all parameters except <paramref name="other"/> are null, default values are used.
         * </summary>
         * 
         * <param name="other">Another instance to compare with this one.</param>
         * <param name="delta">
         *     Maximum absolute difference between the values. If null, then it is assumed to be infinity.
         *     If all parameters except <paramref name="other"/> are null, delta is assumed to be 1e-15.
         * </param>
         * <param name="relativeDelta">
         *     Maximum relative difference between the values. If null, then it is assumed to be infinity.
         * </param>
         * <param name="ulpsApart">
         *     Maximum number of possible representations between the two values. If null, then it is assumed to be infinity.
         *     If all parameters except <paramref name="other"/> are null, ulpsApart is assumed to be 10.
         * </param>
         * 
         * <returns>
         *     A boolean value indicating whether at least one test was passed. False when comparing NaNs.
         * </returns>
        **/
        bool ApproximatelyEquals(
            T other,
            double? delta = null,
            double? relativeDelta = null,
            long? ulpsApart = null
        );
    }


    
    public static class ApproximatelyEquatable
    {
        /// <see cref="IApproximatelyEquatable{T}.ApproximatelyEquals(T, double?, double?, long?)"/>
        public static bool ApproximatelyEquals(
            this double self,
            double other,
            double? delta = null,
            double? relativeDelta = null,
            long? ulpsApart = null
        )
        {
            if (double.IsNaN(self) || double.IsNaN(other)) return false;

            if (delta == null && relativeDelta == null && ulpsApart == null)
            {
                delta = 1e-15;
                ulpsApart = 10;
            }

            var diff = Math.Abs(other - self);
            var relDiff = Math.Abs(diff / self);

            if (delta != null && diff <= delta) return true;

            if (relativeDelta != null && relDiff <= relativeDelta) return true;

            if (ulpsApart != null && Math.Abs(self.UlpsApart(other)) <= ulpsApart) return true;
            

            return false;
        }


        /// <see cref="IApproximatelyEquatable{T}.ApproximatelyEquals(T, double?, double?, long?)"/>
        public static bool ApproximatelyEquals(
            this float self,
            float other,
            double? delta = null,
            double? relativeDelta = null,
            long? ulpsApart = null
        )
        {
            if (double.IsNaN(self) || double.IsNaN(other)) return false;

            if (delta == null && relativeDelta == null && ulpsApart == null)
            {
                delta = 1e-15;
                ulpsApart = 10;
            }

            var diff = Math.Abs(other - self);
            var relDiff = Math.Abs(diff / self);

            if (delta != null && diff <= delta) return true;

            if (relativeDelta != null && relDiff <= relativeDelta) return true;

            if (ulpsApart != null && Math.Abs(self.UlpsApart(other)) <= ulpsApart) return true;


            return false;
        }


        public static IApproximatelyEquatable<double> ToApproximatelyEquatable(this double d) => (ApproximatelyEquatableDouble)d;
        public static IApproximatelyEquatable<float > ToApproximatelyEquatable(this float  f) => (ApproximatelyEquatableSingle)f;

        public struct ApproximatelyEquatableDouble : IApproximatelyEquatable<double>, IApproximatelyEquatable<ApproximatelyEquatableDouble>
        {
            double Value;
            public ApproximatelyEquatableDouble(double value) => Value = value;

            public bool ApproximatelyEquals(double other, double? delta = null, double? relativeDelta = null, long? ulpsApart = null)
                => Value.ApproximatelyEquals(other, delta, relativeDelta, ulpsApart);
            
            public bool ApproximatelyEquals(ApproximatelyEquatableDouble other, double? delta = null, double? relativeDelta = null, long? ulpsApart = null)
                => Value.ApproximatelyEquals(other.Value, delta, relativeDelta, ulpsApart);

            public bool Equals(double other) => Value == other;

            public bool Equals(ApproximatelyEquatableDouble other) => Value == other.Value;

            public static implicit operator double(ApproximatelyEquatableDouble d) => d.Value;
            public static implicit operator ApproximatelyEquatableDouble(double d) => new ApproximatelyEquatableDouble(d);
        }

        public struct ApproximatelyEquatableSingle : IApproximatelyEquatable<float>, IApproximatelyEquatable<ApproximatelyEquatableSingle>
        {
            float Value;
            public ApproximatelyEquatableSingle(float value) => Value = value;

            public bool ApproximatelyEquals(float other, double? delta = null, double? relativeDelta = null, long? ulpsApart = null)
                => Value.ApproximatelyEquals(other, delta, relativeDelta, ulpsApart);
            
            public bool ApproximatelyEquals(ApproximatelyEquatableSingle other, double? delta = null, double? relativeDelta = null, long? ulpsApart = null)
                => Value.ApproximatelyEquals(other.Value, delta, relativeDelta, ulpsApart);

            public bool Equals(float other) => Value == other;

            public bool Equals(ApproximatelyEquatableSingle other) => Value == other.Value;

            public static implicit operator float(ApproximatelyEquatableSingle f) => f.Value;
            public static implicit operator ApproximatelyEquatableSingle(float f) => new ApproximatelyEquatableSingle(f);
            public static implicit operator ApproximatelyEquatableDouble(ApproximatelyEquatableSingle f) => new ApproximatelyEquatableDouble(f.Value);
        }

    }

    
}
