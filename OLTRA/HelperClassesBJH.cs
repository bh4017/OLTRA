namespace HelperClassesBJH
{
    using System;
    using Gtk;

	public class MessageBox
	{
        public static void Show(string msg, MessageType type = MessageType.Info)
		{
            MessageDialog md = new MessageDialog(null, DialogFlags.Modal, type, ButtonsType.Ok, msg);
			md.Run();
			md.Destroy();
		}
	}

    public class ConsoleMessage
    {
        public static void WriteLine(string message)
        {
            DateTime dt = DateTime.Now;
            Console.WriteLine(String.Format("{0:yyyy-MM-dd  HH:mm:ss} | {1}", dt, message));
        }
    }
}

