using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WebSphereLib.Messages;

namespace WebSphereLibTest.Test
{
    class WebSphereLibTest
    {
        /// <summary>
        /// Serilizes the object to string
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="obj">Obj</param>
        /// <returns></returns>
        public string SerializeObject<T>(T obj)
        {
            XmlSerializer serializer = null;
            serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, obj);
                writer.Flush();
                return sb.ToString();
            }
        }

        static void Main(string[] args)
        {
            WebSphereLibTest obj = new WebSphereLibTest();

            PartDeliveredMessage partDeliveredMessage = new PartDeliveredMessage();
            partDeliveredMessage.MessageType = MessageType.PartDelivered;
            partDeliveredMessage.RouteColor = RouteColor.Red;
            partDeliveredMessage.PartNumber = "100";
            partDeliveredMessage.Timestamp = DateTime.Now;
            partDeliveredMessage.UID = 1;

            string xmlPartDeliveredMessage = obj.SerializeObject<PartDeliveredMessage>(partDeliveredMessage);

            PartReorderMessage partReorderMessage = new PartReorderMessage();
            partReorderMessage.MessageType = MessageType.PartReorder;
            partReorderMessage.RouteColor = RouteColor.Blue;
            partReorderMessage.PartNumber = "101";
            partReorderMessage.Timestamp = DateTime.Now;
            partReorderMessage.UID = 1;

            string xmlPartReorderMessage = obj.SerializeObject<PartReorderMessage>(partReorderMessage);

            MQManager myMQ = new MQManager();

            string strQueueManagerName = string.Empty;
            string strQueueName = string.Empty;
            string strChannelInfo = string.Empty;
            string getQueueName = string.Empty;
            string putQueueName = string.Empty;
            string putText = string.Empty;            

            strQueueManagerName = "queuemanager";
            strQueueName = "omniIdQueue";
            strChannelInfo = "SYSTEM.ADMIN.SVRCONN/TCP/192.168.10.221(1414)";
            getQueueName = "omniIdQueue";
            putQueueName = "omniIdQueue";
            putText = xmlPartDeliveredMessage;

            var connectStatus = myMQ.ConnectMQ(strQueueManagerName, strChannelInfo);


            var writeStatus = myMQ.WriteLocalQMsg(putText, putQueueName);
            var getStatus = myMQ.ReadLocalQMsg(getQueueName);

        }
    }
}
