using Nexus.App.VMs;
using Nexus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows;
using Nexus.Core;
using Nexus.Data;
namespace Nexus.App {
   /// <summary>
   /// Interaction logic for Addbook.xaml
   /// </summary>
   public partial class Addbook : Window {
      public Addbook (BookVM vm, Hub<Book> h) {
         InitializeComponent ();
         Title = $"{(h.Get (vm.ID) != null ? "Edit" : "Add")} Book";
         BtnOK.Click += (_, _) => DialogResult = true;
         BtnCancel.Click += (_, _) => { DialogResult = false; Close (); };
         DataContext = vm;
      }
   }
}
