#region License
/* ****************************************************************************
 * Copyright (c) Llewellyn Pritchard. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. 
 * A copy of the license can be found in the License.html file at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 * ***************************************************************************/
#endregion


using System;
using System.Diagnostics;
using System.Text;
using System.Collections;
using Microsoft.Scripting.Math;

namespace IronScheme.Runtime
{
  /// <summary>
  /// Implementation of the complex number data type.
  /// </summary>
  [Serializable]
  public class ComplexFraction
  {
    private readonly Fraction real, imag;

    public static ComplexFraction MakeImaginary(Fraction imag)
    {
      return new ComplexFraction(0, imag);
    }

    public static ComplexFraction MakeReal(Fraction real)
    {
      return new ComplexFraction(real, 0);
    }

    public static ComplexFraction Make(Fraction real, Fraction imag)
    {
      return new ComplexFraction(real, imag);
    }

    public ComplexFraction(Fraction real)
      : this(real, 0)
    {
    }

    public ComplexFraction(Fraction real, Fraction imag)
    {
      this.real = real;
      this.imag = imag;
    }

    public Complex64 ToComplex64()
    {
      return this;
    }

    public bool IsZero
    {
      get
      {
        return real == 0 && imag == 0;
      }
    }

    public Fraction Real
    {
      get
      {
        return real;
      }
    }

    public Fraction Imag
    {
      get
      {
        return imag;
      }
    }

    public ComplexFraction Conjugate()
    {
      return new ComplexFraction(real, 0 - imag);
    }


    public override string ToString()
    {
      if (real == 0) return imag.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat) + "i";
      else if (imag < 0) return string.Format(System.Globalization.CultureInfo.InvariantCulture.NumberFormat, "{0}{1}i", real, imag);
      else return string.Format(System.Globalization.CultureInfo.InvariantCulture.NumberFormat, "{0}+{1}i", real, imag);
    }

    public static implicit operator ComplexFraction(int i)
    {
      return MakeReal(i);
    }



    public static implicit operator ComplexFraction(short i)
    {
      return MakeReal(i);
    }



    [CLSCompliant(false)]
    public static implicit operator ComplexFraction(sbyte i)
    {
      return MakeReal(i);
    }

    public static implicit operator ComplexFraction(byte i)
    {
      return MakeReal(i);
    }


    public static implicit operator ComplexFraction(BigInteger i)
    {
      if (object.ReferenceEquals(i, null))
      {
        throw new ArgumentNullException("i");
      }

      // throws an overflow exception if we can't handle the value.
      return MakeReal(i);
    }

    public static bool operator ==(ComplexFraction x, ComplexFraction y)
    {
      return x.real == y.real && x.imag == y.imag;
    }

    public static bool operator !=(ComplexFraction x, ComplexFraction y)
    {
      return x.real != y.real || x.imag != y.imag;
    }

    public static ComplexFraction Add(ComplexFraction x, ComplexFraction y)
    {
      return x + y;
    }

    public static ComplexFraction operator +(ComplexFraction x, ComplexFraction y)
    {
      return new ComplexFraction(x.real + y.real, x.imag + y.imag);
    }

    public static ComplexFraction Subtract(ComplexFraction x, ComplexFraction y)
    {
      return x - y;
    }

    public static ComplexFraction operator -(ComplexFraction x, ComplexFraction y)
    {
      return new ComplexFraction(x.real - y.real, x.imag - y.imag);
    }

    public static ComplexFraction Multiply(ComplexFraction x, ComplexFraction y)
    {
      return x * y;
    }

    public static ComplexFraction operator *(ComplexFraction x, ComplexFraction y)
    {
      return new ComplexFraction(x.real * y.real - x.imag * y.imag, x.real * y.imag + x.imag * y.real);
    }

    public static ComplexFraction Divide(ComplexFraction x, ComplexFraction y)
    {
      return x / y;
    }

    public static ComplexFraction operator /(ComplexFraction a, ComplexFraction b)
    {
      if (b.IsZero) throw new DivideByZeroException();

      Fraction real, imag, den, r;

      if (b.real.Abs() >= b.imag.Abs())
      {
        r = b.imag / b.real;
        den = b.real + r * b.imag;
        real = (a.real + a.imag * r) / den;
        imag = (a.imag - a.real * r) / den;
      }
      else
      {
        r = b.real / b.imag;
        den = b.imag + r * b.real;
        real = (a.real * r + a.imag) / den;
        imag = (a.imag * r - a.real) / den;
      }

      return new ComplexFraction(real, imag);
    }

    public static ComplexFraction Mod(ComplexFraction x, ComplexFraction y)
    {
      return x % y;
    }

    public static ComplexFraction operator %(ComplexFraction x, ComplexFraction y)
    {
      if (object.ReferenceEquals(x, null))
      {
        throw new ArgumentNullException("x");
      }
      if (object.ReferenceEquals(y, null))
      {
        throw new ArgumentNullException("y");
      }

      if (y == 0) throw new DivideByZeroException();

      throw new NotImplementedException();
    }

    public static ComplexFraction Negate(ComplexFraction x)
    {
      return -x;
    }

    public static ComplexFraction operator -(ComplexFraction x)
    {
      return new ComplexFraction(0 - x.real, 0 - x.imag);
    }

    public static ComplexFraction Plus(ComplexFraction x)
    {
      return +x;
    }

    public static ComplexFraction operator +(ComplexFraction x)
    {
      return x;
    }

    public static Fraction Hypot(Fraction x1, Fraction y1)
    {
      return Complex64.Hypot((double)x1, (double)y1);
    }

    public Fraction Abs()
    {
      return Hypot(real, imag);
    }

    public static implicit operator Complex64(ComplexFraction cf)
    {
      return new Complex64((double)cf.real, (double)cf.imag);
    }

    public static implicit operator ComplexFraction(Complex64 cf)
    {
      return new ComplexFraction(cf.Real, cf.Imag);
    }


    public ComplexFraction Power(ComplexFraction y)
    {
      return ((Complex64)this).Power(y);
    }

    public override int GetHashCode()
    {
      return (int)real + (int)imag * 1000003;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is ComplexFraction)) return false;
      return this == ((ComplexFraction)obj);
    }
  }
}