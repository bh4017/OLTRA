namespace OLTRA
{
    using System;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml.Serialization;

    [Serializable]
    public class Settings : ISerializable
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        #endregion
        #region CONSTRUCTORS
        public Settings() {}
        protected Settings(SerializationInfo si, StreamingContext sc)
        {
            ProjectDir = si.GetString("ProjectDir");
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
        public string ProjectDir {get; set;}
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("ProjectDir", ProjectDir);
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

