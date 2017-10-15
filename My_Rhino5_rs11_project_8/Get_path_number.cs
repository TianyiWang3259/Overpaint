using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("89bc3c29-74f1-4ab4-b803-37fff4f97c96")]
    public class Get_path_number : Command
    {
        static Get_path_number _instance;
        public Get_path_number()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Get_path_number command.</summary>
        public static Get_path_number Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Get_path_number"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.

            GetObject gpath = new GetObject();
            gpath.SetCommandPrompt("get the pathp");
            gpath.GeometryFilter = Rhino.DocObjects.ObjectType.Curve;
            gpath.SubObjectSelect = false;
            gpath.Get();
            if (gpath.CommandResult() != Rhino.Commands.Result.Success)
                return gpath.CommandResult();
            Rhino.DocObjects.ObjRef my_objref = gpath.Object(0);
            Rhino.DocObjects.RhinoObject my_obj = my_objref.Object();
            if (my_obj == null)
                return Rhino.Commands.Result.Failure;
            int path_number = my_obj.Geometry.UserDictionary.Getint("path_number", -1);
            
            RhinoApp.WriteLine("the number of the object is {0}", path_number);
            return Result.Success;
        }
    }
}
