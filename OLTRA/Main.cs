namespace OLTRA
{
    using Gtk;
    using System;
    using System.Configuration;

    public class MainWindow
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
		[Builder.Object] private Button btn_proj_new;				
		[Builder.Object] private Button btn_proj_status;			
		[Builder.Object] private Button btn_proj_delete;
        [Builder.Object] private Entry ent_proj_path;
        
        #endregion
        #region CONSTRUCTORS
        public MainWindow()
        {
            Gtk.Application.Init();
            Builder Gui = new Builder();
            Gui.AddFromFile("../../Resources/MainWindow.glade");
			Gui.Autoconnect(this);
            Gtk.Settings.Default.ThemeName = "Adwaita-Dark-Green";
            Gtk.Application.Run();
            GLib.Idle.Add(Startup);
        }
        #endregion
        #region DESTRUCTORS
        #endregion
        #region DELEGATES
        public delegate bool InitialStartupCallBack();
        #endregion
        #region EVENTS
        #endregion
        #region ENUMS
        #endregion
        #region INTERFACES
        #endregion
        #region PROPRERTIES
        public Settings OLTRAsettings { get; set;}
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public static void Main()
        {
            new MainWindow();


        }
        public void ConsoleMessage(string message)
        {
            DateTime dt = DateTime.Now;
            Console.WriteLine(String.Format("{0:yyyy-MM-dd  HH:mm:ss} | {1}", dt, message));
        }
        public bool Startup()
        {
            OLTRAsettings = new Settings();
            ConsoleMessage("Running initial startup routine");
            return true;
        }
        public bool CloseApplication()
        {
            Application.Quit();
            return true;
        }
        #region EVENT HANDLERS
        private void OnDeleteEvent(object sender, DeleteEventArgs e)
        {           
            e.RetVal = CloseApplication();
            Application.Quit();
            string m = "OnDeleteEvent done: " + e.RetVal;
            ConsoleMessage(m);
            ConsoleMessage("Bye!");
        }
        private void onProjNewClicked(object sender, EventArgs e)
        {
            ConsoleMessage("hello");
        }
        private void onProjStatusClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void onProjDeleteClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
		private void onProjChoosePathClicked(object sender, EventArgs e)
		{
			FileChooserDialog fc = new FileChooserDialog ("Choose Project Path", null, FileChooserAction.SelectFolder, "Cancel",ResponseType.Cancel, "OK",ResponseType.Accept);
			if (fc.Run () == (int)ResponseType.Accept) 
            {
                OLTRAsettings.HomeVar = fc.Filename;
			} 
			else
				Console.WriteLine ("Cancelled Project Path setting");
			
            fc.Destroy ();
		}
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion           
    }
}

