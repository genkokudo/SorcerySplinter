using Prism.Mvvm;

namespace SorcerySplinter.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "SorcerySplinter";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
