using NotesApp.Model;
using NotesApp.Services.Interfaces;
using NotesApp.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly INoteService _noteService;

        public ObservableCollection<Note> Notes { get; } = new ObservableCollection<Note>();
        public ICommand AddNoteCommand { get; }
        public ICommand SelectNoteCommand { get; }
        public ICommand RefreshCommand { get; }

        public string PageTitle => "Мои заметки"; // Фиксированный заголовок

        public MainPageViewModel(INoteService noteService)
        {
            _noteService = noteService;

            AddNoteCommand = new Command(async () =>
            await Shell.Current.GoToAsync($"{nameof(NoteDetailPage)}?id=0"));

            SelectNoteCommand = new Command<Note>(note =>
            {
                if (note != null)
                    Shell.Current.GoToAsync($"{nameof(NoteDetailPage)}?id={note.Id}");
            });

            RefreshCommand = new Command(async () => await LoadNotes());

            LoadNotes();
        }

        public async Task LoadNotes()
        {
            Notes.Clear();
            var notes = await _noteService.GetNotesAsync();
            foreach (var note in notes) Notes.Add(note);
        }
    }
}