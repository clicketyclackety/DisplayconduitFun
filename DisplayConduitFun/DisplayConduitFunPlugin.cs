using Rhino;
using System;

namespace DisplayConduitFun
{
    public class DisplayConduitFunPlugin : Rhino.PlugIns.PlugIn
    {

        public DisplayConduitFunPlugin()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the DisplayConduitFunPlugin plug-in.</summary>
        public static DisplayConduitFunPlugin Instance { get; private set; }

    }
}