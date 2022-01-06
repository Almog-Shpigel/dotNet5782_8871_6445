using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAL
{
    class XmlTools
    {
        static string dir = @"..\..\..\..\Data\";
        static XElement Root;
        static XmlTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        #region Save Load With Xml Serializer
        public static void SaveListToXmlSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new(dir + filePath, FileMode.Create);
                XmlSerializer x = new(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new XmlFileLoadCreateException(filePath, $"failed to create xml file: {filePath}", ex);
            }
        }

        public static List<T> LoadListFromXmlSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new(typeof(List<T>));
                    FileStream file = new(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (FileNotFoundException ex)
            {
                throw new XmlFileLoadCreateException(filePath, $"failed to load xml file: {filePath}", ex);
            }
        }

        public static XElement LoadListFromXElement(string filePath)
        {
            if (!File.Exists(filePath))
                return CreateFiles(filePath);
            else
                return LoadData(filePath);
        }
        public static void SaveListToXElement(XElement EntityList, string filePath)
        {
            try
            {
                EntityList.Save(dir+filePath);
            }
            catch (Exception ex)
            {
                throw new XmlFileLoadCreateException(filePath, $"failed to create xml file: {filePath}", ex);
            }
        }

        private static XElement LoadData(string filePath)
        {
            try
            {
                return XElement.Load(filePath);
            }
            catch (FileNotFoundException ex)
            {
                throw new XmlFileLoadCreateException(filePath, $"failed to load xml file: {filePath}", ex);
            }
        }

        private static XElement CreateFiles(string filePath)
        {
            Root = new XElement("");
            Root.Save(filePath);
            return Root;
        }
        #endregion
    }
}
