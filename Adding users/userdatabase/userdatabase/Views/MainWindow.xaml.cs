using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using userdatabase.ViewModel;
using userdatabase.Models;


namespace userdatabase {
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();
         MainViewModel mainViewModel = new MainViewModel ();
         // in order to make data available to the view & bind it to thet viewmodel we need to set the datacontext of the view to the viewmodel
         this.DataContext = mainViewModel;
         // it sets default obj for data binding to this main view model

      }

      private void FilterTextBox_TextChanged (object sender, TextChangedEventArgs e) {

         UserList.Items.Filter = FilterMethod;
      }

      private bool FilterMethod (object obj) {

         var user = obj as User;
         return user!.Name!.Contains(Filter.Text, StringComparison.OrdinalIgnoreCase);

      }
   }
}

// command parameter so this object will be passed to the logic that will be executing the command in this case the showwindow method in mainviewmodel,
// so we can pass any object we want to the logic of the command when invoking it from the ui, for example we can pass the user that we want to edit or delete or
// whatever action we want to perform on it, so we can have a single command for multiple actions
// and we can differentiate between them by passing different objects as command parameter.