using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WebSphereLib.Messages
{
    [XmlRoot]
    public class PartDeliveredMessage
    {
        public PartDeliveredMessage()
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

    public enum MessageType
    {
        [XmlEnum(Name = "PartReorder")]
        PartReorder,
        [XmlEnum(Name = "PartDelivered")]
        PartDelivered
    }

    public enum RouteColor
    {
        [XmlEnum(Name = "Red")]
        Red,
        [XmlEnum(Name = "Blue")]
        Blue,
        [XmlEnum(Name = "Purple")]
        Purple,
        [XmlEnum(Name = "Orange")]
        Orange
    }    
}
