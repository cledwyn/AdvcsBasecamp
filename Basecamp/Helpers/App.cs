using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Basecamp.Helpers
{
    public class App
    {

        public static string SqlScrubString(string input)
        {
            if (input == null) { return ""; }
            if (input.Length == 0) { return ""; }
            return input.Replace("'", "''");
        }

        /// <summary>
        /// Converts string to bool based on Y.  Anything Else will return false.
        /// </summary>
        /// <param name="ynString">Single charachter string "Y" = true.</param>
        /// <returns></returns>
        public static bool ConvertYNtoBool(string ynString)
        {
            return ynString == "Y" ? true : false;
        }
        public static string ConvertBooltoYN(bool inAns)
        {
            return inAns ? "Y" : "N";
        }

        public static string GetWebPage(string URI)
        {
            return GetWebPage(URI, 10);
        }


        public static string GetWebPage(string URI, int timeoutSeconds)
        {
            try
            {
                // used to build entire input
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)
                                         WebRequest.Create(URI);

                request.Timeout = timeoutSeconds * 1000;
                request.UserAgent = "Macalester Advancement Matrix (advcs@macalester.edu)";

                // execute the request
                HttpWebResponse response = (HttpWebResponse)
                                           request.GetResponse();

                // we will read data via the response stream
                Stream resStream = response.GetResponseStream();

                string tempString = null;
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                } while (count > 0); // any more data to read?

                // print out page source
                return sb.ToString();
            }
            catch
            {
                return "";
            }

        }
    }
}