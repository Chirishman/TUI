using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TuiBase
{
    public class ComboBoxItem<ValueType>
    {
        public ComboBoxItem(string text, ValueType value)
        {
            Text = text;
            Value = value;
        }

        public ComboBoxItem(string text,Action action)
        {
            Text = text;
            Action = action;
        }



        public string Text { get; private set; }
        public ValueType Value { get; private set; }
        public Action Action { get; private set; }

    }
}
