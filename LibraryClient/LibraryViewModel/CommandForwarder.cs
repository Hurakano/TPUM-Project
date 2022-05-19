using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PresentationLayer.LibraryViewModel
{
    internal class CommandForwarder : ICommand
    {
        private readonly Action CommandAction;

        public event EventHandler CanExecuteChanged;

        public CommandForwarder(Action execute)
        {
            CommandAction = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CommandAction();
        }
    }

    internal class CommandForwarder_P1<T> : ICommand
    {
        private readonly Action<T> CommandAction;

        public event EventHandler CanExecuteChanged;

        public CommandForwarder_P1(Action<T> execute)
        {
            CommandAction = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CommandAction((T)parameter);
        }
    }
}
