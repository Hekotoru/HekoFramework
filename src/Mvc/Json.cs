using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mvc
{
    /// <summary>
    /// Class responsible for sending json style.
    /// </summary>
    public class Json : AResult
    {
        public byte[] data;
        public int StatusCode;
        public string jso;
        public Json(string _json, int _statusCode)
        {
            StatusCode = _statusCode;
            data = Encoding.ASCII.GetBytes(_json);
        }

        /// <summary>
        /// constructor with parameter: string: json.
        /// </summary>
        /// <param name="statusCode">int: Status Code of the respond.</param>
        /// <param name="jsonString">string: Json contain. </param>
        public Json(object Object, int statusCode)
        {
            StatusCode = statusCode;
            jso = JsonConvert.SerializeObject(Object);
        }
        string AResult.ContentType()
        {
            return "application/json";
        }

        int AResult.StatusCode()
        {
            return StatusCode;
        }

        public MemoryStream Content()
        {
            MemoryStream jsonData = new MemoryStream(data);
            return jsonData;
        }
    }
}
