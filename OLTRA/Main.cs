namespace OLTRA
{
    using Gtk;
    using System;
    using System.Configuration;
    using System.IO;
    using HelperClassesBJH;

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
            Gtk.Settings.Default.ThemeName = "Equilux";
            GLib.Idle.Add(Startup); // run the Startup method next time application is idle.
            Gtk.Application.Run();
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
        public string Home { get; set;}
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public static void Main()
        {
            new MainWindow();
        }
        public bool Startup()
        {
            OLTRAsettings = new Settings();
            ConsoleMessage.WriteLine("Running initial startup routine.");
            ConsoleMessage.WriteLine("Detected platform: " + Environment.OSVersion.Platform.ToString());
            /* FIND HOME DIRECTORY */
            string path = Environment.GetEnvironmentVariable("HOME", EnvironmentVariableTarget.Process);   // OLTRA will store settings in $HOME so first it needs to be set!
            if (String.IsNullOrEmpty(path))
            {
                ConsoleMessage.WriteLine("Home directory not set!");
                //                FileChooserDialog fc = new FileChooserDialog("Choose HOME directory", null, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "OK", ResponseType.Accept);
                //                if (fc.Run() == (int)ResponseType.Accept)
                //                {
                //                    Environment.SetEnvironmentVariable("HOME", fc.Filename, EnvironmentVariableTarget.Machine);
                //                }
                //                else
                //                {
                //                    ConsoleMessage("Home directory setting cancelled - application will exit!");
                //                    Application.Quit();
                //                }
                //
                //                fc.Destroy();
                MessageBox.Show("$HOME environment variable not set!\nContact IT administrator!\n\nApplication will now quit", type: MessageType.Error);
                Application.Quit();
            }
            else
            {
                ConsoleMessage.WriteLine("Home directory found: " + path);
                Home = path;
            }
            /* LOAD EXISTING SETTINGS */
            if (File.Exists(Home + ".oltra"))
            {
                // Load Settings
                ConsoleMessage.WriteLine("Loading OLTRA settings");

            }
            else
            {
                // Default Settings
            }

        

            return false;
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
            ConsoleMessage.WriteLine(m);
            ConsoleMessage.WriteLine("Bye!");
        }
        private void onProjNewClicked(object sender, EventArgs e)
        {
            ConsoleMessage.WriteLine("hello");
        }
        private void onProjStatusClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void onProjDeleteClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
		private void OnChooseProjPathClicked(object sender, EventArgs e)
		{
            Button b = (Button)sender;
            ConsoleMessage.WriteLine(b.ToString() + " Button clicked");
		}
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion           
    }
}

