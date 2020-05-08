using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace ConfigApp.Helpers
{
    public static class XMLHelper
    {
        #region public Methods

        public static IDictionary<String, Int32> columnDictionary = null;

        /// <summary>
        /// Generic method for the Get response From API
        /// </summary>
        /// <param name="type">Type of the Object</param>
        /// <param name="webServiceUrl">Api Url</param>
        /// <returns></returns>
        public static object XmlDeserializeObject(Type type, string webServiceUrl)
        {
            Object result = null;

            var request = (HttpWebRequest)WebRequest.Create(webServiceUrl);
            request.UserAgent = Constants.USER_AGENT;
            request.Method = Constants.METHOD_GET;
            request.Accept = Constants.REQUEST_ACCESPT_XML;
            request.Credentials = new NetworkCredential(Constants.API_USER_NAME, Constants.API_PASSWORD);
            request.ContentType = Constants.REQUEST_CONTENT_TYPE;

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) return null;
                var dataStream = response.GetResponseStream();
                var xSerializer = new XmlSerializer(type);
                if (dataStream != null)
                {
                    result = xSerializer.Deserialize(dataStream);
                }
            }
            catch (Exception exception)
            {
                // Need to Log exception 
            }
            return result;
        }

        /// <summary>
        ///  Output the result in either XML or Json format.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public static string FormatXMLString(string data)
        {
            XmlTextWriter xmlWriter = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml(data);

                    // Output formatted XML
                    StringBuilder xmlString = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(xmlString);
                    xmlWriter = new XmlTextWriter(stringWriter);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlResponse.WriteTo(xmlWriter);

                    return xmlString.ToString();
                }
                return "";
            }
            finally
            {
                if (xmlWriter != null)
                    xmlWriter.Close();
            }
        }

        /// <summary>
        ///  Output the result in either XML or Json format.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FormatJSONString(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    return data;
                }
                return "";
            }
            finally
            {
            }
        }

        /// <summary>
        /// This will convert xml node to class properites
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static object ConvertXMLToObject(XmlNode source, object destination)
        {
            foreach (XmlNode item in source.ChildNodes)
            {
                var info = destination.GetType().GetProperty(item.Name);
                if (info != null)
                {
                    info.SetValue(destination, item.InnerText, null);
                }
            }
            return destination;
        }

        public static object ToObject(DataTable table, object destination)
        {
            return destination;
        }

        public static string ToString(this System.Xml.XmlNodeList nodeList, int indentation = 0)
        {
            String returnStr = "";
            if (nodeList != null)
            {
                foreach (XmlNode node in nodeList)
                {
                    returnStr += node.OuterXml;
                }

            }
            return returnStr;
        }

        #endregion
    }
}
