using System;
using System.Windows.Controls;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// Interaction logic for ConsoleOutput.xaml
    /// </summary>
    partial class ConsoleOutput : UserControl, ITextOutput
    {
        public ConsoleOutput()
        {
            InitializeComponent();
        }

        // ITextOutput
        public void Write(string text, bool isError)
        {
            this.Dispatcher.Invoke(new Action<string, bool>(WriteInternal), text, isError);
        }

        private void WriteInternal(string text, bool isError)
        {
            if (isError)
            {
                if (text == String.Empty)
                {
                    this.ErrorText.Text += Environment.NewLine;
                }
                else if (text != null)
                {
                    this.ErrorText.Text += text + Environment.NewLine;
                }
            }
            else
            {
                if (text == String.Empty)
                {
                    this.OutputText.Text += Environment.NewLine;
                }
                else if (text != null)
                {
                    this.OutputText.Text += text + Environment.NewLine;
                }
            }
        }
    }
}