using NotesApp.Services.Implementations;
using NotesApp.Services.Interfaces;
using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace NotesApp
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            InitializeComponent();
            DependencyService.Register<INoteService, NoteService>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            Debug.WriteLine("Приложение запущено");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
