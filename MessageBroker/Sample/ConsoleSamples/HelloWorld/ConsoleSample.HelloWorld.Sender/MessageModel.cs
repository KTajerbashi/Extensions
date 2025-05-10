using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSample.HelloWorld.Sender;

public class MessageModel
{
    public string Name { get; set; }
    public string Message { get; set; }
    public MessageModel(string name, string message)
    {
        Name = name;
        Message = message;
    }
    public static List<MessageModel> GetMessages()
    {
        var messages = new List<MessageModel>();
        messages.Add(new MessageModel("Tajerbashi", "Hello How Are You ?"));
        messages.Add(new MessageModel("Mohammadi", "Hello How Are You ?"));
        messages.Add(new MessageModel("Sharifi", "Hello How Are You ?"));
        messages.Add(new MessageModel("Javadi", "Hello How Are You ?"));
        messages.Add(new MessageModel("Rezaie", "Hello How Are You ?"));
        messages.Add(new MessageModel("Jahferi", "Hello How Are You ?"));
        messages.Add(new MessageModel("Kamali", "Hello How Are You ?"));
        messages.Add(new MessageModel("Akbari", "Hello How Are You ?"));
        messages.Add(new MessageModel("Akrami", "Hello How Are You ?"));
        messages.Add(new MessageModel("Gulami", "Hello How Are You ?"));
        return messages;
    }
}