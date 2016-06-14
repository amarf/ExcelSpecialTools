using System.IO;

namespace Core.Interfaces
{
    public interface IDataService
    {
        void SerializeObject<T>(T data, string path);

        void SerializeObject<T>(T data, Stream stream);

        T DeserializeObject<T>(string path);

        T DeserializeObject<T>(Stream stream);
    }
}