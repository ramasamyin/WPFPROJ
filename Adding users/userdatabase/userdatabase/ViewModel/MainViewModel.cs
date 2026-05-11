using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using userdatabase.Commands;
using userdatabase.Models;
using userdatabase.Views;
//deals with storing the data and presentation logic of the main window, which is the list of users and the command to show the add user window
namespace userdatabase.ViewModel {

   // this basically binds the observable collection to the ui listview and commands to the button and other ui elements
   public class MainViewModel {

      public ObservableCollection<User> Users { get; set; }

      public ICommand ShowWindowCommand { get; set; }

      public MainViewModel () {
         Users = Usermanager.GetUsers ();
         ShowWindowCommand = new RelayCommand (ShowWindow, CanShowWindow);

      }

      private bool CanShowWindow (object obj) {
         return true;
      }

      private void ShowWindow (object obj) {
         var mainWindow = obj as MainWindow;
         AddUser addUserWin = new AddUser (); 
         // when invoking a command, u cud actually pass an object thru commandparameter property
         addUserWin.Owner = mainWindow;
         addUserWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
         addUserWin.ShowDialog ();
      }
   }
}
