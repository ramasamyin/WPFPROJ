using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    class Rectangle : Shape
    {
      public double Length { get; set; }
      public double Breadth { get; set; }
      public double Area { get; private set; }
      public double Perimeter { get; private set; }
      public void Calculate () {
         Area = Length * Breadth;
         Perimeter = 2 * (Length + Breadth);
      }
    }
}
