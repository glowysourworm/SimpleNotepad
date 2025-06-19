using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using SimpleWpf.Extensions;
using SimpleWpf.SimpleCollections.Collection;

namespace SimpleNotepad.Component
{
    /// <summary>
    /// Holds the state of the keyboard input device; and processes text.
    /// </summary>
    public class VirtualKeyboard
    {
        Stack<VirtualKeyboardState> _stateStack;

        public IEnumerable<VirtualKeyboardState> StateStack
        {
            get { return _stateStack; }
            set { _stateStack = new Stack<VirtualKeyboardState>(value); }
        }

        public VirtualKeyboard()
        {
            _stateStack = new Stack<VirtualKeyboardState>();
        }

        public string PlaybackStack(string inputText, ref int caretOffset)
        {
            if (caretOffset < 0 ||
                caretOffset > inputText.Length)
                throw new ArgumentException("Invalid caret position:  VirtualKeyboard.cs");

            while (_stateStack.Count > 0)
            {
                var state = _stateStack.Pop();
            }

            return "";
        }

        public void AddEvent(KeyEventArgs e)
        {
            _stateStack.Push(new VirtualKeyboardState(e));
        }
    }
}
