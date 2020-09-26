using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re.Wpf
{
    /// <summary> Message types </summary>
    public enum MessageType
    {
        Information,
        Warning,
        Error
    }

    /// <summary> Represents a message. </summary>
    public class Message
    {
        /// <summary> Creates an error message .</summary>
        /// <param name="text"> Text of the message </param>
        /// <returns> Created message </returns>
        public static Message CreateError(string text)
        {
            return new Message() { Type = MessageType.Error, Text = text };
        }

        /// <summary> Creates an information message .</summary>
        /// <param name="text"> Text of the message </param>
        /// <returns> Created message </returns>
        public static Message CreateInformation(string text)
        {
            return new Message() { Type = MessageType.Information, Text = text };
        }

        /// <summary> Gets or sets type of the message. </summary>
        public MessageType Type { get; set; }

        /// <summary> Gets or sets text of the message. </summary>
        public string Text { get; set; }
    }
}
