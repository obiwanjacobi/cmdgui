using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CannedBytes.CommandLineGui.Commands
{
    abstract class CommandHandler
    {
        public RoutedCommand Command { get; protected set; }

        protected virtual void ErrorHandler(Action action)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                action();
            }
            else
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    // TODO: better error box
                    MessageBox.Show(App.Current.MainWindow, ex.Message, "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
#else
            try
            {
                action();
            }
            catch (Exception ex)
            {
                // TODO: better error box
                MessageBox.Show(App.Current.MainWindow, ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
#endif
        }

        protected virtual bool CanExecute(object parameter)
        {
            return (Command != null);
        }

        protected virtual bool Execute(object parameter)
        {
            return true;
        }

        private void OnExecutedInternal(object sender, ExecutedRoutedEventArgs e)
        {
            ErrorHandler(() =>
                {
                    e.Handled = Execute(e.Parameter);
                });
        }

        private void OnCanExecuteInternal(object sender, CanExecuteRoutedEventArgs e)
        {
            ErrorHandler(() =>
                {
                    e.CanExecute = CanExecute(e.Parameter);
                });
        }

        public CommandBinding ToCommandBinding()
        {
            if (Command == null) throw new InvalidOperationException("The Command property must be assigned in the derived class ctor.");

            var cmdBinding = new CommandBinding(Command, OnExecutedInternal, OnCanExecuteInternal);

            return cmdBinding;
        }
    }
}