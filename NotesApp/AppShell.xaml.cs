using NotesApp.Views;
using Xamarin.Forms;

namespace NotesApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NoteDetailPage), typeof(NoteDetailPage));
        }
    }
}