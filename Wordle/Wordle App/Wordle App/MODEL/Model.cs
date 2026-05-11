namespace Wordle_App.Model;

public enum LetterStatus {
   Unknown,
   NotPresent,
   Present,
   Positioned
}

public class Model (string secretWord, HashSet<string> words) {
   public string SecretWord { get; } = secretWord.ToUpper ();
   private HashSet<string> dictionary = words;

   public bool IsValidWord (string word) => dictionary.Contains (word.ToUpper ());

   public LetterResult[] CheckGuess (string guess) {
      guess = guess.ToUpper ();
      var result = new LetterResult[5];
      var secret = SecretWord.ToCharArray ();
      var used = new bool[5];
      // GREEN pass
      for (int i = 0; i < 5; i++) if (guess[i] == secret[i]) {
            result[i] = LetterResult.Green;
            used[i] = true;
         }
      // YELLOW / GRAY pass
      for (int i = 0; i < 5; i++) {
         if (result[i] == LetterResult.Green) continue;
         bool found = false;
         for (int j = 0; j < 5; j++) if (!used[j] && guess[i] == secret[j]) {
               found = true;
               used[j] = true;
               break;
            }
         result[i] = found ? LetterResult.Yellow : LetterResult.Gray;
      }
      return result;
   }

   public bool IsCorrect (string guess) => guess.ToUpper () == SecretWord;
}

public enum LetterResult {
   Gray,
   Yellow,
   Green
}
