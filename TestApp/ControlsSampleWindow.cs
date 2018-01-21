using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase;
using TuiBase.Console;

namespace TestApp
{
    class ControlsSampleWindow : TuiBase.Window
    {
        CheckBox _checkBox;
        ComboBox<string> _comboBox;
        TextBox _textBox;
        TextPanel _textPanel;



        public ControlsSampleWindow()
        {
            Text = "Simple Controls";
            Size = new Coordinates(60, 30);
            Location = new Coordinates(10, 3);
            Foreground = ConsColor.Gray;
            Background = ConsColor.DarkGreen;

            _textPanel = new TextPanel();
            _textPanel.Location = new Coordinates(1, 1);
            _textPanel.Size = new Coordinates(this.Size.X - 3, 5);
            _textPanel.Text = "Use <Tab> and <Shift>+<Tab> to navigate through\r\nthe Controls\r\nUse Cursor-Down on Combobox to popup\r\nPress Esc to Exit";
            AddControl(_textPanel);

            _checkBox = new CheckBox();
            _checkBox.Location = new Coordinates(1, 6);
            _checkBox.Size = new Coordinates(46, 1);
            _checkBox.Text = "Press Space to select option, when focused";
            AddControl(_checkBox);

            _textBox = new TextBox("Type some Text:");
            _textBox.Location = new Coordinates(1, 8);
            _textBox.Size = new Coordinates(46, 1);
            AddControl(_textBox);

            _comboBox = new ComboBox<string>("Select item:");
            _comboBox.Location = new Coordinates(1, 10);
            _comboBox.Size = new Coordinates(46, 1);
            _comboBox.Items = new List<ComboBoxItem<string>>();
            _comboBox.Items.Add(new ComboBoxItem<string>("Item 1", "v1"));
            _comboBox.Items.Add(new ComboBoxItem<string>("Item 2", "v2"));
            _comboBox.Items.Add(new ComboBoxItem<string>("Item 3", "v3"));
            _comboBox.Items.Add(new ComboBoxItem<string>("Item 4", "v4"));
            AddControl(_comboBox);

        }
    }
}
