using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace OLTRA
{
    public class Project : ISerializable
    {
        /*
        * TITLE:       [TITLE]
        * DESCRIPTION: [DESCRIPTION]
        * AUTHOR:      [NAME]
        * DATE:        [DATE]
        * VERSION:     [VERSION]
        */

        #region FIELDS
        /* CONSTANT FIELDS */
        /* FIELDS */
        #endregion
        #region CONSTRUCTORS
        public Project() {}
        #endregion
        #region FINALIZERS
        /* FINALIZERS */
        #endregion
        #region DELEGATES
        /* DELEGATES */
        #endregion
        #region EVENTS
        /* EVENTS */
        #endregion
        #region ENUMS
        /* ENUMS */
        #endregion
        #region INTERFACES
        /* INTERFACES */
        #endregion
        #region PROPERTIES
        /* PROPERTIES */
        public BindingList<ExtractionEngine> ExtractionEngines { get; set; }
        public BindingList<Logger> Loggers { get; set; }
        #endregion
        #region INDEXERS
        /* INDEXERS */
        #endregion
        #region METHODS
        /* METHODS */
        public void GetObjectData(SerializationInfo si, StreamingContext sc)
        {

        }
        #endregion
        #region STRUCTS
        /* STRUCTS */
        #endregion
        #region CLASSES
        /* CLASSES */
        #endregion
    }
}

