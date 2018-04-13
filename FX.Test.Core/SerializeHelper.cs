using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FX.Test.Core
{
        public static class SerializeHelper
        {
            public static void SerializeToFile<T>(this T obj, string fileName)
            {
                if (obj == null)
                    throw new ArgumentNullException(nameof(obj));
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentNullException(nameof(fileName));

                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                File.WriteAllText(fileName, obj.Serialize());
            }

            public static string Serialize<T>(this T obj)
            {
                if (obj == null)
                    throw new ArgumentNullException(nameof(obj));

                var ms = new MemoryStream();
                new XmlSerializer(obj.GetType()).Serialize(ms, obj);
                var xmlString = Encoding.UTF8.GetString(ms.ToArray());
                return xmlString;
            }

            public static T DeserializeFromFile<T>(string fileName)
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentNullException(nameof(fileName));

                var str = File.ReadAllText(fileName);

                var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));

                return (T)new XmlSerializer(typeof(T)).Deserialize(ms);
            }

            public static object DeserializeFromFile(string fileName, Type type)
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentNullException(nameof(fileName));
                if (type == null)
                    throw new ArgumentNullException(nameof(type));

                var str = File.ReadAllText(fileName);

                var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));

                return new XmlSerializer(type).Deserialize(ms);
            }

            public static T DeserializeFromXml<T>(this XmlDocument data)
            {
                if (data == null)
                    throw new ArgumentNullException(nameof(data));

                // ReSharper disable once AssignNullToNotNullAttribute
                var reader = new XmlNodeReader(data.DocumentElement);
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }

            public static T DeserializeFromString<T>(this string xmlString)
            {
                if (string.IsNullOrWhiteSpace(xmlString))
                    throw new ArgumentNullException(nameof(xmlString));

                return (T)new XmlSerializer(typeof(T)).Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            }

            /// <summary>
            /// DeSerialize serialized string
            /// </summary>
            /// <param name="serializedData"></param>
            /// <param name="arrayTypes"></param>
            /// <returns></returns>
            public static ArrayList DeSerializeArrayList(this string serializedData, Type[] arrayTypes)
            {
                ArrayList list;
                var extraTypes = arrayTypes;
                XmlSerializer serializer = new XmlSerializer(typeof(ArrayList), extraTypes);
                XmlReader xReader = XmlReader.Create(new StringReader(serializedData));

                try
                {
                    object obj = serializer.Deserialize(xReader);
                    list = (ArrayList)obj;
                }
                finally
                {
                    xReader.Close();
                }
                return list;
            }
        }
    }
