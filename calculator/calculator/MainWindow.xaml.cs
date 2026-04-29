using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace calculator {
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window {

      private double currentValue = 0;
      private string currentOperator = "";
      private bool isNewEntry = true;
      private bool justCalculated = false;
      public MainWindow () {
         InitializeComponent ();
         DisplayText.Text = "0";
         ExpressionText.Text = "";
      }


      private void Button_Click (object sender, RoutedEventArgs e) {
         var button = sender as Button;
         string content = button!.Content.ToString ()!;

         switch (content) {
            case "C":
               ClearAll ();
               break;
            case "CE":
               ClearEntry ();
               break;
            case "⌫":
               Backspace ();
               break;
            case "=":
               Calculate ();
               break;
            case "+/-":
               ToggleSign ();
               break;
            case ".":
               AddDecimal ();
               break;
            case "+":
            case "-":
            case "*":
            case "/":
               OperatorPressed (content);
               break;
            default:
               if (int.TryParse (content, out _))
                  NumberPressed (content);
               break;
         }
      }

      private void NumberPressed (string number) {
         if (isNewEntry || DisplayText.Text == "0" || justCalculated) {
            DisplayText.Text = number;
            isNewEntry = false;
            justCalculated = false;
         } else {
            DisplayText.Text += number;
         }
      }

      private void OperatorPressed (string op) {
         _ = double.TryParse (DisplayText.Text, out double inputNumber);

         if (!string.IsNullOrEmpty (currentOperator) && !justCalculated)
            currentValue = PerformOperation (currentValue, inputNumber, currentOperator);
         else
            currentValue = inputNumber;

         currentOperator = op;
         ExpressionText.Text = currentValue + " " + currentOperator;
         isNewEntry = true;
         justCalculated = false;
      }

      private void Calculate () {
         _ = double.TryParse (DisplayText.Text, out double inputNumber);

         if (!string.IsNullOrEmpty (currentOperator)) {
            currentValue = PerformOperation (currentValue, inputNumber, currentOperator);
            DisplayText.Text = currentValue.ToString ();
            currentOperator = "";
            ExpressionText.Text = "";
         } else {
            currentValue = inputNumber;
         }

         isNewEntry = true;
         justCalculated = true;
      }

      private void ClearAll () {
         DisplayText.Text = "0";
         ExpressionText.Text = "";
         currentValue = 0;
         currentOperator = "";
         isNewEntry = true;
         justCalculated = false;
         DisplayText.Focus ();
      }

      private void ClearEntry () {
         DisplayText.Text = "0";
         isNewEntry = true;
         justCalculated = false;
         DisplayText.Focus ();
      }

      private void Backspace () {
         if (!string.IsNullOrEmpty (DisplayText.Text) && DisplayText.Text.Length > 1)
            DisplayText.Text = DisplayText.Text.Substring (0, DisplayText.Text.Length - 1);
         else
            DisplayText.Text = "0";

         isNewEntry = DisplayText.Text == "0";
      }

      private void AddDecimal () {
         if (isNewEntry || justCalculated) {
            DisplayText.Text = "0.";
            isNewEntry = false;
            justCalculated = false;
         } else if (!DisplayText.Text.Contains (".")) {
            DisplayText.Text += ".";
         }
      }

      private void ToggleSign () {
         if (double.TryParse (DisplayText.Text, out double value))
            DisplayText.Text = (-value).ToString ();
      }

      private static double PerformOperation (double a, double b, string op) {
         return op switch {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => b != 0 ? a / b : 0,
            _ => b,
         };
      }

      private void Window_KeyDown (object sender, KeyEventArgs e) {

         if (e.Key >= Key.D0 && e.Key <= Key.D9) {
            if (!Keyboard.Modifiers.HasFlag (ModifierKeys.Shift))
               NumberPressed ((e.Key - Key.D0).ToString ());
         } else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) {
            NumberPressed ((e.Key - Key.NumPad0).ToString ());
         } else if (e.Key == Key.Add) OperatorPressed ("+");
         else if (e.Key == Key.Subtract) OperatorPressed ("-");
         else if (e.Key == Key.Multiply) OperatorPressed ("*");
         else if (e.Key == Key.Divide) OperatorPressed ("/");
         else if (e.Key == Key.Enter || e.Key == Key.Return) Calculate ();
         else if (e.Key == Key.Back) Backspace ();
         else if (e.Key == Key.Escape) ClearAll ();
      }

      private void Window_TextInput (object sender, TextCompositionEventArgs e) {
         string text = e.Text; if (text == "*") OperatorPressed ("*");
         else if (text == "+") OperatorPressed ("+");
         else if (text == "-") OperatorPressed ("-");
         else if (text == "/") OperatorPressed ("/");
         else if (text == ".") AddDecimal ();
      }
   }
}
