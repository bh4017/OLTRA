using System;
using System.IO;
using Gtk;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace OLTRA
{
	public static class ProjectLister
	{
        #region PROPERTIES
        public static List<Project> Projects = new List<Project>();
        #endregion
        #region METHODS
        public static void AddProject(Project p)
        {
            Projects.Add(p);
            //l.Append(
        }
        #endregion
	}
    public class Project
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        #endregion
        #region CONSTRUCTORS
        public Project () {}
        public Project (string title, string description, bool status)
        {
            this.Title = title;
            this.Description = description;
            this.Status = status;
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
		public List<ListenerBase> Listeners { get; protected set; }
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
       
        #region EVENT HANDLERS
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion
    }
}

