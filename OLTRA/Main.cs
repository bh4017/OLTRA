namespace OLTRA
{
    using Gtk;
    using System;

    public class MainWindow
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
		[Builder.Object] private Button btn_proj_new;				
		[Builder.Object] private Button btn_proj_status;			
		[Builder.Object] private Button btn_proj_delete;
        #endregion
        #region CONSTRUCTORS
        public MainWindow()
        {
            Gtk.Application.Init();
            Builder Gui = new Builder();
            Gui.AddFromFile("../../Resources/MainWindow.glade");
			Gui.Autoconnect(this);
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

        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion           
    }
}

