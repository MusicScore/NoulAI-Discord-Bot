using System.IO;

namespace NoulAIBotNetCore.Configuration
{
    public interface ILoadedConfig
    {
        T Load<T>(Stream stream);
    }
}
