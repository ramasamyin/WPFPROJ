using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Wordle_App.ViewModel;
public class CellViewModel : INotifyPropertyChanged {
   private string value = "";
   private Brush color = Brushes.White;

   public string Value {
      get => value;
      set {
         this.value = value;
         OnPropertyChanged ();
      }
   }

   public Brush Color {
      get => color;
      set {
         color = value;
         OnPropertyChanged ();
      }
   }
   public event PropertyChangedEventHandler? PropertyChanged;

   protected void OnPropertyChanged ([CallerMemberName] string? propertyName = null) {
      PropertyChanged?.Invoke (this,
          new PropertyChangedEventArgs (propertyName));
   }

   public void Reset () {
      Value = "";
      Color = Brushes.White;
   }
}


