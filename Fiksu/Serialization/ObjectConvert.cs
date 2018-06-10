using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fiksu.Serialization
{
    public static class ObjectConvert
    {
        [ThreadStatic]
        private static BinaryFormatter _formatter;

        public static string ToBase64(object obj)
        {
            if (_formatter == null)
                _formatter = new BinaryFormatter();

            using(var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static T FromBase64<T>(string encoded)
        {
            if (_formatter == null)
                _formatter = new BinaryFormatter();

            var data = Convert.FromBase64String(encoded);
            using (var ms = new MemoryStream(data))
                return (T)_formatter.Deserialize(ms);
        }
    }
}
