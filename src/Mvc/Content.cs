using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc
{
    /// <summary>
    /// Class responsible for displaying plain text in browser.
    /// </summary>
    public class Content : AResult
    {
        public int statusCode;
        public string text;

        public Content(int _statusCode, string _text)
        {
            statusCode = _statusCode;
            text = _text;
        }
        string AResult.ContentType()
        {
            return "text/plain";
        }

        int AResult.StatusCode()
        {
            return statusCode;
        }
        MemoryStream AResult.Content()
        {
            return new MemoryStream(Encoding.ASCII.GetBytes(text));
        }
    }
}
