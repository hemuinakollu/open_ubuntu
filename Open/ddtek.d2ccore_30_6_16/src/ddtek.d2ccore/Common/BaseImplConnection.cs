using System;
//using ddtek.d2ccore.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDInt.Utility;

namespace ddtek.d2ccore.Common
{
    public abstract class BaseImplConnection
    {

        /**
         * Fooprint information field
         */
        static private string footprint = "$Revision: #5 $";

        /*
         * Empty row insert syntax constants
         */


        // Owning connection object
       // public BaseConnection connection;

        /**
         * Cannot insert an empty row.
         */
        static readonly public int ERIS_NOT_SUPPORTED = 0;


        /**
         * Can insert an empty row using SQL 92 syntax
         * i.e. insert into TABLE default values
         */
        static readonly public int ERIS_SQL_92 = 1;


        /**
         * Can insert an empty row with an empty values list
         * i.e. insert into TABLE values ()
         */
        static readonly public int ERIS_EMPTY_VALUES_LIST = 2;


        /**
         * Can insert an empty row with an empty values list
         * that contains the default keywork for each column
         * in the target table.
         * e.g. for a table with 2 columns:
         * insert into TABLE values (default, default)
         */
        static readonly public int ERIS_DEFAULT_VALUES_LIST = 3;

        /**
         * Can insert an empty row with an empty values list
         * that contains the NULL keywork for each column
         * in the target table.
         * e.g. for a table with 2 columns:
         * insert into TABLE values (NULL, NULL)
         */
        static readonly public int ERIS_NULL_VALUES_LIST = 4;

        /**
         * Returned from testConnection
         */

        /**
         * Connection to server is still valid.
         */
        static readonly public int CONNECTION_ENTACT = 0;

        /**
         * Connection to the server has been lost.
         */
        static readonly public int CONNECTION_LOST = 1;

        /**
         * Unable to determine connection status.
         */
        static readonly public int CONNECTION_UNKNOWN = 2;

        /**
         * Connection status
         */
        public int connectionStatus = CONNECTION_UNKNOWN;

        /**
         * Connection properties.
         */
        //public BaseConnectionProperties connectProps;


        /**
         * Whether the connection has been put in readonly mode by the app.
         */
        protected bool readOnlyMode;


        /**
         * Warning list.
         */
        //public BaseWarnings warnings;


        /**
         * Exception generator;
         */
        //public BaseExceptions exceptions;


        /**
         * Debug facility.
         */
        public UtilDebug debug;


        /**
         * The character which to be interpreted (by the framework)
         * as the quoting character when processing SQL
         * statements. The native driver should change this if its
         * quoting character is something other than the double quote(").
         */
        public char quotingChar = '"';


        /**
         * True if the "into" in an insert statement is optional
         */
        public bool intoIsOptional = false;

        /**
         * True if the database supports DML with results 
         * (for implementing the getGeneratedKeys() functions like
         * Statement.executeUpdate(String sql, String[] columnNames) )
         */
        public bool supportsDMLWithResults = false;

        /**
         * True if the database supports native batch sql statements 
         * (used to determine if we need to check for batches in sql
         * when failover mode is SELECT) )
         */
        public bool supportsNativeBatchSQLForFailover = true;

        /**
         * True if the database server allows UPDATE and DELETE statements
         * to be issued against the database.  False if these statements are
         * not allowed.
         */
        public bool supportsDBUpdatesAndDeletes = true;

        /**
         * True if "A" should be sorted above "_"
         * 
         * Basically this determines if we are sorting strictly by the ASCII value (true)
         * or if all non-alphaNumeric characters should preceed alphanumeric chars (false).
         */
        public bool useASCIISort = false;


        /**
         * XAConnection property
         */
        protected bool isXAConnection;

        /**
         * Set by the native layer to inform the Framework how may different
         * BaseImplResultSet derivatives that it exposes.  This is used 
         * in the caching logic of BaseImplResultSets.
         * 
         * If a native layer only has one type then it can ignore this field.
         * Otherwise this field should be set during the contruction of the
         * native implementation.
         */
        protected int numImplResultSetTypes = 1;

        /**
         * BaseImplResultSet cache size.
         */
        private readonly int IMPL_RESULT_SET_CACHE_SIZE = 32;

        /**
         * BaseImplResultSet cache.
         */
        //private BaseImplResultSet[][] implResultSetCache;
        private int[] implResultSetCacheNextIndices;


        /*
         * This should be the message associated with the SQLException 
         * that is thrown from the native layer when a timeout is experienced
         * as part of the socket based query timeout implemenation.
         */
        static readonly public string INTERNAL_SOCKET_TIMEOUT = "SOCKETTIMEOUT";

        /*
         * Set by the Framework, and used by the native to to determine
         * if a query timeout related cancel reponse is on the wire.  If
         * it is 'true' then the native layer should invoke 
         * socketActivityPostCancel before reading the from the wire (or 
         * perhaps otherwise accessing the socket.
         */
        public bool cancelPending;

        /*
         * BaseImplStatement associated with the socket based query timeout,
         */
        //private BaseImplStatement cancelPendingStatement;

        /*
         * Timeout that the following up cancel cleanup / response reading
         * should use.
         */
        private int cancelPendingTimeout;

        /*
         * Normally the "Impl" side of the Framework's architecture
         * doesn't have references back the abstract side of the bridge.
         * However, this rule is violated if there's a practical need.
         * In this case the BaseConnection is needed so that implcit  
         * closing can occur in the case of a communications failure 
         * associated with socket a query time out, cancel response cleanup.
         * 
         * Of course I could have gotten around this issue other ways.
         */
        //private BaseConnection cancelPendingConnection;

        /*
         * This field holds the driver/database's default holdability for ResultSet objects. 
         * 
         * This field is initialized to ResultSet.HOLD_CURSORS_OVER_COMMIT, since that value is
         * appropriate for most drivers. But, some drivers are able to support a holdability of
         * CLOSE_CURSORS_AT_COMMIT (by connect option or otherwise). Those drivers should change the 
         * value of this field by calling the "setDefaultDriverResultSetHoldability" method from their
         * ImplConnection.open method.
         * 
         * The value of this field is returned by DatabaseMetaData.getResultSetHoldability(). 
         */
        //private int defaultDriverResultSetHoldability = ResultSet.HOLD_CURSORS_OVER_COMMIT;

        /*
         *  This field initially contains the value of the "defaultDriverResultSetHoldability" field (above).
         *  During the connection establishment, a native driver call to "setDefaultDriverResultSetHoldability" 
         *  will change the value of this field as well as "defaultDriverResultSetHoldability".
         *  
         *  The value of this field can be changed by an application call to Connection.setHoldability 
         *  (assuming the native driver supports different holdabilities).
         *  
         *  This field is used to propagate the connection-level holdability down to statements 
         *  and result sets created on the connection.
         */
        //private int currentConnectionResultSetHoldability = ResultSet.HOLD_CURSORS_OVER_COMMIT;

        /**
         * The following fields are used to contain instances of
         * BaseData derivatives which native drivers can utlize to
         * return data from ImplResultSet.getData calls.
         */
        //private BaseData m_reusableBaseDataASCIIInputStream = null;
        //private BaseData m_reusableBaseDataBigDecimal = null;
        //private BaseData m_reusableBaseDataBinaryInputStream = null;
        //private BaseData m_reusableBaseDataBlob = null;
        //private BaseData m_reusableBaseDataBoolean = null;
        //private BaseData m_reusableBaseDataByte = null;
        //private BaseData m_reusableBaseDataByteArray = null;
        //private BaseData m_reusableBaseDataCharacterStreamReader = null;
        //private BaseData m_reusableBaseDataClob = null;
        //private BaseData m_reusableBaseDataDate = null;
        //private BaseData m_reusableBaseDataDB2Date = null;
        //private BaseData m_reusableBaseDataDB2Time = null;
        //private BaseData m_reusableBaseDataDB2Timestamp = null;
        //private BaseData m_reusableBaseDataDB2TimestampWithTimeZone = null;
        //private BaseData m_reusableBaseDataDouble = null;
        //private BaseData m_reusableBaseDataFloat = null;
        //private BaseData m_reusableBaseDataInteger = null;
        //private BaseData m_reusableBaseDataLong = null;
        //private BaseData m_reusableBaseDataShort = null;
        //private BaseData m_reusableBaseDataSmallDecimal = null;
        //private BaseData m_reusableBaseDataString = null;
        //private BaseData m_reusableBaseDataTime = null;
        //private BaseData m_reusableBaseDataTimestamp = null;
        //private BaseData m_reusableBaseDataUCS2InputStream = null;
        //private BaseData m_reusableBaseDataUTF8InputStream = null;

        /**
         * Sets the connection properties field. <p>
         *
         * It is gauranteed that this method will be called before
         * 'open' is called.
         *
         * @param connectProps - Properties that configure the connection.
         * @param warnings - Warning list.
         * @param exceptions - Exception generator.
         *
         * @exception (none)
         */
        //final protected void setup(
        //    BaseConnection connection,
        //    //BaseConnectionProperties connectProps,
        //    //BaseWarnings warnings,
        //    //BaseExceptions exceptions,
        //    UtilDebug debug)
        //{

        //    this.connection = connection;
        //    //this.connectProps = connectProps;
        //    //this.warnings = warnings;
        //    //this.exceptions = exceptions;
        //    this.debug = debug;
        //    readOnlyMode = false;
        //    connectionStatus = CONNECTION_UNKNOWN;

        //    //implResultSetCache = new BaseImplResultSet[numImplResultSetTypes][];
        //    implResultSetCacheNextIndices = new int[numImplResultSetTypes];

        //    //for (int i = 0; i < numImplResultSetTypes; i++)
        //    //{

        //    //    implResultSetCache[i] =
        //    //        new BaseImplResultSet[IMPL_RESULT_SET_CACHE_SIZE];
        //    //}
        //}


        /**
         * Puts this connection in read-only mode as a hint to enable database optimizations. <p>
         *
         * @param readOnly - mode
         * @ 
         */
        public void setReadOnly(
                bool readOnly)
        {
            readOnlyMode = readOnly;
        }


        /**
         * Adds a warning to the warning list.
         *
         * @param reason - a description of the warning
         * @param SQLstate - an XOPEN code identifying the warning
         * @param vendorCode - a database vendor specific warning code
         *
         * @return - first SQLWarning
         *
         * @exception - (none)
         */
        //public void addWarning(
        //    int reason,
        //    string SQLstate,
        //    int vendorCode)
        //{

        //    warnings.add(reason, SQLstate, vendorCode);
        //}

        /**
         * Returns a native driver specified derivative of BaseImplDatabaseMetaData,
         * based on the specified driver name.
         *
         * @param String rootName
         */
        //protected abstract BaseImplDatabaseMetaData createImplDatabaseMetaData();


        /**
         * Fill given vector with driver specific property info.<p>
         *
         * <b>Driver Writer Notes:</b><br>
         * - You must call the this implemenation as part of your implementation.
         *
         * @param driverPropInfos - Property info collection.
         *
         * @return (none)
         *
         * @exception (none)
         */
        //protected abstract void getImplPropertyInfo(BaseDriverPropertyInfos driverPropInfos);

        /**
         * Determines if the native implementation of batch processing is
         * compliant with the JDBC spec.
         *
         * Note:
         * This property is actually a driver level property but it can't be
         * associated with the driver derivative because the static initializer
         * will register with driver manager in the DataSource context which is
         * undesireable.)
         *
         * @returns true if compliant, false if not
         */
        public bool getBatchIsJDBCCompliant()
        {

            return true;
        }


        /**
         * Factory for DBMS specific BaseEscapeTranslator. <p>
         *
         * @return Driver specific BaseEscapeTranslator,
         *  null if backend natively handles escapes.
         *
         * @exception SQLException
         */
        //public BaseEscapeTranslator createEscapeTranslator()
        //{

        //    return null;
        //}


        /**
         * This method gives the native driver a chance to inspect
         * and (conditionally) modify the connect properties which were
         * explicitly set by the application. The BaseConnectionProperties
         * object passed to this method DOES NOT CONTAIN default property
         * values, unless the app explicitly specified the default value
         * for a connect property.
         *
         * @param appProperties - User-specified collection of connect 
         * properties.
         *
         * @return (none)
         *
         * @exception (none)
         */
        //public void modifyUserSpecifiedConnectProperties(
        //    BaseConnectionProperties appProperties, BaseConnection connection, BaseExceptions exceptions)

        //{
        //}

        /**
         * Opens a connection to the DBMS. <p>
         *
         * <b>Driver Writer Notes:</b>
         * - Must be overriden.
         *
         * - This method will only be called once;
         * i.e. connection instances are not reusable.
         *
         * @exception SQLException
         */
        public abstract void open();


        //public void mergeAlternateConnectProps(
        //    BaseConnectionProperties alternate,
        //    BaseConnectionProperties to)
        //{


        //    Enumeration names = alternate.propertyNames();

        //    while (names.hasMoreElements()) {

        //        string name = (string)names.nextElement();
        //        if (name.CompareTo("USER") == 0)
        //        {
        //            char[] user = alternate.getUser();
        //            if (user != null)
        //            {
        //                to.setUser(user);
        //                // Erase the user value from memory for security
        //                for (int i = 0; i < user.Length; i++) user[i] = 0;
        //            }
        //        }
        //        else if (name.CompareTo("PASSWORD") == 0)
        //        {
        //            char[] pwd = alternate.getPassword();
        //            if (pwd != null)
        //            {
        //                to.setPassword(pwd);
        //                // Erase the password value from memory for security
        //                for (int i = 0; i < pwd.Length; i++) pwd[i] = 0;
        //            }
        //        }
        //        else if (name.CompareTo("NEWPASSWORD") == 0)
        //        {
        //            char[] newpwd = alternate.getNewPassword();
        //            if (newpwd != null)
        //            {
        //                to.setNewPassword(newpwd);
        //                // Erase the password value from memory for security
        //                for (int i = 0; i < newpwd.Length; i++) newpwd[i] = 0;
        //            }
        //        }
        //        else
        //        {
        //            string value = alternate.get(name);
        //            to.put(name, value);
        //        }
        //    }
        //}


        /**
         * Closes a connection to the DBMS. <p>
         *
         * <b>Driver Writer Notes:</b>
         * - Must be overriden.
         *
         * - This method will only be called once;
         * i.e. connection instances can not be re-opened.
         *
         */
        public abstract void close();


        /**
         * Resets the connection to its orginal state.
         * Resetting catalog and txn isolation is handled by the Framework.
         */
        public void reset()
        {


        }


        /**
         * Transitions driver from auto to manual commit mode.
         *
         * Note:  Driver is expected to initially be in auto commit mode.
         *
         * @exception SQLException
         */
        protected abstract void startManualTransactionMode()
            ;


        /**
         * Rolls back the current transaction.
         *
         * Note:  This method will only be called if the driver is in manual commit mode.
         *
         * @exception SQLException
         */
        protected abstract void rollbackTransaction()
             ;


        /**
         * Commits the current transaction.
         *
         * Note:  This method will only be called if the driver is in manual commit mode.
         *
         * @exception SQLException
         */
        protected abstract void commitTransaction()
            ;


        /**
         * Sets a savepoint within the current transaction
         */
        protected void setSavepoint(
            string savepoint)

        {

        }


        /**
         * Releases a savepoint
         */
        protected void releaseSavepoint(
            string savepoint)

        {

        }


        /**
         * Called just prior to BaseImpConnection.setSavepoint if
         * an unrelease savepoint of the same name exists.
         * A native layer may or may not need to explicitly
         * release the existing savepoint at this point.
         */
        protected void implicitReleaseSavepoint(
            string savepoint)

        {

        }


        /**
         * Rolls back the current transaction to the savepoint
         *
         * Note:  This method will only be called if the driver is in manual commit mode.
         *
         * @exception SQLException
         */
        protected void rollbackTransaction(
            string savepoint)

        {

        }


        /**
         * Sets the current catalog.
         *
         * @param newCatalog - new catalog to switch to
         * @exception SQLException
         */
        protected void setCatalog(
            string newCatalog)

        {

        }


        /**
         * Returns the current catalog.
         *
         * @param newCatalog - new catalog to switch to
         * @exception SQLException
         */
        protected string getCatalog()

        {

            return null;
        }


        /**
         * Gets this Connection's current transaction isolation level. <p>
         *
         * @returns int - Isolation level.
         *
         * @exception SQLException
         */
        //protected int getTransactionIsolation()

        //{

        //    return Connection.TRANSACTION_NONE;
        //}


        /**
         * Clears all warnings reported for this Connection object. <p>
         *
         * @param level - isolation level to set
         * @exception SQLException
         */
        protected void setTransactionIsolation(int level)

        {

        }


        /**
         * Transitions driver from manual to auto commit mode.
         *
         * @exception SQLException
         */
        protected abstract void stopManualTransactionMode()
            ;



        /**
         * Factory for DBMS specific BaseImplStatement. <p>
         *
         * This method must be overridden to return a driver
         * specific BaseImplStatement
         *
         * It is not mandatory that the returned BaseImplStatement
         * support the parameter values.  The are just setup hints
         * as to what needs to ultimately be exposed to the application.
         * The Framework will be able to emulate result set types and
         * concurrency values that native BaseImplStatements don't support.
         *
         * @param resultSetType - result set type
         *
         * @param resultSetConcurrency - result set concurrency
         *
         * @return Driver specific BaseImplStatement
         *
         * @exception SQLException
         */
        //protected abstract BaseImplStatement createImplStatement(
        //   int resultSetType,
        //   int resultSetConcurrency)
        //    ;


        /**
         * Factory for DBMS specific BaseImplStatement. <p>
         *
         * This method must be overridden to return a driver
         * specific BaseImplStatement
         *
         * It is not mandatory that the returned BaseImplStatement
         * support the parameter values.  The are just setup hints
         * as to what needs to ultimately be exposed to the application.
         * The Framework will be able to emulate result set types and
         * concurrency values that native BaseImplStatements don't support.
         *
         * @param resultSetType - result set type
         *
         * @param resultSetConcurrency - result set concurrency
         * 
         * @param resultSetHoldability - result set holdability
         * 
         * @return Driver specific BaseImplStatement
         *
         * @exception SQLException
         * 
         * This method simply stores the result set holdability argument 
         * in the created BaseImplStatememt. Drivers which need to do something
         * with the holdability argument at ImplStatement creation time should
         * override this method.
         */
        //protected BaseImplStatement createImplStatement(
        //     int resultSetType,
        //     int resultSetConcurrency,
        //     int resultSetHoldability)

        //{

        //    BaseImplStatement implStatementForReturn = createImplStatement(resultSetType, resultSetConcurrency);

        //    implStatementForReturn.setResultSetHoldability(resultSetHoldability);

        //    return implStatementForReturn;
        //}

        /**
         * Factory for DBMS specific BaseResultSetMeta.
         * 
         * Allows for native layer specific classes to perform 
         * defer expensive processing to determine resultset metadata
         * until the application requests it.
         */
        //protected BaseResultSetMetaData createResultSetMetaData(
        //    object associate,
        //   BaseExceptions exceptions)
        //{

        //    return BaseClassUtility.classCreator.createResultSetMetaData(associate, exceptions);
        //}




        /**
         * This method maps a Java type (represented by a value defined
         * in the BaseData class) to a JDBC type (as defined in
         * java.sql.Types). The mappings implemented by this method are
         * defined in the Appendix of the JDBC specification.
         *
         * @param sqlType The Java type for which the associated JDBC type is
         * desired.
         */
        //static public int mapJavaTypeToSQLType(
        //    int javaType)
        //{

        //    switch (javaType)
        //    {

        //        case BaseData.STRING:
        //            return Type.CHAR;

        //        case BaseData.BIGDECIMAL:
        //            return Type.NUMERIC;

        //        case BaseData.BOOLEAN:
        //            return Type.BIT;

        //        case BaseData.BYTE:
        //            return Type.TINYINT;

        //        case BaseData.SHORT:
        //            return Type.SMALLINT;

        //        case BaseData.INTEGER:
        //            return Type.INTEGER;

        //        case BaseData.LONG:
        //            return Type.BIGINT;

        //        case BaseData.FLOAT:
        //            return Type.REAL;

        //        case BaseData.DOUBLE:
        //            return Type.DOUBLE;

        //        case BaseData.BYTE_ARRAY:
        //            return Type.BINARY;

        //        case BaseData.DATE:
        //            return Type.DATE;

        //        case BaseData.TIME:
        //            return Type.TIME;

        //        case BaseData.TIMESTAMP:
        //            return Type.TIMESTAMP;

        //        case BaseData.BLOB:
        //            return Type.BLOB;

        //        case BaseData.CLOB:
        //            return Type.CLOB;

        //        case BaseData.ARRAY:
        //            return Type.ARRAY;

        //        case BaseData.STRUCT:
        //            return Type.STRUCT;

        //        case BaseData.BINARYINPUTSTREAM:
        //            return Type.LONGVARBINARY;

        //        case BaseData.ASCIIINPUTSTREAM:
        //        case BaseData.UTF8INPUTSTREAM:
        //        case BaseData.UCS2INPUTSTREAM:
        //        case BaseData.CHARACTERSTREAMREADER:
        //            return Type.LONGVARCHAR;
        //    }

        //    return Type.NULL;
        //}


        /**
         * Determines if "SELECT *, column FROM TABLE_SPEC" is valid.
         *
         * It is assumed that most databases do, so the default is 'true'.
         */
        public bool supportsSelectStarCommaColumn()
        {

            return true;
        }

        /**
         * Indicates whether the driver supports the SQL
         * type ARRAY.
         */
        public bool supportsArrayType()
        {

            return false;
        }

        /**
         * Indicates whether the driver supports the SQL
         * type STRUCT.
         */
        public bool supportsStructType()
        {

            return false;
        }

        /**
         * Indicates whether the driver supports a native bulk
         * load operation.
         */
        public bool supportsNativeBulkLoad()
        {

            return false;
        }

        //public BaseImplDDBulkLoad createImplBulkLoad()
        //{
        //    // Eventually, this will need to create an object
        //    // which emulates a bulk load via param batches.


        //    return new BaseImplDDBulkLoad(this.connection);
        //}

        /**
         * Informs the Framework about which type of 'insert empty row' strategy
         * should be used.
         */
        public int getEmptyRowInsertSyntax()
        {

            return ERIS_NOT_SUPPORTED;
        }


        /**
         * Used to determine if the Framework should guard against, throw exceptions
         * in the event of, the application creating more than max statement per
         * connection as reported by BaseImplDatabaseMetaData derivatives.
         */
        public bool enableStatementPerConnectionGuarding()
        {

            return true;
        }


        /**
         * Returns true if native layer supports cancel.
         */
        public bool supportsCancel()
        {

            return false;
        }


        /**
         * Returns true if native layer supports query timeout.
         */
        public bool supportsQueryTimeout()
        {

            return false;
        }

        /**
         * Returns true if native layer supports network timeout.
         */
        public bool supportsNetworkTimeout()
        {

            return false;
        }


        /**
         * Returns true if native layer supports query timeout.
         */
        //public Socket getQueryTimeoutSocket()
        //{

        //    return null;
        //}


        /**
         * Indicates this ImplConnection is used for XA
         */
        protected void prepareForXA()
        {
            isXAConnection = true;
        }


        /**
         * Return true if the driver should expose get/setUnicodeStream
         */
        protected bool shouldExposeGetSetUnicodeStream()
        {

            return false;
        }


        /**
         * Return the maximum number of bytes that a should be cached
         * when a long data field is being cached by the framework.
         *
         * 0 - means defer to the general 'max field size' property settable
         * via Statement.setMaxFieldSize.  The default for that property is
         * no limit.
         */
        protected int getMaxLongDataFieldCacheSize()
        {

            return 0;
        }


        /**
         * Returns whether or not an exception from the native layer during an
         * execution of DatabaseMetaData ResultSet SQL should result in the Framework
         * exposing an empty result.
         */
        protected bool exposeEmptyDBMDResultSetOnExecutionError()
        {

            return true;
        }


        /**
         * Returns 'true' if the concept of a 'rowid' is supported.
         */
        protected bool supportsRowId()
        {

            return false;
        }


        /**
         * Returns a list of aggregate function names.
         *
         * This method only needs to be override by native layers that
         * returns 'true' from supportsRowId
         */
        protected string[] getAggregateFunctions()
        {

            return null;
        }


        protected bool getAutoIncrementAttribute(string tableName, string colName)

        {
            return false;
        }


        /**
         * Sets the status to one of the CONNECT_xxx constants
         */
        public void setConnectionStatus(int status)
        {

            connectionStatus = status;
        }


        /**
         * Returns 'true' if Xlob should be emulated on top of LONGVARx data
         */
        protected bool enableXlobOnLongVarX()
        {

            return false;
        }


        /**
         * Return the most recently cached BaseImplResultSet or null if no
         * entries have been cached.
         *
         * @return BaseImplResultSet
         */
        //public BaseImplResultSet getCachedImplResultSet(
        //    int type)
        //{

        //    BaseImplResultSet implResultSet = null;
        //    BaseImplResultSet[] typeCache = implResultSetCache[type];
        //    int cacheIndex = implResultSetCacheNextIndices[type];

        //    if (cacheIndex >= 0)
        //    {

        //        implResultSet = typeCache[cacheIndex--];
        //        implResultSetCacheNextIndices[type] = cacheIndex;
        //    }

        //    return implResultSet;
        //}


        /**
         * Adds a BaseImplResultSet to the cache unless it's full.
         */
        //public void cacheImplResultSet(
        //        BaseImplResultSet implResultSet)
        //{

        //    BaseImplResultSet[] typeCache = implResultSetCache[implResultSet.type];
        //    int cacheIndex = implResultSetCacheNextIndices[implResultSet.type];

        //    if (cacheIndex < IMPL_RESULT_SET_CACHE_SIZE - 1)
        //    {

        //        typeCache[++cacheIndex] = implResultSet;
        //        implResultSetCacheNextIndices[implResultSet.type] = cacheIndex;
        //    }
        //}

        /**
         * Indicates whether the native driver needs the
         * framework to obtain the column descriptions
         * for generated key columns. This virtual is called
         * by the framework only if the native driver has
         * set the ImplConnection.supportsDMLWithResults
         * flag and the app calls one of the prepareStatement
         * methods for which generated key column values are
         * requested.
         */
        public bool requiresColDescriptionsForGeneratedKeys()
        {
            return false;
        }

        /**
         * Default implementation assumes the native driver requires a 
         * user ID to be supplied at connect time
         * integrated security.
         *
         * @return - true
         */
        public bool requiresUserId()
        {

            return true;
        }

        /**
         * If the native layer uses Framework's socket based query timeout 
         * emulation, this method is called to setup the associated deferred 
         * cancel response handling.
         * 
         * Note that this sets the cancelPending flag to 'true'.  Native layers
         * that use the Frameworks socket based query timeout handling should
         * check this flag prior to reading from (and maybe writing from) the
         * wire.  If this flag is set, the native layers should invoke 
         * socketActivityPostCancel on their associated BaseImplConnection 
         * derivative.  
         */
        //void setCancelResponsePending(
        //    BaseConnection connection,
        //    BaseImplStatement implStatement,
        //    int cancelTimeout)
        //{

        //    cancelPendingConnection = connection;
        //    cancelPendingStatement = implStatement;
        //    cancelPendingTimeout = cancelTimeout;
        //    cancelPending = true;

        //}


        /**
         * This method is called by the native layer before it uses the socket
         * if and only if the cancelPending flag is 'true'.
         * 
         * This method drives the clean up of a previous cancel request that 
         * was made as part of the Framework's socket based query timeout 
         * emulation.  The cancel reponse is postponed in order to expedite 
         * returning control to the application as soon as possible when the
         * query timeout expires.  Reading the reponse at query timeout time
         * may take much longer than expected if there is a communications
         * failure.  
         */
      //  public void socketActivityPostCancel()


      //  {

      //      Socket queryTimeoutSocket = null;


      //      int currentTimeout = 0;

      //      try {

      //          // reset the cancel pending flag
      //          cancelPending = false;

      //          if (cancelPendingTimeout > 0) {

      //              // Set a timeout for reading the cancel response

      //              queryTimeoutSocket = getQueryTimeoutSocket();

      //              currentTimeout = queryTimeoutSocket.getSoTimeout();

      //              queryTimeoutSocket.setSoTimeout(cancelPendingTimeout * 1000);
      //          }

      //          cancelPendingStatement.processCancelResponse();
      //      }
      //      catch (Exception e) {

      //          if (e instanceof SQLException) {

      //              SQLException se = (SQLException)e;

      //              if (se.getMessage().indexOf(INTERNAL_SOCKET_TIMEOUT) != -1) {

      //                  try {

      //                      queryTimeoutSocket.close();
      //                  }
      //                  catch (IOException ioe) {

      //                      //ignore
      //                  }

      //                  //force a closed state on the associated connection
      //                  cancelPendingConnection.implConnection = null;

      //                  throw exceptions.getException(BaseLocalMessages.ERR_QTO_CONNECTION_CLOSED, "08S01");
      //              }
      //              else {

      //                  // Note that this exception may be related to a different statement 
      //                  // other than the object (statement or otherwise) that is triggering 
      //                  // this clean up.


      //                  throw se;
      //              }
      //          }
    		//else {

      //              // socket error from setting/getting socket timout value
      //              //
      //              // - enqueue a warning?
      //              // - throw SQLException?
      //              //
      //              // - do nothing here assuming a major socket problem 
      //              //   handled on it's next use.
      //          }
      //      }
      //      finally {

      //          try {

      //              if (queryTimeoutSocket != null) {

      //                  queryTimeoutSocket.setSoTimeout(currentTimeout);
      //              }
      //          }
      //          catch (Exception se) {

      //              //ignore
      //          }
      //      }
      //  }


        protected string setClientApplicationName(
            string name)


        {

            return name;
        }

        protected string setApplicationName(
                string name)


        {

            return name;
        }


        protected string setClientHostName(
            string hostname)


        {

            return hostname;
        }


        protected string setClientUser(
            string user)


        {

            return user;
        }

        public string getAttribute(
            string attrName)


        {

            return null;
        }

        public void setAttribute(
            string attrName,
            object attrValue)


        {
        }

        //public void addClientInfoAttributesToProperties(
        //    Properties props)


        //{
        //}

        protected string setClientAccountingInfo(
            string info)


        {

            return info;
        }

        protected string setClientProgramID(
                string pid)


        {

            return pid;
        }

        protected string setProgramID(
                string pid)


        {

            return pid;
        }

        /**
         * Processes all the client monitoring information connection properties
         *     post native connection.  
         * 	 
         * @
         */
        //protected void processClientMonitoringProperties()
        //{

        //    String clientAppName = connectProps.get("applicationName");
        //    String clientUser = connectProps.get("clientUser");
        //    String clientHost = connectProps.get("clientHostName");
        //    String acctInfo = connectProps.get("accountingInfo");
        //    String progID = connectProps.get("programID");

        //    // Calling these setter methods adds failover connection events
        //    // by default if failover is turned on because they are public
        //    // methods.  We don't want that since the connection string
        //    // options will be replayed on any failed over connect attempt
        //    // anyway.  So, temporarily turning off failover to prevent the
        //    // addition of unwanted connection events.

        //    int originalFailoverMode = connection.failoverMode;
        //    connection.failoverMode = 0;

        //    try
        //    {
        //        // Turn errors into warnings, except for communication failures
        //        if ((clientAppName != null) && (clientAppName != ""))
        //        {
        //            try
        //            {
        //                connection.setClientApplicationName(clientAppName);
        //            }
        //            catch (QLException ex)
        //            {
        //                if (ex.getSQLState().startsWith("08"))
        //                {
        //                    throw ex;
        //                }
        //                warnings.addExceptionAsWarning(ex);
        //            }
        //        }
        //        if ((clientUser != null) && (clientUser != ""))
        //        {
        //            try
        //            {
        //                connection.setClientUser(clientUser);
        //            }
        //            catch (SQLException ex)
        //            {
        //                if (ex.getSQLState().startsWith("08"))
        //                {
        //                    throw ex;
        //                }
        //                warnings.addExceptionAsWarning(ex);
        //            }
        //        }
        //        if ((clientHost != null) && (clientHost != ""))
        //        {
        //            try
        //            {
        //                connection.setClientHostName(clientHost);
        //            }
        //            catch (SQLException ex)
        //            {
        //                if (ex.getSQLState().startsWith("08"))
        //                {
        //                    throw ex;
        //                }
        //                warnings.addExceptionAsWarning(ex);
        //            }
        //        }
        //        if ((acctInfo != null) && (acctInfo != ""))
        //        {
        //            try
        //            {
        //                connection.setClientAccountingInfo(acctInfo);
        //            }
        //            catch (SQLException ex)
        //            {
        //                if (ex.getSQLState().startsWith("08"))
        //                {
        //                    throw ex;
        //                }
        //                warnings.addExceptionAsWarning(ex);
        //            }
        //        }
        //        if ((progID != null) && (progID != ""))
        //        {
        //            try
        //            {
        //                connection.setClientProgramID(progID);
        //            }
        //            catch (SQLException ex)
        //            {
        //                if (ex.getSQLState().startsWith("08"))
        //                {
        //                    throw ex;
        //                }
        //                warnings.addExceptionAsWarning(ex);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        connection.failoverMode = originalFailoverMode;
        //    }
        //}

        /*
         * Getter for the "currentConnectionResultSetHoldability" field.
         */
        //final public int getCurrentConnectionResultSetHoldability()
        //{
        //    return currentConnectionResultSetHoldability;
        //}

        /*
         * Setter for the "currentConnectionResultSetHoldability" field.
         */
        //final public void setCurrentConnectionResultSetHoldability(int holdability)


        //{

        //    currentConnectionResultSetHoldability = holdability;
        //}

        /*
         * Getter for the "defaultDriverResultSetHoldability" field.
         */
        //final protected int getDefaultDriverResultSetHoldability()
        //{
        //    return defaultDriverResultSetHoldability;
        //}

        /*
         * Setter for the "defaultDriverResultSetHoldability" field. Intended to
         * be called from ImplConnection.open by drivers which are able to support
         * different result set holdabilities.
         * 
         * This method sets both the "defaultDriverResultSetHoldability" and
         * "currentConnectionResultSetHoldability" fields.
         */
        //final protected void setDefaultDriverResultSetHoldability(int holdability)
        //{
        //    currentConnectionResultSetHoldability = defaultDriverResultSetHoldability = holdability;
        //}

        /*
         * Most drivers support only a single ResultSet holdability. The default implementation
         * of this method is a convenience for those drivers. The implementation returns "true"
         * only if the specified holdability is the same as the driver/database default.
         * 
         * Drivers which can support other holdability values should override
         * this method.
         */
        //protected bool supportsResultSetHoldability(int holdability)


        //{

        //    return (holdability == defaultDriverResultSetHoldability);
        //}

        /**
         * Returns true if native layer supports reauthentication.
         */
        protected bool supportsReauthentication()
        {
            return false;
        }

        /**
         * Returns the current authorized user of the connection.
         */
        protected string getCurrentUser()


        {
            return "";
        }

        /**
         * Sets the current authorized user to the user specifiec by username.
         *
         * @param username - new user to switch to
         * @param options - a list named options which control how the driver handles
         * switching the authenticated user.
         * 
         * @exception SQLException
         */
        //protected void setCurrentUser(
        //        String username,
        //        Properties options)


        //{

        //}

        /**
         * Resets the current user of the connection to the user that was specified
         * when the connection was initially created.
         *
         * @exception SQLException
         */
        protected void resetUser()


        {

        }

        // If any JDBC statements are to be executed as part of the initial connect 
        // process, these statements should be executed using this method by
        // overriding this method in the sub class.
        //
        // These statements need to be executed after the initial connect (implconn.open())
        // because when the failover mode is select and a failover occurs statement's
        // BaseConnection and ImplConnection references need to be updated with the
        // newly opened connection before executing the Statement.
        protected void doFinalConnectionSetup()


        {

        }

        /**
         * Returns the cancelPendingTimeout.
         */
        public int getCancelPendingTimeout()
        {
            return cancelPendingTimeout;
        }

        //public String getDatabaseName()
        //{
        //    return connectProps.get("databaseName");
        //}

        // Return the server's charset in the CSV defined code page name.
        // The Csv reader checks to decide if the driver can load the character 
        // columns without translation.
        // For example, the charset for Oracle server is 1252 and the codepage of Csv file is 1252.
        public String getServerCharsetAsCsvEncoding()
        {

            return null;
        }

        // Return driver specific various options for the bulk load process.
        public int getBulkLoadOptions()
        {

            return 0;
        }

        // Return the default code page for the bulk CSV file.
        // Note, this method exists on the implConnection so the driver can
        // override the default.
        //public String getBulkCsvFileDefaultEncoding()
        //{
        //    String vmEncoding = System.getProperty("file.encoding");
        //    String codePage = BaseCsvDataReader.mapVMCodePageToCSVCodePage(vmEncoding);
        //    if (codePage == null)
        //    {
        //        codePage = "utf-8";
        //    }
        //    return codePage;
        //}

        // By default, the drivers currently do not support timeout on bulk load.  If the driver
        // implementation can support it, they can override this method and report true.
        public bool getBulkSupportsTimeout()
        {
            return false;
        }

        //public BaseData getReusableASCIIInputStream()
        //{

        //    if (m_reusableBaseDataASCIIInputStream == null)
        //    {
        //        m_reusableBaseDataASCIIInputStream = createDataInstance(BaseData.ASCIIINPUTSTREAM, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataASCIIInputStream;
        //}

        //public BaseData getReusableBigDecimal()
        //{

        //    if (m_reusableBaseDataBigDecimal == null)
        //    {
        //        m_reusableBaseDataBigDecimal = createDataInstance(BaseData.BIGDECIMAL, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataBigDecimal;
        //}

        //public BaseData getReusableBinaryInputStream()
        //{

        //    if (m_reusableBaseDataBinaryInputStream == null)
        //    {
        //        m_reusableBaseDataBinaryInputStream = createDataInstance(BaseData.BINARYINPUTSTREAM, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataBinaryInputStream;
        //}

        //public BaseData getReusableBlob()
        //{

        //    if (m_reusableBaseDataBlob == null)
        //    {
        //        m_reusableBaseDataBlob = createDataInstance(BaseData.BLOB, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataBlob;
        //}

        //public BaseData getReusableBoolean()
        //{

        //    if (m_reusableBaseDataBoolean == null)
        //    {
        //        m_reusableBaseDataBoolean = createDataInstance(BaseData.BOOLEAN, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataBoolean;
        //}

        //public BaseData getReusableByte()
        //{

        //    if (m_reusableBaseDataByte == null)
        //    {
        //        m_reusableBaseDataByte = createDataInstance(BaseData.BYTE, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataByte;
        //}

        //public BaseData getReusableByteArray()
        //{

        //    if (m_reusableBaseDataByteArray == null)
        //    {
        //        m_reusableBaseDataByteArray = createDataInstance(BaseData.BYTE_ARRAY, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataByteArray;
        //}

        //public BaseData getReusableCharacterStreamReader()
        //{

        //    if (m_reusableBaseDataCharacterStreamReader == null)
        //    {
        //        m_reusableBaseDataCharacterStreamReader = createDataInstance(BaseData.CHARACTERSTREAMREADER, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataCharacterStreamReader;
        //}

        //public BaseData getReusableClob()
        //{

        //    if (m_reusableBaseDataClob == null)
        //    {
        //        m_reusableBaseDataClob = createDataInstance(BaseData.CLOB, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataClob;
        //}

        //public BaseData getReusableDate()
        //{

        //    if (m_reusableBaseDataDate == null)
        //    {
        //        m_reusableBaseDataDate = createDataInstance(BaseData.DATE, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataDate;
        //}

        //public BaseData getReusableDB2Date()
        //{

        //    if (m_reusableBaseDataDB2Date == null)
        //    {
        //        m_reusableBaseDataDB2Date = createDataInstance(BaseData.DB2_DATE, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataDB2Date;
        //}

        //public BaseData getReusableDB2Time()
        //{

        //    if (m_reusableBaseDataDB2Time == null)
        //    {
        //        m_reusableBaseDataDB2Time = createDataInstance(BaseData.DB2_TIME, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataDB2Time;
        //}

        //public BaseData getReusableDB2Timestamp()
        //{

        //    if (m_reusableBaseDataDB2Timestamp == null)
        //    {
        //        m_reusableBaseDataDB2Timestamp = createDataInstance(BaseData.DB2_TIMESTAMP, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataDB2Timestamp;
        //}

        //public BaseData getReusableDB2TimestampWithTimeZone()
        //{

        //    if (m_reusableBaseDataDB2TimestampWithTimeZone == null)
        //    {
        //        m_reusableBaseDataDB2TimestampWithTimeZone = createDataInstance(BaseData.DB2_TIMESTAMP_WITH_TIME_ZONE, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataDB2TimestampWithTimeZone;
        //}


        //public BaseData getReusableDouble()
        //{

        //    if (m_reusableBaseDataDouble == null)
        //    {
        //        m_reusableBaseDataDouble = createDataInstance(BaseData.DOUBLE, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataDouble;
        //}

        //public BaseData getReusableFloat()
        //{

        //    if (m_reusableBaseDataFloat == null)
        //    {
        //        m_reusableBaseDataFloat = createDataInstance(BaseData.FLOAT, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataFloat;
        //}

        //public BaseData getReusableInteger()
        //{

        //    if (m_reusableBaseDataInteger == null)
        //    {
        //        m_reusableBaseDataInteger = createDataInstance(BaseData.INTEGER, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataInteger;
        //}

        //public BaseData getReusableLong()
        //{

        //    if (m_reusableBaseDataLong == null)
        //    {
        //        m_reusableBaseDataLong = createDataInstance(BaseData.LONG, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataLong;
        //}

        //public BaseData getReusableShort()
        //{

        //    if (m_reusableBaseDataShort == null)
        //    {
        //        m_reusableBaseDataShort = createDataInstance(BaseData.SHORT, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataShort;
        //}

        //public BaseData getReusableSmallDecimal()
        //{

        //    if (m_reusableBaseDataSmallDecimal == null)
        //    {
        //        m_reusableBaseDataSmallDecimal = createDataInstance(BaseData.SMALLDECIMAL, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataSmallDecimal;
        //}

        //public BaseData getReusableString()
        //{

        //    if (m_reusableBaseDataString == null)
        //    {
        //        m_reusableBaseDataString = createDataInstance(BaseData.STRING, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataString;
        //}

        //public BaseData getReusableTime()
        //{

        //    if (m_reusableBaseDataTime == null)
        //    {
        //        m_reusableBaseDataTime = createDataInstance(BaseData.TIME, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataTime;
        //}

        //public BaseData getReusableTimestamp()
        //{

        //    if (m_reusableBaseDataTimestamp == null)
        //    {
        //        m_reusableBaseDataTimestamp = createDataInstance(BaseData.TIMESTAMP, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataTimestamp;
        //}

        //public BaseData getReusableUCS2InputStream()
        //{

        //    if (m_reusableBaseDataUCS2InputStream == null)
        //    {
        //        m_reusableBaseDataUCS2InputStream = createDataInstance(BaseData.UCS2INPUTSTREAM, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataUCS2InputStream;
        //}

        //public BaseData getReusableUTF8InputStream()
        //{

        //    if (m_reusableBaseDataUTF8InputStream == null)
        //    {
        //        m_reusableBaseDataUTF8InputStream = createDataInstance(BaseData.UTF8INPUTSTREAM, BaseDataMetaDataDummy.instance);
        //    }

        //    return m_reusableBaseDataUTF8InputStream;
        //}

        //	Give the driver a chance to override what is reported for the extra
        //	version info.  The rootComponentVersions string passed in has the 
        //	format:
        //		(Fa.Ub)
        //	where a is the framework buildId and b is the Util buildId.  The 
        //	String that is returned is appended to the driver version info.
        protected string getComponentVersions(string rootComponentVersions)
        {

            return rootComponentVersions;
        }

        protected Array createArray(string typeName,object[] elements )


        {

            return null;
        }

        //protected Struct createStruct(String typeName, Object attributes [])


        //{

        //    return null;
        //}

        //protected bool isConnectionAlive(int timeout)
        //{

        //    Statement testStatement = null;

        //    bool valid = true;

        //    try
        //    {

        //        testStatement = this.connection.createStatement();
        //        ((BaseStatement)testStatement).resetKeepAliveOnlyMode();
        //        int originalTimeout = testStatement.getQueryTimeout();
        //        testStatement.setQueryTimeout(timeout);
        //        testStatement.execute("Test Server Connectivity");
        //        testStatement.setQueryTimeout(originalTimeout);
        //    }
        //    catch (SQLException se)
        //    {

        //        // Note all native layers throw "08" if creating the socket
        //        // causes an exception.
        //        // Or Query timeout

        //        if (se.getSQLState().charAt(0) == '0' &&
        //                se.getSQLState().charAt(1) == '8' ||
        //                se.getSQLState().equalsIgnoreCase("HYT00"))
        //        {

        //            valid = false;
        //            string[] args = new string[1];
        //            args[0] = se.getMessage();

        //            warnings.add(
        //                    BaseLocalMessages.EMPTY_1_ARG_MESSAGE,
        //                    args,
        //                    se.getSQLState());
        //        }

        //        if (testStatement != null)
        //        {

        //            try
        //            {

        //                testStatement.close();
        //            }
        //            catch (SQLException se2)
        //            {

        //            }
        //        }
        //    }

        //    return valid;

        //}


        //public String getCommunicationCharset()


        //{

        //    String[]
        //    exceptionArg = { "Connection.getCommunicationCharset" };

        //    throw exceptions.getException(BaseLocalMessages.METHOD_NOT_SUPPORTED, exceptionArg);
        //}

        //public String getUnicodeCommunicationCharset()


        //{

        //    String[]
        //    exceptionArg = { "Connection.getUnicodeCommunicationCharset" };

        //    throw exceptions.getException(BaseLocalMessages.METHOD_NOT_SUPPORTED, exceptionArg);
        //}

        //public bool requiresBufferStreamForLobs()
        //{
        //    return false;
        //}

        //public BaseData createDataInstance(
        //    int type,
        //    BaseDataMetaData metaData,
        //    Object data)
        //{

        //    BaseData returnObject = createDataInstance(type, metaData);

        //    returnObject.setData(type, data);
        //    return returnObject;
        //}

        //public com.ddtek.jdbc.extensions.DDBulkLoad createBulkLoadObject()


        //{

        //    BaseMessages msgs = new BaseMessages();
        //    throw new SQLException(msgs.getMessage(BaseLocalMessages.ERR_BAD_CONN_FOR_BL_CREATE, null, false), "HY000");
        //}

        //public BaseData createDataInstance(
        //    int type,
        //    BaseDataMetaData metaData)
        //{

        //    BaseData returnObject;

        //    switch (type)
        //    {

        //        case BaseData.ASCIIINPUTSTREAM:
        //            returnObject = new BaseDataASCIIInputStream();
        //            break;

        //        case BaseData.BIGDECIMAL:
        //            returnObject = new BaseDataBigDecimal();
        //            break;

        //        case BaseData.BINARYINPUTSTREAM:
        //            returnObject = new BaseDataBinaryInputStream();
        //            break;

        //        case BaseData.BLOB:
        //            returnObject = new BaseDataBlob();
        //            break;

        //        case BaseData.BOOLEAN:
        //            returnObject = new BaseDataBoolean(connection);
        //            break;

        //        case BaseData.BYTE:
        //            returnObject = new BaseDataByte(connection);
        //            break;

        //        case BaseData.BYTE_ARRAY:
        //            returnObject = new BaseDataByteArray();
        //            break;

        //        case BaseData.CHARACTERSTREAMREADER:
        //            returnObject = new BaseDataCharacterStreamReader();
        //            break;

        //        case BaseData.CLOB:
        //            returnObject = new BaseDataClob();
        //            break;

        //        case BaseData.DATE:
        //            returnObject = new BaseDataDate();
        //            break;

        //        case BaseData.DB2_DATE:
        //            returnObject = new BaseDataDB2Date();
        //            break;

        //        case BaseData.DB2_TIME:
        //            returnObject = new BaseDataDB2Time();
        //            break;

        //        case BaseData.DB2_TIMESTAMP:
        //            returnObject = new BaseDataDB2Timestamp();
        //            break;

        //        case BaseData.DB2_TIMESTAMP_WITH_TIME_ZONE:
        //            returnObject = new BaseDataDB2TimestampWithTimeZone();
        //            break;

        //        case BaseData.DOUBLE:
        //            returnObject = new BaseDataDouble(connection);
        //            break;

        //        case BaseData.FLOAT:
        //            returnObject = new BaseDataFloat(connection);
        //            break;

        //        case BaseData.INTEGER:
        //            returnObject = new BaseDataInteger(connection);
        //            break;

        //        case BaseData.LONG:
        //            returnObject = new BaseDataLong(connection);
        //            break;

        //        case BaseData.SHORT:
        //            returnObject = new BaseDataShort(connection);
        //            break;

        //        case BaseData.SMALLDECIMAL:
        //            returnObject = new BaseDataSmallDecimal();
        //            break;

        //        case BaseData.STRING:
        //            returnObject = new BaseDataString();
        //            break;

        //        case BaseData.TIME:
        //            returnObject = new BaseDataTime();
        //            break;

        //        case BaseData.TIMESTAMP:
        //            returnObject = new BaseDataTimestamp();
        //            break;

        //        case BaseData.UCS2INPUTSTREAM:
        //            returnObject = new BaseDataUCS2InputStream();
        //            break;

        //        case BaseData.UTF8INPUTSTREAM:
        //            returnObject = new BaseDataUTF8InputStream();
        //            break;

        //        case BaseData.SQLSERVER_DATETIME:
        //            returnObject = new BaseDataSQLServerDatetime();
        //            break;

        //        case BaseData.SQLSERVER_SMALLDATETIME:
        //            returnObject = new BaseDataSQLServerSmallDatetime();
        //            break;

        //        case BaseData.SQLSERVER_UNIQUEIDENTIFIER:
        //            returnObject = new BaseDataSQLServerUniqueIdentifier();
        //            break;

        //        case BaseData.SQLSERVER_DATE_TDS72:
        //            returnObject = new BaseDataSQLServerDateTDS72();
        //            break;

        //        case BaseData.SQLSERVER_DATETIME2_TDS72:
        //            returnObject = new BaseDataSQLServerDatetime2TDS72();
        //            break;

        //        case BaseData.SQLSERVER_TIME_TDS72:
        //            returnObject = new BaseDataSQLServerTimeTDS72();
        //            break;

        //        case BaseData.SQLSERVER_DATETIMEOFFSET_TDS72:
        //            returnObject = new BaseDataSQLServerDatetimeoffsetTDS72();
        //            break;

        //        case BaseData.SQLSERVER_DATE_TDS73:
        //            returnObject = new BaseDataSQLServerDateTDS73();
        //            break;

        //        case BaseData.SQLSERVER_DATETIME2_TDS73:
        //            returnObject = new BaseDataSQLServerDatetime2TDS73();
        //            break;

        //        case BaseData.SQLSERVER_TIME_TDS73:
        //            returnObject = new BaseDataSQLServerTimeTDS73();
        //            break;

        //        case BaseData.SQLSERVER_DATETIMEOFFSET_TDS73:
        //            returnObject = new BaseDataSQLServerDatetimeoffsetTDS73();
        //            break;

        //        case BaseData.SYBASE_DATETIME:
        //            returnObject = new BaseDataSybaseDateTime();
        //            break;

        //        case BaseData.SYBASE_BIGDATETIME:
        //            returnObject = new BaseDataSybaseBigDateTime();
        //            break;

        //        case BaseData.SYBASE_BIGTIME_AS_TIMESTAMP:
        //        case BaseData.SYBASE_BIGTIME:
        //            returnObject = new BaseDataSybaseBigTime(type);
        //            break;

        //        case BaseData.SYBASE_TIME_AS_TIMESTAMP:
        //        case BaseData.SYBASE_TIME:
        //            returnObject = new BaseDataSybaseTime(type);
        //            break;

        //        default:
        //            // Create an instance which can handle all data types.
        //            returnObject = new BaseDataGeneral(type);
        //            break;
        //    }

        //    returnObject.connection = connection;
        //    // REVISIT - Is it really necessary to specifically set the value to null?  It seems like most
        //    // BaseData objects would construct to a null state by default.
        //    returnObject.setNull();
        //    return returnObject;
        //}

        //public BaseData createDataInstance(
        //    Object data,
        //    BaseDataMetaData metaData)
        //{

        //    int type = BaseData.getJavaObjectType(data, metaData.getSqlType());
        //    BaseData returnObject = createDataInstance(type, metaData);

        //    returnObject.setData(type, data);
        //    return returnObject;

        //}

        //public bool isDataStreamed(int type)
        //{
        //    switch (type)
        //    {

        //        case BaseData.BINARYINPUTSTREAM:
        //        case BaseData.ASCIIINPUTSTREAM:
        //        case BaseData.UTF8INPUTSTREAM:
        //        case BaseData.UCS2INPUTSTREAM:
        //        case BaseData.CHARACTERSTREAMREADER:
        //        case BaseData.BLOB:
        //        case BaseData.CLOB:
        //            return true;

        //        default:
        //            return false;
        //    }
        //}


        /**
         * Alters the SQL statement from ANSI syntax to data source specific syntax if required.
         * @param sql	This is the original SQL statement passed in by the application
         * @return		This is the SQL statement returned by the driver that may include
         * 				any data source specific modifications. 
         * 
         * This default implementation simply returns "null", indicating that no data source specific
         * modifications are necessary. Native drivers can override this method as appropriate. 
         */
        public String alterSQL(
            String sql)
        {
            return null;
        }

        // Method for additional info needed by the D2C ODBC driver.
        public int[] getD2CInfo()


        {

            return null;
        }

        // Method for DAS to set the client time zone.
        public void setD2CClientTimeZone(String tz)


        {

            return;
        }
    }
}

