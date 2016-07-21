using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Common
{
    public class TokenLogin : Token
    {

        /**
         * The JDBC client should pass this bit to indicate that timestamp values should be returned as the number of millis
         * since the epoch. ODBC client should not set this bit to assure timestamps are decomposed into its year, month,
         * day, hour, minute, second values.
         */
        public static readonly int HANDLE_TIMESTAMP_AS_EPOCH_MILLIS = 0x1;
        /**
         * Special ProgressID connection option for WebUI. When set, the password is checked against the password in the
         * DataStore table with the ProgressID (user name).
         */
        public static readonly int INTERNAL_CONNECT = 0x2;
        /**
         * Is there a String with a list of extended properties after the flags value? While intended for an on-premise DAS,
         * this may also be useful for the D2C DAS. Format: <property name>="<property value>" [; ...]
         */
        public static readonly int EXTENDED_PROPERTIES = 0x4;
        /**
         * Should the login return additional info to support ODBC's SQLGetInfo?
         */
        public static readonly int ODBC_EXTENDED_INFO = 0x8;
        /**
         * Slow fetching data by sleeping for a second before every
         * ResultSet.next operation, to make testing cancel easier.
         */
        //public static readonly int CANCEL_TESTING = 0x80000000;

        private int version;
        private String dataSource;
        private String userID;
        private String password;
        private String dataStoreUserID;
        private String dataStorePassword;
        private String clientID;
        private String vendorID;
        private int flags;
        private String extendedProperties;

        // This field is used internally by the DAS and is not sent over the wire. As such it
        // doesn't need to be marshalled in or out in the methods writeAsBinary and readAsBinary
        private long loginRecordId = -1;

        /*
         * The amount of time in milliseconds to add to UTC to get standard time in this time zone. May have 'D' on the end
         * of the string to indicate whether or not daylight savings time is used in this time zone. If not null, used to
         * validate the timezone provided.
         */

        private String timezoneOffset;

        private String timezone;

        public TokenLogin() : base(TID_LOGIN)
        { }



        public override void writeAsBinary(BinaryDataOutput output)
        {
            output.writeUnsignedByte(tokenType);
            output.writeCompressedInt(version);
            output.writeString(dataSource);
            output.writeString(userID);
            output.writeString(password);
            output.writeString(dataStoreUserID);
            output.writeString(dataStorePassword);
            output.writeString(clientID);
            output.writeString(timezone);
            output.writeString(timezoneOffset);

            if (extendedProperties != null) {
                flags |= EXTENDED_PROPERTIES;
            }
            else {
                flags &= ~EXTENDED_PROPERTIES;
            }

            output.writeCompressedInt(flags);

            if (extendedProperties != null) {
                output.writeString(extendedProperties);
            }

            output.writeString(vendorID);
        }


        public override void readAsBinary(BinaryDataInput input) 
        {
            version = input.readCompressedInt ();
		if (version > Message.CURRENT_PROTOCOL_VERSION || version < Message.REQUIRED_PROTOCOL_VERSION) {
                throw new Exception("Protocol mismatch.  Driver Protocol Version: " + version + ".  Service expects: " + Message.REQUIRED_PROTOCOL_VERSION + " up to "
                        + Message.CURRENT_PROTOCOL_VERSION + ".");
            }
            dataSource = input.readString ();
            userID = input.readString ();
            password = input.readString ();
            dataStoreUserID = input.readString ();
            dataStorePassword = input.readString ();
            clientID = input.readString ();
            timezone = input.readString ();
            timezoneOffset = input.readString ();
            flags = input.readCompressedInt ();

		if ((flags & EXTENDED_PROPERTIES) != 0) {
                extendedProperties = input.readString();
            }

		// Vendor ID branding
		if (version >= 8) {
                vendorID = input.readString();
            }
        }

        
    public void describe(LogBuilder builder)
        {
            base.describe(builder);
            builder.addLogPair("dataSource", dataSource);
            builder.addLogPair("userID", userID);
            builder.addLogPair("password", password != null ? "<specified>" : "<null>");
            if (dataStoreUserID != null && dataStoreUserID.Length > 0)
            {
                builder.addLogPair("dataStoreUserID", dataStoreUserID);
            }
            if (dataStorePassword != null && dataStorePassword.Length > 0)
            {
                builder.addLogPair("dataStorePassword", "<specified>");
            }
            builder.addLogPair("clientID", clientID);
            if (timezone != null)
            {
                builder.addLogPair("timezone", timezone);
            }
            if (timezoneOffset != null)
            {
                builder.addLogPair("timezoneOffset", timezoneOffset);
            }
            builder.addLogPair("flags", flags);
        }

        public String getDataSource()
        {
            return dataSource;
        }

        public void setDataSource(String dataSource)
        {
            this.dataSource = dataSource;
        }

        public String getUserID()
        {
            return userID;
        }

        public void setUserID(String userID)
        {
            this.userID = userID;
        }

        public String getVendorID()
        {
            return vendorID;
        }

        public void setVendorID(String vendorID)
        {
            this.vendorID = vendorID;
        }

        public String getPassword()
        {
            return password;
        }

        public void setPassword(String password)
        {
            this.password = password;
        }

        /**
         * @return the timezone
         */
        public String getTimezone()
        {
            return timezone;
        }

        /**
         * @param timezone
         *            the timezone to set
         */
        public void setTimezone(String timezone)
        {
            this.timezone = timezone;
        }

        /**
         * @return the timezoneOffset
         */
        public String getTimezoneOffset()
        {
            return timezoneOffset;
        }

        /**
         * @param timezone
         *            the timezone to set
         */
        public void setTimezoneOffset(String timezoneOffset)
        {
            this.timezoneOffset = timezoneOffset;
        }

        /**
         * @return the version
         */
        public int getVersion()
        {
            return version;
        }

        /**
         * @param version
         *            the version to set
         */
        public void setVersion(int version)
        {
            this.version = version;
        }

        /**
         * @return the dataStoreUserID
         */
        public String getDataStoreUserID()
        {
            return dataStoreUserID;
        }

        /**
         * @param dataStoreUserID
         *            the dataStoreUserID to set
         */
        public void setDataStoreUserID(String dataStoreUserID)
        {
            this.dataStoreUserID = dataStoreUserID;
        }

        /**
         * @return the dataStorePassword
         */
        public String getDataStorePassword()
        {
            return dataStorePassword;
        }

        /**
         * @param dataStorePassword
         *            the dataStorePassword to set
         */
        public void setDataStorePassword(String dataStorePassword)
        {
            this.dataStorePassword = dataStorePassword;
        }

        /**
         * @return the clientID
         */
        public String getClientID()
        {
            return clientID;
        }

        /**
         * @param clientID
         *            the clientID to set
         */
        public void setClientID(String clientID)
        {
            this.clientID = clientID;
        }

        public int getFlags()
        {
            return flags;
        }

        public void setFlags(int flags)
        {
            this.flags = flags;
        }

        public String getExtendedProperties()
        {
            return extendedProperties;
        }

        public void setExtendedProperties(String props)
        {
            extendedProperties = props;
        }

        public bool isFlagSet(int flag)
        {
            return ((flags & flag) != 0);
        }

        public long getLoginRecordId()
        {
            return loginRecordId;
        }

        public void setLoginRecordId(long loginRecordId)
        {
            this.loginRecordId = loginRecordId;
        }
    }
}
