using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    using Cplx = Numerics.Complex;
    struct Complex : IComplexArithmetic<Cplx>
    {
        public Cplx Zero => 0;
        public Cplx One => 1;
        public Cplx ImaginaryUnit => new Cplx(0, 1);

        public Cplx Infinity => double.PositiveInfinity;
        public Cplx NaN => new Cplx(double.NaN, double.NaN);
        public Cplx PI => Math.PI;
        public Cplx E => Math.E;


        public Cplx RealPart(Cplx a) => a.Real;
        public Cplx ImaginaryPart(Cplx a) => a.Imaginary;

        public bool IsNaN(Cplx a)      => double.IsNaN(a.Real)      || double.IsNaN(a.Imaginary);
        public bool IsInfinity(Cplx a) => double.IsInfinity(a.Real) || double.IsInfinity(a.Imaginary);

        public Cplx Add(Cplx a, Cplx b) => a + b;
        public Cplx Subtract(Cplx a, Cplx b) => a - b;
        public Cplx Multiply(Cplx a, Cplx b) => a * b;
        public Cplx Divide(Cplx a, Cplx b) => a / b;

        public Cplx Negation(Cplx a) => -a;
        public Cplx Abs(Cplx a) => a.Magnitude;
        public Cplx Sign(Cplx a) => a / a.Magnitude;
        public bool AreEqual(Cplx a, Cplx b) => a == b;

        public bool AreApproximatelyEqual(Cplx a, Cplx b)
        {
            throw new NotImplementedException();
        }


        public Cplx Exp(Cplx a) => Cplx.Exp(a);
        public Cplx Pow(Cplx x, Cplx y) => Cplx.Pow(x, y);
        public Cplx Sqrt(Cplx a) => Cplx.Sqrt(a);

        public Cplx Log(Cplx a) => Cplx.Log(a);
        public Cplx Log10(Cplx a) => Cplx.Log10(a);

        public Cplx Sin(Cplx a) => Cplx.Sin(a);
        public Cplx Cos(Cplx a) => Cplx.Cos(a);

        public Cplx Asin(Cplx a) => Cplx.Asin(a);
        public Cplx Acos(Cplx a) => Cplx.Acos(a);
        public Cplx Tan(Cplx a) => Cplx.Tan(a);
        public Cplx Atan(Cplx a) => Cplx.Atan(a);
        public Cplx Atan2(Cplx y, Cplx x) => Cplx.Atan(y/x);
        public Cplx Sinh(Cplx a) => Cplx.Sinh(a);
        public Cplx Cosh(Cplx a) => Cplx.Cosh(a);
        public Cplx Tanh(Cplx a) => Cplx.Tanh(a);
    }
}
