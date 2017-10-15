using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.FileIO;

namespace My_Rhino5_rs11_project_8
{
    class MyObject
    {
        public Brep my_brep { get; set; }
        public bool if_got_brep { get; set; }
        //public ObjectName ON { get;  set;}
        //private string my_path = "D:/Desktop/MyObject.3dm";
        public File3dm my_file { get; set; }
        public MyObject()
        {
            my_brep = new Brep();
            //my_path = "D:/Desktop/MyObject.3dm";
            //my_file = new File3dm();
            //ON = new ObjectName("MyFirstObject");
            if_got_brep = false;
            RhinoApp.WriteLine("new my object");
        }
        public bool readfile(string layername)
        {
            string my_path = "D:/Desktop/assembly.3dm";
            RhinoApp.WriteLine(my_path);
            try
            {
                RhinoApp.WriteLine("start trying");
                //RhinoApp.WriteLine("the number of objects is {0}", my_file.Objects.Count);
                my_file = File3dm.Read(my_path, File3dm.TableTypeFilter.ObjectTable, File3dm.ObjectTypeFilter.Any);
                //System.Threading.Thread.Sleep(500);
                //string error;
                //my_file.IsValid(out error);
                //RhinoApp.WriteLine("error is {0}", error);
                RhinoApp.WriteLine("the number of objects is {0}", my_file.Objects.Count);
                RhinoApp.WriteLine("the objects is {0}", my_file.Objects.ToString());

                //GeometryBase my_geometry = my_file.Objects[0].Geometry.Duplicate();
                //RhinoApp.WriteLine("name {0}", my_file.Objects[0].Name);
                //uint number = 0;
                File3dmObject[] my_obj = my_file.Objects.FindByLayer("Default");
                RhinoApp.WriteLine("after read");
                //RhinoApp.WriteLine("after read geometry");
                int count = my_file.Objects.Count;
                for(int i = 0; i<count; i++)
                {
                    if (my_obj[i].Geometry.HasBrepForm )
                    {
                        RhinoApp.WriteLine("Has Brep form");
                        Brep my_brep_1 = Brep.TryConvertBrep(my_obj[i].Geometry);
                        if (my_brep_1.Surfaces.Count > 1)
                        {
                            my_brep = my_brep_1;
                            break;
                        }
                    }
                }
                adduserdata();
                if_got_brep = true;
                //doc.Objects.AddBrep(my_brep);
                //doc.Views.Redraw();
                RhinoApp.WriteLine("Successfully setup the object");
                return true;

            }
            catch (FileNotFoundException e)
            {
                RhinoApp.WriteLine("No Right Path");
                return false;
            }
            catch (IndexOutOfRangeException e)
            {
                RhinoApp.WriteLine("No Object In This 3dm File");
                return false;
            }
        }
        public void adduserdata()
        {
            //ObjectName ud = my_brep.UserData.Find(typeof(ObjectName)) as ObjectName;

            ObjectName ud = new ObjectName("MyFirstObject");
            RhinoApp.WriteLine("set a name for the object");
            my_brep.UserDictionary.Set("name", "myfirstobject");
            my_brep.UserDictionary.Set("isMovable", true);
            Point3d p = new Point3d(0, 0, -5);
            Vector3d v = new Vector3d(0, 0, 1);
            my_brep.UserDictionary.Set("CurrentPosition", p);
            my_brep.UserDictionary.Set("CurrentDirection", v);
            my_brep.UserDictionary.Set("Color", "Red");
            Guid pin_id = Guid.NewGuid();
            my_brep.UserDictionary.Set("PinID", pin_id);
        }
    }
}
