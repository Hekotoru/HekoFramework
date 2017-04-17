using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mvc
{

    /// <summary>
    /// This class represent the file in the request.
    /// </summary>
    public class HttpFile
    {
        /// <summary>
        /// This field represent the content length of file.
        /// </summary>
        public int ContentLength;

        /// <summary>
        /// This field represent the content type of file.
        /// </summary>
        public string ContentType;

        /// <summary>
        /// This field represent the name of file.
        /// </summary>
        public string FileName;

        /// <summary>
        /// This field represent the file name of file.
        /// </summary>
        public Stream InputStream;

        /// <summary>
        /// Default Constructor whit params of the request file. 
        /// </summary>
        /// <param name="contentLength">The content length of file.</param>
        /// <param name="contentType">The content type of file.</param>
        /// <param name="fileName">The name of file.</param>
        /// <param name="inputStream">The file name of file.</param>
        public HttpFile(int contentLength, string contentType, string fileName, Stream inputStream)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (inputStream == null)
                throw new ArgumentNullException("inputStream");

            ContentLength = contentLength;
            ContentType = contentType;
            FileName = fileName;
            InputStream = inputStream;
        }
    }
}