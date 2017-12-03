using System.Windows.Input;
using Xamarin.Forms;

namespace Sample
{
    public class MainViewModel : BindableObject
    {
        int _count = 0;
        public ICommand ButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    _count++;
                    LabelText = _count.ToString();
                });
            }
        }

        private string _labelText = "0";
        public string LabelText { get { return _labelText; } set { _labelText = value; OnPropertyChanged(); } }

    }
}
