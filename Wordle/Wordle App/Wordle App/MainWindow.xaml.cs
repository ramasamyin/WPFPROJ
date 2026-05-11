using System.Windows;

namespace Wordle_App;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
   public MainWindow () {
      InitializeComponent ();
      DataContext = new ViewModel.ViewModel ();
   }
}
