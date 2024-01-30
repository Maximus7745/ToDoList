using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ToDoList.Views;
using Plugin.LocalNotification;
using ToDoList.Models;
using ToDoList.Data;

namespace ToDoList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SelectTargetPage), typeof(SelectTargetPage));
            Routing.RegisterRoute(nameof(TargetsList), typeof(TargetsList));
            Routing.RegisterRoute(nameof(Notes), typeof(Notes));
            Routing.RegisterRoute(nameof(NewNote), typeof(NewNote));
            NotificationCenter.Current.NotificationReceived += SendNewNotification;
        }
        private async void SendNewNotification(EventArgs e)
        {
            App.TargetsDB.SendNotification();
        }

    }


 }