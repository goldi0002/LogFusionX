using LogFusionX.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LogFusionX.StructuredLogWriter
{
    internal sealed class XFusionXMLWriter : XFileLoggerBase
    {
        public XFusionXMLWriter(string xmlFilePath) : base(xmlFilePath)
        {
            if (string.IsNullOrEmpty(xmlFilePath)) throw new ArgumentNullException(nameof(xmlFilePath));
        }

        public override void Write(object data)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(_fileFullPath, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("LogEntry");
                    if (data != null)
                    {
                        foreach (var property in data.GetType().GetProperties())
                        {
                            writer.WriteElementString(property.Name, property.GetValue(data)?.ToString());
                        }
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing to XML file: {ex.Message}");
            }
        }
    }
}
