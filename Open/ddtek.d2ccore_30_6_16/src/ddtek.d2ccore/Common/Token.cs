using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Common
{
    public abstract class Token
    {
        public const int TID_SESSION_TOKEN = 0x01;
        public const int TID_LOGOUT = 0x02;
        public const int TID_DB_INFO = 0x03;

        //public  static int TID_DATABASE_METADATA = 0x04;
        public const int TID_ERROR = 0x05;
        public const int TID_WARN = 0x06;
        public const int TID_ROW = 0x07;
        public const int TID_RESULT_SET_END = 0x08;

        //public final static int TID_EXECUTE_WITH_PARAMS_END = 0x09;
        public static int TID_RESULT_SET = 0x0A;
        public static int TID_ROWCOUNT = 0x0B;
        public static int TID_CONTINUATION = 0x0C;
        public static int TID_EXECUTE = 0x0D;
        //public final static int TID_EXCUTE_WITH_PARAMS = 0x0E;
        public static int TID_RS_MORE = 0x0F;
        public static int TID_LOGIN = 0x10;

        public static int TID_DBMD_TABLES = 0x11;
        public static int TID_DBMD_COLUMNS = 0x12;
        public static int TID_DBMD_TYPE_INFO = 0x13;
        public static int TID_DBMD_SCHEMAS = 0x14;
        public static int TID_DBMD_CATALOGS = 0x15;
        public static int TID_DBMD_ATTRIBUTES = 0x16;
        public static int TID_DBMD_BEST_ROW_IDENTIFIER = 0x17;
        public static int TID_DBMD_CLIENT_INFO_PROPERTIES = 0x18;
        public static int TID_DBMD_CROSS_REFERENCE = 0x19;
        public static int TID_DBMD_EXPORTED_KEYS = 0x1A;
        public static int TID_DBMD_FUNCTION_COLUMNS = 0x1B;
        public static int TID_DBMD_FUNCTIONS = 0x1C;
        public static int TID_DBMD_IMPORTED_KEYS = 0x1D;
        public static int TID_DBMD_INDEX_INFO = 0x1E;
        public static int TID_DBMD_PRIMARY_KEYS = 0x1F;
        public static int TID_DBMD_PROCEDURE_COLUMNS = 0x20;
        public static int TID_DBMD_PROCEDURES = 0x21;
        public static int TID_DBMD_SUPER_TABLES = 0x22;
        public static int TID_DBMD_SUPER_TYPES = 0x23;
        public static int TID_DBMD_TABLE_PRIVILEGES = 0x24;
        public static int TID_DBMD_TABLE_TYPES = 0x25;
        public static int TID_DBMD_UDTS = 0x26;
        public static int TID_DBMD_VERSION_COLUMNS = 0x27;
        public static int TID_DBMD_COLUMN_PRIVILEGES = 0x28;

        public static int TID_PREPARED_EXECUTE = 0x30;
        public static int TID_DESCRIBE_PARAMS_AND_RESULT_SET = 0x31;
        public static int TID_PARAM_DESCRIPTIONS = 0x32;
        public static int TID_AUTO_GEN_KEYS = 0x33;
        public static int TID_EXECUTE_PARAM_BATCH = 0x34;
        public static int TID_PARAM_SET = 0x35;
        public static int TID_PARAM_SET_END = 0x36;
        public static int TID_BATCH_ROW_COUNTS = 0x37;

        public static int TID_END = 0xFF;

        public static int NUM_TOKEN_TYPES = 256;

        public static int TIMESTAMP_UTC = 2100;

        private static string[] tokenTypeToString;

        /**
         * Constants used to control how and whether auto generated keys are returned from the Execute operations.
         */
        public static int AUTO_GEN_NONE = 0x00;
        public static int AUTO_GEN_ON = 0x01;
        public static int AUTO_GEN_SPECIFIED_ORDINALS = 0x02;
        public static int AUTO_GEN_SPECIFIED_NAMES = 0x03;


        public readonly int tokenType;

        public Token()
        {

        }
        static Token()
        {
            tokenTypeToString = new string[NUM_TOKEN_TYPES];

            tokenTypeToString[TID_SESSION_TOKEN] = "Session Token";
            tokenTypeToString[TID_LOGOUT] = "Logout";
            tokenTypeToString[TID_DB_INFO] = "DB Info";
            tokenTypeToString[TID_ERROR] = "Error";
            tokenTypeToString[TID_WARN] = "Warning";
            tokenTypeToString[TID_ROW] = "Row";
            tokenTypeToString[TID_RESULT_SET_END] = "Result Set End";
            tokenTypeToString[TID_RESULT_SET] = "Result Set";
            tokenTypeToString[TID_ROWCOUNT] = "Row Count";

            tokenTypeToString[TID_CONTINUATION] = "Continuation";
            tokenTypeToString[TID_EXECUTE] = "Execute";
            tokenTypeToString[TID_RS_MORE] = "Result Set More";
            tokenTypeToString[TID_LOGIN] = "Login";

            tokenTypeToString[TID_DBMD_TABLES] = "DBMD Tables";
            tokenTypeToString[TID_DBMD_COLUMNS] = "DBMD Columns";
            tokenTypeToString[TID_DBMD_TYPE_INFO] = "DBMD Type Info";
            tokenTypeToString[TID_DBMD_SCHEMAS] = "DBMD Schema";
            tokenTypeToString[TID_DBMD_CATALOGS] = "DBMD Catalogs";
            tokenTypeToString[TID_DBMD_ATTRIBUTES] = "DBMD Attributes";
            tokenTypeToString[TID_DBMD_BEST_ROW_IDENTIFIER] = "DBMD Best Row Identifier";
            tokenTypeToString[TID_DBMD_CLIENT_INFO_PROPERTIES] = "DBMD Client Info Properties";
            tokenTypeToString[TID_DBMD_CROSS_REFERENCE] = "DBMD Cross Reference";
            tokenTypeToString[TID_DBMD_EXPORTED_KEYS] = "DBMD Exported Keys";
            tokenTypeToString[TID_DBMD_FUNCTION_COLUMNS] = "DBMD Function Columns";
            tokenTypeToString[TID_DBMD_FUNCTIONS] = "DBMD Functions";
            tokenTypeToString[TID_DBMD_IMPORTED_KEYS] = "DBMD Imported Keys";
            tokenTypeToString[TID_DBMD_INDEX_INFO] = "DBMD Index Info";
            tokenTypeToString[TID_DBMD_PRIMARY_KEYS] = "DBMD Primary Keys";
            tokenTypeToString[TID_DBMD_PROCEDURE_COLUMNS] = "DBMD Procedure Columns";
            tokenTypeToString[TID_DBMD_PROCEDURES] = "DBMD Procedures";
            tokenTypeToString[TID_DBMD_SUPER_TABLES] = "DBMD Super Tables";
            tokenTypeToString[TID_DBMD_SUPER_TYPES] = "DBMD Super Types";
            tokenTypeToString[TID_DBMD_TABLE_PRIVILEGES] = "DBMD Table Privileges";
            tokenTypeToString[TID_DBMD_TABLE_TYPES] = "DBMD Table Types";
            tokenTypeToString[TID_DBMD_UDTS] = "DBMD UDTS";
            tokenTypeToString[TID_DBMD_VERSION_COLUMNS] = "DBMD Version Columns";
            tokenTypeToString[TID_DBMD_COLUMN_PRIVILEGES] = "DBMD Column Privileges";

            tokenTypeToString[TID_PREPARED_EXECUTE] = "Prepared Execute";
            tokenTypeToString[TID_DESCRIBE_PARAMS_AND_RESULT_SET] = "Describe Params and Result Set";
            tokenTypeToString[TID_PARAM_DESCRIPTIONS] = "Parameter Descriptions";
            tokenTypeToString[TID_AUTO_GEN_KEYS] = "Auto Generated Keys";
            tokenTypeToString[TID_EXECUTE_PARAM_BATCH] = "Execute Parameter Batch";
            tokenTypeToString[TID_PARAM_SET] = "Parameter Set";
            tokenTypeToString[TID_PARAM_SET_END] = "Parameter Set End";

            tokenTypeToString[TID_END] = "End";
        }


        protected Token(int tokenType)
        {
            this.tokenType = tokenType;
        }

        public abstract void writeAsBinary(BinaryDataOutput output);
        public abstract void readAsBinary(BinaryDataInput input);

        public void describe(LogBuilder builder)
        {
            builder.addLogPair("token", TokenTypeToString(tokenType));

        }

        public static string mapAutoGenModeToString(int mode)
        {
            if (mode == AUTO_GEN_NONE)
            {
                return "NONE";
            }
            if (mode == AUTO_GEN_ON)
            {
                return "ON";
            }
            if (mode == AUTO_GEN_SPECIFIED_ORDINALS)
            {
                return "SPECIFIED BY ORDINALS";
            }
            if (mode == AUTO_GEN_SPECIFIED_NAMES)
            {
                return "SPECIFIED BY NAMES";
            }
            return "UNKNOWN";
        }

        public static string TokenTypeToString(int tokenType)
        {
            if (tokenType < 0 || tokenType >= NUM_TOKEN_TYPES)
            {
                tokenType = 0;
            }
            string name = tokenTypeToString[tokenType];
            if (name == null)
            {
                return "0x" + tokenType.ToString("X4");
            }
            return name;
        }

        public int getTokenType()
        {
            return tokenType;
        }
    }
}
