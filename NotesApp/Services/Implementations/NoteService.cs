using NotesApp.Model;
using NotesApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using System.Linq;

namespace NotesApp.Services.Implementations
{
    public class NoteService : INoteService
    {
        private SQLiteAsyncConnection _db;

        public NoteService()
        {
            _db = new SQLiteAsyncConnection(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "notes.db3"));
            _db.CreateTableAsync<Note>().Wait();
        }

        public async Task<List<Note>> GetNotesAsync() => await _db.Table<Note>().ToListAsync();

        public async Task<List<Note>> SearchNotesAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetNotesAsync();

            return await _db.Table<Note>()
                .Where(n =>
                    n.Title.Contains(searchText) ||
                    n.Content.Contains(searchText))
                .ToListAsync();
        }

        public async Task<int> AddNoteAsync(Note note)
        {
            // Получаем максимальный существующий ID
            var allNotes = await _db.Table<Note>().ToListAsync();
            note.Id = allNotes.Count > 0 ? allNotes.Max(n => n.Id) + 1 : 1;

            await _db.InsertAsync(note);
            return note.Id;
        }
        public async Task UpdateNoteAsync(Note note) 
        {
            if (note.Id <= 0) throw new ArgumentException("Invalid note ID");
            await _db.UpdateAsync(note);
        }

        public async Task DeleteNoteAsync(Note note) => await _db.DeleteAsync(note);

        public async Task<Note> GetNoteAsync(int id)          {
            return await _db.Table<Note>()
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
