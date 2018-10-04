using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ImageApi_Tests.TestFiles
{
    public static class EmbededData
    {
        public static string AsString(string fullName)
        {
            var stream = AsStream(fullName);
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream AsStream(string name)
        {
            try
            {
                var callingAssembly = Assembly.GetCallingAssembly();
                var resources = callingAssembly.GetManifestResourceNames()
                    .Where(r => r.EndsWith(name))
                    .ToList();

                if (resources.Count == 0)
                {
                    throw new ArgumentException($"Embeded resource not found, name: {name}");
                }

                if (resources.Count > 1)
                {
                    throw new ArgumentException($"Multiple Embeded reosurces found, name: {name}");
                }

                var fullName = resources[0];
                return callingAssembly.GetManifestResourceStream(fullName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                throw;
            }
        }


    }
}
