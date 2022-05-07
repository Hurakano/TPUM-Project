using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PresentationLayer.LibraryModel;

namespace PresentationLayer.LibraryViewModel
{
    public abstract class LibraryViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected LibraryModel.AbstractLibraryModel DataModel;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
