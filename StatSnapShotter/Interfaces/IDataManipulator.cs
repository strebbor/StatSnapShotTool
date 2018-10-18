using StatSnapShotter.Entities;

namespace StatSnapShotter.Interfaces
{
    public interface IDataManipulator
    {
        string url { get; set; }        
        object Read(QueryParams queryParts = null);
        bool Write(object dataToWrite, bool overrideExisting);
        bool Write(object dataToWrite);
        object Deserialize<T>();
    }
}
