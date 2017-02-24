using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHttp.Application
{
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
        event EventHandler PreApplicationStart;
        event EventHandler ApplicationStart;
        #endregion Events

        #region Delegate
        Delegate PreApplicationStartMethod(Type type, string method);

        Delegate ApplicationStartMethod(Type type, string method);
        #endregion Delegate

    }
}
