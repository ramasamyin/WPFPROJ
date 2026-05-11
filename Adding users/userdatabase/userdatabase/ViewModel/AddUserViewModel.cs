using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using userdatabase.Commands;
using userdatabase.Models;

namespace userdatabase.ViewModel {
   public class AddUserViewModel {

      public ICommand AddUserCommand { get; set; } 
      public string? Name { get; set; }
      public string? Email { get; set; }
       public AddUserViewModel () {

         AddUserCommand = new RelayCommand (AddUser,CanAddUser);

      }

      private bool CanAddUser (object obj) {
         return true;
      }

      private void AddUser (object obj) {
         
         Usermanager.AddUser(new User() { Name = Name, Email = Email });
      }
   }
}
