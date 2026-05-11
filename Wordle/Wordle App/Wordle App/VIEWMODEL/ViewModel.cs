using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Wordle_App.Model;

namespace Wordle_App.ViewModel;

public enum EnterResult {
   Success,
   NotEnoughLetters,
   InvalidWord,
   NotAllowedToRetry
}

public class ViewModel {
   public ObservableCollection<CellViewModel> Cells { get; set; }

   public Commands KeyCommand { get; set; }
   public Commands EnterCommand { get; set; }
   public Commands BackspaceCommand { get; set; }


   private Model.Model engine;

   private string currentInput = "";
   private int row = 0;
   private int col = 0;

   // INIT
   public ViewModel () {
      Cells = [];
      for (int i = 0; i < 30; i++) Cells.Add (new CellViewModel ());
      var random = new Random ();
      string secret = Wordlist.Words.ElementAt ( random.Next (Wordlist.Words.Count));
      engine = new Model.Model (secret, Wordlist.Words);
      KeyCommand = new Commands (OnKey);
      EnterCommand = new Commands (OnEnter);
      BackspaceCommand = new Commands (OnBackspace);
   }

   // KEY INPUT
   private void OnKey (object obj) {
      if (row >= 6 || col >= 5) return;
      string letter = obj.ToString ()!.ToUpper ();
      int index = row * 5 + col;
      Cells[index].Value = letter;
      currentInput += letter;
      col++;
   }

   // BACKSPACE
   private void OnBackspace (object obj) {
      if (col <= 0) return;
      col--;
      currentInput = currentInput[..^1];
      int index = row * 5 + col;
      Cells[index].Value = "";
      Cells[index].Color = Brushes.White;
   }

   // ENTER
   private void OnEnter (object obj) {
      Window owner = Application.Current.MainWindow;
      if (currentInput.Length < 5) {
         MessageBox.Show (owner,"Not enough letters!");
         return;
      }
      string guess = currentInput.ToUpper ();
      if (!engine.IsValidWord (guess)) {
         MessageBox.Show (owner,"Not a valid word!");
         return;
      }
      var result = engine.CheckGuess (guess);
      ApplyColors (result);
      if (engine.IsCorrect (guess)) {
         MessageBox.Show (owner, "You Win!"); ;
         return;
      }
      row++;
      col = 0;
      currentInput = "";
      if (row == 6) MessageBox.Show ($"Game Over! Word was {engine.SecretWord}");
   }

   // COLOURS 
   private void ApplyColors (LetterResult[] result) {
      for (int i = 0; i < 5; i++) {
         int index = row * 5 + i;
         Cells[index].Color =
             result[i] == LetterResult.Green ? Brushes.Green :
             result[i] == LetterResult.Yellow ? Brushes.Goldenrod :
             Brushes.Gray;
      }
   }
}
