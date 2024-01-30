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

    public partial class Notes : ContentPage
    {
        public Notes()
        {
            InitializeComponent();
            
        }
        protected override async void OnAppearing()
        {
            await App.TargetsDB.CreateNotesTableAsync();
            listView.ItemsSource = await App.TargetsDB.GetNotesListAsync();
            base.OnAppearing();
        }
        private async void CreateNewNote(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NewNote));
        }
        private async void CkickedDelete(object sender, EventArgs e)
        {
            ImageButton imageButton = (ImageButton)sender;
            await App.TargetsDB.DeleteNoteAsync(Int32.Parse(imageButton.ClassId));
            OnAppearing();
        }
        private async void ClickedNote(object sender, SelectedItemChangedEventArgs e)
        {
            Note note = (Note)e.SelectedItem;
            await Shell.Current.GoToAsync($"{nameof(NewNote)}?{nameof(NewNote.NoteId)}={note.Id.ToString()}");

        }
        
        
    }
}