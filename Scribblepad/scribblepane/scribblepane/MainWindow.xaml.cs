using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace scribblepane {
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window {
      private Polyline? currentLine;
      private bool isDrawing = false;

      public MainWindow () {
         InitializeComponent ();
      }

      private void Canvas_MouseDown (object sender, MouseButtonEventArgs e) {
         isDrawing = true;
         currentLine = new Polyline {
            Stroke = GetSelectedColor (),
            StrokeThickness = BrushSlider.Value
         };
         currentLine.Points.Add (e.GetPosition (DrawCanvas));
         DrawCanvas.Children.Add (currentLine);
      }
      private void Canvas_MouseUp (object sender, MouseButtonEventArgs e) {
         isDrawing = false;
      }
      private void Canvas_MouseMove (object sender, MouseEventArgs e) {
         if (isDrawing && currentLine != null) currentLine.Points.Add (e.GetPosition (DrawCanvas));
      }
      private void Clear_Click (object sender, RoutedEventArgs e) {
         DrawCanvas.Children.Clear ();
      }
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
      public class StrokeData {
         public string? Color { get; set; }
         public double Thickness { get; set; }
         public List<Point>? Points { get; set; }
      }
      private void Save_Click (object sender, RoutedEventArgs e) {
         SaveFileDialog dlg = new SaveFileDialog ();
         dlg.Filter = "JSON (*.json)|*.json|Binary(*.bin)|*.bin";
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
            if (dlg.FilterIndex == 1) {
               string json = JsonSerializer.Serialize (strokes);
               File.WriteAllText (dlg.FileName, json);
            } else {
               using var fs = new FileStream (dlg.FileName, FileMode.Create);
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

      private void Load_Click (object sender, RoutedEventArgs e) {
         OpenFileDialog dlg = new OpenFileDialog ();
         dlg.Filter = "JSON (*.json)|*.json|Binary(*.bin)|*.bin";
         if (dlg.ShowDialog () == true) {
            DrawCanvas.Children.Clear ();
            if (dlg.FilterIndex == 1) {
               string json = File.ReadAllText (dlg.FileName);
               var strokes = JsonSerializer.Deserialize<List<StrokeData>> (json);
               foreach (var s in strokes!) {
                  Polyline line = new Polyline {
                     Stroke = new BrushConverter ().ConvertFromString (s.Color!) as Brush,
                     StrokeThickness = s.Thickness
                  };
                  foreach (var p in s.Points!) line.Points.Add (p);
                  DrawCanvas.Children.Add (line);
               }
            } else {
               using var fs = new FileStream (dlg.FileName, FileMode.Open);
               using var br = new BinaryReader (fs);
               int count = br.ReadInt32 ();
               for (int i = 0; i < count; i++) {
                  string color = br.ReadString ();
                  double thickness = br.ReadDouble ();
                  int pointCount = br.ReadInt32 ();
                  Polyline line = new Polyline {
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

