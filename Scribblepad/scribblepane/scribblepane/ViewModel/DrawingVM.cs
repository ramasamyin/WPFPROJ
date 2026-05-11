using scribblepad.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scribblepad.ViewModel {
   class DrawingVM {
      public List<Stroke> Strokes { get; set; } = new ();

      private Stroke? currentStroke;

      public double BrushSize { get; set; } = 2;
      public string SelectedColor { get; set; } = "Black";

      public void StartStroke (Point p) {
         currentStroke = new Stroke {
            Color = SelectedColor,
            Thickness = BrushSize
         };
         currentStroke.Points.Add (p);
         Strokes.Add (currentStroke);
      }

      public void AddPoint (Point p) {
         currentStroke?.Points.Add (p);
      }

      public void EndStroke () {
         currentStroke = null;
      }

      public void Clear () {
         Strokes.Clear ();

      }
   }
}
