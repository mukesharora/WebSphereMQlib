using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WebSphereLib.Messages
{
    [XmlRoot]
    public class EntryLoadArea
    {
        public EntryLoadArea()
        {
        }

        [XmlElement]
        public string RFIDDeviceID
        {
            get;
            set;
        }

        [XmlElement]
        public string RFIDDeviceAddress
        {
            get;
            set;
        }

        [XmlElement]
        public string RFIDDeviceAntennaID
        {
            get;
            set;
        }

        [XmlElement]
        public string RFIDTagEPC
        {
            get;
            set;
        }

        [XmlElement]
        public DateTime TimeStamp
        {
            get;
            set;
        }

        [XmlElement]
        public int UID
        {
            get;
            set;
        }

        [XmlElement]
        public string PartNumber
        {
            get;
            set;
        }
    }
}
