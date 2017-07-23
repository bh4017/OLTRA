namespace OLTRA
{
    using Gtk;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.RegularExpressions;
    using HelperClassesBJH;

    public class MainWindow
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        #pragma warning disable 169     // Disable the "object never used" warning.  This warning happens when we are using controls that are generated via the Glade file.
		[Builder.Object] private Button btn_proj_new;				
		[Builder.Object] private Button btn_proj_status;			
		[Builder.Object] private Button btn_proj_delete;
        [Builder.Object] private Entry ent_proj_path;
        [Builder.Object] private ListStore lst_listeners;
        [Builder.Object] private ListStore lst_projects;
        [Builder.Object] private TreeViewColumn col_proj_name;
        [Builder.Object] private CellRenderer cell_proj_text_name;
        [Builder.Object] private TreeViewColumn col_proj_description;
        [Builder.Object] private CellRenderer cell_proj_text_description;
        [Builder.Object] private TreeViewColumn col_proj_status;
        [Builder.Object] private CellRenderer cell_proj_toggle_status;
        [Builder.Object] private ListStore lst_projects2;

        #pragma warning restore 169
        #endregion
        #region CONSTRUCTORS
        public MainWindow()
        {
            Gtk.Application.Init();
            Builder Gui = new Builder();
            Gui.AddFromFile("../../Resources/MainWindow.glade");
			Gui.Autoconnect(this);
            //Gtk.Settings.Default.ThemeName = "Equilux";
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
        public string SettingsFile { get; private set;}
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
            /* INSTANTIATE SETTINGS AND DETECT PLATFORM */
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

            /* LOAD EXISTING PROJECTS */
            LoadProjects();

            /* DEFINE COLUMN RENDER METHODS */
            col_proj_name.SetCellDataFunc(cell_proj_text_name, new TreeCellDataFunc(RenderProjectName));
            col_proj_description.SetCellDataFunc(cell_proj_text_description, new TreeCellDataFunc(RenderProjectDescription));
            col_proj_status.SetCellDataFunc(cell_proj_toggle_status, new TreeCellDataFunc(RenderProjectStatus));

            return false;
        }
        private void RenderProjectName(TreeViewColumn col, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                Project p = (Project)model.GetValue(iter, 0);
                (cell as CellRendererText).Text = p.Title;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageType.Error);
            }
        }
        private void RenderProjectDescription(TreeViewColumn col, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                Project p = (Project)model.GetValue(iter, 0);
                (cell as CellRendererText).Text = p.Description;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageType.Error);
            }
        }
        private void RenderProjectStatus(TreeViewColumn col, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            try
            {
                Project p = (Project)model.GetValue(iter, 0);
                (cell as CellRendererToggle).Active = p.Status;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageType.Error);
            }
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
                                ProjectLister.Projects.Add(p);
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
        private void OnProjNewClicked(object sender, EventArgs e)
        {
            Project p = new Project();
            ProjectLister.AddProject(p);
            lst_projects2.AppendValues(p);
        }
        private void OnProjStatusClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void OnProjDeleteClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
		private void OnChooseProjPathClicked(object sender, EventArgs e)
		{
            Button b = (Button)sender;
            ConsoleMessage.WriteLine(b.ToString() + " Button clicked");
		}
        private void OnProjSaveClicked(object sender, EventArgs e)
        {

        }
        private void OnCellTextNameEdited(object sender, EditedArgs e)
        {
            TreeIter iter;
            lst_projects.GetIter(out iter, new TreePath(e.Path));

        }
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion           
    }
}

