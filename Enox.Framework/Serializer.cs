using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Enox.Framework
{
    public static class Serializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="objectToSerialize"></param>
        public static void SerializeObject(string filename, object objectToSerialize)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                formatter.Serialize(stream, objectToSerialize);
                byte[] serializedData = stream.ToArray();

                File.WriteAllBytes(filename, serializedData);
            }
            catch (Exception ex)
            {
                Console.Write(string.Format("Error!\nError Message: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static object DeserializeObject(string filename)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(filename);

                MemoryStream stream = new MemoryStream(bytes);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;

                return (Object)formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                Console.Write(string.Format("Error on deserialization!\nError Message: {0}\n{1}", ex.Message, ex.StackTrace));
                return null;
            }

        }
    }
}
