using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using ToDoList.Models;
using System.Threading.Tasks;
using System.Linq;
using Plugin.LocalNotification;

namespace ToDoList.Data
{
    public class TargetsDB
    {
        SQLiteAsyncConnection db;
        public TargetsDB(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            
        }
        public async Task CreateDbAsync()
        {
            await db.CreateTableAsync<Target>();
        }
        public async Task<List<Target>> GetTargetsListAsync()
        {
            return await db.Table<Target>().OrderBy(x => x.DateStart).ThenBy(x => x.TimeStart)
            .Where(x =>  x.DateStart > DateTime.Now.Date || (x.DateStart == DateTime.Now.Date && x.TimeEnd > DateTime.Now.TimeOfDay)
            || (x.DateStart == DateTime.Now.Date && x.TimeStart > DateTime.Now.TimeOfDay))
            .ToListAsync();
        }
        public async Task<List<Target>> GetAllTargetsListAsync()
        {
            return await db.Table<Target>().OrderBy(x => x.DateStart).ThenBy(x => x.TimeStart).ToListAsync();
        }
        public async Task<Target> GetTargetAsync(int Id)
        {
            return await db.GetAsync<Target>(Id);
        }
        public async Task DeleteTargetAsync(int Id)
        {
            await db.DeleteAsync<Target>(Id);
            NotificationCenter.Current.Cancel(Id);
        }
        public async Task<int> ClearAllTargetsAsync()
        {
            return await db.DeleteAllAsync<Target>();
        }
        public async Task<int> SaveTargetAsync(Target target)
        {
            if (target.IsSetTimeEnd == false)
            {
                target.TimeEnd = null;
            }
            if (target.Id != 0)
            {
                if (await IsFreeUpdatedTime(target))
                {
                    await db.UpdateAsync(target);
                    NotificationCenter.Current.Cancel(target.Id);
                    List<Target> targets = await App.TargetsDB.GetTargetsListAsync();
                    SendNotification(targets);
                }
                return target.Id;
            }
            else
            {
                if (await IsFreeTime(target))
                {
                    int id = await db.InsertAsync(target);
                    List<Target> targets = await App.TargetsDB.GetTargetsListAsync();
                    SendNotification(targets);
                    return id;
                }
                else
                {
                    return target.Id;
                }
            }
        }
        public async Task<int> SaveTargetWithOutNotifiAsync(Target target)
        {
            if (target.IsSetTimeEnd == false)
            {
                target.TimeEnd = null;
            }
            if (target.Id != 0)
            {
                if (await IsFreeUpdatedTime(target))
                {
                    await db.UpdateAsync(target);
                    NotificationCenter.Current.Cancel(target.Id);
                    List<Target> targets = await App.TargetsDB.GetTargetsListAsync();
                }
                return target.Id;
            }
            else
            {
                if (await IsFreeTime(target))
                {
                    int id = await db.InsertAsync(target);
                    List<Target> targets = await App.TargetsDB.GetTargetsListAsync();
                    return id;
                }
                else
                {
                    return target.Id;
                }
            }
        }
        async private void SendNotification(List<Target> targets)
        {
            DateTime dt = (DateTime)targets[0].DateStart;
            dt = dt.Add(targets[0].TimeStart);
            var notification = new NotificationRequest
            {
                Title = "Notification",
                Description = targets[0].Description,
                NotificationId = targets[0].Id,
                Schedule = new NotificationRequestSchedule() { NotifyTime = dt }
            };
            await NotificationCenter.Current.Show(notification);
        }
        async public void SendNotification()
        {
            List<Target> targets = await App.TargetsDB.GetTargetsListAsync();
            if (targets[0].IsReplay == true)
            {
                Target target = new Target() { DateEnd = targets[0].DateEnd, DateStart = targets[0].DateStart,
                    Description = targets[0].Description,
                    IsReplay = targets[0].IsReplay,
                    IsDone = targets[0].IsDone,
                    IsSetTimeEnd = targets[0].IsSetTimeEnd,
                    TimeEnd = targets[0].TimeEnd,
                    TimeStart = targets[0].TimeStart
                };
                target.DateStart = target.DateStart.Value.AddDays(7);
                target.DateEnd = target.DateEnd.Value.AddDays(7);
                await App.TargetsDB.SaveTargetWithOutNotifiAsync(target);
            }
            if (targets.Count > 1)
            {
                DateTime dt = (DateTime)targets[1].DateStart;
                dt = dt.Add(targets[1].TimeStart);
                var notification = new NotificationRequest
                {
                    Title = "Notification",
                    Description = targets[1].Description,
                    NotificationId = targets[1].Id,
                    Schedule = new NotificationRequestSchedule() { NotifyTime = dt }
                };
                await NotificationCenter.Current.Show(notification);
            }
        }
        private async Task<bool> IsFreeTime(Target target)
        {
            List<Target> targets = await App.TargetsDB.GetTargetsListAsync();
            DateTime targetStartDT = ((DateTime)target.DateStart).Add(target.TimeStart);
            if (target.TimeEnd == null)
            {
                foreach (var t in targets)
                {
                    DateTime dtStart = ((DateTime)t.DateStart).Add(t.TimeStart);
                    if (t.TimeEnd != null)
                    {
                        DateTime dtEnd = ((DateTime)t.DateEnd).Add((TimeSpan)t.TimeEnd);
                        if (dtStart <= targetStartDT && dtEnd >= targetStartDT)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (dtStart == targetStartDT)
                        {
                            return false;
                        }
                    }

                }
                return true;
            }
            if (target.TimeEnd < target.TimeStart )
            {
                return false;
            }
            
            DateTime targetEndDT = ((DateTime)target.DateEnd).Add((TimeSpan)target.TimeEnd);
            foreach (var t in targets)
            {
                DateTime dtStart = ((DateTime)t.DateStart).Add(t.TimeStart);
                if (t.TimeEnd != null)
                {
                    DateTime dtEnd = ((DateTime)t.DateEnd).Add((TimeSpan)t.TimeEnd);
                    if (targetStartDT <= dtStart && targetEndDT >= dtEnd || targetStartDT >= dtStart && targetEndDT <= dtEnd
    || targetStartDT >= dtStart && targetEndDT >= dtEnd && dtStart >= targetEndDT || targetStartDT <= dtStart && targetEndDT <= dtEnd && dtEnd <= targetStartDT)
                    {
                        return false;
                    }
                }
                else
                {
                    if (dtStart >= targetStartDT && dtStart <= targetEndDT)
                    {
                        return false;
                    }
                }

            }
            return true;
        }
        private async Task<bool> IsFreeUpdatedTime(Target target)
        {
            List<Target> targets = await App.TargetsDB.GetTargetsListAsync();
            DateTime targetStartDT = ((DateTime)target.DateStart).Add(target.TimeStart);
            if (target.TimeEnd == null)
            {
                foreach (var t in targets)
                {
                    if (t.Id != target.Id)
                    {
                        DateTime dtStart = ((DateTime)t.DateStart).Add(t.TimeStart);
                        if (t.TimeEnd != null)
                        {
                            DateTime dtEnd = ((DateTime)t.DateEnd).Add((TimeSpan)t.TimeEnd);
                            if (dtStart <= targetStartDT && dtEnd >= targetStartDT)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (dtStart == targetStartDT)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
                    
            }
            if (target.TimeEnd < target.TimeStart)
            {
                return false;
            }

            DateTime targetEndDT = ((DateTime)target.DateEnd).Add((TimeSpan)target.TimeEnd);
            foreach (var t in targets)
            {
                if (t.Id != target.Id)
                {
                    DateTime dtStart = ((DateTime)t.DateStart).Add(t.TimeStart);
                    if (t.TimeEnd != null)
                    {
                        DateTime dtEnd = ((DateTime)t.DateEnd).Add((TimeSpan)t.TimeEnd);
                        if (targetStartDT <= dtStart && targetEndDT >= dtEnd || targetStartDT >= dtStart && targetEndDT <= dtEnd
        || targetStartDT >= dtStart && targetEndDT >= dtEnd && dtStart >= targetEndDT || targetStartDT <= dtStart && targetEndDT <= dtEnd && dtEnd <= targetStartDT)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (dtStart >= targetStartDT && dtStart <= targetEndDT)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        //Database for Notes
        public async Task CreateNotesTableAsync()
        {
            await db.CreateTableAsync<Note>();
        }
        public async Task<List<Note>> GetNotesListAsync()
        {
            return await db.Table<Note>().ToListAsync();
        }
        public async Task<Note> GetNoteAsync(int id)
        {
            return await db.GetAsync<Note>(id);
        }
        public async Task DeleteNoteAsync(int id)
        {
            await db.DeleteAsync<Note>(id);
        }
        public async Task<int> SaveNoteAsync(Note note)
        {
            if (note.Id != 0)
            {
                return await db.UpdateAsync(note);
            }
            else
            {
                return await db.InsertAsync(note);
            }
        }

    }
}
