using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Common
{
    public class Message
    {
        public static int CURRENT_PROTOCOL_VERSION = 9;
        public static int REQUIRED_PROTOCOL_VERSION = 6;

        List<Token> tokens = new List<Token>();

        public  static int NO_COMPRESSION = 0;
        public  static int DEFLATE_COMPRESSION = 1;
        public  static int GZIP_COMPRESSION = 2;

        public static  string CONTENT_BINARY = "application/octet-stream";
	public static  string CONTENT_DEFLATE_COMPRESSION = "deflate";
	public static  string CONTENT_GZIP_COMPRESSION = "gzip";
	public static  string HEADER_AUTHORIZATION = "Authorization";
	public static  string HEADER_AUTH_TOKEN = "X-DataDirect-Easyl-AuthToken";
	public static  string HEADER_CONNECTOR_ID = "X-DataDirect-OPAS-Hostid";
	public static  string HEADER_HOST = "host";
	public static  string HEADER_OP_HOST_INFO = "OPHostInfo";
	public static  string HEADER_VERSION = "Version";
	public static  string HEADER_DEVICE_DESCRIPTION = "deviceDescription";
	public static  string HEADER_DEVICE_NAME = "deviceName";
	public static  string HEADER_LABEL = "label";

	public static  string PATH_CLEAR 			= "/clear";
	public static  string CLEAR_RESULTS		= "clearResults";
	public static  string PATH_CANCEL 			= "/cancel";
	public static  string PATH_UPLOAD 			= "/upload";
	public static  string PATH_LOGOUT 			= "/logout";
	public static  string PATH_LOGIN 			= "/login";
	public static  string PATH_EXECUTE 		= "/execute";
	public static  string PATH_CONTINUE 		= "/continue";
	public static  string PATH_REMOVE_SESSION		= "/removesession";
	public static  string PATH_AUTH			= "/auth";
	public static  string PATH_DEVICES_POST		= "/devices/";
	public static  string PATH_VERSION_INFO		= "/versioninfo";

	public  static string SQLSTATE_COMMUNICATION_LINK_FAILURE = "08S01";
        public  static string SQLSTATE_GENERAL_ERROR = "HY000";
        public  static string SQLSTATE_TIMEOUT = "HYT00";
        public  static string SQLSTATE_CONNECTION_TIMEOUT = "HYT01";
        public  static string SQLSTATE_INVALID_AUTHORIZATION = "28000";
        public  static string SQLSTATE_INTEGRITY_CONSTRAINT_VIOLATION = "23000";
        public  static string SQLSTATE_SERVER_REJECTED_CONNECTION = "08004";
        public  static string SQLSTATE_NUMERIC_VALUE_OUT_OF_RANGE = "22003";
        public  static string SQLSTATE_String_DATA_TRUNCATED = "22001";
        public  static string SQLSTATE_GENERAL_WARNING = "01000";

        public static  string URI_PARAM_STATEMENT_ID = "stmt_id";
	public static  string URI_PARAM_EXECUTE_ID  = "exec_id";
	public static  string URI_PARAM_STREAM_ID   = "stream_id";
	public static  string URI_PARAM_SEQUENCE_ID = "seq_id";
	public static  string URI_PARAM_MORE        = "more";

	private static  string[] messageTypeToString;

	public int messageType;

        public  static int MID_LOGIN_REQUEST = 0x01;
        public  static int MID_LOGIN_RESPONSE = 0x81;
        public  static int MID_EXECUTE_REQUEST = 0x02;
        public  static int MID_EXECUTE_RESPONSE = 0x82;
        public  static int MID_CONTINUE_REQUEST = 0x03;
        public  static int MID_CONTINUE_RESPONSE = 0x83;
        public  static int MID_LOGOUT_REQUEST = 0x04;
        public  static int MID_LOGOUT_RESPONSE = 0x84;
        public  static int MID_CLEAR_RESULTS_REQUEST = 0x05;
        public  static int MID_CLEAR_RESULTS_RESPONSE = 0x85;
        public  static int MID_REMOVE_SESSION_REQUEST = 0x06;
        public  static int MID_REMOVE_SESSION_RESPONSE = 0x86;

        public  static int NUM_MESSAGE_TYPES = 256;

        static Message(){
		messageTypeToString = new string[NUM_MESSAGE_TYPES];

		messageTypeToString[MID_LOGIN_REQUEST] = "Login Request";
		messageTypeToString[MID_LOGIN_RESPONSE] = "Login Response";
		messageTypeToString[MID_EXECUTE_REQUEST] = "Execute Request";
		messageTypeToString[MID_EXECUTE_RESPONSE] = "Error Response";
		messageTypeToString[MID_CONTINUE_REQUEST] = "Continue Request";
		messageTypeToString[MID_CONTINUE_RESPONSE] = "Continue Response";
		messageTypeToString[MID_LOGOUT_REQUEST] = "Logout Request";
		messageTypeToString[MID_LOGOUT_RESPONSE] = "Logout Response";
		messageTypeToString[MID_CLEAR_RESULTS_REQUEST] = "Clear Results Request";
		messageTypeToString[MID_CLEAR_RESULTS_RESPONSE] = "Clear Results Response";
		messageTypeToString[MID_REMOVE_SESSION_REQUEST] = "Remove Session Request";
		messageTypeToString[MID_REMOVE_SESSION_RESPONSE] = "Remove Session Response";
	}

    public Message(int messageType)
    {
        //this.messageFactory = messageFactory;
        this.messageType = messageType;
    }


    //@Override
    protected void writeAsBinary(BinaryDataOutput output)
    {

        output.writeUnsignedByte(messageType);

		for (int i = 0; i < tokens.Count; ++i) {
            Token token = (Token) tokens[i];
            token.writeAsBinary(output);
        }

        output.writeUnsignedByte(Token.TID_END);
    }

    //@Override
    //	protected void readAsBinary(BinaryDataInput input) throws IOException {
    //		int tokenType = input.readUnsignedByte();
    //		while (tokenType != Token.TID_END) {
    //			Token token =  messageFactory.createToken(tokenType);
    //			token.readAsBinary(input);
    //			tokens.add(token);
    //			tokenType = input.readUnsignedByte();
    //		}
    //
    //	}

    public void addToken(Token token)
    {
        tokens.Add(token);
    }

    public int getTokenCount()
    {
        return tokens.Count;
    }

    public Token getToken(int i)
    {
        return (Token)tokens[i];
    }

    public int getMessageType()
    {
        return messageType;
    }

    public void setMessageType(int messageType)
    {
        this.messageType = messageType;
    }

    //	protected abstract void writeAsBinary(BinaryDataOutput output) throws IOException;
    //	protected abstract void readAsBinary(BinaryDataInput input) throws IOException;


    //	protected abstract void writeAsBinary(CloudByteArrayOutputStream output) throws IOException;
    //	protected abstract void readAsBinary(CloudByteArrayInputStream input) throws IOException;

    public void describe(StringBuilder builder)
    {
        builder.Append("messageType: ");
        builder.Append(MessageTypeToString(messageType));
    }

    //	public void write (CloudByteArrayOutputStream output) throws IOException {
    //		int startSize = output.size();
    //
    //		output.writeInt(0); // dummy length
    //		output.writeInt(messageType);
    //
    //		writeAsBinary(output);
    //
    //		int endSize = output.size();
    //
    //		byte[] buffer = output.getBuffer();
    //		MessageUtil.writeInt(endSize - startSize, buffer, startSize);
    //
    //	}


    public static string MessageTypeToString(int messageType)
    {
        if (messageType < 0 || messageType >= NUM_MESSAGE_TYPES)
        {
            messageType = 0;
        }
        string name = messageTypeToString[messageType];
        if (name == null)
        {
            return "Unknown Message";
        }
        return name;
    }

    //	protected String messageTypeToString(int messageType) {
    //		if (messageType < 0 || messageType >= NUM_MESSAGE_TYPES) {
    //			messageType = 0;
    //		}
    //		return messageTypeToString[messageType];
    //	}
}
}
