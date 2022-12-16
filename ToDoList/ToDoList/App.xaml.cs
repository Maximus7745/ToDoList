using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ToDoList.Data;
using System.IO;

using Plugin.LocalNotification;

namespace ToDoList
{
    public partial class App : Application
    {
        public static TargetsDB targetsDB;
        public static TargetsDB TargetsDB
        {
            get
            {
                if (targetsDB == null)
                {
                    targetsDB = new TargetsDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "targets.db"));
                }
                return targetsDB;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
