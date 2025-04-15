using NotesApp.Model;
using NotesApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using System.IO;

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
        public async Task AddNoteAsync(Note note) => await _db.InsertAsync(note);
        public async Task UpdateNoteAsync(Note note) => await _db.UpdateAsync(note);
        public async Task DeleteNoteAsync(Note note) => await _db.DeleteAsync(note);
        public async Task<Note> GetNoteAsync(int id)
        {
            return await _db.Table<Note>().Where(n => n.Id == id).FirstOrDefaultAsync();
        }
    }
}
