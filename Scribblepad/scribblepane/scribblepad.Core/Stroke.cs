using System.Drawing;

namespace scribblepad.Core {
   public class Stroke {
      public string Color { get; set; } = "Black";
      public double Thickness { get; set; }
      public List<Point> Points { get; set; } = [];
   }
}
