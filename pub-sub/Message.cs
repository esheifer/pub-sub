using System;
namespace pub_sub
{
    public class Message
    {
        public string Data { get; set; }

        public Message(string data)
        {
            Data = data;
        }
    }

    public class ProcessedMessage
    {
        public string Data { get; set; }

        public ProcessedMessage(string data)
        {
            Data = data;
        }
    }
}
