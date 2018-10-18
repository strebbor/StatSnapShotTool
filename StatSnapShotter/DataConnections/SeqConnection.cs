using Seq.Api;
using Seq.Api.Model.Data;
using StatSnapShotter.Entities;
using StatSnapShotter.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;

namespace StatSnapShotter.DataConnections
{
    public class SeqDataConnection : IDataManipulator
    {
        private string _apiKey;

        public SeqDataConnection(DataProviderSettings settings)
        {
            url = settings.Location.ToString();
            _apiKey = settings.APIKey.ToString();
        }

        public string url { get; set; }

        public object Deserialize<T>()
        {
            throw new NotImplementedException();
        }

        public object Read(QueryParams queryParts)
        {
            //setup the query
            string query = queryParts.query;
            DateTime startDate = queryParts.startDate;
            DateTime endDate = queryParts.endDate;

            var result = new QueryResultPart();

            //run the query
            var connection = (SeqConnection)OpenConnection();
            Task.Run(async () =>
            {
                await Run(connection);
            }).Wait();


            async Task Run(SeqConnection useConnection)
            {
                try
                {
                    result = await connection.Data.QueryAsync(query, startDate, endDate);
                }
                catch (Exception ee)
                {
                    Log.Error(ee, "Error has occured");
                    result = null;
                }
            }

            return result;
        }

        public bool Write(object dataToWrite)
        {
            throw new NotImplementedException();
        }

        public bool Write(object dataToWrite, bool overrideExisting)
        {
            throw new NotImplementedException();
        }

        private object OpenConnection()
        {
            var connection = new SeqConnection(url, _apiKey);
            return connection;
        }
    }
}
