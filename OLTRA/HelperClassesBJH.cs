namespace HelperClassesBJH
{
    using System;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public static class ConsoleMessage
    {
        /* METHODS */
        public static void WriteLine(string message, MessageBoxIcon type = MessageBoxIcon.Information)
        {
            DateTime dt = DateTime.Now;
            switch(type)
            {
                case MessageBoxIcon.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case MessageBoxIcon.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
            Console.WriteLine(String.Format("{0:yyyy-MM-dd  HH:mm:ss.ff} | {1}", dt, message));
            MessageEventArgs e = new MessageEventArgs(dt, message, type);
            MessageOutput(null, e);
            Console.ResetColor();
        }
        /* EVENTS */
        public static event EventHandler<MessageEventArgs> MessageOutput = delegate {};

        /* CLASSES */

    }
    public class MessageEventArgs
    {
        public DateTime Dt { get; private set;}
        public string Message { get; private set;}
        public MessageBoxIcon Type { get; private set; }

        /* CONSTRUCTORS */
        public MessageEventArgs()
        {

        }
        public MessageEventArgs(DateTime dt, string message, MessageBoxIcon type)
        {
            Dt = dt;
            Message = message;
            Type = type;
        }
    }
}

