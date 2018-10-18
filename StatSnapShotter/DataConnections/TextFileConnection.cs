using Newtonsoft.Json;
using StatSnapShotter.Entities;
using StatSnapShotter.Interfaces;
using Serilog;
using System;
using System.IO;

namespace StatSnapShotter.DataConnections
{
    public class TextFileConnection : IDataManipulator
    {
        public string url { get; set; }

        public TextFileConnection(DataProviderSettings settings)
        {
            url = settings.Location.ToString();
        }

        public object Read(QueryParams queryParts)
        {
            var result = "";

            if (File.Exists(url))
            {
                return File.ReadAllText(url);
            }
            else
            {
                if (CreateFile())
                {
                    result = "";
                    return result;
                }
                else
                {
                    return null;
                }

            }
        }

        private bool CreateFile()
        {
            try
            {
                var f = File.Create(url);
                f.Dispose();
                return true;
            }
            catch (Exception ee)
            {
                Log.Error(ee, "Error has occured");
                return false;
            }
        }

        public bool Write(object dataToWrite, bool overrideExisting)
        {
            if (dataToWrite.GetType() != typeof(string)) { return false; }

            if (overrideExisting)
            {
                if (CreateFile())
                {
                    File.AppendAllText(url, dataToWrite.ToString());
                }
            }
            else
            {
                var currentFileContent = Read(null);
                var newFileContent = dataToWrite.ToString();
                File.AppendAllText(url, newFileContent);
            }

            return true;
        }

        public bool Write(object dataToWrite)
        {
            return Write(dataToWrite, false);
        }

        public object Deserialize<T>()
        {
            var text = Read(null);

            var output = JsonConvert.DeserializeObject<T>(text.ToString());
            return output;
        }
    }
}
