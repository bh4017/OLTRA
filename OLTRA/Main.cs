namespace OLTRA
{
    using Gtk;
    using System;

    public class MainWindow
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
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
        static void onDeleteEvent(object o, DeleteEventArgs args)
        {
            Application.Quit();
        }
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion           
    }
}

