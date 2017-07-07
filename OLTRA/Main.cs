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
            Gtk.Settings.Default.ThemeName = "Breeze Dark";
            Gtk.Application.Run();
        }
        #endregion
        #region DESTRUCTORS
        #endregion
        #region DELEGATES
        #endregion
        #region EVENTS
        #endregion
        #region ENUMS
        #endregion
        #region INTERFACES
        #endregion
        #region PROPRERTIES
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public static void Main()
        {
            new MainWindow();
        }
        #region EVENT HANDLERS
        private void onDeleteEvent(object sender, DeleteEventArgs e)
        {
            Application.Quit();
        }
        private void onProjNewClicked(object sender, EventArgs e)
        {
			btn_proj_new.Label = "hello";
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
                System.Configuration.ConfigurationSettings.AppSettings["ProjectsPath"] = fc.Filename;

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

