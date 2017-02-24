using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHttp.Application
{
    #region delegate
    public delegate string PreApplicationStartMethod(Type type, string method);

    public delegate string ApplicationStartMethod(Type type, string method);
    #endregion delegate

    public interface IPHttpApplication
    {
        #region Properties
        string Name { get; set; }   
        #endregion Properties

        #region Methods
        string Start();

        string ExecuteAction();
        #endregion Methods

        #region Events
        event PreApplicationStartMethod PreApplicationStart;
        event ApplicationStartMethod ApplicationStart;
        #endregion Events

    }
}
