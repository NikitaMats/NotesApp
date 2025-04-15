using NotesApp.Services.Interfaces;
using NotesApp.ViewModels;
using Xamarin.Forms;

namespace NotesApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(DependencyService.Get<INoteService>());

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainPageViewModel vm)
            {
                vm.LoadNotes().ConfigureAwait(false);
            }
        }
    }
}
