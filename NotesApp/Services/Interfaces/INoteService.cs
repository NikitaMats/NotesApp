using NotesApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<List<Note>> GetNotesAsync();
        Task<Note> GetNoteAsync(int id);
        Task AddNoteAsync(Note note);
        Task UpdateNoteAsync(Note note);
        Task DeleteNoteAsync(Note note);
    }
}
