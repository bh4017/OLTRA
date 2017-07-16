using System;
using Gtk;
namespace HelperClassesBJH
{
	public class MessageBox
	{
        public static void Show(string msg, MessageType type = MessageType.Info)
		{
            MessageDialog md = new MessageDialog(null, DialogFlags.Modal, type, ButtonsType.Ok, msg);
			md.Run();
			md.Destroy();
		}
	}
}

