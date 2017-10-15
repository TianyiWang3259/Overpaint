using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("8622a2b7-2496-4fec-9712-96bbc5e5cd46")]
    public class MyRhino5rs11project8Command : Command
    {
        public MyRhino5rs11project8Command()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static MyRhino5rs11project8Command Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "MyRhino5rs11project8Command"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            RhinoApp.WriteLine("The {0} command will add a line right now.", EnglishName);
            MyRhino5rs11project8PlugIn.Instance.LoadUI();
            return Result.Success;
        }
    }
}
