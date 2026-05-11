using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shapes
{
    interface Shape
    {
      double Area { get; }
      double Perimeter { get; }
      void Calculate ();
    }
}
