using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ToDoList.Models;
using ToDoList.Data;
using Plugin.LocalNotification;

namespace ToDoList.Views
{
    [QueryProperty(nameof(TargetId), nameof(TargetId))]
    public partial class SelectTargetPage : ContentPage
    {
        
        public string targetId { get; set; }
        public string TargetId
        {
            get => targetId;
            set
            {
                targetId = value;
                LoadTarget(value);
            }
        }
        public SelectTargetPage()
        {
            InitializeComponent();

            BindingContext = new Target();
        }
        protected async override void OnAppearing()
        { 
            base.OnAppearing();
            datePicker.MinimumDate = DateTime.Today;
        }
        private async void LoadTarget(string value)
        {
            BindingContext = await App.TargetsDB.GetTargetAsync(Int32.Parse(value));
        }
        private async void SaveNewTarget(object sender, EventArgs e)
        {
            Target target = (Target)BindingContext;
            if (target.DateStart == null)
            {
                target.DateStart = DateTime.Now.Date;
            }
            if (target.DateEnd == null)
            {
                target.DateEnd = DateTime.Now.Date;
            }
            await App.TargetsDB.SaveTargetAsync(target);
            if (target.Id != 0)
            {
                TargetId = target.Id.ToString();
            }
            await Shell.Current.GoToAsync("..");
        }
        private async void CancelNewTarget(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private void SetMinimumDateEnd(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            datePickerEnd.MinimumDate = datePicker.Date;
            datePickerEnd.Date = datePicker.Date;
        }

        private void ChangedTimePickerStart(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            timePickerEnd.Time = timePickerStart.Time.Add(new TimeSpan(600000000));
        }
    }
}