using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace shapes
{
    class Circle : Shape
    {
      public double Radius { get; set; }
      public double Area { get; private set; }
      public double Perimeter { get; private set; }
      public void Calculate () {
         Area = Math.PI * Radius * Radius;
         Perimeter = 2 * Math.PI * Radius;
      }

   }
}
