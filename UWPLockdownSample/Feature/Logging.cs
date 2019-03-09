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
        public StorageFile LogFile { get; set; }
        public Logging()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            DateTimeFormatter formatter = new DateTimeFormatter("longtime");

            new Task(async () => { LogFile = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.ReplaceExisting); }).RunSynchronously();
        }
        public void AppendWrite(List<LogModel> logs,FileInfo logfile)
        {
            List<LogModel> old = new List<LogModel>();
            string jsonstring = string.Empty;
            new Task(async () => 
            {
                jsonstring = await FileIO.ReadTextAsync(LogFile);
            }).RunSynchronously();
            if (!string.IsNullOrEmpty(jsonstring))
            {
                logs.AddRange(JsonConvert.DeserializeObject<List<LogModel>>(jsonstring));
            }
            new Task(async () => 
            {
                await FileIO.WriteTextAsync(LogFile, JsonConvert.SerializeObject(logs));
            }).RunSynchronously();
        }
    }
}
