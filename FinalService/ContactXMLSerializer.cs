using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Homework1;

namespace FinalService
{
    public class ContactXMLSerializer
    {
        public void SaveToXML(List<Contact> contacts, string fileaddress)

        {
            using (FileStream fileStream = new FileStream(fileaddress, FileMode.OpenOrCreate))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Contact>));
                xmlSerializer.Serialize(fileStream, contacts);
            }
        }
          
    }
}