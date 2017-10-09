﻿namespace OLTRA
{
	using System;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.IO;
	using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;
    using HelperClassesBJH;

    #region LISTENER BASE CLASS
    [Serializable]
    public abstract class ListenerBase
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        #endregion
        #region CONSTRUCTORS
        public ListenerBase() {}
        public ListenerBase(string title = null, string description = null, bool enabled = true)
        {
            Title = title;
            Description = description;
            Enabled = enabled;
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
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public abstract void Listen();
        public abstract void Edit();
        #region EVENT HANDLERS
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion
    }
    #endregion
    #region FILE LISTENER CLASS
    [Serializable]
    public class FileListener : ListenerBase, ISerializable
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        private string _filePath;
        private int _listenInterval;
        #endregion
        #region CONSTRUCTORS
        public FileListener() {}
        public FileListener(string filepath, int listeninterval = 1000, string title = null, string description = null, bool enabled = true)
            :base(title, description, enabled)
        {
            //tmr = new Timer();
            FilePath = filepath;
            ListenInterval = listeninterval;
        }
        protected FileListener(SerializationInfo si, StreamingContext sc)
        {
            Title = si.GetString("Title");
            Description = si.GetString("Description");
            Enabled = si.GetBoolean("Enabled");
            FilePath = si.GetString("FilePath");
            ListenInterval = si.GetInt32("ListenInterval");
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
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (!Directory.Exists(value))
                    MessageBox.Show("WARNING: The path you have entered does not exist on this system!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _filePath = value;
            }
        }
        public int ListenInterval { get; set; }
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("Title", Title);
            si.AddValue("Description", Description);
            si.AddValue("Enabled", Enabled);
            si.AddValue("FilePath", FilePath);
            si.AddValue("ListenInterval", ListenInterval);
        }
        public void GetInput(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            ConsoleMessage.WriteLine(this.Title + " is listening");
        }
        public override void Listen()
        {
            var autoEvent = new AutoResetEvent(false);
            System.Threading.Timer tmrListener = new System.Threading.Timer(GetInput, autoEvent, 0, Convert.ToInt32(ListenInterval));
        }
        public override void Edit()
        {
            
        }
        #region EVENT HANDLERS
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion
    }             
	#endregion
    #region NETWORK LISTENER CLASS
    [Serializable]
    public class NetListener : ListenerBase, ISerializable
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        //private Timer tmr;
        #endregion
        #region CONSTRUCTORS
        public NetListener() {}
        public NetListener(string ip, int port, string title = "hello", string description = null, bool enabled = true)
            :base(title, description, enabled)
        {
            IP = ip;
            Port = port;
        }
        protected NetListener(SerializationInfo si, StreamingContext sc)
        {
            Title = si.GetString("Title");
            Description = si.GetString("Description");
            Enabled = si.GetBoolean("Enabled");
            IP = si.GetString("IP");
            Port = si.GetInt32("Port");
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
        public string IP { get; protected set; }
        public int Port { get; set; }
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("Title", Title);
            si.AddValue("Description", Description);
            si.AddValue("Enabled", Enabled);
            si.AddValue("IP", IP);
            si.AddValue("Port", Port);
        }
        public override void Listen()
        {
            //while (true)
            //{
            //    ConsoleMessage.WriteLine("Net Listener\n" + Title + "\n" + Description);
            //    Thread.Sleep(500);
            //}
        }
        public override void Edit()
        {
            throw new NotImplementedException();
        }
        #region EVENT HANDLERS
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion
    }             
    #endregion

}

