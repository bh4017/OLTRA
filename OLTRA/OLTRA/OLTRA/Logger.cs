using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;
using System.Data.SqlClient;

namespace OLTRA
{
    #region LOGGER BASE
    abstract class LoggerBase
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
        public LoggerBase()
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
        public SqlConnection SQL_Connection { get; set; }
        #endregion
        #region INDEXERS
        /* INDEXERS */
        #endregion
        #region METHODS
        /* METHODS */
        public abstract int Run();
        public int LogToSQL()
        {
            return 0;
        }
        public int LogToMySQL()
        {
            throw new NotImplementedException();
        }
        public int LogToSQLite()
        {
            throw new NotImplementedException();
        }
        public int LogToDelimitedTextFile()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region STRUCTS
        /* STRUCTS */
        #endregion
        #region CLASSES
        /* CLASSES */
        #endregion
    }
    #endregion
    #region TextFileRegexLogger
    class TextFileRegexLogger : LoggerBase, ISerializable
    {
        /*
        * TITLE:       TextFileRegexLogger
        * DESCRIPTION: A class which can process a text file and extract parameters using regular expressions
        * AUTHOR:      Brian J Hoskins
        * DATE:        19th May 2017
        * VERSION:     0.1D
        */

        #region FIELDS
        /* CONSTANT FIELDS */
        /* FIELDS */
        private Timer FileScanTimer;    // The event handler for this timer will scan a particular path for the presence of result files
        #endregion
        #region CONSTRUCTORS
        public TextFileRegexLogger()
        { 
            FileScanTimer = new Timer();
        }
        protected TextFileRegexLogger(SerializationInfo si, StreamingContext sc)
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
        public double FileScanInterval
        {
            get
            { 
                return FileScanTimer.Interval;
            }
            set
            {
                FileScanInterval = value;
                FileScanTimer.Interval = value;
            }
        }
        public string FilePath { get; set; }
        public string ResultRegexPattern { get; private set; }
        public string UpperLimitRegexPattern { get; private set; }
        public string LowerLimitRegexPattern { get; private set; }
        public string StatusRegexPattern { get; private set; }
        #endregion
        #region INDEXERS
        /* INDEXERS */
        #endregion
        #region METHODS
        /* METHODS */
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {

        }
        public override int Run()
        {
            //SqliteConnection
            return 0;
        }
        #endregion
        #region STRUCTS
        /* STRUCTS */
        #endregion
        #region CLASSES
        /* CLASSES */
        public class TestResultElement
        {
            public int LineNumber { get; private set; }
            public string Title { get; private set; }
            public string Result { get; private set; }
            public string UpperLimit { get; private set; }
            public string LowerLimit { get; private set; }
            public string Status { get; private set; }
        }
        #endregion
    }
    #endregion
    #region TRW TYPE C LOGGER
    class TRW_TYPE_C : LoggerBase
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
        public TRW_TYPE_C()
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

        #endregion
        #region INDEXERS
        /* INDEXERS */
        #endregion
        #region METHODS
        /* METHODS */
        public override int Run()
        {
            //SqliteConnection
            return 0;
        }
        #endregion
        #region STRUCTS
        /* STRUCTS */
        #endregion
        #region CLASSES
        /* CLASSES */
        #endregion
    }
    #endregion
    #region TRW TYPE A LOGGER
    class TRW_TYPE_A : LoggerBase
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
        public TRW_TYPE_A()
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

        #endregion
        #region INDEXERS
        /* INDEXERS */
        #endregion
        #region METHODS
        /* METHODS */
        public override int Run()
        {
            //SqliteConnection
            return 0;
        }
        #endregion
        #region STRUCTS
        /* STRUCTS */
        #endregion
        #region CLASSES
        /* CLASSES */
        #endregion
    }
    #endregion
}