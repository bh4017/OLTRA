using System;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        private SelectedProjectEditor selectedProjectEditor = new SelectedProjectEditor();          // This variable is used to control which Project editor area is selected at any time.        
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
        enum SelectedProjectEditor
        {
            None,
            Projects,
            Listeners,
            ExtractionEngines,
            Loggers,
            Alerts
        }
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
            ConsoleMessage.MessageOutput += new EventHandler<MessageEventArgs>(On_MessageOutput);
            ConsoleMessage.WriteLine("Running initial startup routine.");
            ConsoleMessage.WriteLine("Detected platform: " + Environment.OSVersion.Platform.ToString());

            /* INSTANTIATE SETTINGS */
            OLTRAsettings = new Settings();
            /* FIND HOME DIRECTORY */
            string path = Environment.GetEnvironmentVariable("HOME", EnvironmentVariableTarget.User);   // OLTRA will store settings in $HOME so first it needs to be set!
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
            dgv_Projects.DataSource = lst_projects;
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
        private void SelectProjectEditor(SelectedProjectEditor spe, GroupBox gbx)
        {
            /* CLEAR SELECTION OF ALL EDITORS */
            selectedProjectEditor = SelectedProjectEditor.None;
            foreach (GroupBox g in tcl_Engineering.TabPages[0].Controls)
            {
                g.BackColor = Color.Transparent;
            }
            /* SELECT CHOSEN EDITOR */
            gbx.BackColor = Color.LimeGreen;
            selectedProjectEditor = spe;
            /* ENABLE/DISABLE EDITOR BUTTONS */
            if (selectedProjectEditor != SelectedProjectEditor.None)
            {
                btn_Projects_Add.Enabled = btn_Projects_Delete.Enabled = true;
            }
            else
            {
                btn_Projects_Add.Enabled = btn_Projects_Delete.Enabled = false;
            }

            ConsoleMessage.WriteLine("Selected Projects Editor: " + gbx.Text);
        }
        #region EVENT HANDLERS
        private void On_MessageOutput(object sender, MessageEventArgs e)
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
        private void On_Editor_Listener_Selected(object sender, EventArgs e)
        {
            GroupBox gbx = (GroupBox)sender;
            SelectProjectEditor(SelectedProjectEditor.Listeners, gbx);
            cmb_Projects_Type.DataSource = lst_lsnr_types;
        }
        private void On_Editor_Projects_Selected(object sender, EventArgs e)
        {
            GroupBox gbx = (GroupBox)sender;
            SelectProjectEditor(SelectedProjectEditor.Projects, gbx);
            cmb_Projects_Type.DataSource = null;
        }
        private void On_Editor_Loggers_Selected(object sender, EventArgs e)
        {
            GroupBox gbx = (GroupBox)sender;
            SelectProjectEditor(SelectedProjectEditor.Loggers, gbx);
            cmb_Projects_Type.DataSource = null;
        }
        private void On_Editor_ExtractionEngines_Selected(object sender, EventArgs e)
        {
            GroupBox gbx = (GroupBox)sender;
            SelectProjectEditor(SelectedProjectEditor.ExtractionEngines, gbx);
            cmb_Projects_Type.DataSource = null;
        }
        private void On_btn_Projects_Add_Click(object sender, EventArgs e)
        {
            switch (selectedProjectEditor)
            {
                case SelectedProjectEditor.Projects:
                    {
                        Project p = new Project();
                        p.Title = "New Project " + (lst_projects.Count + 1).ToString();
                        p.Description = "Blank Project";
                        p.Status = false;
                        lst_projects.Add(p);
                        dgv_Projects.DataSource = lst_projects;
                        break;
                    }
                case SelectedProjectEditor.Listeners:
                    {
                        if (dgv_Projects.SelectedCells.Count > 0)
                        {
                            ConsoleMessage.WriteLine("Adding listener to " + lst_projects[dgv_Projects.SelectedCells[0].RowIndex].Title);
                            var l = (ListenerBase)cmb_Projects_Type.SelectedValue;  // Get the selected value from the Add combobox.
                            var type = l.GetType();                                 // Determine the type of the object
                            l = (ListenerBase)Activator.CreateInstance(type);       // Create a new instance of the type
                            l.Title = "New Listener " + (lst_projects[dgv_Projects.SelectedCells[0].RowIndex].lst_Listeners.Count + 1).ToString();
                            l.Description = l.ToString();
                            lst_projects[dgv_Projects.SelectedCells[0].RowIndex].lst_Listeners.Add(l);
                            dgv_Listeners.DataSource = lst_projects[dgv_Projects.SelectedCells[0].RowIndex].lst_Listeners;
                        }
                        else
                        {
                            MessageBox.Show("Select a project to add the Listener to!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    }
                case SelectedProjectEditor.Loggers:
                    {

                        break;
                    }
                case SelectedProjectEditor.ExtractionEngines:
                    {

                        break;
                    }
                case SelectedProjectEditor.Alerts:
                    {

                        break;
                    }
                default:
                    {

                        break;

                    }
            }
            btn_Projects_Save.Enabled = true;
        }                    
        private void On_btn_Projects_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dgv_Projects.SelectedRows)
            {
                lst_projects.Remove((Project)item.DataBoundItem);
            }
            btn_Projects_Save.Enabled = true;
        }
        private void On_btn_Projects_Save_Click(object sender, EventArgs e)
        {
            foreach (Project p in lst_projects)
            {
                IFormatter bf = new BinaryFormatter();
                try
                {
                    using (FileStream fs = File.Create(OLTRAsettings.ProjectDir + "/" + p.Title + ".olp"))
                        bf.Serialize(fs, p);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            btn_Projects_Save.Enabled = false;
            //foreach (DataGridViewRow item in dgv_Projects.SelectedRows)
            //{
            //    Project p = (Project)item.DataBoundItem;
            //    IFormatter bf = new BinaryFormatter();
            //    try
            //    {
            //        using (FileStream fs = File.Create(OLTRAsettings.ProjectDir + "/" + p.Title + ".olp"))
            //            bf.Serialize(fs, p);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
        }
        private void On_Projects_Cell_Value_Changed(object sender, DataGridViewCellEventArgs e)
        {
            btn_Projects_Save.Enabled = true;
        }
        private void On_Projects_Cell_Mouse_Clicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgv_Listeners.DataSource = lst_projects[e.RowIndex].lst_Listeners;
            // TODO
            // Do the same for the other datagridviews
        }
        private void btn_Global_1_Click(object sender, EventArgs e)
        {
            foreach (Project p in lst_projects)
            {
                foreach (ListenerBase l in p.lst_Listeners)
                {
                    l.Listen();
                }
            }
        }
        private void On_Listeners_Cell_Enter(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private void btn_Edit_Click(object sender, EventArgs e)
        {
            switch (selectedProjectEditor)
            {
                case SelectedProjectEditor.Projects:
                    {
                       break;
                    }
                case SelectedProjectEditor.Listeners:
                    {
                        dgv_Listeners.DataSource = null;
                        dgv_Listeners.Columns.Clear();
                        dgv_Listeners.DataSource = lst_projects[dgv_Projects.CurrentCell.RowIndex].lst_Listeners;
                        
                        Project p = lst_projects[dgv_Projects.CurrentCell.RowIndex];                // Find which project is selected and create a reference to the databound project inside lst_projects.
                        var type = p.lst_Listeners[dgv_Listeners.CurrentCell.RowIndex].GetType();   // Get the runtime type of the selected listener object
                        var classProperties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);  // Get the instance properties only
                        var baseProperties = type.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance);                      // Get the base class properties only

                        /* POPULATE DATAGRID WITH COLUMNS */
                        foreach (var prop in classProperties)
                        {
                            DataGridViewCell cell = new DataGridViewTextBoxCell();
                            DataGridViewColumn col = new DataGridViewColumn(cellTemplate: cell);

                            col.HeaderText = prop.Name;
                            dgv_Listeners.Columns.Insert(dgv_Listeners.Columns.Count, col);
                        }

                        /* RESTRICT EDITING TO LISTENERS OF SAME TYPE */
                        /* POPULATE CELLS WITH VALUES FROM LISTENER */
                        foreach (DataGridViewRow row in dgv_Listeners.Rows)
                        {
                            // Restrict editing
                            if(row.DataBoundItem.GetType() != type)
                            {
                                row.ReadOnly = true;
                                row.DefaultCellStyle.ForeColor = Color.LightGray;
                                //row.Visible = false;
                            }
                            foreach(DataGridViewCell cell in row.Cells)
                            {
                                // Populate cells
                                foreach (var prop in classProperties)
                                {
                                    var listener = cell.OwningRow.DataBoundItem;
                                    if (cell.OwningColumn.HeaderText == prop.Name)
                                    {
                                        try
                                        {
                                            cell.Value = prop.GetValue(cell.OwningRow.DataBoundItem, null);
                                        }
                                        catch { };
                                    }
                                }
                            }
                        }
                        break;

                        /* CUSTOM LISTENER EDITING */
                        // If your listener requires more complex editing than can be serviced in the datagridview,
                        // then you can write your own editing handler and these instructions will call it.
                        // If you do not require this functionality, leave the edit() method blank.
                        ListenerBase l = (ListenerBase)dgv_Listeners.CurrentRow.DataBoundItem;
                        l.Edit();
                    }
                case SelectedProjectEditor.Loggers:
                    {
                        break;
                    }
                case SelectedProjectEditor.ExtractionEngines:
                    {

                        break;
                    }
                case SelectedProjectEditor.Alerts:
                    {

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        private void On_Listeners_Cell_End_Edit(object sender, DataGridViewCellEventArgs e)
        {
            btn_Projects_Save.Enabled = true;
            DataGridView dgv = (DataGridView)sender;
            /* GET PROPERTIES OF CURRENT LISTENER */
            var listener = dgv.CurrentRow.DataBoundItem;
            var type = listener.GetType();
            var classProperties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);  // Get the instance properties only

            foreach (var prop in classProperties)
            {
                if (prop.Name == dgv.CurrentCell.OwningColumn.HeaderText)
                    prop.SetValue(listener, Convert.ChangeType(dgv.CurrentCell.Value, prop.PropertyType), null);
            }
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
