using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ToDoList.Models
{
    public class Target
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public string Description { get; set; }
        public bool IsSetTimeEnd { get; set; }
        public bool IsDone { get; set; }
        public bool IsReplay { get; set; }
    }
}
