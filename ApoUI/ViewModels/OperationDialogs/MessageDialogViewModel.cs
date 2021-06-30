namespace ApoUI
{
    /// <summary>
    /// ViewModel for dialog with custom text
    /// </summary>
    public class MessageDialogViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text"></param>
        public MessageDialogViewModel(string text)
        {
            Text = text;
        }

        #endregion

        #region Public properties

        // text displayed on dialog
        public string Text
        {
            get => _Text;
            set
            {
                if (_Text == value) return;
                _Text = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private fields

        private string _Text;

        #endregion
    }
}
