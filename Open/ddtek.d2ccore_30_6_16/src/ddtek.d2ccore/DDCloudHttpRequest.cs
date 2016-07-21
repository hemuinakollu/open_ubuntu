using ddtek.d2ccore.Common;
using ddtek.d2ccore.Utility;
using System;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ddtek.d2ccore
{
    public class DDCloudHttpRequest
    {

        static readonly string RESPONSE_HEADER_CONTENT_TYPE = "Content-Type";

        private static readonly int INIT_BUFFER_SIZE = 1024 * 16; // 16 KB
        private static readonly int MAX_BUFFER_SIZE = 1048576 * 100;

        private readonly DDCloudImplConnection implConnection;

        private BinaryMessageReader binaryReader;
        //private UtilStreamBuffer streamBuffer ;
        private UtilStreamBuffer streamBuffer;

        private HttpClient post;
        private HttpClient get;
        private BinaryMessageWriter messageWriter;

        //Logger logger;

        DDCloudHttpRequest(DDCloudImplConnection implConnection, int initialRequestBufferSize, int initialReponseBufferSize)
        {

            //this.logger = implConnection.logger;
            this.implConnection = implConnection;
            binaryReader = new BinaryMessageReader(implConnection.transliterator);
            streamBuffer = new UtilStreamBuffer(INIT_BUFFER_SIZE);
        }

        public BinaryDataOutput prepareForPostRequest()
        {
            if (post == null)
            {
                messageWriter = new BinaryMessageWriter(implConnection.transliterator);
                post = new HttpClient();
            }
            //else {
            //              post.reset();
            //          }
            messageWriter.initialize(streamBuffer.getOutputStream());
            return messageWriter;
        }

        BinaryDataInput submitGetRequest(string path, bool allowRetry, bool isCancel)
        {

            if (get == null)
            {
                get = new HttpClient();
            }
            //else {
            //          get.reset();
            //      }

            //get.setURI(URI.create(path));
            get.BaseAddress = new Uri(path);
            int retryCount = 0;
            while (true)
            {
                try
                {
                    //setSessionToken(get);
                    // setConnectorID(get);

                    //if (logger.isLoggable(Level.FINE))
                    //{
                    //    logger.logp(Level.FINE, logger.getName(), implConnection.sessionToken, "GET - SessionToken: " + implConnection.sessionToken + " PATH: " + path);
                    //}

                    HttpClient httpClient = isCancel ? implConnection.getHttpClientCancel() : implConnection.getHttpClient();
                    //HttpResponse response = httpClient.execute (implConnection.target, get);

                    
                    Task<HttpResponseMessage> response =  httpClient.GetAsync(DDCloudImplConnection.Url);
                    HttpResponseMessage msg;
                    
                    return BufferResponse(response.Result);
                }
                catch (IOException ioException)
                {
                    if (allowRetry && retryCount < implConnection.wsRetryCount)
                    {
                        //implConnection.logger.config("Detected timeout. Retrying operation.  Retry count: " + retryCount);
                    }
                    else
                    {
                        // throw implConnection.ioException(ioException, false);
                    }
                }
            }
        }

        public BinaryDataInput submitPostRequest(string path, bool allowRetry)
        {

            post.GetAsync(path);
            try
            {
                messageWriter.close();
            }
            catch (Exception ioException)
            {
                throw;//.ioException(ioException, false);
            }
            //setSessionToken(post);
            //setConnectorID(post);

            int retryCount = 0;
            while (true)
            {
                try
                {
                    byte[] requestbuffer = streamBuffer.getBuffer();
                    int requestlength = streamBuffer.getSize();

                    //if (logger.isloggable(level.fine))
                    //{
                    //    string sessiontoken = implconnection.sessiontoken == null ? "" : implconnection.sessiontoken;
                    //    //logger.logp(level.fine, logger.getname(), implconnection.sessiontoken, "post - content length:  " + integer.tostring(requestlength) + " sessiontoken: " + sessiontoken
                    //    +" path: " + path);
                    //}

                    //if (logger.isloggable(level.finest))
                    //{
                    //    logbytes(requestbuffer, requestlength, true);
                    //}
                    string res = Encoding.UTF8.GetString(requestbuffer);
                    StringContent entity = new StringContent(res);
                    //post.(entity);

                    //HttpResponse response = implConnection.getHttpClient().execute(implConnection.target, post);
                    Task<HttpResponseMessage> response =  implConnection.getHttpClient().PostAsync(DDCloudImplConnection.Url, entity);

                    BinaryDataInput bufferResponse = BufferResponse(response);
                    
                    return bufferResponse;

                }
                catch (IOException ioException)
                {
                    if (allowRetry && retryCount < implConnection.wsRetryCount)
                    {
                        // implConnection.logger.config("Detected timeout. Retrying operation.  Retry count: " + retryCount);
                    }
                    else
                    {
                        throw;// implConnection.ioException(ioException, false);
                    }
                    ++retryCount;
                }
            }
        }

        private BinaryDataInput BufferResponse(Task<HttpResponseMessage> response)
        {
            throw new NotImplementedException();
        }


        //  void clearStreamOnServer(TokenContinue continueToken) 
        //  {
        //      StringBuilder urlBuilder = implConnection.urlBuilder;
        //      urlBuilder.setLength (DDCloudImplConnection.PREFIX_PATH_LENGTH);
        //      urlBuilder.append (Message.PATH_CLEAR);

        //      continueToken.buildURL (urlBuilder, 2);
        //      BinaryDataInput binaryReader = submitGetRequest (urlBuilder.toString (), true, false);

        //// TODO: Does processing the reply even make sense? We kind of don't really care if it fails.
        //try {
        //              int tokenType = binaryReader.readUnsignedByte();
        //              while (tokenType != Token.TID_END)
        //              {
        //                  switch (tokenType)
        //                  {
        //                      case Token.TID_ERROR:
        //                          throw implConnection.ddcloudException(binaryReader, null);
        //                      case Token.TID_WARN:
        //                          implConnection.processWarning(binaryReader);
        //                          tokenType = binaryReader.readUnsignedByte();
        //                          break;
        //                      default:
        //                          throw implConnection.exceptions.getException(DDCloudLocalMessages.DDCLOUD_UNEXPECTED_RESPONSE_TYPE);
        //                  }
        //                  tokenType = binaryReader.readUnsignedByte();
        //              }
        //          }
        //      catch (IOException ioException) {
        //  throw;// implConnection.ioException(ioException, false);
        //      }
        //  }

        //  BinaryDataInput requestContinuation(TokenContinue continueToken) 
        //  {
        //      StringBuilder urlBuilder = implConnection.urlBuilder;
        //      urlBuilder.Length=(DDCloudImplConnection.PREFIX_PATH_LENGTH);
        //      urlBuilder.Append (Message.PATH_CONTINUE);

        //      continueToken.buildURL (urlBuilder, 3);

        //      BinaryDataInput reader = submitGetRequest (urlBuilder.ToString (), true, false);

        //return reader;
        //  }

        private BinaryDataInput BufferResponse(HttpResponseMessage response)
        {

            int responseLength = 0;
            HttpContent content = response.Content;
            byte[] byteArray = content.ReadAsByteArrayAsync().Result;
            long contentlength = byteArray.Length;
            //HttpContext responseEntity = response.Body;
            //StatusLine statusLine = response.getStatusLine();
            //Header header = response.Headers.getFirstHeader(RESPONSE_HEADER_CONTENT_TYPE);

            //    //Normally we get back a response header and response body.
            //    // If not, then give up now.Note, for some status codes we want to

            //    //capture the body in the log, so we let the

            //    //post handler run first.
            //    if (responseEntity == null || header == null)
            //    {
            //        //int statusCode = statusLine.getStatusCode();
            //        //string statusReason = statusLine.getReasonPhrase();
            //        //if (logger.isLoggable(Level.INFO))
            //        //{
            //        //    logger.logp(Level.INFO, logger.getName(), implConnection.sessionToken, "REQUEST RETURNED: " + statusCode + " " + statusReason);
            //        //}
            //        //throw implConnection.exceptions.getException(DDCloudLocalMessages.DDCLOUD_UNEXPECTED_HTTP_CODE,
            //        //new string[] { Integer.toString (statusCode), statusReason
            //        //}, DDCloudImplConnection.SQLSTATE_COMMUNICATION_LINK_FAILURE);
            //    }

            //string contentType = header.getValue();

            try
            {
            //    content = responseEntity..getcontent();
            //    long contentlength = responseEntity.getcontentlength();

                // if (logger.isloggable (level.fine))
                //          {
                //  int statuscode = statusline.getstatuscode();
                //              string statusreason = statusline.getreasonphrase();
                //              logger.logp (level.fine, logger.getname (), implconnection.sessiontoken, "request returned: " + statuscode + " " + statusreason + " content type: " + contenttype + " content length: "
                //+ contentlength);
                // }

                // sanity check. the servlet should not be returning anything too big in a single request.
                if (contentlength > MAX_BUFFER_SIZE)
                {
                    // this should not happen.
                    //throw implconnection.exceptions.getexception (ddcloudlocalmessages.ddcloud_unexpected_response_type);
                }

                if (contentlength >= 0)
                {
                    streamBuffer.getBuffer((int)contentlength);
                }
                //streamBuffer.Load (content);

                responseLength = streamBuffer.getSize();

                //if (logger.isloggable (level.finest))
                //         {

                //             logbytes(streambuffer.getbuffer (), responselength, false);
                //}

                // todo: if we throw an exception on the server, then there is
                // probably a specific status code (500?). we should map that to a
                // different
                // error since that should be expected in some scenarios.
                // we could also consider walking the html that is returned to get
                // the actual error.

                //int statuscode = StatusLine.getstatuscode();

                //if (statuscode != 200)
                //{
                //    //throw implconnection.exceptions.getexception (ddcloudlocalmessages.ddcloud_unexpected_http_code, new string[] { integer.tostring (statuscode), statusline.getreasonphrase () });
                //}

                //if (!contentType.Equals(Message.content_binary))
                //{
                //    //throw implconnection.exceptions.getexception (ddcloudlocalmessages.ddcloud_unexpected_content_type, new string[] { contenttype });
                //}
            }
            catch (IOException ioe)
            {
                throw;// implConnection.ioException (ioe, false);
            }
            finally
            {
                if (content != null)
                {

                    try
                    {
                        //content.Close();
                        content = null;
                    }
                    catch (IOException ioe)
                    {
                        throw;// implConnection.ioException (ioe, false);
                    }
                }
            }

                binaryReader.initialize (streamBuffer.getInputStream ());

                return binaryReader;
        }

        //private void setSessionToken(HttpRequestBase method)
        //   {
        //   string sessionToken = implConnection.sessionToken;
        //   if (sessionToken != null)
        //       {
        //           method.setHeader(Message.HEADER_AUTHORIZATION, sessionToken);
        //       }
        //   }

        //public static void append2(StringBuilder str, int val)
        //{
        //    if (val <= 9) str.Append('0');
        //    str.Append(val);
        //}

        //public static void append3(StringBuilder str, int val)
        //{
        //    if (val <= 9) str.Append("00");
        //    else if (val <= 99) str.Append('0');
        //    str.Append(val);
        //}

        //private void setConnectorID(HttpRequestBase method)
        //{
        //    String connectorID = implConnection.connectorID;
        //    if (connectorID != null)
        //    {
        //        method.setHeader(Message.HEADER_CONNECTOR_ID, connectorID);

        //        if ((implConnection.OnPremiseTraceOptions & implConnection.TimestampInHeader) != 0)
        //        {
        //            Calendar cal = implConnection.msgHdrCal;
        //            StringBuilder str = implConnection.msgHdrStr;
        //            cal.setTimeInMillis(System.currentTimeMillis());
        //            str.Length=0;
        //            str.Append(cal.get(Calendar.YEAR));
        //            str.Append('-');
        //            append2(str, cal.get(Calendar.MONTH) + 1);
        //            str.Append('-');
        //            append2(str, cal.get(Calendar.DAY_OF_MONTH));
        //            str.Append('T');
        //            append2(str, cal.get(Calendar.HOUR_OF_DAY));
        //            str.Append(':');
        //            append2(str, cal.get(Calendar.MINUTE));
        //            str.Append(':');
        //            append2(str, cal.get(Calendar.SECOND));
        //            str.Append('.');
        //            append3(str, cal.get(Calendar.MILLISECOND));
        //            method.setHeader("MsgTS", str.ToString());
        //        }
        //    }
        //}

        void logBytes(byte[] buffer, int length, bool isRequest)
        {
            int lineSize = 32;
            bool showAscii = true;
            StringBuilder line = new StringBuilder();

            int secretStart = int.MaxValue;
            int secretEnd = int.MaxValue;
            int pos = 0;

            line.Append(isRequest ? " request: " : " reply: ");
            line.Append(length);
            line.Append(" bytes\r\n");

            for (;;)
            {

                line.Append("\t");

                // Print the hexadecimal representation.
                for (int i = pos; i < pos + lineSize; i++)
                {
                    if (i < length)
                    {
                        byte b;
                        if (i >= secretStart && i < secretEnd)
                        {
                            b = 0;
                        }
                        else
                        {
                            b = buffer[i];
                        }
                        line.Append(byteToHex(b));
                    }
                    else
                    {
                        line.Append("  ");
                    }
                    line.Append(' ');
                }
                line.Append("        ");

                if (showAscii)
                {
                    for (int i = pos; i < pos + lineSize; i++)
                    {
                        if (i < length)
                        {
                            byte b;
                            if (i >= secretStart && i < secretEnd)
                            {
                                b = 0;
                            }
                            else
                            {
                                b = buffer[i];
                            }
                            line.Append(byteToASCII(b));
                        }
                        else
                        {
                            line.Append(' ');
                        }
                    }
                }

                line.Append("\r\n");

                pos += lineSize;
                if (pos > length)
                {
                    line.Append("\r\n");
                    break;
                }
            }

            //this.logger.logp(Level.FINEST, this.logger.getName(), implConnection.sessionToken, line.ToString());
        }

        static char[] hex_map = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        private readonly object final;

        static char[] byteToHex(byte b)
        {
            char[] hex = { hex_map[(b >> 4) & 0x0F], hex_map[b & 0x0F] };
            return hex;
        }

        static char byteToASCII(byte b)
        {
            if (b > 31 && b < 127)
                return (char)b;
            else
                return '.';
        }
    }
}
