using System.Windows;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// Interaction logic for ToolWindow.xaml
    /// </summary>
    partial class ToolWindow : Window
    {
        public ToolWindow()
        {
            InitializeComponent();
        }

        public static ToolWindow Create(System.Windows.Controls.Control content, string title)
        {
            var tw = new ToolWindow();

            tw.Title = title;
            tw.Content = content;

            return tw;
        }
    }
}