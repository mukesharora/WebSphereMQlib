using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WebSphereLib.Messages
{
    [XmlRoot]
    public class PartReorderMessage
    {
        public PartReorderMessage()
        {
        }

        [XmlElement]
        public DateTime Timestamp
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

        [XmlElement]
        public int UID
        {
            get;
            set;
        }

        [XmlElement]
        public MessageType MessageType
        {
            get;
            set;
        }

        [XmlElement]
        public RouteColor RouteColor
        {
            get;
            set;
        }
    }
}
