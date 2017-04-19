using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHttp
{
    enum ClientState
    {
        ReadingProlog,
        ReadingHeaders,
        ReadingContent,
        WritingHeaders,
        WritingContent,
        Closed
    }
}
