using System;
using System.IO;
using Gtk;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using HelperClassesBJH;

namespace OLTRA
{
    [Serializable]
    public class Project:ISerializable
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        #endregion
        #region CONSTRUCTORS
        public Project () 
        {
            Listeners = new ListStore(typeof(ListenerBase));
        }
        public Project(SerializationInfo si, StreamingContext sc)
        {
            Title = si.GetString("Title");
            Description = si.GetString("Description");
            Status = si.GetBoolean("Status");
            ListenersTBS = (List<ListenerBase>)si.GetValue("ListenersTBS", typeof(List<ListenerBase>));
            /* LISTENER DESERIALISATION */
            // Convert the .net List into the ListStore
            Listeners = new ListStore(typeof(ListenerBase));
            foreach (ListenerBase l in ListenersTBS)
            {
                Listeners.AppendValues(l);
            }
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
        public bool Status { get; set; }
		public ListStore Listeners { get; set; }
        public List<ListenerBase> ListenersTBS { get; set; }        // A ListStore is not serialisable so I use a regular .net List as a go-between.
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("Title", Title);
            si.AddValue("Description", Description);
            si.AddValue("Status", Status);
            /* LISTENER SERIALISATION */
            // Convert the Listeners ListStore into a regular .net List for serialisation
            ListenersTBS = new List<ListenerBase>();
            foreach (ListenerBase l in Listeners)
            {
                ListenersTBS.Add(l);
            }
            si.AddValue("ListenersTBS", ListenersTBS);
        }
        #region EVENT HANDLERS
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion
    }
}

