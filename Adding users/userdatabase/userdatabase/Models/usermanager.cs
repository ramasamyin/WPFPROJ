using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userdatabase.Models {
   public class Usermanager {

      public static ObservableCollection<User> _DatabaseUsers = new ObservableCollection<User> () { new User () { Email = "cam@gmail.com", Name = "Cam" }, new User { Email = "jack@gmail.com", Name = "Jack" } };

      public static ObservableCollection<User> GetUsers() {
         return _DatabaseUsers;
      }

      public static void AddUser (User user) {
             _DatabaseUsers.Add (user);

      }
   }
}
