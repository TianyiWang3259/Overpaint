using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("d2e4befc-e6bc-407a-9549-4bafab112820")]
    public class Test_command : Command
    {
        static Test_command _instance;
        public Test_command()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Test_command command.</summary>
        public static Test_command Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Test_command"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            Brep brep = new Brep();

            brep.UserDictionary.Set("Color", System.Drawing.Color.Red.Name);
            String color = brep.UserDictionary.GetString("Color");
            RhinoApp.WriteLine("Color is {0}", color);
            RhinoApp.WriteLine("type is {0}", color.GetType());
            return Result.Success;
        }
    }
}
