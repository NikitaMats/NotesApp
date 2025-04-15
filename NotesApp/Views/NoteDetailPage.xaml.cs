using NotesApp.Services.Interfaces;
using NotesApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteDetailPage : ContentPage
    {
        public NoteDetailPage()
        {
            InitializeComponent();
            BindingContext = new NoteDetailViewModel(DependencyService.Get<INoteService>());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is NoteDetailViewModel vm)
            {
                var noteId = 0;
                var query = Shell.Current.CurrentState.Location.OriginalString;
                if (query.Contains("id="))
                {
                    var idStr = query.Split('=')[1].Split('&')[0]; // Безопасное извлечение ID
                    int.TryParse(idStr, out noteId);
                }
                await vm.LoadNote(noteId);
            }
        }
    }
}