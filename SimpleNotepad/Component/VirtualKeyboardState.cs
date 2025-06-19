using System.Windows.Input;

using SimpleWpf.Extensions;
using SimpleWpf.SimpleCollections.Collection;

namespace SimpleNotepad.Component
{
    /// <summary>
    /// Contains the detailed data of the keyboard device for one state
    /// </summary>
    public class VirtualKeyboardState
    {
        Key _eventKey;
        SimpleDictionary<Key, KeyStates> _keyboardState;

        public VirtualKeyboardState()
        {
            _keyboardState = new SimpleDictionary<Key, KeyStates>();
        }

        public VirtualKeyboardState(KeyEventArgs e)
        {
            if (e.IsRepeat)
                throw new ArgumentException("Key event is a repeat of the previous pressed-key:  VirtualKeyboardState.cs");

            if (e.Key.IsModifier())
                throw new ArgumentException("Key event is a modifier:  VirtualKeyboardState.cs");

            _keyboardState = new SimpleDictionary<Key, KeyStates>();
            _eventKey = e.Key;

            foreach (var key in Enum.GetValues(typeof(Key)).Cast<Key>())
            {
                if (e.KeyboardDevice.IsKeyDown(key))
                    _keyboardState.Add(key, e.KeyboardDevice.GetKeyStates(key));
            }
        }

        public string GetOutputText(string inputText, ref int caretOffset)
        {
            if (caretOffset < 0 ||
                caretOffset > inputText.Length)
                throw new ArgumentException("Invalid caret position:  VirtualKeyboardState.cs");

            // Procedure:
            //
            // 1) Check for caret position change
            // 2) Check for modifiers (toggles are already processed with the text)
            // 3) Move Caret / Insert / Delete Text accordingly!
            //

            switch (_eventKey)
            {
                case Key.Tab:
                    inputText.Insert(caretOffset, "\t");
                    break;
                case Key.Enter:
                    inputText.Insert(caretOffset, "\n");
                    break;
                case Key.Space:
                    inputText.Insert(caretOffset, " ");
                    break;
                case Key.End:
                    break;
                case Key.Home:
                    break;
                case Key.Left:
                    break;
                case Key.Up:
                    break;
                case Key.Right:
                    break;
                case Key.Down:
                    break;
                case Key.Insert:
                    break;
                case Key.Delete:
                    break;
                case Key.D0:
                    break;
                case Key.D1:
                    break;
                case Key.D2:
                    break;
                case Key.D3:
                    break;
                case Key.D4:
                    break;
                case Key.D5:
                    break;
                case Key.D6:
                    break;
                case Key.D7:
                    break;
                case Key.D8:
                    break;
                case Key.D9:
                    break;
                case Key.A:
                    break;
                case Key.B:
                    break;
                case Key.C:
                    break;
                case Key.D:
                    break;
                case Key.E:
                    break;
                case Key.F:
                    break;
                case Key.G:
                    break;
                case Key.H:
                    break;
                case Key.I:
                    break;
                case Key.J:
                    break;
                case Key.K:
                    break;
                case Key.L:
                    break;
                case Key.M:
                    break;
                case Key.N:
                    break;
                case Key.O:
                    break;
                case Key.P:
                    break;
                case Key.Q:
                    break;
                case Key.R:
                    break;
                case Key.S:
                    break;
                case Key.T:
                    break;
                case Key.U:
                    break;
                case Key.V:
                    break;
                case Key.W:
                    break;
                case Key.X:
                    break;
                case Key.Y:
                    break;
                case Key.Z:
                    break;
                case Key.NumPad0:
                    break;
                case Key.NumPad1:
                    break;
                case Key.NumPad2:
                    break;
                case Key.NumPad3:
                    break;
                case Key.NumPad4:
                    break;
                case Key.NumPad5:
                    break;
                case Key.NumPad6:
                    break;
                case Key.NumPad7:
                    break;
                case Key.NumPad8:
                    break;
                case Key.NumPad9:
                    break;
                case Key.Multiply:
                    break;
                case Key.Add:
                    break;
                case Key.Separator:
                    break;
                case Key.Subtract:
                    break;
                case Key.Decimal:
                    break;
                case Key.Divide:
                    break;
                case Key.OemSemicolon:
                    break;
                case Key.OemPlus:
                    break;
                case Key.OemComma:
                    break;
                case Key.OemMinus:
                    break;
                case Key.OemPeriod:
                    break;
                case Key.OemQuestion:
                    break;
                case Key.OemTilde:
                    break;
                case Key.AbntC1:
                    break;
                case Key.AbntC2:
                    break;
                case Key.OemOpenBrackets:
                    break;
                case Key.OemPipe:
                    break;
                case Key.OemCloseBrackets:
                    break;
                case Key.OemQuotes:
                    break;
                case Key.OemBackslash:
                    break;
                default:
                    throw new Exception("Unhandled event key:  VirtualKeyboardState.cs");
            }

            return "";
        }
    }
}
