using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mvc;
namespace App
{
    /// <summary>
    /// Main class of the class where you put a pattern to you URL
    /// </summary>
    public class App : HekoTest
    {
        public App() : base()
        {
            RegisterURLPatern("{controller}/{action}/{id}");
        }
    }
}
