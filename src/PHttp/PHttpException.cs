using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHttp
{
    [Serializable]
    class PHttpException : Exception
    {
        public PHttpException()
        {
        }

        public PHttpException(string message)
            : base(message)
        {
        }

        public PHttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PHttpException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
