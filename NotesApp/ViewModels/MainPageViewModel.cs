using NotesApp.Model;
using NotesApp.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly INoteService _noteService;
        private bool _isRefreshing;
        private string _pageTitle = "Мои заметки";

        public ObservableCollection<Note> Notes { get; } = new ObservableCollection<Note>();
        public ICommand AddNoteCommand { get; }
        public ICommand SelectNoteCommand { get; }
        public ICommand RefreshCommand { get; }

        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public MainPageViewModel(INoteService noteService)
        {
            _noteService = noteService ?? throw new System.ArgumentNullException(nameof(noteService));
            Notes = new ObservableCollection<Note>();

            // Инициализация команд
            AddNoteCommand = new Command(OnAddNote);
            SelectNoteCommand = new Command<Note>(OnNoteSelected);
            RefreshCommand = new Command(async () => await LoadNotes());
        }

        private async void OnAddNote()
        {
            await Shell.Current.GoToAsync(nameof(Views.NoteDetailPage));
        }

        private async void OnNoteSelected(Note note)
        {
            if (note != null)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.NoteDetailPage)}?noteId={note.Id}");
            }
        }

        public async Task LoadNotes()
        {
            try
            {
                IsRefreshing = true;
                Notes.Clear();

                var notes = await _noteService.GetNotesAsync();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}