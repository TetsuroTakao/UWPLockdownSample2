using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockdownSample.Model;
using Windows.Globalization.DateTimeFormatting;
using Windows.Storage;

namespace UWPLockdownSample.Feature
{
    public class Logging
    {
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public bool LogExists
        {
            get
            {
                bool exists = false;
                new Task(async () => 
                {
                    try
                    {
                        LogFile = await localFolder.GetFileAsync("log.log");
                        exists = true;
                    }
                    catch
                    {
                        exists = false;
                    }
                }).RunSynchronously();
                return exists;
            }
        }
        public StorageFile LogFile { get; set; }
        public Logging()
        {
            if (!LogExists)
            {
                new Task(async () => { LogFile = await localFolder.CreateFileAsync("log.log", CreationCollisionOption.ReplaceExisting); }).RunSynchronously();
            }
        }
        public void AppendWrite(List<LogModel> logs) 
        {
            string jsonstring = string.Empty;
            if (LogExists)
            {
                new Task(async () =>
                {
                    jsonstring = await FileIO.ReadTextAsync(LogFile);
                }).RunSynchronously();
            }
            if (!string.IsNullOrEmpty(jsonstring))
            {
                logs.AddRange(JsonConvert.DeserializeObject<List<LogModel>>(jsonstring));
            }
            new Task(async () => 
            {
                await FileIO.WriteTextAsync(LogFile, JsonConvert.SerializeObject(logs));
            }).RunSynchronously();
        }
        public List<LogModel> ReadLogs()
        {
            List<LogModel> logs = new List<LogModel>();
            string jsonstring = string.Empty;
            if (LogExists)
            {
                new Task(async () =>
                {
                    jsonstring = await FileIO.ReadTextAsync(LogFile);
                }).RunSynchronously();
            }
            if (!string.IsNullOrEmpty(jsonstring))
            {
                logs=JsonConvert.DeserializeObject<List<LogModel>>(jsonstring);
            }
            return logs;
        }
    }
}
