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
}