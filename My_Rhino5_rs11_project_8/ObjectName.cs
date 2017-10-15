using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using System.Runtime.InteropServices;
using Rhino.DocObjects.Custom;
using Rhino.FileIO;

namespace My_Rhino5_rs11_project_8
{
    [Guid("006F7654-2D62-49F0-B1CE-9DE07660AA94")]
    class ObjectName : Rhino.DocObjects.Custom.UserData
    {
        public string my_object_name { set; get; }
        public ObjectName() { }
        public ObjectName(string name)
        {
            my_object_name = name;
        }
        public override string Description { get { return "The name of this object"; } }
        public override string ToString() { return my_object_name; }
        protected override void OnDuplicate(UserData source)
        {
            ObjectName name = source as ObjectName;
            if (name != null) { my_object_name = name.my_object_name; }
        }
        public override bool ShouldWrite { get { return (my_object_name != null); } }
        protected override bool Read(BinaryArchiveReader archive)
        {
            my_object_name = archive.ReadString();
            return true;
        }
        protected override bool Write(BinaryArchiveWriter archive)
        {
            archive.WriteString(my_object_name);
            return true;
        }
    }
}
