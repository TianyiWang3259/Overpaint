using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.FileIO;

namespace My_Rhino5_rs11_project_8
{
    public class My_object_functions
    {
        public static Brep Initialize(string file_name)
        {
            File3dm my_file = File3dm.Read(file_name, File3dm.TableTypeFilter.ObjectTable, File3dm.ObjectTypeFilter.Brep);
            File3dmObject[] my_obj = my_file.Objects.FindByLayer("Default");
            int count = my_file.Objects.Count;
            Brep my_brep = new Brep();
            for (int i = 0; i < count; i++)
            {
                if (my_obj[i].Geometry.HasBrepForm)
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

            my_brep.UserDictionary.Set("Name", "DefaultName");
            my_brep.UserDictionary.Set("Category", "DefaultCategory");
            my_brep.UserDictionary.Set("Color", Color.Empty.Name);
            my_brep.UserDictionary.Set("ComponentID", Guid.NewGuid());
            my_brep.UserDictionary.Set("Position", new Point3d(0,0,0));
            my_brep.UserDictionary.Set("Z_Direction", new Vector3d(0, 0, 1));
            my_brep.UserDictionary.Set("Y_Direction", new Vector3d(0, 1, 0));
            my_brep.UserDictionary.Set("X_Direction", new Vector3d(1, 0, 0));
            my_brep.UserDictionary.Set("PinQuantity", 0);

            return my_brep;
        }

        public static void SetName(Brep brep, string object_name)
        {
            brep.UserDictionary.Set("Name", object_name);
        }
        public static string GetName(Brep brep)
        {
            string object_name = brep.UserDictionary.GetString("Name", "DefaultName");
            return object_name;
        }

        public static void SetCategoryName(Brep brep, string category_name)
        {
            brep.UserDictionary.Set("Category", category_name);
        }
        public static string GetCategoryName(Brep brep)
        {
            string category_name = brep.UserDictionary.GetString("Category", "DefaultCatagory");
            return category_name;
        }

        public static void SetColor(Brep brep, Color color)
        {
            string color_string = color.Name;
            brep.UserDictionary.Set("Color", color_string);
        }
        public static Color GetColor(Brep brep)

        {
            string color_string = brep.UserDictionary.GetString("Color");
            Color color = Color.FromName(color_string);
            return color;
        }

        public static void SetComponentID(Brep brep, Guid id)
        {
            brep.UserDictionary.Set("ComponentID", id);
        }
        public static Guid GetComponentID(Brep brep)
        {
            return brep.UserDictionary.GetGuid("ComponentID");
        }

        public static void SetPosition(Brep brep, Point3d p)
        {
            brep.UserDictionary.Set("Position", p);
        }
        public static Point3d GetPosition(Brep brep)
        {
            return brep.UserDictionary.GetPoint3d("Position");
        }

        public static void SetZ(Brep brep, Vector3d z)
        {
            z.Unitize();
            brep.UserDictionary.Set("Z_Direction", z);
        }
        public static Vector3d GetZ(Brep brep)
        {
            return brep.UserDictionary.GetVector3d("Z_Direction");
        }
        public static void SetY(Brep brep, Vector3d y)
        {
            y.Unitize();
            brep.UserDictionary.Set("Y_Direction", y);
        }
        public static Vector3d GetY(Brep brep)
        {
            return brep.UserDictionary.GetVector3d("Y_Direction");
        }
        public static void SetX(Brep brep, Vector3d x)
        {
            x.Unitize();
            brep.UserDictionary.Set("X_Direction", x);
        }
        public static Vector3d GetX(Brep brep)
        {
            return brep.UserDictionary.GetVector3d("X_Direction");
        }

        //set the number of pins and reset the information for all pins
        public static void SetPinQuantity(Brep brep, int q)     
        {
            brep.UserDictionary.Set("PinQuantity", q);
            for(int i = 0; i < 1; i++)
            {
                string pin_id_name = string.Format("Pin_{0}_Guid", i);
                string pin_position_name = string.Format("Pin_{0}_Coordination", i);
                brep.UserDictionary.Set(pin_id_name, Guid.Empty);
                brep.UserDictionary.Set(pin_position_name, new Point3d(0, 0, 0));
            }
        }
        public static int GetPinQuantity(Brep brep)
        {
            return brep.UserDictionary.Getint("PinQuantity", 0);
        }

        public static void SetPinCoordination(Brep brep, int pin_num, Point3d pin_coordiantion)
        {
            string pin_position_name = string.Format("Pin_{0}_Coordination", pin_num);
            brep.UserDictionary.Set(pin_position_name, pin_coordiantion);
        }
        public static Point3d GetPinCoordination(Brep brep, int pin_num)
        {
            string pin_position_name = string.Format("Pin_{0}_Coordination", pin_num);
            Point3d pin_coordination = brep.UserDictionary.GetPoint3d(pin_position_name);
            return pin_coordination;
        }
        public static Point3d GetPinPosition(Brep brep, int pin_num)
        {
            string pin_position_name = string.Format("Pin_{0}_Coordination", pin_num);
            Point3d pin_coordination = brep.UserDictionary.GetPoint3d(pin_position_name);
            Point3d object_position = GetPosition(brep);
            Vector3d x = GetX(brep);
            Vector3d y = GetY(brep);
            Vector3d z = GetZ(brep);
            Point3d pin_position = object_position + ( x * pin_coordination.X + y * pin_coordination.Y + z * pin_coordination.Z );
            return pin_position;
        }

        public static void SetPinGuid(Brep brep, int pin_num, Guid id)
        {
            string pin_id_name = string.Format("Pin_{0}_Guid", pin_num);
            brep.UserDictionary.Set(pin_id_name, id);
        }
        public static Guid GetPinGuid(Brep brep, int pin_num)
        {
            string pin_id_name = string.Format("Pin_{0}_Guid", pin_num);
            Guid pin_id = brep.UserDictionary.GetGuid(pin_id_name);
            return pin_id;
        }

        public static int FindPinNumber(Brep brep, Guid pin_id)
        {
            int pin_quantity = GetPinQuantity(brep);
            if(pin_quantity <= 0) { return -1; }
            for(int i = 0; i < pin_quantity; i++)
            {
                Guid id = GetPinGuid(brep, i);
                if(id == pin_id) { return i; }
            }
            return -1;
        }

        public static void SetPinFunction(Brep brep, int pin_num, string function_name)
        {
            string pin_function_name = string.Format("Pin_{0}_Function", pin_num);
            brep.UserDictionary.Set(pin_function_name, function_name);
        }
        public static string GetPinFunction(Brep brep, int pin_num)
        {
            string pin_function_name = string.Format("Pin_{0}_Function", pin_num);
            string function_name = brep.UserDictionary.GetString(pin_function_name);
            return function_name;
        }

        public static void TranslateTo(Brep brep, Point3d new_position)
        {
            Point3d current_position = GetPosition(brep);
            brep.Translate(new_position - current_position);
            SetPosition(brep, new_position);
        }
        public static void RotateVerticallyTo(Brep brep, Vector3d new_direction)
        {
            new_direction.Unitize();
            Vector3d current_direction = GetZ(brep);
            Point3d rotation_center = GetPosition(brep);
            /*
            double rotation_angle = Vector3d.VectorAngle(current_direction, new_direction);
            Vector3d rotation_axis = Vector3d.CrossProduct(current_direction, new_direction);
            brep.Rotate(rotation_angle, rotation_axis, rotation_center);
            SetZ(brep, new_direction);
            Vector3d y = GetY(brep);
            y.Rotate(rotation_angle, rotation_axis);
            SetY(brep, y);
            Vector3d x = GetX(brep);
            x.Rotate(rotation_angle, rotation_axis);
            SetY(brep, x);
            */

            Transform r = Transform.Rotation(current_direction, new_direction, rotation_center);
            brep.Transform(r);
            Vector3d z = GetZ(brep);
            z.Transform(r);
            SetZ(brep, z);
            //SetZ(brep, new_direction);
            Vector3d y = GetY(brep);
            y.Transform(r);
            SetY(brep, y);
            Vector3d x = GetX(brep);
            x.Transform(r);
            SetX(brep, x);

        }
        public static void RotateHorizontally(Brep brep, double rotation_angle)
        {
            Vector3d rotation_axis = GetZ(brep);
            Point3d rotation_center = GetPosition(brep);
            brep.Rotate(rotation_angle, rotation_axis, rotation_center);
            Vector3d y = GetY(brep);
            y.Rotate(rotation_angle, rotation_axis);
            SetY(brep, y);
            Vector3d x = GetX(brep);
            x.Rotate(rotation_angle, rotation_axis);
            SetX(brep, x);
        }

        public static void RotateHorizontallyTo(Brep brep, Vector3d new_direction)
        {
            new_direction.Unitize();
            Vector3d current_direction = GetY(brep);
            Point3d rotation_center = GetPosition(brep);
            Transform r = Transform.Rotation(current_direction, new_direction, rotation_center);
            brep.Transform(r);
            Vector3d z = GetZ(brep);
            z.Transform(r);
            SetZ(brep, z);
            //SetZ(brep, new_direction);
            Vector3d y = GetY(brep);
            y.Transform(r);
            SetY(brep, y);
            Vector3d x = GetX(brep);
            x.Transform(r);
            SetX(brep, x);
        }

    }
}
