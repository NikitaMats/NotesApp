using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NotesApp.ViewModels;
using NotesApp.Services.Interfaces;

namespace NotesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(NoteId), "noteId")]  //  Прием параметра из URL
    public partial class NoteDetailPage : ContentPage
    {
        private string _noteId;

        public string NoteId
        {
            get => _noteId;
            set
            {
                _noteId = value;
                LoadNote();  //  Запуск загрузки при изменении ID
            }
        }

        public NoteDetailPage()
        {
            InitializeComponent();
            BindingContext = new NoteDetailViewModel(DependencyService.Get<INoteService>());
        }

        private async void LoadNote()
        {
            if (int.TryParse(NoteId, out var id))
            {
                await ((NoteDetailViewModel)BindingContext).LoadNote(id);  //  Загрузка данных
            }
        }
    }
}