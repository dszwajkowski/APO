using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ApoCore
{
    /// <summary>
    /// Provides static method that allows deep clone object
    /// </summary>
    public static class DeepCloneObject
    {
        /// <summary>
        /// Creates deep clone of the object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
