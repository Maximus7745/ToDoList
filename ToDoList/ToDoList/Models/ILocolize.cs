using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ToDoList.Models
{
    public interface ILocolize
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
