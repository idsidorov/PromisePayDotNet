using System.IO;
using System.Reflection;

namespace PromisePayDotNet.Tests
{
    class Files
    {
        private static string loc = Path.GetDirectoryName( typeof(Files).GetTypeInfo().Assembly.Location);
        public static string ReadAllText(string path)
        {
            return File.ReadAllText(Path.Combine(loc, path));
        }
    }
}
