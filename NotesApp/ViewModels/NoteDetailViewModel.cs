using NotesApp.Model;
using NotesApp.Services.Interfaces;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesApp.ViewModels
{
    public class NoteDetailViewModel : BaseViewModel
    {
        private readonly INoteService _noteService;
        private Note _note;

        public Note Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public NoteDetailViewModel(INoteService noteService)
        {
            _noteService = noteService;
            SaveCommand = new Command(async () => await SaveNote());
            DeleteCommand = new Command(async () => await DeleteNote());

            Note = new Note(); // Инициализация по умолчанию
        }

        public async Task LoadNote(int id)
        {
            if (id != 0)
            {
                Note = await _noteService.GetNoteAsync(id);
            }
            else
            {
                Note = new Note();
            }
        }

        private async Task SaveNote()  // ⚡ Основная логика сохранения
        {
            if (string.IsNullOrWhiteSpace(Note.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите заголовок", "OK");
                return;
            }

            if (Note.Id == 0)
                await _noteService.AddNoteAsync(Note);
            else
                await _noteService.UpdateNoteAsync(Note);  

            await Shell.Current.GoToAsync("..");  
        }

        private async Task DeleteNote()
        {
            if (Note.Id != 0)
            {
                await _noteService.DeleteNoteAsync(Note);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}