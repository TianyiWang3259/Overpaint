using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("7c62b431-8a93-459f-97e2-96a8cd17bf47")]
    public class Get_object_name : Command
    {
        static Get_object_name _instance;
        public Get_object_name()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Get_object_name command.</summary>
        public static Get_object_name Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Get_object_name"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            RhinoApp.WriteLine("The {0} command is under construction.", EnglishName);
            GetObject gbrep = new GetObject();
            gbrep.SetCommandPrompt("get the brep");
            gbrep.GeometryFilter = Rhino.DocObjects.ObjectType.Brep;
            gbrep.SubObjectSelect = false;
            gbrep.Get();
            if (gbrep.CommandResult() != Rhino.Commands.Result.Success)
                return gbrep.CommandResult();
            Rhino.DocObjects.ObjRef my_objref = gbrep.Object(0);
            Rhino.DocObjects.RhinoObject my_obj = my_objref.Object();
            if (my_obj == null)
                return Rhino.Commands.Result.Failure;
            Brep brep = my_objref.Brep();
            if (brep == null)
                return Result.Failure;
            my_obj.Select(false);
            RhinoApp.WriteLine("the name of the object is {0}", brep.UserDictionary.GetString("name"));
            return Result.Success;
        }
    }
}
