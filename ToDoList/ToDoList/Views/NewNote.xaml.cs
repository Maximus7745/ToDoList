using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ToDoList.Models;

namespace ToDoList.Views
{
    [QueryProperty(nameof(NoteId), nameof(NoteId))]
    public partial class NewNote : ContentPage
    {
        public string noteId { get; set; }
        public string NoteId
        {
            get => noteId;
            set
            {
                noteId = value;
                LoadNote(value);
            }
        }
        public NewNote()
        {
            InitializeComponent();
            BindingContext = new Note();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void LoadNote(string value)
        {
            BindingContext = await App.TargetsDB.GetNoteAsync(Int32.Parse(value));
        }
        private async void SaveNote(object sender, EventArgs args)
        {
            Note note = (Note) BindingContext;
            await App.TargetsDB.SaveNoteAsync(note);
            if(note.Id != 0)
            {
                NoteId = note.Id.ToString();
            }
            await Shell.Current.GoToAsync("..");
        }
        private async void Cancel(object sender, EventArgs args)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}