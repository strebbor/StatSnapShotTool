using StatSnapShotter.Entities;
using StatSnapShotter.Interfaces;
using System.Collections.Generic;

namespace StatSnapShotter
{
    public class StatConfiguration : IStatConfiguration
    {
        private IDataManipulator _configurationProvider;     

        public StatConfiguration(IDataManipulator configurationProvider, string intervalHour)
        {
            _configurationProvider = configurationProvider;

            int parseResult = -1;
            int.TryParse(intervalHour, out parseResult);
            if (parseResult == 0)
            {
                parseResult = 1;
            }

            Interval = parseResult;            
        }

        public int Interval { get; set; }

        public object GetConfiguration<T>()
        {
            var statElements = _configurationProvider.Deserialize<T>();
            return statElements;
        }
    }
}
