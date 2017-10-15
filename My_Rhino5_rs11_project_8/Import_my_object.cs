using System;
using System.Drawing;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("a51e8285-9c71-4c37-9a72-54795410c227")]
    public class Import_my_object : Command
    {
        static Import_my_object _instance;
        public Import_my_object()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Import_my_object command.</summary>
        public static Import_my_object Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Import_my_object"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            RhinoApp.WriteLine("The {0} command is under construction.", EnglishName);
            MyObject my_first_object = new MyObject();
            if (my_first_object.readfile(doc.Layers.CurrentLayer.Name))
            {
                RhinoApp.WriteLine("Get the object");
                ObjectAttributes my_attributes = new ObjectAttributes();
                my_attributes.ObjectColor = Color.FromName(my_first_object.my_brep.UserDictionary.GetString("Color"));
                my_attributes.ColorSource = ObjectColorSource.ColorFromObject;
                doc.Objects.AddBrep(my_first_object.my_brep,my_attributes);
                doc.Views.Redraw();
                return Result.Success;
            }
            else
            {
                RhinoApp.WriteLine("No Such Object");
                return Result.Failure;
            }
        }
    }
}
