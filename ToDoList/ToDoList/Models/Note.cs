using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ToDoList.Models
{
    public class Note
    {
        [AutoIncrement,PrimaryKey]
        public int Id { get; set; }
        public string Discribtion { get; set; }
    }
}
