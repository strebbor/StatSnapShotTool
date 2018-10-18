using Autofac.Features.Indexed;
using Seq.Api.Model.Data;
using Serilog;
using StatSnapShotter.DataConnections;
using StatSnapShotter.Entities;
using StatSnapShotter.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StatSnapShotter
{
    public class StatProcessor : IApplication
    {
        private IStatConfiguration _configuration;
        private readonly IDataManipulator _dataSource;
        private IDataManipulator _dataStorage;

        public StatProcessor(IStatConfiguration configuration, IIndex<string, IDataManipulator> dataManipulators)
        {
            _configuration = configuration;
            _dataSource = dataManipulators["dataSource"];
            _dataStorage = dataManipulators["dataStorage"];
        }
        public StatProcessor(IStatConfiguration configuration, IDataManipulator dataSource, IDataManipulator dataStorage)
        {
            _configuration = configuration;
            _dataSource = dataSource;
            _dataStorage = dataStorage;
        }

        public int Run()
        {
            int interval = _configuration.Interval;
            var dateFrom = DateTime.Now.AddHours(-interval);
            var dateTo = DateTime.Now;
            Log.Information("SeqSnapShot started");
                     
            var returnConfig = _configuration.GetConfiguration<List<StatElement>>();
            List<StatElement> statConfiguration = (List<StatElement>)returnConfig;

            List<StatRowItem> rowsToStore = new List<StatRowItem>();

            int successfulWrites = 0;
            Parallel.ForEach(statConfiguration, (stat) =>
             {
                 if (!string.IsNullOrWhiteSpace(stat.query))
                 {
                     QueryParams parts = new QueryParams()
                     {
                         query = stat.query,
                         startDate = dateFrom,
                         endDate = dateTo
                     };

                     QueryResultPart seqQueryResult = (QueryResultPart)_dataSource.Read(parts);

                     if (seqQueryResult != null)
                     {
                         stat.count = seqQueryResult.Rows.Length;

                         rowsToStore.Add(
                             new StatRowItem()
                             {
                                 DateFrom = dateFrom.ToString(),
                                 DateTo = dateTo.ToString(),
                                 WriteDate = DateTime.Now.ToString(),
                                 StatName = stat.name,
                                 StatCount = stat.count
                             }
                         );              
                     }
                 }
             });

            _dataStorage.Write(rowsToStore);
            successfulWrites = rowsToStore.Count;

            return successfulWrites;
        }
    }
}
