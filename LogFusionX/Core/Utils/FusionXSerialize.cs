using LogFusionX.StructuredLogWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LogFusionX.Core.Utils
{
    internal class FusionXSerialize
    {
        public string SerializeToXml(StructuredLogEntry structuredLogEntry)
        {
            var xmlSerializer = new XmlSerializer(typeof(StructuredLogEntry));
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, structuredLogEntry);
                }
                return stringWriter.ToString();
            }
        }
        public string SerializeToCsv(StructuredLogEntry structuredLogEntry)
        {
            var csvBuilder = new StringBuilder();
            if (structuredLogEntry.logData != null)
            {
                foreach (var item in structuredLogEntry.logData)
                {
                    csvBuilder.AppendLine($"{item.Key},{item.Value}");
                }
            }
            return csvBuilder.ToString();
        }
        public string SerializeToConsole(StructuredLogEntry structuredLogEntry)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine($"Message: {structuredLogEntry.message ?? string.Empty}");
            csvBuilder.AppendLine("Data:");
            if (structuredLogEntry.logData != null)
            {
                foreach (var item in structuredLogEntry.logData)
                {
                    csvBuilder.AppendLine($"{item.Key} : {item.Value}");
                }
            }
            return csvBuilder.ToString();
        }
    }
}
