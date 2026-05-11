namespace Nexus.Core {
   public class Shape () {
      public int x;
      public int y;
      public virtual double Area () {
         return 0;
      }
      public virtual double Perimeter () { return 0; }
   }

   public class Rectangle () : Shape {
      public double length { get; set; }
      public double breadth { get; set; }
      public override double Area () {
         return length * breadth;
      }

      public override double Perimeter () {
         return 2 * (length + breadth);

      }
   }
   public class Circle () : Shape {
      public double Radius { get; set; }
      public override double Area () {
         return Math.PI * Radius * Radius;
      }

      public override double Perimeter () {
         return 2 * Math.PI * Radius;
      }

   }

}
