using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ToDoList.Models;
using System.Collections.ObjectModel;
namespace ToDoList.Views
{
    
    public partial class TargetsList : ContentPage
    {
        public TargetsList()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await App.TargetsDB.CreateDbAsync(); 
            //listView.ItemsSource = await App.TargetsDB.GetTargetsListAsync();


            List<Target> targets = await App.TargetsDB.GetTargetsListAsync();
            List<Target> dateTargets = new List<Target>();
            List<DateTime> dateTimes = new List<DateTime>();
            for (int i = 0; i < targets.Count; i++)
            {
                if (!dateTimes.Contains((DateTime)targets[i].DateStart))
                {
                    dateTimes.Add((DateTime)targets[i].DateStart);
                    dateTargets.Add(targets[i]);
                }
                else
                {
                    Target target = new Target()
                    {
                        DateStart = null,
                        Description = targets[i].Description,
                        IsDone = targets[i].IsDone,
                        Id = targets[i].Id,
                        TimeEnd = targets[i].TimeEnd,
                        TimeStart = targets[i].TimeStart
                    };
                    dateTargets.Add(target);
                }
            }
            lv.ItemsSource = dateTargets;
            


            base.OnAppearing();
        }


        private async void ClickedDate(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Target target = await App.TargetsDB.GetTargetAsync(Int32.Parse(label.ClassId));
            await Shell.Current.GoToAsync($"{nameof(SelectTargetPage)}?{nameof(SelectTargetPage.TargetId)}={ target.Id.ToString()}");
        }
        private async void ClickedStartTime(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Target target = await App.TargetsDB.GetTargetAsync(Int32.Parse(label.ClassId));
            await Shell.Current.GoToAsync($"{nameof(SelectTargetPage)}?{nameof(SelectTargetPage.TargetId)}={ target.Id.ToString()}");
        }
        private async void ClickedEndTime(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Target target = await App.TargetsDB.GetTargetAsync(Int32.Parse(label.ClassId));
            await Shell.Current.GoToAsync($"{nameof(SelectTargetPage)}?{nameof(SelectTargetPage.TargetId)}={ target.Id.ToString()}");
        }
        private async void CkickedDelete(object sender, EventArgs e)
        {
            Image image = (Image)sender;
            await App.TargetsDB.DeleteTargetAsync(Int32.Parse(image.ClassId));
            //Label label = (Label)sender;
            //await App.TargetsDB.DeleteTargetAsync(Int32.Parse(label.ClassId));
            OnAppearing();
        }
        private async void ClickedDescription(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Target target = await App.TargetsDB.GetTargetAsync(Int32.Parse(label.ClassId));
            await Shell.Current.GoToAsync($"{nameof(SelectTargetPage)}?{nameof(SelectTargetPage.TargetId)}={ target.Id.ToString()}");
        }
        private async void ClickedNewTarget(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SelectTargetPage));
        }
    }
}