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
        /// <summary>
        /// File initialilize
        /// </summary>
        static XmlTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        #region Save Load With Xml Serializer
        /// <summary>
        /// Generic function to save a list in the XML file that match the given path using XmlSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
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
        /// <summary>
        /// Generic function that returns a list of items from the file that match the given path using XmlSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns> List of items located in the wanted XML file</returns>

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
        #endregion
        #region  Save Load With XElement

        // <summary>
        /// Generic function that returns a list of items from the file that match the given path using XElement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns> XElement items located in the wanted XML file</returns>
        public static XElement LoadListFromXElement(string filePath)
        {
            if (File.Exists(dir + filePath))
                return LoadData(filePath);
            else
                return CreateFiles(filePath);
        }
        /// <summary>
        /// Generic function to save a list in the XML file that match the given path using XElement
        /// </summary>
        /// <param name="EntityList"></param>
        /// <param name="filePath"></param>

        public static void SaveListToXElement(XElement EntityList, string filePath)
        {
            try
            {
                EntityList.Save(dir + filePath);
            }
            catch (Exception ex)
            {
                throw new XmlFileLoadCreateException(filePath, $"failed to create xml file: {filePath}", ex);
            }
        }
        /// <summary>
        /// Return XElement type in the XML file located in the given path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>XElement</returns>

        public static XElement LoadData(string filePath)
        {
            try
            {
                return XElement.Load(dir + filePath);
            }
            catch (FileNotFoundException ex)
            {
                throw new XmlFileLoadCreateException(filePath, $"failed to load xml file: {filePath}", ex);
            }
        }
        /// <summary>
        /// Create XML file with the given path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>XElement</returns>

        public static XElement CreateFiles(string filePath)
        {
            string rootName = filePath.Split(".")[0];
            Root = new XElement(rootName);
            Root.Save(dir + filePath);
            return Root;
        }
        #endregion
    }
}
