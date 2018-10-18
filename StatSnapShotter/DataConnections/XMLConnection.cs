using StatSnapShotter.Entities;
using StatSnapShotter.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace StatSnapShotter.DataConnections
{
    public class XMLConnection : IDataManipulator
    {
        public XMLConnection(DataProviderSettings settings)
        {
            url = settings.Location.ToString();
        }

        public string url { get; set; }

        public object Read(QueryParams queryParts)
        {
            List<StatElement> statElements = new List<StatElement>();            

            XmlReader reader;
            try
            {
                reader = XmlReader.Create(url);
            }
            catch (Exception ee)
            {
                Log.Error(ee, "Error has occured");
                reader = null;
            }

            return reader;
        }

        public bool Write(object datatoWrite)
        {
            throw new NotImplementedException();
        }

        public bool Write(object dataToWrite, bool overrideExisting)
        {
            throw new NotImplementedException();
        }

        public object Deserialize<T>()
        {         
            XmlReader xmlString = (XmlReader)Read(null);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            object outputObject = null;
            try
            {
                outputObject = serializer.Deserialize(xmlString);
            }
            catch (Exception ee)
            {
                Log.Error(ee, "Error has occured");
                outputObject = null;
            }

            return outputObject;
        }
    }
}
