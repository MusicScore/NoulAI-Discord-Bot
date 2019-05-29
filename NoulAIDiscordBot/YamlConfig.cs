using System.IO;
using YamlDotNet.Serialization;

namespace NoulAIBotNetCore.Configuration
{
    public class YamlConfig : ILoadedConfig
    {
        public IDeserializer Deserializer;
        public ISerializer Serializer;

        public YamlConfig(INamingConvention convention)
        {
            Deserializer = new DeserializerBuilder().WithNamingConvention(convention).Build();
            Serializer = new SerializerBuilder().WithNamingConvention(convention).Build();
        }

        public T Load<T>(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return Deserializer.Deserialize<T>(reader);
            }
        }
    }
}
