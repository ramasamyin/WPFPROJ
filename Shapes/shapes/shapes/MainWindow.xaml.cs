using System.Windows;

namespace shapes {
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();
      }

      List<Shape> shapes = new List<Shape> ();
      private void CreateCircle_Click (object sender, RoutedEventArgs e) {
         Circle c = new Circle () {
            Radius = double.Parse (txtRadius.Text)
         };
         c.Calculate ();
         shapes.Add (c);
      }

      private void CreateRectangle_Click (object sender, RoutedEventArgs e) {
         Rectangle r = new Rectangle () {
            Length = double.Parse (txtLength.Text),
            Breadth = double.Parse (txtBreadth.Text)
         };
         r.Calculate ();
         shapes.Add (r);
      }

      private void CreateSquare_Click (object sender, RoutedEventArgs e) {
         Square s = new Square () {
            Side = double.Parse (txtSide.Text)
         };
         s.Calculate ();
         shapes.Add (s);
      }
   }
}
