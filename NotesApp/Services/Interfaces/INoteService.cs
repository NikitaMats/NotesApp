using NotesApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<List<Note>> GetNotesAsync();
        Task<List<Note>> SearchNotesAsync(string searchText);
        Task<Note> GetNoteAsync(int id);
        Task<int> AddNoteAsync(Note note);
        Task UpdateNoteAsync(Note note);
        Task DeleteNoteAsync(Note note);
    }
}
