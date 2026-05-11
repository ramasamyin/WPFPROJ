using System.IO;

namespace Wordle_App.Model;

public static class Wordlist {
   public static readonly HashSet<string> Words = [.. File.ReadAllLines ("C:\\Work\\WPF\\Wordle App\\Wordle App\\DATA\\dict-5.txt")
                                                     .Select (w => w.Trim ().ToUpper ())];
}
