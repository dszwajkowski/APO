using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace ApoUI.ViewModels
{
    /// <summary>
    /// Base view model that fires PropertyChanged events
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
