using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ToDoList.Models;

namespace ToDoList
{
    [ContentProperty("Text")]
    class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
        public string Text { get; set; }
        public TranslateExtension()
        {
            ci = DependencyService.Get<ILocolize>().GetCurrentCultureInfo();
        }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return "";
            }
            ResourceManager resourceManager = new ResourceManager("ToDoList.Resource", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var translation = resourceManager.GetString(Text, ci);
            if (translation == null)
            {
                translation = Text;
            }
            return translation;

        }
    }
}
