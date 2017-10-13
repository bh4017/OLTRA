namespace OLTRA
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    [Serializable]
    public abstract class ExtractionEngineBase
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        #endregion
        #region CONSTRUCTORS
        public ExtractionEngineBase() { }
        public ExtractionEngineBase(string title = "New Extraction Engine", string description = "")
        {
            this.Title = title;
            this.Description = description;
        }
        public ExtractionEngineBase(SerializationInfo si, StreamingContext sc)
        {
            Title = si.GetString("Title");
            Description = si.GetString("Description");
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
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("Title", Title);
            si.AddValue("Description", Description);
        }
        public abstract void Extract();
        #region EVENT HANDLERS
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        #endregion
    }
    [Serializable]
    public class ExtractionEngineRegex :ExtractionEngineBase
    {
        #region CONSTANT FIELDS
        #endregion
        #region FIELDS
        #endregion
        #region CONSTRUCTORS
        public ExtractionEngineRegex() 
        {
            lst_Elements = new BindingList<ExtractionElement>();
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
        public BindingList<ExtractionElement> lst_Elements { get; set; }
        #endregion
        #region INDEXERS
        #endregion
        #region METHODS
        public override void Extract()
        {
            
        }
        #region EVENT HANDLERS
        #endregion
        #endregion
        #region STRUCTS
        #endregion
        #region CLASSES
        [Serializable]
        public class ExtractionElement
        {
            public string Pattern { get; set; }
            public int OutputVariable { get; set; }

            public ExtractionElement(string pattern, int outputVariable)
            {
                Pattern = pattern;
                OutputVariable = outputVariable;
            }
        }         
        #endregion
    }
}
