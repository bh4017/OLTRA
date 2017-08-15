namespace HelperClassesBJH
{
    using System;
    using Gtk;

	public class MessageBox
	{
        public static void Show(string msg, MessageType type = MessageType.Info)
		{
            MessageDialog md = new MessageDialog(null, DialogFlags.Modal, type, ButtonsType.Ok, msg);
            ConsoleMessage.WriteLine(msg, type);
			md.Run();
			md.Destroy();
		}
	}

    public class ConsoleMessage
    {
        public static void WriteLine(string message, MessageType type = MessageType.Info)
        {
            DateTime dt = DateTime.Now;
            switch(type)
            {
                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
            Console.WriteLine(String.Format("{0:yyyy-MM-dd  HH:mm:ss.ff} | {1}", dt, message));
            Console.ResetColor();
        }
    }
}

