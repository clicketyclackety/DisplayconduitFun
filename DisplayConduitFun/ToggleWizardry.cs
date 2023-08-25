using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;

namespace DisplayConduitFun
{
    public class ToggleWizardry : Command
    {
        private readonly Conduit myConduit;

        public ToggleWizardry()
        {
            Instance = this;
            myConduit = new Conduit();
        }

        ///<summary>The only instance of this command.</summary>
        public static ToggleWizardry Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => nameof(ToggleWizardry);

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            myConduit.Enabled = !myConduit.Enabled;

            return Result.Success;
        }
    }
}
