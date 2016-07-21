using System;
//using ddtek.d2ccore.cloudMessage;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
//using System.Data.SqlClient;
using System.Net.Http;
using ddtek.d2ccore.Common;
using DDInt.Utility;

namespace ddtek.d2ccore
{
    public class DDCloudImplConnection : BaseImplConnection
    {
        public static Uri Url = new Uri("https://web-main-int.aws-test.progress.com/d2c-ui/#/datasourceform");
        public static StringContent queryString = null;
        protected string DEFAULT_D2C_SERVERNAME = "service.datadirectcloud.com";
        //private static final String DEFAULT_D2C_SERVERNAME = "service-alpha-int.aws-test.progress.com";
        private static string DEFAULT_D2C_PORT = "443";

        public static string CLOUDSERVICE_CLOUD_SQL = "/cloudservice/CloudSql";
        public static int PREFIX_PATH_LENGTH = 22;
        /*
        // ERROR CODES - should be in message package.
        public static final int NOPERSISTENTSESSION = 222206013;
        public static final int RENEWAL_REQUIRED = 222206026;

        public static final String SQLSTATE_COMMUNICATION_LINK_FAILURE = "08S01";
            public static final String SQLSTATE_GENERAL_ERROR = "HY000";
            public static final String SQLSTATE_TIMEOUT = "HYT00";
            public static final String SQLSTATE_CONNECTION_TIMEOUT = "HYT01";
            public static final String SQLSTATE_INVALID_AUTHORIZATION = "28000";
            public static final String SQLSTATE_INTEGRITY_CONSTRAINT_VIOLATION = "23000";
            public static final String SQLSTATE_SERVER_REJECTED_CONNECTION = "08004";
            public static final String SQLSTATE_NUMERIC_VALUE_OUT_OF_RANGE = "22003";
            public static final String SQLSTATE_STRING_DATA_TRUNCATED = "22001";
            */
            public static string PROXY_HOST = "proxyhost";
            public static string PROXY_PASSWORD = "proxypassword";
            public static string PROXY_PORT = "proxyport";
            public static string PROXY_USER = "proxyuser";
            
        public static string DATASOURCE_USERID = "datasourceUserId";
        public static string DATASOURCE_PASSWORD = "datasourcePassword";
        public static string AUTHENTICATION_METHOD = "authenticationMethod";

        public static string WS_RETRY_COUNT = "wsretrycount";
        public static string WS_TIMEOUT = "wstimeout";

        public static string OPTION_TRANSACTION_MODE = "transactionmode";

        public static string TX_MODE_IGNORE = "Ignore";
        public static string TX_MODE_NO_TRANSACTIONS = "NoTransactions";

        public static string DEFAULT_TX_MODE = TX_MODE_NO_TRANSACTIONS;

        public static string EXTENDED_PROPERTIES = "extendedProperties";
        public static string CONNECTOR_ID = "connectorID";

        public static int DEFAULT_WS_TIMEOUT = 120000;
        public static int DEFAULT_WS_RETRY_COUNT = 3;

        public int wsTimeout = DEFAULT_WS_TIMEOUT;
        public int wsRetryCount = DEFAULT_WS_RETRY_COUNT;

        public int queryTimeout = 0;

        static int TX_IGNORE = 1;
        static int TX_NO_TRANSACTIONS = 2;

       // Logger logger;
        string sessionToken;

        private string cachedBaseURL;
        private bool useSSL;

        // All requests, except cancel requests, need to use the same HttpClient
        // so they all use the same session affinity cookie (and the same DAS).

        private HttpClient httpClient;
        private HttpClient httpClientCancel;

        // So httpClient and httpClientCancel share the same cookie store (and
        // the same session affinity cookie).

        //BasicCookieStore cookieStore = new BasicCookieStore();

        //private DDCloudHttpRequest httpRequestLongData;

        private int transactionMode;

        private int stmtId = 0;

        private DDCloudHttpRequest httpRequest;

        public readonly UtilTransliterator transliterator;
        //TokenDatabaseInfo dbInfo;

        StringBuilder urlBuilder = new StringBuilder(256);

        private static string CLIENT_ID_PREFIX = "[DataDirect]";
        private static string VENDOR_FILE_PREFIX = "[##]";
        /*
        public static final HashSet<String> utcTimeZones;
        private static final String[] GMTEquivalents = {"ETC/GMT", "ETC/GMT+0", "ETC/UCT", "ETC/UTC", "ETC/UNIVERSAL", "ETC/ZULU", "GMT0", "GREENWICH", "UCT", "UTC", "UNIVERSAL", "ZULU", "ETC/GMT-0", "ETC/GMT0", "ETC/GREENWICH"};

            static {
                //Create the set of names that correspond to UTC (GMT)
                utcTimeZones = new HashSet<String>();

                for (int i = 0; i<GMTEquivalents.length; i++) {

                    utcTimeZones.add(GMTEquivalents[i]);

                }		
            }
        */
        //HttpHost target;
        public string data;

       // private BaseImplDatabaseMetaData databaseMetaData;
        //public BaseTypeInfos typeInfos;
        Dictionary<string, short> typeNameSearchableMap;
        Dictionary<short, string> sqlDataTypeSearchableMap;
        //public HashMap<String, Short> typeNameSearchableMap;
        //public HashMap<Short, Short> sqlDataTypeSearchableMap;

        // used to determine if the datatypes are signed or not - fetched from the datatypeinfo at login.
        // will be used to "promote" datatypes because we cannot determine if the value is signed vs unsigned.
        bool signedTinyInt = false;
        bool bothTinyInt = false;
        bool bothSmallInt = false;   // currently there is no driver that has just unsigned smallInt
        bool bothInteger = false;    // currently there is no driver that has just unsigned integer

        private string extendedProperties;
        public string connectorID;

        public int OnPremiseTraceOptions;

        public Calendar msgHdrCal;
        public StringBuilder msgHdrStr;

        // Calendar corresponding to the client time zone. Will be null
        // if the clientTimezone connection option was not specified.
        private Calendar clientCalendar;
        // Calendar corresponding to the user.timezone system property.
        // Will be null if clientTimezone connection option was specified.
        private Calendar userCalendar;
        // TimeZone corresponding to the user.timezone system property.
        // I.e., TimeZone.getDefault().
        // Will be null if clientTimezone connection option was specified.
        //private TimeZone currentTZ;

        public static int TimestampInHeader = 0x00000001;

        public int varbinaryToLongThreshold, varcharToLongThreshold;

        public bool requestD2Cinfo;

        public static int[] D2Cinfo = new int[100];

        public int protocolVersion;

        public bool enableCancelTimeout;
        /*
        public DDCloudImplConnection()
        {
            transliterator = new UtilTransliteratorForUTF8();
            dbInfo = new TokenDatabaseInfo(false);
            httpRequest = new DDCloudHttpRequest(this, 1024, 2048);
        }
        */
        //protected BaseImplDatabaseMetaData createImplDatabaseMetaData()
        //{

        //    return databaseMetaData;
        //}
        /*
        @Override
            protected boolean enableXlobOnLongVarX()
        {
            // Indicate to the framework that this driver needs its BLOB/CLOB
            // emulation.
            return true;
        }

        @Override
            protected void getImplPropertyInfo(BaseDriverPropertyInfos driverPropInfos)
        {
            driverPropInfos.put("user", "user ID", "", null, true);
            driverPropInfos.put("password", "Password", "", null, true);
            driverPropInfos.put("portNumber", "Port Number", "443", null, true);
            driverPropInfos.put("databaseName", "Database Name", "", null, false);
            driverPropInfos.put(DATASOURCE_USERID, "Data Store User Id", "", null, false);
            driverPropInfos.put(DATASOURCE_PASSWORD, "Data Store Password", "", null, false);
            driverPropInfos.put("serverName", "Server Name", DEFAULT_D2C_SERVERNAME, null, false);
            driverPropInfos.put(PROXY_HOST, "Proxy Host", "", null, false);
            driverPropInfos.put(PROXY_PORT, "Proxy Port", "", null, false);
            driverPropInfos.put(PROXY_USER, "Proxy User", "", null, false);
            driverPropInfos.put(PROXY_PASSWORD, "Proxy Password", "", null, false);
            driverPropInfos.put("loginTimeout", "Login Timeout", "0", null, false);
            driverPropInfos.put(WS_TIMEOUT, "Web Service Timeout", Integer.toString(DEFAULT_WS_TIMEOUT), null, false);
            driverPropInfos.put(WS_RETRY_COUNT, "Web Service Retry Count", Integer.toString(DEFAULT_WS_RETRY_COUNT), null, false);
            driverPropInfos.put(OPTION_TRANSACTION_MODE, "Transaction Mode", DEFAULT_TX_MODE, new String[] { TX_MODE_NO_TRANSACTIONS, TX_MODE_IGNORE }, false);
            driverPropInfos.put(OPTION_LOG_CONFIG_FILE, "Log Config File", DEFAULT_LOG_CONFIG_FILE, null, false);

            driverPropInfos.put("bulkLoadBatchSize", "Specifies the number of rows that the driver sends to the database at a time during bulk operations.", "10000", null, false);
        }

        @Override
            public int getEmptyRowInsertSyntax()
        {
            return ERIS_NULL_VALUES_LIST;
        }


        public DDCloudHttpRequest getLongDataHttpRequest()
        {
            if (httpRequestLongData == null)
            {
                httpRequestLongData = new DDCloudHttpRequest(this, 0, dbInfo.getIntegerInfo(TokenDatabaseInfo.DBINFO_INT_MAX_RESPONSE_SIZE) + 1024);
            }
            return httpRequestLongData;
        }
        */
        public override void open()
        {

            int extLen;
            extendedProperties = getConnectPropAsString(EXTENDED_PROPERTIES, null);

            if (extendedProperties != null)
            {

                // In a URL it's necessary to enclose the extendedProperties within
                // parenthesis to handle the embedded quotes.
                // extendedProperties=(url="jdbc:datadirect:sforce://login.salesforce.com";className="com.ddtek.jdbc.sforce.SForceDriver";dataStore="Salesforce")

                if (((extLen = extendedProperties.Length) > 0) &&
                  (extendedProperties[0] == '(') &&
                  (extendedProperties[extLen - 1] == ')'))
                {
                    extendedProperties = extendedProperties.Substring(1, extLen - 1);
                }
            }
            if ((connectorID = getConnectPropAsString(CONNECTOR_ID, null)) != null)
            {
                char[] user = new char[100];
                //string opowner;

                ////if ((OPowner = connectProps.get("OPowner")) != null)
                //{
                //            //StringBuilder sb = new StringBuilder(connectorID.Length + OPowner.Length);
                //            StringBuilder sb = new StringBuilder(connectorID.Length + OPowner.Length);

                //            sb.Append(connectorID);
                //    sb.Append(OPowner);
                //    connectorID = sb.ToString();
                //}
                //else if ((user = connectProps.getUser()) != null)
                //{
                //    StringBuilder sb = new StringBuilder(connectorID.Length + user.Length);

                //    sb.Append(connectorID);
                //    sb.Append(user);
                //    connectorID = sb.ToString();
                ////}
            }

            //String ect = connectProps.get (BaseConnection.ENABLE_CANCEL_TIMEOUT);
            //enableCancelTimeout = bool.Parse(ect);

            //String txModeAsSString = getConnectPropAsString(OPTION_TRANSACTION_MODE, DEFAULT_TX_MODE);
            //if(txModeAsSString.Equals(TX_MODE_IGNORE,StringComparison.InvariantCultureIgnoreCase))
            ////if (txModeAsSString.equalsIgnoreCase(TX_MODE_IGNORE))
            //{
            //    transactionMode = TX_IGNORE;
            //}
            //else
            //{
            //    transactionMode = TX_NO_TRANSACTIONS;
            //}

            //// hidden option for SSL. Defaults to false for now, but will need
            //// default to true.
            //string encryptionMethod = "EncryptionMethod";
            string encryptionMethod = "SSL";
            if (encryptionMethod != null)
            {
                if (encryptionMethod.Equals("noEncryption"))
                {
                    useSSL = false;
                }
                else if (encryptionMethod.Equals("SSL"))
                {
                    useSSL = true;
                }
                else
                {
                    string[] s = { "EncryptionMethod=" + encryptionMethod };
                    //throw exceptions.getException(BaseLocalMessages.ERR_INVALID_PROP_VALUE, s, "08001");
                    throw new Exception("Sravan put this exception in encyption method in CloudImplConnection");
                }
            }
            else
            {
                useSSL = true;
            }

            //// Hidden option for On-Premise debugging
            //String onPremiseTraceOptions = connectProps.get("OnPremiseTraceOptions");
            //if (onPremiseTraceOptions != null)
            //{
            //    OnPremiseTraceOptions = int.Parse(onPremiseTraceOptions);
            //    if ((OnPremiseTraceOptions & TimestampInHeader) != 0)
            //    {
            //        msgHdrCal = Calendar.GetInstance(TimeZone.getTimeZone("UTC"));
            //        msgHdrStr = new StringBuilder(32);
            //    }
            //}
            //else
            //{
            //    OnPremiseTraceOptions = 0;
            //}

            //String toLongThresholdStr = connectProps.get("varcharToLongThreshold");
            //if (toLongThresholdStr != null)
            //{
            //    varcharToLongThreshold = int.Parse(toLongThresholdStr);
            //}
            //else
            //{
            //    varcharToLongThreshold = 32768;
            //}

            //toLongThresholdStr = connectProps.get("varbinaryToLongThreshold");
            //if (toLongThresholdStr != null)
            //{
            //    varbinaryToLongThreshold = int.Parse(toLongThresholdStr);
            //}
            //else
            //{
            //    varbinaryToLongThreshold = 32768;
            //}

            //String requestD2CinfoStr = connectProps.get("requestD2Cinfo");
            //if (requestD2CinfoStr != null)
            //{
            //    requestD2Cinfo = Boolean.Parse(requestD2CinfoStr);
            //}
            //else
            //{
            //    requestD2Cinfo = false;
            //}

            string hostname = "web-main-int.aws-test.progress.com";//connectProps.get("serverName");
            string port = "443";//connectProps.get("portNumber");
                             //wsTimeout = int.Parse(connectProps.get(WS_TIMEOUT));
                             //if (wsTimeout < 0)
                             //{
                             //    throw exceptions.getException(BaseLocalMessages.ERR_INVALID_PROP_VALUE, new String[] { WS_TIMEOUT }, "08001");
                             //}
                             //wsRetryCount = int.Parse(connectProps.get(WS_RETRY_COUNT));
                             //if (wsRetryCount < 0)
                             //{
                             //    throw exceptions.getException(BaseLocalMessages.ERR_INVALID_PROP_VALUE, new String[] { WS_RETRY_COUNT }, "08001");
                             //}

            //urlBuilder.SetLength(0);
            //if (useSSL)
            //{
            //    urlBuilder.Append("https://");
            //}
            //else
            //{
            //    urlBuilder.Append("http://");
            //}

            //if (hostname == null)
            //{

            //    hostname = DEFAULT_D2C_SERVERNAME;
            //}
            //if (port == null)
            //{

            //    port = DEFAULT_D2C_PORT;
            //}

            if(useSSL)
            {
                data = "https";
            }
            else
            {
                data = "http";
            }
            queryString = new StringContent(data);
            


            urlBuilder.Append(hostname);
            urlBuilder.Append(":");
            urlBuilder.Append(port);
            urlBuilder.Append(CLOUDSERVICE_CLOUD_SQL);
            cachedBaseURL = urlBuilder.ToString();
            //target = new HttpHost(hostname, int.Parse(port), useSSL ? "https" : "http");

            urlBuilder.Clear();
            urlBuilder.Append(CLOUDSERVICE_CLOUD_SQL);

            try
            {
                login(false);
                /*
                typeNameSearchableMap = new Dictionary<string, short>();
                sqlDataTypeSearchableMap = new Dictionary<short, string>();

                databaseMetaData = new DDCloudImplDatabaseMetaData(this);
                typeInfos = ((DDCloudImplDatabaseMetaData)databaseMetaData).getTypeInfo();

                bool hasUnsignedTinyIntType = false, hasSignedTinyIntType = false;
                for (int i = 0; i < typeInfos.count(); i++)
                {
                    BaseTypeInfo baseTypeInfo = typeInfos.get(i);

                    typeNameSearchableMap.put(baseTypeInfo.typeName, (short)(baseTypeInfo.searchable + 1));
                    sqlDataTypeSearchableMap.put(baseTypeInfo.dataType, (short)(baseTypeInfo.searchable + 1));

                    if (baseTypeInfo.dataType == Types.TINYINT)
                    {

                        if (baseTypeInfo.unsignedAttribute)
                        {

                            hasUnsignedTinyIntType = true;
                        }
                        else
                        {

                            hasSignedTinyIntType = true;
                        }
                    }
                    else if (baseTypeInfo.dataType == Types.SMALLINT)
                    {

                        if (baseTypeInfo.unsignedAttribute)
                        {

                            bothSmallInt = true;
                        }
                    }
                    else if (baseTypeInfo.dataType == Types.INTEGER)
                    {

                        if (baseTypeInfo.unsignedAttribute)
                        {

                            bothInteger = true;
                        }
                    }
                }

                if (hasSignedTinyIntType && hasUnsignedTinyIntType)
                {

                    bothTinyInt = true;
                }
                else if (!hasUnsignedTinyIntType)
                {

                    // As long as we don't an unsigned TINYINT type, then use signed parameter binding.
                    signedTinyInt = true;
                }

                if ((D2Cinfo != null) && (!databaseMetaData.supportsMultipleResultSets))
                {

                    // If multiple result sets is not supported, then nothing should be reported for batches.

                    if (D2Cinfo.Length > BaseConstantsD2C.DBINFO_ODBC_INT_BATCH_ROW_COUNT)
                    {
                        D2Cinfo[BaseConstantsD2C.DBINFO_ODBC_INT_BATCH_ROW_COUNT] = 0;
                    }
                    if (D2Cinfo.Length > BaseConstantsD2C.DBINFO_ODBC_INT_BATCH_SUPPORT)
                    {
                        D2Cinfo[BaseConstantsD2C.DBINFO_ODBC_INT_BATCH_SUPPORT] = 0;
                    }
                }

                setDefaultDriverResultSetHoldability(dbInfo.getIntegerInfo(TokenDatabaseInfo.DBINFO_INT_RESULT_SET_HOLDABILITY));
                */
            }
            catch (Exception e)
            {
                //if (httpClient != null)
                //{
                //    httpClient.getConnectionManager().shutdown();

                //    httpClient = null;
                //} 

                //throw e;
            }
        }/*



    public boolean supportsCancel()
{
    return true;
}

int getTransactionMode()
{
    return transactionMode;
}

static String getSystemPropertyAsString(final String name)
{
    return AccessController.doPrivileged(new PrivilegedAction<String>()
    {

            public String run()
{
    return System.getProperty(name);
}
		});
	}
*/
        void login(bool renewal)
        {
            //if (httpClient != null)
            //{
            //    httpClient.getConnectionManager().shutdown();
            //    cookieStore.clear();
            //}
            //httpClient = initHttpClient();

            //if (httpClientCancel != null)
            //{
            //    httpClientCancel.getConnectionManager().shutdown();
            //    httpClientCancel = null;
            //}

            //string userid = Convert.ToString(connectProps.getUser());

            //if (userid == null || userid.Length == 0)

            //{
            //    throw exceptions.getException(BaseLocalMessages.ERR_REQUIRED_PROP_NOT_SPECIFIED, new String[] { "user" }, "08001");
            //}

            //string password = Convert.ToString(connectProps.getPassword());

            //if (password == null || password.Length == 0)
            //{
            //    throw exceptions.getException(BaseLocalMessages.ERR_REQUIRED_PROP_NOT_SPECIFIED, new String[] { "password" }, "08001");
            //}

            //string databaseName = connectProps.get("DATABASENAME");

            //if (databaseName == null || databaseName.Length == 0)
            //{
            //    throw exceptions.getException(BaseLocalMessages.ERR_REQUIRED_PROP_NOT_SPECIFIED, new String[] { "databaseName" }, "08001");
            //}

            //logger = Logger.getLogger("datadirect.jdbc.ddcloud." + userid + "." + databaseName);
            //httpRequest.logger = logger;

            //if (!renewal)
            //{
            //    sessionToken = null;
            //}

            BinaryDataOutput output = httpRequest.prepareForPostRequest();
            try
            {
                //int flags;
                //string authenticationMethod;
                TokenLogin loginToken = new TokenLogin();
                loginToken.setVersion(protocolVersion = Message.CURRENT_PROTOCOL_VERSION);
                //loginToken.setDataSource( "GaConnection");
                //loginToken.setUserID("hinakoll");
                //loginToken.setPassword("Hemanth@10312");
                loginToken.setDataSource("sforceConnection");
                loginToken.setUserID("smaddira");
                loginToken.setPassword("Progress@2016");
                loginToken.setDataStoreUserID("odbc02@ddqa.com");
                loginToken.setDataStorePassword("sqlnk001");
                loginToken.setExtendedProperties(extendedProperties);

                string vendorID = VENDOR_FILE_PREFIX.Substring(1, VENDOR_FILE_PREFIX.Length - 1);
                loginToken.setVendorID(vendorID.ToUpper().Equals("##") ? null : vendorID);

                string clientId;// timezone;
                if (CLIENT_ID_PREFIX.Length > 100)
                {
                    clientId = CLIENT_ID_PREFIX.Substring(1, 100);
                }
                else
                {
                    clientId = CLIENT_ID_PREFIX.Substring(1, CLIENT_ID_PREFIX.Length - 1);
                }
                // clientId = "DD Cloud OP JDBC"; // Uncomment to test on-premise
                //loginToken.setClientID(clientId + " JDBC (" + getDriverVersion() + ")");

                //if ((timezone = connectProps.get("clientTimezone")) == null)
                //{
                //    userCalendar = Calendar.getInstance(currentTZ = TimeZone.getDefault());
                //    timezone = currentTZ.getID();

                //    if (utcTimeZones.contains(timezone.toUpperCase()))
                //    {
                //        // PSC00323514: Pre-Oracle 12 servers don't support the Universal time zone.
                //        timezone = "GMT";
                //    }
                //}
                //else
                //{
                //    clientCalendar = Calendar.getInstance(TimeZone.getTimeZone(timezone));
                //}
                //loginToken.setTimezone(timezone);
                //flags = (((authenticationMethod = connectProps.get(AUTHENTICATION_METHOD)) != null) &&
                // authenticationMethod.equalsIgnoreCase("internalProgressID")) ?
                //  (TokenLogin.HANDLE_TIMESTAMP_AS_EPOCH_MILLIS | TokenLogin.INTERNAL_CONNECT) : TokenLogin.HANDLE_TIMESTAMP_AS_EPOCH_MILLIS;
                //if (requestD2Cinfo) flags += TokenLogin.ODBC_EXTENDED_INFO;
                //loginToken.setFlags(flags);

                bool retry = true;
                reconnect:
                while (true)
                {
                    loginToken.writeAsBinary(output);

                    output.writeUnsignedByte(Token.TID_END);

                    urlBuilder.Length  = PREFIX_PATH_LENGTH;
                    urlBuilder.Append(Message.PATH_LOGIN);

                    BinaryDataInput input = httpRequest.submitPostRequest(urlBuilder.ToString(), false);
                    connectionStatus = CONNECTION_ENTACT;

                    int tokenType = input.readUnsignedByte();
                    while (tokenType != Token.TID_END)
                    {
                        switch (tokenType)
                        {
                            case Token.TID_SESSION_TOKEN:
                                sessionToken = input.readString();
                                break;
                            //case Token.TID_ERROR:
                            //    SqlException exception, firstException = null, lastException = null;
                            //    TokenError error = new TokenError(Token.TID_ERROR);

                            //    while (true)
                            //    {
                            //        error.readAsBinary(input);
                            //        //;exception = ddcloudException(error);

                            //        if (error.getErrorCode() == 0 && retry && error.getMessage().contains("Protocol"))
                            //        {
                            //            string msg = error.getMessage();
                            //            int uptoIndex = msg.IndexOf("up to ");
                            //            if (uptoIndex > -1)
                            //            {
                            //                try
                            //                {
                            //                    protocolVersion = int.Parse(msg.Substring(uptoIndex + 6, msg.Length - 1));
                            //                    loginToken.setVersion(protocolVersion);
                            //                    output = httpRequest.prepareForPostRequest();
                            //                    retry = false;
                            //                    continue reconnect;
                            //                }
                            //                catch (FormatException nfe) { }
                            //            }
                            //        }

                            //        if (firstException == null)
                            //        {
                            //            firstException = exception;
                            //        }
                            //        else
                            //        {
                            //            lastException = exception;
                            //        }

                            //        try
                            //        {
                            //            if (input.readUnsignedByte() != Token.TID_ERROR)
                            //                break;
                            //        }
                            //        catch (Exception e)
                            //        {
                            //            break;
                            //        }

                            //        lastException = exception;
                            //    }

                            //    throw firstException;
                            //case Token.TID_WARN:
                            //    processWarning(input);
                            //    break;
                            //case Token.TID_DB_INFO:
                            //    dbInfo.setODBCinfo(requestD2Cinfo);
                            //    dbInfo.readAsBinary(input);
                            //    D2Cinfo = dbInfo.getODBCinfo();
                            //    break;
                            default:
                                //throw exceptions.getException(DDCloudLocalMessages.DDCLOUD_UNEXPECTED_RESPONSE_TYPE);
                                throw new Exception("DDCLOUD_UNEXPECTED_RESPONSE_TYPE");
                        }
                        tokenType = input.readUnsignedByte();
                    }
                    break;
                }

            }
            catch (IOException ioException)
            {
                throw ioException;
            }
        }

      //  public HttpClient initHttpClient()
      //  {
      //      HttpClient httpClient = new HttpClient();

      //      string proxyHost = "";
      //      string proxyPort = "";

      //      if ((proxyHost != null && proxyHost.Length != 0) && (proxyPort != null && proxyPort.Length != 0))
      //      {
      //          int port = int.Parse(proxyPort);

      //          string proxyUser = "";
      //          string proxyPassword = "";

      //          httpClient.getCredentialsProvider().setCredentials(new AuthScope(proxyHost, port), new UsernamePasswordCredentials(proxyUser, proxyPassword));
      //          HttpHost proxy = new HttpHost(proxyHost, port);
      //          httpClient.getParams().setParameter(ConnRoutePNames.DEFAULT_PROXY, proxy);
      //      }

      //      HttpParams params = httpClient.getParams();
      //      HttpConnectionParams.setConnectionTimeout(params, wsTimeout);
      //      HttpConnectionParams.setSoTimeout(params, wsTimeout);
		    //params.setParameter(ClientPNames.COOKIE_POLICY, CookiePolicy.BROWSER_COMPATIBILITY);

      //      // Share the cookie store among all HttpClients, so all
      //      // use the same session affinity cookie.
      //      httpClient.setCookieStore(cookieStore);

      //      return httpClient;
      //  }

        //private string getDriverVersion()
        //{
        //    string driverName = connection.driverName.toLowerCase();
        //    int major = getVersionInternal("driverMajorVersion", driverName);
        //    int minor = getVersionInternal("driverMinorVersion", driverName);
        //    int packN = getVersionInternal("servicePackNumber", driverName);

        //    string defaultId = "??";
        //    string nativeLayerId = defaultId;
        //    string componentVersionInfo = "";
        //    try
        //    {
        //        nativeLayerId = new UtilResource(connection.implConnection.getClass(),
        //            connection.driverName.toLowerCase() + ".properties").getAsProperties().getProperty("buildid", defaultId);
        //    }
        //    catch (Exception e) { }

        //    return major + "." + minor + "." + packN + "." + nativeLayerId + componentVersionInfo;
        //}

        //private int getVersionInternal(string which, string rootName)
        //{

        //    try
        //    {
        //        string version;

        //        version = new UtilResource(this.getClass(), rootName + ".properties").getAsProperties().getProperty(which, "");

        //        if (version.Length == 0)
        //        {
        //            version = new UtilResource(Class.forName("com.ddtek.jdbc.base.BaseDatabaseMetaData"), "base.properties").getAsProperties().getProperty(which, "");
        //        }

        //        return int.Parse(version);
        //    }
        //    catch (Exception e) { }

        //    return -1;
        //}

        /*
        @Override
            protected String getCatalog() throws SQLException
        {
                return dbInfo.getStringInfo (TokenDatabaseInfo.DBINFO_STR_CATALOG_NAME);
        }

        @Override
            public void close() throws SQLException
        {

                try {
                if (sessionToken == null)
                {

                    // If there is no session token don't bother sending out the
                    // logout request
                    // because the DAS will just reject it with an invalid session
                    // token
                    // error.
                    return;
                }

                urlBuilder.setLength(PREFIX_PATH_LENGTH);
                urlBuilder.append(Message.PATH_LOGOUT);

                BinaryDataInput input = httpRequest.submitGetRequest(urlBuilder.toString(), true, false);

                try
                {
                    int tokenType = input.readUnsignedByte();
                    while (tokenType != Token.TID_END)
                    {
                        switch (tokenType)
                        {
                            case Token.TID_ERROR:
                                throw ddcloudException(input, null);
                            case Token.TID_WARN:
                                processWarning(input);
                                break;
                            default:
                                throw exceptions.getException(DDCloudLocalMessages.DDCLOUD_UNEXPECTED_RESPONSE_TYPE);
                        }
                        tokenType = input.readUnsignedByte();
                    }
                }
                catch (IOException ioException)
                {
                    if (logger.isLoggable(Level.CONFIG))
                    {
                        logger.logp(Level.CONFIG, logger.getName(), sessionToken, "IOException: " + ioException.getMessage(), ioException);
                    }
                }

                httpClient.getConnectionManager().shutdown();
                if (httpClientCancel != null)
                {
                    httpClientCancel.getConnectionManager().shutdown();
                    httpClientCancel = null;
                }
            }
                finally {
                sessionToken = null;
            }
        }

        @Override
            protected void startManualTransactionMode() throws SQLException
        {
                if (transactionMode == TX_IGNORE) {
                return;
            }

                throw exceptions.getException (DDCloudLocalMessages.DDCLOUD_TX_NOT_ENABLED, "HYC00");
        }

        @Override
            protected void rollbackTransaction() throws SQLException
        {
                if (transactionMode == TX_IGNORE) {
                throw exceptions.getException(DDCloudLocalMessages.DDCLOUD_TX_NO_ROLLBACK_IN_IGNORE_MODE, "25000");
            }

        }

        @Override
            public boolean supportsQueryTimeout()
        {
            return true;
        }

        @Override
            protected void commitTransaction() throws SQLException
        {

        }

        @Override
            protected void stopManualTransactionMode() throws SQLException
        {

        }

        @Override
            protected BaseImplStatement createImplStatement(int resultSetType, int resultSetConcurrency) throws SQLException
        {
                return new DDCloudImplStatement(this);
            }

            int getNextId()
        {
            return ++stmtId;
        }

        String getBaseURL()
        {
            return cachedBaseURL;
        }

        SQLException ioException(IOException ioException, boolean login)
        {
            if (logger.isLoggable(Level.CONFIG))
            {
                logger.logp(Level.CONFIG, logger.getName(), sessionToken, "IOException: " + ioException.getMessage(), ioException);
            }
            if (ioException instanceof SocketTimeoutException || ioException instanceof ConnectTimeoutException || ioException instanceof SocketException) {
                if (login)
                {
                    return exceptions.getException(ioException, SQLSTATE_CONNECTION_TIMEOUT);
                }
                return exceptions.getException(ioException, SQLSTATE_TIMEOUT);
            }
                else if (ioException instanceof UnknownHostException) {
                ioException = new IOException("Unknown host:" + ioException.getMessage());
            }
                else if (ioException instanceof NoHttpResponseException) {
                return exceptions.getException(ioException, SQLSTATE_COMMUNICATION_LINK_FAILURE);
            }

            return exceptions.getException(ioException);
        }

        SQLException utilException(UtilException exception)
        {
            if (logger.isLoggable(Level.CONFIG))
            {
                logger.logp(Level.CONFIG, logger.getName(), sessionToken, "UtilException: " + exception.getMessage(), exception);
            }
            return exceptions.getException(exception);
        }

        private static String buildLogMessage(TokenError token)
        {

            StringBuilder builder = new StringBuilder(256);
            builder.append("ERROR - CODE: ");
            builder.append(token.getErrorCode());
            builder.append(" SQLSTATE: ");
            builder.append(token.getSqlState());
            builder.append(" ORIGIN: ");
            builder.append(TokenError.mapOriginToString(token.getOrigin()));
            builder.append(" ROW: ");
            builder.append(token.getRowNumber());
            builder.append(" COLUMN: ");
            builder.append(token.getColumnNumber());
            builder.append(" MESSAGE: ");
            builder.append(token.getMessage());

            return builder.toString();
        }

        SQLException ddcloudException(BinaryDataInput input, DDCloudRequest cloudRequest)

                    throws IOException
        {
            SQLException exception, firstException = null, lastException = null;
            TokenError error = new TokenError(Token.TID_ERROR);

                while (true) {
                    error.readAsBinary(input);
                    exception = ddcloudException(error);

                    if (firstException == null) {
                        firstException = exception;
                    }
                    else {
                        lastException.setNextException(exception);
                    }

                    try {
                        int tokenType;

                        if ((tokenType = input.readUnsignedByte()) != Token.TID_ERROR) {
                            // Do we need to save the tokenType?
                            if ((tokenType == Token.TID_END) && (cloudRequest != null)) {
                                cloudRequest.setProcessingDone ();
                            }
                            break;
                        }
                    }
                    catch (Throwable e) {
                        break;
                    }

                    lastException = exception;
                }

                return firstException;
            }

            private static final String DDCLOUD_SERVER_NAME = "Service";
            private static final String DDCLOUD_SERVER_INTERNAL = "Service";

            SQLException ddcloudException(TokenError error)
        {
            if (logger.isLoggable(Level.CONFIG))
            {
                String logMessage = buildLogMessage(error);
                logger.logp(Level.CONFIG, logger.getName(), sessionToken, logMessage);
            }

            String originString = getOriginString(error);
            String msg = error.getMessage();
            String sqlState = error.getSqlState();

            if ((msg == null) || (sqlState == null) || (originString == null))
            {
                File tmpFile;
                FileOutputStream fos;
                PrintStream prts;

                try
                {
                    tmpFile = File.createTempFile("ddcloud", ".log");

                    fos = new FileOutputStream(tmpFile, true);

                    prts = new PrintStream(fos);

                    prts.print("\t");
                    prts.println(new Date());

                    prts.print("message:  ");
                    prts.println((msg == null) ? "<null>" : msg);

                    prts.print("SQLState: ");
                    prts.println((sqlState == null) ? "<null>" : sqlState);

                    prts.print("Origin:   ");
                    if (originString == null)
                        prts.println(error.getOrigin());
                    else
                        prts.println(originString);

                    prts.print("errCode:  ");
                    prts.println(error.getErrorCode());

                    prts.flush();
                    prts.close();
                }
                catch (FileNotFoundException e)
                {
                    ;
                }
                catch (IOException e)
                {
                    ;
                }

                if (msg == null)
                {
                    msg = "No error message";
                }
                if (sqlState == null)
                {
                    sqlState = "HY000";
                }
                if (originString == null)
                {
                    originString = DDCLOUD_SERVER_INTERNAL;
                }
            }

            return exceptions.getServerException(msg, sqlState, error.getErrorCode(), originString);
        }

        BatchUpdateException ddcloudException(TokenError error, int[] rows)
        {
            if (logger.isLoggable(Level.CONFIG))
            {
                String logMessage = buildLogMessage(error);
                logger.logp(Level.CONFIG, logger.getName(), sessionToken, logMessage);
            }

            String originString = getOriginString(error);

            SQLException serverException = exceptions.getServerException(error.getMessage(), error.getSqlState(), error.getErrorCode(), originString);
            return new BatchUpdateException(serverException.getMessage(), serverException.getSQLState(), serverException.getErrorCode(), rows);
        }

        private String getOriginString(TokenError error)
        {
            String productName;
            String originString;
            switch (error.getOrigin())
            {
                case TokenError.ORIGIN_DRIVER:
                    originString = dbInfo.getStringInfo(TokenDatabaseInfo.DBINFO_STR_DATABASE_PRODUCT_NAME);
                    break;
                case TokenError.ORIGIN_DB_SERVER:
                    productName = dbInfo.getStringInfo(TokenDatabaseInfo.DBINFO_STR_DATABASE_PRODUCT_NAME);
                    if (productName == null)
                    {
                        originString = DDCLOUD_SERVER_NAME;
                    }
                    else
                    {
                        originString = productName;
                    }
                    break;
                default:
                case TokenError.ORIGIN_SERVICE:
                    originString = DDCLOUD_SERVER_NAME;
                    break;
                case TokenError.ORIGIN_INTERNAL:
                    originString = DDCLOUD_SERVER_INTERNAL;
                    break;
            }
            return originString;
        }

        @Override
            public BaseData createDataInstance(int type, BaseDataMetaData metaData)
        {
            int precision, scale;

            switch (type)
            {
                case DDCloudData.DDCLOUD_BYTE:
                    return new DDCloudDataByte(connection);
                case DDCloudData.DDCLOUD_INTEGER:
                    return new DDCloudDataInteger(connection);
                case DDCloudData.DDCLOUD_SHORT:
                    return new DDCloudDataShort(connection);
                case DDCloudData.DDCLOUD_UNSIGNED_BYTE:
                    return new DDCloudDataUnsignedByte(connection);
                case DDCloudData.DDCLOUD_LONG:
                    return new DDCloudDataLong(connection);
                case DDCloudData.DDCLOUD_STRING:
                    if ((precision = metaData.getPrecision()) <= varcharToLongThreshold)
                    {
                        return new DDCloudDataString(connection, precision, transliterator);
                    }
                case DDCloudData.DDCLOUD_LONG_STRING:
                    return new DDCloudDataLongString(connection, this, transliterator);
                case DDCloudData.DDCLOUD_BOOLEAN:
                    return new DDCloudDataBoolean(connection);
                case DDCloudData.DDCLOUD_DOUBLE:
                    return new DDCloudDataDouble(connection);
                case DDCloudData.DDCLOUD_FLOAT:
                    return new DDCloudDataFloat(connection);
                case DDCloudData.DDCLOUD_DECIMAL:
                    precision = metaData.getPrecision();
                    scale = metaData.getScale();
                    return new DDCloudDataDecimal(connection, this, DDCloudDataDecimal.getLength(precision, scale), precision, scale);
                case DDCloudData.DDCLOUD_TIME:
                    return new DDCloudDataTime(connection);
                case DDCloudData.DDCLOUD_DATE:
                    return new DDCloudDataDate(connection);
                case DDCloudData.DDCLOUD_TIMESTAMP:
                    return new DDCloudDataTimestamp(connection, metaData.getScale());
                case DDCloudData.DDCLOUD_TIMESTAMP_UTC:
                    return new DDCloudDataTimestampUTC(connection, metaData.getScale());
                case DDCloudData.DDCLOUD_BINARY:
                    if ((precision = metaData.getPrecision()) <= varbinaryToLongThreshold)
                    {
                        return new DDCloudDataBinary(connection, precision);
                    }
                case DDCloudData.DDCLOUD_LONG_BINARY:
                    return new DDCloudDataLongBinary(connection, this, transliterator);
            }
            return super.createDataInstance(type, metaData);
        }

        public static final String DEFAULT_LOG_CONFIG_FILE = "ddcloudlogging.properties";
            public static final String OPTION_LOG_CONFIG_FILE = "logconfigfile";

            static boolean loggingInitialized = false;

        static synchronized void initializeLogging()
        {

            if (!loggingInitialized)
            {
                File configFile = new File(DEFAULT_LOG_CONFIG_FILE);
                boolean exists = configFile.exists();

                if (exists)
                {
                    // Retain the full pathname for the file for reporting in
                    // SYSTEM_SESSIONINFO
                    // String logConfigFileName = configFile.getAbsolutePath();
                    LogManager logManager = LogManager.getLogManager();
                    FileInputStream inputStream;
                    try
                    {
                        inputStream = new FileInputStream(configFile);
                        logManager.readConfiguration(inputStream);
                        inputStream.close();
                    }
                    catch (FileNotFoundException e)
                    {
                        logInitalizeException(configFile, e);
                    }
                    catch (SecurityException e)
                    {
                        logInitalizeException(configFile, e);
                    }
                    catch (IOException e)
                    {
                        logInitalizeException(configFile, e);
                    }
                }

                // No matter what happened, mark the logging initialized. We only
                // want to do this once.
                loggingInitialized = true;
            }
        }

        public static String initializeLogging(String userid, String databaseName)
        {

            initializeLogging();

            // Build the name of the spy logger.
            // The actual logger name for spy will have the database name at the
            // very end.
            String loggerName = "datadirect.jdbc.spy." + userid + "." + databaseName;

            Logger spyLog = Logger.getLogger(loggerName);

            // We enable spy logging at INFO currently;
            if (spyLog.isLoggable(Level.FINER))
            {
                StringBuilder builder = new StringBuilder();
                builder.append("(log=(generic)com.ddtek.jdbc.ddcloud.DDCloudCommonLoggingWriter:");
                builder.append(loggerName);
                // if DEBUG is on, then include Spy option to include logging input
                // streams
                if (spyLog.isLoggable(Level.FINEST))
                {
                    builder.append(";logIS=yes");
                }
                builder.append(")");
                return builder.toString();
            }
            return null;
        }

        private static void logInitalizeException(File configFile, Exception exception)
        {
            Logger logger = Logger.getLogger("datadirect.jdbc.ddcloud");
            logger.config("Failed to initialize logging from file: " + configFile.getAbsolutePath() + " Exception message: " + exception.getMessage());
        }
        */
        string getConnectPropAsString(string optionName, string defaultValue)
        {
            string valueAsString = optionName;//connectProps.get(optionName);
            if (valueAsString == null || valueAsString.Length == 0)
            {
                return defaultValue;
            }
            return valueAsString;
        }

    public void processWarning(BinaryDataInput input)
    {
		int errorCode = input.readCompressedInt ();
        input.readCompressedInt (); // origin
        input.readCompressedInt (); // rownumber
        input.readCompressedInt (); // columnnumber
        string sqlState = input.readString ();
        string message = input.readString ();

        //warnings.add (BaseLocalMessages.EMPTY_1_ARG_MESSAGE, new string[] { message }, sqlState, errorCode);
	}

	//public Calendar getCalendar()
 //   {

 //       if (clientCalendar != null)
 //       {
 //           // If the clientTimezone connection option was specified,
 //           // always return a Calendar corresponding to it.
 //           return clientCalendar;
 //       }

 //       TimeZone defaultTZ = TimeZone.getDefault();

 //       if (!defaultTZ.equals(currentTZ))
 //       {
 //           userCalendar = Calendar.getInstance(currentTZ = defaultTZ);
 //       }

 //       return userCalendar;
 //   }

    public HttpClient getHttpClient()
    {
        return httpClient;
    }

    public HttpClient getHttpClientCancel()
    {
        if (httpClientCancel == null)
        {
            //httpClientCancel = initHttpClient();
        }
        return httpClientCancel;
    }

    public int[] getD2CInfo() 
    {
        return D2Cinfo;
    }

        //protected override BaseImplDatabaseMetaData createImplDatabaseMetaData()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override void getImplPropertyInfo(BaseDriverPropertyInfos driverPropInfos)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void open()
        //{
        //    throw new NotImplementedException();
        //}

        public override void close()
        {
            throw new NotImplementedException();
        }

        protected override void startManualTransactionMode()
        {
            throw new NotImplementedException();
        }

        protected override void rollbackTransaction()
        {
            throw new NotImplementedException();
        }

        protected override void commitTransaction()
        {
            throw new NotImplementedException();
        }

        protected override void stopManualTransactionMode()
        {
            throw new NotImplementedException();
        }

        //protected override BaseImplStatement createImplStatement(int resultSetType, int resultSetConcurrency)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
