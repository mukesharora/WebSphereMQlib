//-----------------------------------------------------------------------
//
// <copyright file="PartReorderMessage.cs" company="Omni-ID, Ltd.">
//
// Copyright (c) 2013, 2014 - Omni-ID, Ltd. All rights reserved.
//
// <author>Omni-ID</author>
//
// </copyright>
//
//-----------------------------------------------------------------------

using System;
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
