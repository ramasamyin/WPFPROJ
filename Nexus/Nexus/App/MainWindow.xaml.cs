using Nexus.App.VMs;
using Nexus.Core;
using Nexus.Data;
using Nexus.UI;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nexus.App {

   public partial class MainWindow : Window {

      #region Constructor
      public MainWindow () {
         InitializeComponent ();

         IDB<User> udb = DB.Get<User> ();
         IDB<Book> bdb = DB.Get<Book> ();

         mHub = new Hub<User> (udb);
         mBHub = new Hub<Book> (bdb);

         foreach (var u in mHub.All)
            Users.Add (new UserVM (u, mHub));

         foreach (var b in mBHub.All)
            Books.Add (new BookVM (b, mBHub));

         Init ();
      }
      #endregion

      #region Properties
      ObservableCollection<UserVM> Users { get; set; } = new ();
      ObservableCollection<BookVM> Books { get; set; } = new ();

      UserVM SelectedUser => LstUsers.SelectedItem as UserVM;
      BookVM SelectedBook => LstBooks.SelectedItem as BookVM;

      bool IsUsersTab => MainTabs.SelectedIndex == 0;
      bool IsBooksTab => MainTabs.SelectedIndex == 1;
      #endregion

      #region Init
      void Init () {

         // Bind data
         LstUsers.ItemsSource = Users;
         LstBooks.ItemsSource = Books;

         // Setup columns
         LoadUsersView ();
         LoadBooksView ();

         // Commands
         CommandBindings.Add (new CommandBinding (Commands.Add, (_, _) => DoAddEdit ()));
         CommandBindings.Add (new CommandBinding (Commands.Edit, (_, _) => DoAddEdit (), CanExecute));
         CommandBindings.Add (new CommandBinding (Commands.Delete, (_, _) => DoRemove (), CanExecute));
      }
      #endregion

      #region Views

      void LoadUsersView () {
         GridView gv = new ();

         gv.Columns.Add (new GridViewColumn {
            Header = "ID",
            DisplayMemberBinding = new Binding ("ID"),
            Width = 80
         });

         gv.Columns.Add (new GridViewColumn {
            Header = "First Name",
            DisplayMemberBinding = new Binding ("FirstName"),
            Width = 120
         });

         gv.Columns.Add (new GridViewColumn {
            Header = "Last Name",
            DisplayMemberBinding = new Binding ("LastName"),
            Width = 120
         });

         gv.Columns.Add (new GridViewColumn {
            Header = "Age",
            DisplayMemberBinding = new Binding ("Age"),
            Width = 80
         });

         gv.Columns.Add (new GridViewColumn {
            Header = "Email",
            DisplayMemberBinding = new Binding ("Email"),
            Width = 150
         });

         gv.Columns.Add (new GridViewColumn {
            Header = "Phone",
            DisplayMemberBinding = new Binding ("Phone"),
            Width = 120
         });

         LstUsers.View = gv;
      }

      void LoadBooksView () {
         string[] cols = { "ID", "BookName", "Author"};

         GridView gv = new ();

         foreach (var col in cols) {
            gv.Columns.Add (new GridViewColumn {
               Header = col,
               DisplayMemberBinding = new Binding (col),
               Width = 120
            });
         }

         LstBooks.View = gv;
      }

      #endregion

      #region Add/Edit (COMMON)

      void DoAddEdit () {
         if (IsUsersTab) {
            DoAddEditUser (SelectedUser);
         } else if (IsBooksTab) {
            DoAddEditBook (SelectedBook);
         }
      }

      #endregion

      #region User Logic

      void DoAddEditUser (UserVM vm = null) {
         bool isNew = vm == null;

         var u = isNew ? mHub.Create () : vm.Clone ();

         UserVM wvm = new UserVM (u, mHub);
         AddUserDlg dlg = new AddUserDlg (wvm, mHub) { Owner = this };

         if (dlg.ShowDialog () == true) {
            if (isNew) {
               wvm.Save ();
               Users.Add (wvm);
            } else {
               vm.UpdateFrom (u);
               vm.Save ();
               LstUsers.Items.Refresh ();
            }
         }
      }

      #endregion

      #region Book Logic

      void DoAddEditBook (BookVM vm = null) {
         bool isNew = vm == null;

         var b = isNew ? mBHub.Create () : vm.Clone ();

         BookVM wvm = new BookVM (b, mBHub);
         Addbook dlg = new Addbook (wvm, mBHub) { Owner = this };

         if (dlg.ShowDialog () == true) {
            if (isNew) {
               wvm.Save ();
               Books.Add (wvm);
            } else {
               vm.UpdateFrom (b);
               vm.Save ();
               LstBooks.Items.Refresh ();
            }
         }
      }

      #endregion

      #region Delete

      void DoRemove () {
         if (IsUsersTab && SelectedUser != null) {
            if (MessageBox.Show ($"Delete {SelectedUser.FirstName}?",
                "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {

               SelectedUser.Delete ();
               Users.Remove (SelectedUser);
            }
         } else if (IsBooksTab && SelectedBook != null) {
            if (MessageBox.Show ($"Delete {SelectedBook.BookName}?",
                "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {

               SelectedBook.Delete ();
               Books.Remove (SelectedBook);
            }
         }
      }

      #endregion

      #region CanExecute

      void CanExecute (object sender, CanExecuteRoutedEventArgs e) {
         if (IsUsersTab)
            e.CanExecute = SelectedUser != null;
         else if (IsBooksTab)
            e.CanExecute = SelectedBook != null;
      }

      #endregion

      #region Private Data
      readonly Hub<User> mHub;
      readonly Hub<Book> mBHub;
      #endregion

   }
}