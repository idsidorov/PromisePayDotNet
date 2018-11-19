using System.Configuration;

namespace PromisePayDotNet.Settings
{
    public interface ISettings
    {
        string Url { get; }
        string Login { get; }
        string Password { get; }
    }

    public class Settings : ISettings
    {
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
