namespace OLTRA
{
    using Gtk;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.RegularExpressions;
    using System.Reflection.Context;
    using System.Linq;
    using HelperClassesBJH;

    public class MainWindow
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        #region GLADE FIELDS
        #pragma warning disable 169     // Disable the "object never used" warning.  This warning happens when we are using controls that are generated via the Glade file.
        #pragma warning disable 649
        #region WINDOWS
        [Builder.Object] private Window window1;
        #endregion
        #region BUTTONS
		[Builder.Object] private Button btn_proj_new;				
		[Builder.Object] private Button btn_proj_status;			
		[Builder.Object] private Button btn_proj_delete;
        #endregion
        #region ENTRY
        [Builder.Object] private Entry ent_proj_path;
        #endregion
        #region LISTSTORES & TREEVIEWS
        [Builder.Object] private ListStore lst_listeners;
        [Builder.Object] private ListStore lst_lsnr_types;
        [Builder.Object] private ListStore lst_projects;
        [Builder.Object] private ListStore lst_debug;
        [Builder.Object] private TreeView trv_projects;
        [Builder.Object] private CellRendererText cell_text_proj_name;
        [Builder.Object] private CellRendererText cell_text_proj_description;
        [Builder.Object] private CellRendererToggle cell_toggle_proj_status;
        [Builder.Object] private CellRendererText cell_text_lsnr_types;
        [Builder.Object] private TreeViewColumn col_proj_description;
        [Builder.Object] private TreeViewColumn col_proj_status;
        [Builder.Object] private TreeViewColumn col_proj_name;
        [Builder.Object] private TreeViewColumn col_lsnr_base;                  // A column in lst_lsnr_types of type ListenerBase
        [Builder.Object] private TreeViewColumn col_lsnr_type;                  // A column in lst_lsnr_types of type string
        #endregion
        #region COMBOBOXES
        [Builder.Object] private ComboBox cmb_lsnr_types;
        #endregion
        #region DIALOGUES
        /* ADD LSNR DIALOG */
        [Builder.Object] private Dialog AddLsnrDialog;
        [Builder.Object] private Button btn_add_lsnr;
        [Builder.Object] private Button btn_add_lsnr_cancel;
        [Builder.Object] private Entry ent_lsnr_title;
        [Builder.Object] private Entry ent_lsnr_description;
        [Builder.Object] private ToggleButton tgl_lsnr_status;
        /* ABOUT DIALOG */
        [Builder.Object] private AboutDialog dlg_about;
        #endregion
        #pragma warning restore 169
        #pragma warning restore 649
        #endregion

        #endregion
        #region CONSTRUCTORS
        public MainWindow()
        {
            Gtk.Application.Init();
            Builder Gui = new Builder();
            Gui.AddFromFile("../../Resources/MainWindow.glade");
			Gui.Autoconnect(this);
            //Gtk.Settings.Default.ThemeName = "VimixDark";
            Gtk.CssProvider css_provider = new Gtk.CssProvider();
            css_provider.LoadFromPath("../../Resources/OLTRAtheme/gtk.css");
            Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, css_provider, 800);
            GLib.Idle.Add(Startup); // run the Startup method next time application is idle.
            Gtk.Application.Run();
        }
        #endregion
        #region DESTRUCTORS
        #endregion
        #region DELEGATES
        public delegate bool InitialStartupCallBack();
        public delegate void Test(CellRenderer cr);
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
        public string SettingsFile { get; private set;}
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public static void Main()
        {
            new MainWindow();
        }
        public bool CloseApplication()
        {
            Application.Quit();
            return true;
        }
        private void ApplyCss (Widget widget, CssProvider provider, uint priority)
        {
            widget.StyleContext.AddProvider (provider, priority);
            var container = widget as Container;
            if (container != null) 
            {
                foreach (var child in container.Children) 
                {
                    ApplyCss (child, provider, priority);
                }
            }
        }
        private bool Startup()
        {
            ConsoleMessage.MessageOutput += new EventHandler<MessageEventArgs>(MessageOutput);
            ConsoleMessage.WriteLine("Running initial startup routine.");
            ConsoleMessage.WriteLine("Detected platform: " + Environment.OSVersion.Platform.ToString());
            ConsoleMessage.WriteLine("Maximizing window size...");
            window1.Maximize();
            /* INSTANTIATE SETTINGS AND DETECT PLATFORM */
            OLTRAsettings = new Settings();
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
                SettingsFile = Home + "/" + "settings.oltra";
            }
            /* LOAD EXISTING SETTINGS */
            if (File.Exists(SettingsFile))
            {
                // Load Settings
                ConsoleMessage.WriteLine("Loading OLTRA settings");
                try
                {
                    IFormatter bf = new BinaryFormatter();
                    using (FileStream fs = File.OpenRead(SettingsFile))
                    {
                        Settings s = (Settings)bf.Deserialize(fs);
                        OLTRAsettings = s;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Failed to deserialise settings file\n" + ex.Message + "\nApplication will now quit!", MessageType.Error);
                    Application.Quit();
                }
                finally
                {
                    ConsoleMessage.WriteLine("Settings successfully loaded.  Project Directory: " + OLTRAsettings.ProjectDir + "\n");

                }
            }
            else
            {
                // Default Settings
                ConsoleMessage.WriteLine("OLTRA settings not found, creating default", MessageType.Warning);
                OLTRAsettings.ProjectDir = Home + "/OLTRAprojects";
                if (!Directory.Exists(OLTRAsettings.ProjectDir))
                {
                    ConsoleMessage.WriteLine("Creating new project directory at: " + OLTRAsettings.ProjectDir);
                    Directory.CreateDirectory(OLTRAsettings.ProjectDir);
                }
                ConsoleMessage.WriteLine("Writing default settings to: " + SettingsFile);
                try
                {
                    IFormatter bf = new BinaryFormatter();
                    using (FileStream fs = File.Create(SettingsFile))
                        bf.Serialize(fs, OLTRAsettings);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Failed to write default settings! Error:\n" + ex.Message + " Application will now quit", MessageType.Error);
                    Application.Quit();
                }
            }
            /* SETUP PROJECT TREEVIEW & ASSOCIATED LISTSTORE */
            lst_projects = new ListStore(typeof(Project));
            trv_projects.Model = lst_projects;

            /* SETUP CELL DATA FUNCTIONS */
            SetupCellDataFunctions();
            /* LOAD EXISTING PROJECTS */
            LoadProjects();

            return false;
        }
        private void SaveProject()
        {

        }
        private void LoadProjects()
        {
            ConsoleMessage.WriteLine("Loading existing projects...");
            string regexPattern = @".*\.olp$";      // A regex pattern for matching names ending in .olp
            string[] projects = Directory.GetFiles(OLTRAsettings.ProjectDir);
            if (projects.Length > 0)
            {
                for (int i = 0; i < projects.Length; i++)
                {
                    Match result = Regex.Match(projects[i], regexPattern);
                    if (result.Success)
                    {
                        ConsoleMessage.WriteLine("Found project file: " + projects[i]);
                        IFormatter bf = new BinaryFormatter();
                        try
                        {
                            using (FileStream fs = File.OpenRead(projects[i]))
                            {
                                Project p = (Project)bf.Deserialize(fs);
                                lst_projects.AppendValues(p);
                                ConsoleMessage.WriteLine("Added Project " + p.Title);
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Failed to deserialise Projects file " + projects[i] + "Error: " + ex.Message, MessageType.Error);
                        }
                    }
                }
            }
            else
            {
                ConsoleMessage.WriteLine("No projects found!");
            }
        }
        private void RenderProjectName(TreeViewColumn col, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            Project p = (Project)model.GetValue(iter, 0);
            (cell as CellRendererText).Text = p.Title;
        }
        private void RenderProjectDescription(TreeViewColumn col, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            Project p = (Project)model.GetValue(iter, 0);
            (cell as CellRendererText).Text = p.Description;
        }
        private void RenderProjectStatus(TreeViewColumn col, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            Project p = (Project)model.GetValue(iter, 0);
            (cell as CellRendererToggle).Active = p.Status;
        }
//        private void RenderLsnrTypes(TreeViewColumn col, CellRenderer cell, ITreeModel model, TreeIter iter)
//        {
//            ListenerBase l = (ListenerBase)model.GetValue(iter, 0);
//            (cell as CellRendererText).Text = l.Title;
//        }
        private void SetupCellDataFunctions()
        {
            /* SETUP SETCELLDATAFUNCTIONS */
            ConsoleMessage.WriteLine("Setting up 'SetCellDataFunctions:");
            ConsoleMessage.WriteLine("\t[RenderProjectName]");
            col_proj_name.SetCellDataFunc(cell_text_proj_name, new TreeCellDataFunc(RenderProjectName));
            ConsoleMessage.WriteLine("\t[RenderProjectDescription]");
            col_proj_description.SetCellDataFunc(cell_text_proj_description, new TreeCellDataFunc(RenderProjectDescription));
            ConsoleMessage.WriteLine("\t[RenderProjectStatus]");
            col_proj_status.SetCellDataFunc(cell_toggle_proj_status, new TreeCellDataFunc(RenderProjectStatus));
//            ConsoleMessage.WriteLine("\t[RenderLsnrTypes]");
//            col_lsnr_type.SetCellDataFunc(cell_text_lsnr_types, new TreeCellDataFunc(RenderLsnrTypes));
        }
        #region EVENT HANDLERS
        private void MessageOutput(object sender, MessageEventArgs e)
        {
            string t = String.Format("{0:yyyy-MM-dd  HH:mm:ss.ff}", e.Dt);
            string colour = "Green";
            string type = "INFO";
            switch(e.Type)
            {
                case MessageType.Warning:
                    colour = "Yellow";
                    type = "INFO";
                    break;
                case MessageType.Error:
                    colour = "Red";
                    type = "ERROR";
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
            lst_debug.AppendValues(t, type, e.Message, colour);
        }
        private void OnDeleteEvent(object sender, DeleteEventArgs e)
        {           
            e.RetVal = CloseApplication();
            Application.Quit();
            string m = "OnDeleteEvent done: " + e.RetVal;
            ConsoleMessage.WriteLine(m);
            ConsoleMessage.WriteLine("Bye!");
        }
        private void OnProjNewClicked(object sender, EventArgs e)
        {
            Project p = new Project();
            p.Title = "Enter a title";
            p.Description = "Enter a description";
            lst_projects.AppendValues(p);
        }
        private void OnProjStatusClicked(object sender, EventArgs e)
        {
            SetupCellDataFunctions();
        }
        private void OnProjDeleteClicked(object sender, EventArgs e)
        {
            TreeSelection selection = trv_projects.Selection;
            TreeIter iter;       
            selection.GetSelected (out iter);
            Project p = (Project)lst_projects.GetValue(iter, 0);
            if (p == null)
            {
                MessageBox.Show("Please select the project you want to delete first!", MessageType.Error);
                return;
            }
            lst_projects.Remove(ref iter);
            if(File.Exists(OLTRAsettings.ProjectDir + "/" + p.Title + ".olp"))
                File.Delete(OLTRAsettings.ProjectDir + "/" + p.Title + ".olp");
        }
        private void OnProjSaveClicked(object sender, EventArgs e)
        {
            TreeSelection selection = trv_projects.Selection;
            TreeIter iter;       
            selection.GetSelected (out iter);
            Project p = (Project)lst_projects.GetValue(iter, 0);
            if (p == null)
            {
                MessageBox.Show("Please select the project you want to save first!", MessageType.Error);
                return;
            }
            IFormatter bf = new BinaryFormatter();
            try
            {
                using (FileStream fs = File.Create(OLTRAsettings.ProjectDir + "/" + p.Title + ".olp"))
                    bf.Serialize(fs, p);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void On_btn_lsnr_add_clicked(object sender, EventArgs e)
        {
//            TreeSelection projSelection = trv_projects.Selection;
//            TreeIter iter;
//            projSelection.GetSelected(out iter);
//            Project p = (Project)lst_projects.GetValue(iter, 0);
//            if (p == null)
//            {
//                MessageBox.Show("Please select the project you want to add a listener to first!", MessageType.Error);
//                return;
//            }
//            tgl_lsnr_status.Active = true;
//            /* Setup Combobox */
//            // Get all the sub classes of ListenerBase.
//            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsSubclassOf(typeof(ListenerBase)));
//            // Add an instance of each subclass of ListenerBase to a ListStore.
//            foreach (Type t in types)
//            {
//                var o = (Activator.CreateInstance(t));
//                lst_lsnr_types.AppendValues(o, o.ToString());   // The combobox shows the information in parameter 1 so we know what type of listener we're selecting.  It may be possible to do all this with one column instead using a CellRenderer method. 2017-08-20 BJH.
//            }
//            cmb_lsnr_types.Active = 0;
//            AddLsnrDialog.Run();
//            AddLsnrDialog.Destroy();
        }
        private void On_cell_text_proj_name_edited(object sender, EditedArgs e)
        {
            TreeIter iter;
            lst_projects.GetIter(out iter, new TreePath(e.Path));
            Project p = (Project)lst_projects.GetValue(iter, 0);
            p.Title = e.NewText;
        }
        private void On_cell_text_proj_description_edited(object sender, EditedArgs e)
        {
            TreeIter iter;
            lst_projects.GetIter(out iter, new TreePath(e.Path));
            Project p = (Project)lst_projects.GetValue(iter, 0);
            p.Description = e.NewText;
        }
        private void On_cell_toggle_proj_status_toggled(object sender, ToggledArgs e)
        {
            TreeIter iter;
            lst_projects.GetIter(out iter, new TreePath(e.Path));
            Project p = (Project)lst_projects.GetValue(iter, 0);
            p.Status = !(p.Status);
        }
		private void OnChooseProjPathClicked(object sender, EventArgs e)
		{
            Button b = (Button)sender;
            ConsoleMessage.WriteLine(b.ToString() + " Button clicked");
		}
        private void on_btn_add_lsnr_add_clicked(object sender, EventArgs e)
        {
            AddLsnrDialog.Hide();
        }
        private void on_about_activate(object sender, EventArgs e)
        {
            dlg_about.Show();
        }
        private void on_nbk_engineering_select_page(object sender, SelectPageArgs e)
        {
            ConsoleMessage.WriteLine("page selected");
        }
        private void on_swt_application_debug_state_set(object sender, EventArgs e)
        {
            ConsoleMessage.WriteLine("hello");
        }
        private void on_swt_application_debug_activate(object sender, EventArgs e)
        {
            ConsoleMessage.WriteLine("hello");
        }
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion           
    }
}

