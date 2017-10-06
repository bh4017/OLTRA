using System;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using HelperClassesBJH;

namespace OLTRA
{
    public partial class Main : Form
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        private BindingList<DebugItem> lst_debug = new BindingList<DebugItem>();
        private BindingList<Project> lst_projects = new BindingList<Project>();
        private BindingList<ListenerBase> lst_lsnr_types = new BindingList<ListenerBase>();
        #endregion
        #region CONSTRUCTORS
        public Main()
        {
            InitializeComponent();
            Startup();
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
        public Settings OLTRAsettings { get; set; }
        public string Home { get; set; }
        public string SettingsFile { get; private set; }
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        private bool Startup()
        {
            ConsoleMessage.MessageOutput += new EventHandler<MessageEventArgs>(MessageOutput);
            ConsoleMessage.WriteLine("Running initial startup routine.");
            ConsoleMessage.WriteLine("Detected platform: " + Environment.OSVersion.Platform.ToString());
            ConsoleMessage.WriteLine("Maximizing window size...");

            /* INSTANTIATE SETTINGS AND DETECT PLATFORM */
            OLTRAsettings = new Settings();
            /* FIND HOME DIRECTORY */
            string path = Environment.GetEnvironmentVariable("HOME", EnvironmentVariableTarget.Process);   // OLTRA will store settings in $HOME so first it needs to be set!
            if (String.IsNullOrEmpty(path))
            {
                ConsoleMessage.WriteLine("Home directory not set!");
                MessageBox.Show("$HOME environment variable not set!\nContact IT administrator!\n\nApplication will now quit","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Application.Exit();
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
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to deserialise settings file\n" + ex.Message + "\nApplication will now quit!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                finally
                {
                    ConsoleMessage.WriteLine("Settings successfully loaded.  Project Directory: " + OLTRAsettings.ProjectDir + "\n");

                }
            }
            else
            {
                // Default Settings
                ConsoleMessage.WriteLine("OLTRA settings not found, creating default", MessageBoxIcon.Warning);
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
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to write default settings! Error:\n" + ex.Message + " Application will now quit", "ERROR", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            /* LOAD EXISTING PROJECTS */
            LoadProjects();

            /* SETUP LIST OF LISTENERS */
            ConsoleMessage.WriteLine("Setting up lst_listeners");
          
            /* SETUP LISTENER COMBOBOX */
            ConsoleMessage.WriteLine("Setting up Listener combobox...");
            // First we have to set up the ListStore because it doesn't work if you do this in Glade.
            ConsoleMessage.WriteLine("Setting combobox model to lst_lsnr_types");
            //cmb_lsnr_type.Model = lst_lsnr_types;
            ConsoleMessage.WriteLine("Programatically get all the types of ListenerBase...");
            // Get all the sub classes of ListenerBase.
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsSubclassOf(typeof(ListenerBase)));
            // Add an instance of each subclass of ListenerBase to a ListStore.
            foreach (Type t in types)
            {
                var o = (Activator.CreateInstance(t));
                lst_lsnr_types.Add((ListenerBase)o);   // The combobox shows the information in parameter 1 so we know what type of listener we're selecting.  It may be possible to do all this with one column instead using a CellRenderer method. 2017-08-20 BJH.
                ConsoleMessage.WriteLine("> Found " + o.ToString());
            }
            //cmb_lsnr_type.Active = 0;
            return false;
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
                                lst_projects.Add(p);
                                ConsoleMessage.WriteLine("Added Project " + p.Title);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to deserialise Projects file " + projects[i] + "Error: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                ConsoleMessage.WriteLine("No projects found!");
            }
        }
        #region EVENT HANDLERS
        private void MessageOutput(object sender, MessageEventArgs e)
        {
            string t = String.Format("{0:yyyy-MM-dd  HH:mm:ss.ff}", e.Dt);
            string colour = "Green";
            string type = "INFO";
            switch (e.Type)
            {
                case MessageBoxIcon.Warning:
                    colour = "Yellow";
                    type = "INFO";
                    break;
                case MessageBoxIcon.Error:
                    colour = "Red";
                    type = "ERROR";
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
            DebugItem d = new DebugItem(t, type, e.Message);
            lst_debug.Add(d);
        }
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        public class DebugItem
        {
            /* PROPERTIES */
            public string TimeStamp {get; set;}
            public string Type {get; set;}
            public string Message {get; set;}

            /* CONSTRUCTOR */
            public DebugItem(string timeStamp, string type, string message)
            {
                TimeStamp = timeStamp;
                Type = type;
                Message = message;
            }
        }
        #endregion 
    }
}
