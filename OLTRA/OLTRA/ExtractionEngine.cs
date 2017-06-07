using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

namespace OLTRA
{
    public class TestResultElement
    {
        public string Title { get; private set; }
        public string TxtResult { get; private set; }
        public double NumResult { get; private set; }
        public double UpperLimit { get; private set; }
        public double LowerLimit { get; private set; }
        /// <summary>
        /// Test Status (PASS/FAIL)
        /// </summary>
        /// <value>PASS: 0, FAIL: 1, INDETERMINATE: 2</value>
        public int Status { get; private set; }
    }

    public abstract class ExtractionEngine
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
        private BindingList<TestResultElement> _lstTestResults = new BindingList<TestResultElement>();
        /* FIELDS */
        #endregion
        #region CONSTRUCTORS
        public ExtractionEngine()
        {
        }
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
        public BindingList<TestResultElement> LstTestResults
        {
            get { return _lstTestResults; }
            set { _lstTestResults = value; }
        }
        #endregion
        #region INDEXERS
        /* INDEXERS */
        #endregion
        #region METHODS
        /* METHODS */
        public abstract int Extract();
        #endregion
        #region STRUCTS
        /* STRUCTS */
        #endregion
        #region CLASSES
        /* CLASSES */

        #endregion
    }

    public class TextFileRegexExtractionEngine : ExtractionEngine, ISerializable
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
        public TextFileRegexExtractionEngine()
        {
        }
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
        public string TextFilePath { get; set; }
        #endregion
        #region INDEXERS
        /* INDEXERS */
        #endregion
        #region METHODS
        /* METHODS */
        public override int Extract()
        {
            return 0;
        }
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
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

