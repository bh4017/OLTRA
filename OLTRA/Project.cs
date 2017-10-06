using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
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
            lst_Listeners = new BindingList<ListenerBase>();
        }
        public Project(string title = "", string description = "", bool status = true)
        {
            Title = title;
            Description = description;
            Status = status;
        }
        public Project(SerializationInfo si, StreamingContext sc)
        {
            Title = si.GetString("Title");
            Description = si.GetString("Description");
            Status = si.GetBoolean("Status");
            lst_Listeners = (BindingList<ListenerBase>)si.GetValue("Listeners", typeof(BindingList<ListenerBase>));
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
		public BindingList<ListenerBase> lst_Listeners { get; set; }
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("Title", Title);
            si.AddValue("Description", Description);
            si.AddValue("Status", Status);
            si.AddValue("Listeners", lst_Listeners);    
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

