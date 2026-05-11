using Nexus.Core;
using Nexus.Data;

namespace Nexus.App.VMs {
   public class BookVM: EntityVM<Book> {

      #region Constructor -----------------------------------------------
      public BookVM (Book b, Hub<Book> m) : base (b, m) { mBook = b; }
      #endregion

      #region Properties ------------------------------------------------
      /// <summary>First Name of the user</summary>
      public string BookName {
         get => mBook.BookName;
         set {
            if (mBook.BookName != value) {
               mBook.BookName = value;
               Notify ();
            }
         }
      }

      /// <summary>Last Name of the user</summary>
      public string Author {
         get => mBook.Author;
         set {
            if (mBook.Author != value) {
               mBook.Author = value;
               Notify ();
            }
         }
      }



        #region Private Data ----------------------------------------------
        Book mBook;
      #endregion
   }
}
#endregion