using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;
using System.Data.SqlClient;

namespace OLTRA
{
    #region LOGGER BASE
    public abstract class Logger
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
        public Logger()
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
        public abstract int Log();
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
    #region SQL_LOGGER
    public class SQL_Logger : Logger, ISerializable
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
        public SQL_Logger() {}
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
        public override int Log()
        {
            return 0;
        }
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
    #endregion
    #region MYSQL_LOGGER
    public class MySQL_Logger : Logger, ISerializable
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
        public MySQL_Logger() {}
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
        public override int Log()
        {
            return 0;
        }
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
    #endregion
}