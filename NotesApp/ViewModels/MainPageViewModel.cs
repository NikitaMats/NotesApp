using NotesApp.Model;
using NotesApp.Services.Interfaces;
using NotesApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string _searchText;
        private List<Note> _allNotes;

        public ObservableCollection<Note> Notes { get; } = new ObservableCollection<Note>();

        // Свойство для текста поиска
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    // При изменении текста сразу фильтруем заметки
                    FilterNotes();
                }
            }
        }

        public ICommand SearchCommand { get; }
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
            SearchCommand = new Command(FilterNotes);
        }

        // Метод для фильтрации заметок
        private void FilterNotes()
        {
            if (_allNotes == null) return;

            var filteredNotes = string.IsNullOrWhiteSpace(SearchText)
                ? _allNotes
                : _allNotes.Where(n =>
                    (n.Title?.ToLower().Contains(SearchText.ToLower()) ?? false) ||
                    (n.Content?.ToLower().Contains(SearchText.ToLower()) ?? false))
                  .ToList();

            Notes.Clear();
            foreach (var note in filteredNotes)
            {
                Notes.Add(note);
            }
        }

        private async void OnAddNote()
        {
            await Shell.Current.GoToAsync(nameof(Views.NoteDetailPage));
        }

        private async void OnNoteSelected(Note note)
        {
            if (note != null)
            {
                await Shell.Current.GoToAsync($"{nameof(NoteDetailPage)}?noteId={note.Id}");
            }
        }

        public async Task LoadNotes()
        {
            try
            {
                IsRefreshing = true;
                Notes.Clear();

                _allNotes = await _noteService.GetNotesAsync();
                FilterNotes();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}