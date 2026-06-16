using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace scribblepane {
   public partial class MainWindow : Window {
      private Polyline? currentLine;
      private Polyline? selectedLine;
      private bool isDrawing = false;
      private bool isDragging = false;
      private Point lastDragPoint;
      private Stack<UIElement> undoStack = new ();
      private Stack<UIElement> redoStack = new ();

      // ZOOM + PAN
      private ScaleTransform scaleTransform = new ();
      private TranslateTransform translateTransform = new ();
      private TransformGroup transformGroup = new ();

      private bool isPanning = false;
      private Point panStart;
      private bool spacePressed = false;

      public MainWindow () {
         InitializeComponent ();
         transformGroup.Children.Add (scaleTransform);
         transformGroup.Children.Add (translateTransform);
         DrawCanvas.RenderTransform = transformGroup;
      }

      private void Canvas_MouseDown (object sender, MouseButtonEventArgs e) {
         Point clickedPoint = e.GetPosition (DrawCanvas);
         // PAN MODE
         if (spacePressed) {
            isPanning = true;
            panStart = e.GetPosition (this);
            return;
         }

         // CHECK IF CLICKED ON EXISTING STROKE
         foreach (UIElement element in DrawCanvas.Children) {
            if (element is Polyline line) {
               if (line.IsMouseOver) {
                  SelectLine (line);
                  isDragging = true;
                  lastDragPoint = clickedPoint;
                  return;
               }
            }
         }

         // START DRAWING
         isDrawing = true;
         currentLine = new Polyline {
            Stroke = GetSelectedColor (),
            StrokeThickness = BrushSlider.Value
         };
         currentLine.Points.Add (clickedPoint);
         DrawCanvas.Children.Add (currentLine);
         undoStack.Push (currentLine);
         redoStack.Clear ();
      }

      private void Canvas_MouseMove (object sender, MouseEventArgs e) {
         Point currentPoint = e.GetPosition (DrawCanvas);
         // PAN
         if (isPanning) {
            Point current = e.GetPosition (this);
            translateTransform.X += current.X - panStart.X;
            translateTransform.Y += current.Y - panStart.Y;
            panStart = current;
            return;
         }

         // DRAG SELECTED LINE
         if (isDragging && selectedLine != null) {
            double dx = currentPoint.X - lastDragPoint.X;
            double dy = currentPoint.Y - lastDragPoint.Y;
            for (int i = 0; i < selectedLine.Points.Count; i++) {
               Point p = selectedLine.Points[i];
               selectedLine.Points[i] = new Point (p.X + dx, p.Y + dy);
            }
            lastDragPoint = currentPoint;
            return;
         }

         // DRAW
         if (isDrawing && currentLine != null) {
            currentLine.Points.Add (currentPoint);
         }
      }

      private void Canvas_MouseUp (object sender, MouseButtonEventArgs e) {
         isDrawing = false;
         isDragging = false;
         isPanning = false;
      }

      // =========================
      // SELECT LINE
      // =========================

      private void SelectLine (Polyline line) {
         if (selectedLine != null) {
            selectedLine.StrokeThickness -= 2;
         }
         selectedLine = line;
         selectedLine.StrokeThickness += 2;
      }

      // =========================
      // ZOOM
      // =========================

      private void Canvas_MouseWheel (object sender, MouseWheelEventArgs e) {
         double zoom = e.Delta > 0 ? 1.1 : 0.9;
         scaleTransform.ScaleX *= zoom;
         scaleTransform.ScaleY *= zoom;
      }

      // =========================
      // KEY EVENTS
      // =========================

      private void Window_KeyDown (object sender, KeyEventArgs e) {
         if (e.Key == Key.Space) {
            spacePressed = true;
         }
      }

      private void Window_KeyUp (object sender, KeyEventArgs e) {
         if (e.Key == Key.Space) {
            spacePressed = false;
         }
      }

      // =========================
      // BUTTONS
      // =========================

      private void Clear_Click (object sender, RoutedEventArgs e) {
         DrawCanvas.Children.Clear ();
         selectedLine = null;
         undoStack.Clear ();
         redoStack.Clear ();
      }

      private void Undo_Click (object sender, RoutedEventArgs e) {
         if (DrawCanvas.Children.Count > 0) {
            UIElement last = DrawCanvas.Children[^1];
            DrawCanvas.Children.Remove (last);
            redoStack.Push (last);
         }
      }

      private void Redo_Click (object sender, RoutedEventArgs e) {
         if (redoStack.Count > 0) {
            UIElement item = redoStack.Pop ();
            DrawCanvas.Children.Add (item);
         }
      }

      private void Delete_Click (object sender, RoutedEventArgs e) {
         if (selectedLine != null) {
            DrawCanvas.Children.Remove (selectedLine);
            selectedLine = null;
         }
      }

      // =========================
      // COLOR
      // =========================

      private SolidColorBrush GetSelectedColor () {
         var selectedItem = ColorPicker.SelectedItem as ComboBoxItem;
         string color = selectedItem?.Content.ToString () ?? "Black";
         return color switch {
            "Red" => Brushes.Red,
            "Green" => Brushes.Green,
            "Blue" => Brushes.Blue,
            _ => Brushes.Black
         };
      }

      // =========================
      // SAVE DATA CLASS
      // ========================= 

      public class StrokeData {
         public string? Color { get; set; }
         public double Thickness { get; set; }
         public List<Point>? Points { get; set; }
      }

      // =========================
      // SAVE
      // =========================

      private void Save_Click (object sender, RoutedEventArgs e) {
         SaveFileDialog dlg = new ();
         dlg.Filter = "JSON (*.json)|*.json|Binary (*.bin)|*.bin";
         if (dlg.ShowDialog () == true) {
            var strokes = new List<StrokeData> ();
            foreach (var item in DrawCanvas.Children) {
               if (item is Polyline line) {
                  strokes.Add (new StrokeData {
                     Color = line.Stroke.ToString (),
                     Thickness = line.StrokeThickness,
                     Points = [.. line.Points]
                  });
               }
            }

            // JSON
            if (dlg.FilterIndex == 1) {
               string json = JsonSerializer.Serialize (strokes);
               File.WriteAllText (dlg.FileName, json);
            }

            // BINARY
            else {
               using var fs = new FileStream (
                   dlg.FileName,
                   FileMode.Create
               );
               using var bw = new BinaryWriter (fs);
               bw.Write (strokes.Count);
               foreach (var s in strokes) {
                  bw.Write (s.Color ?? "Black");
                  bw.Write (s.Thickness);
                  bw.Write (s.Points?.Count ?? 0);
                  if (s.Points != null) {
                     foreach (var p in s.Points) {
                        bw.Write (p.X);
                        bw.Write (p.Y);
                     }
                  }
               }
            }
         }
      }

      // =========================
      // LOAD
      // =========================

      private void Load_Click (object sender, RoutedEventArgs e) {
         OpenFileDialog dlg = new ();
         dlg.Filter = "JSON (*.json)|*.json|Binary (*.bin)|*.bin";
         if (dlg.ShowDialog () == true) {
            DrawCanvas.Children.Clear ();

            // JSON
            if (dlg.FilterIndex == 1) {
               string json = File.ReadAllText (dlg.FileName);
               var strokes = JsonSerializer.Deserialize<List<StrokeData>> (json);
               foreach (var s in strokes!) {
                  Polyline line = new () {
                     Stroke = new BrushConverter ().ConvertFromString (s.Color!) as Brush,
                     StrokeThickness = s.Thickness
                  };
                  foreach (var p in s.Points!) {
                     line.Points.Add (p);
                  }
                  DrawCanvas.Children.Add (line);
               }
            }

            // BINARY
            else {
               using var fs = new FileStream (
                   dlg.FileName,
                   FileMode.Open
               );
               using var br = new BinaryReader (fs);
               int count = br.ReadInt32 ();
               for (int i = 0; i < count; i++) {
                  string color = br.ReadString ();
                  double thickness = br.ReadDouble ();
                  int pointCount = br.ReadInt32 ();
                  Polyline line = new () {
                     Stroke = new BrushConverter ().ConvertFromString (color) as Brush,
                     StrokeThickness = thickness
                  };
                  for (int j = 0; j < pointCount; j++) {
                     double x = br.ReadDouble ();
                     double y = br.ReadDouble ();
                     line.Points.Add (new Point (x, y));
                  }
                  DrawCanvas.Children.Add (line);
               }
            }
         }
      }
   }
}

