using System.ComponentModel;

namespace CannedBytes.CommandLineGui
{
    class ObservableObject : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}