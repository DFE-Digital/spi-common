namespace Dfe.Spi.Common.UnitTesting
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class AssemblyExtensions
    {
        public static string GetSample(this Assembly assembly, string name)
        {
            string toReturn = null;

            string[] resources = assembly.GetManifestResourceNames();

            string fullPath = resources.SingleOrDefault(x => x.EndsWith(name));

            using (Stream stream = assembly.GetManifestResourceStream(fullPath))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    toReturn = streamReader.ReadToEnd();
                }
            }

            return toReturn;
        }
    }
}