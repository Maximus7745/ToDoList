using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoList.Models;
using System.Globalization;
using Xamarin.Forms;
using System.Reflection;
[assembly: Dependency(typeof(ToDoList.Droid.Localize))]
namespace ToDoList.Droid
{
    public class Localize : ILocolize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocal = Java.Util.Locale.Default;
            var netLanguage = androidLocal.ToString().Replace("_","-");
            return new CultureInfo(netLanguage);
        }
    }
}