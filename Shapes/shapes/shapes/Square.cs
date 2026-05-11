using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    class Square : Shape
    {
      public double Side { get; set; }
      public double Area { get; private set; }
      public double Perimeter { get; private set; }
      public void Calculate () {
         Area = Side * Side;
         Perimeter = 4 * Side;
      }

   }
}
