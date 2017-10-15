using System;
using System.Drawing;
using System.Collections.Generic;
using RMA.UI;
using Rhino.PlugIns;
using Rhino.Geometry;

namespace My_Rhino5_rs11_project_8
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class MyRhino5rs11project8PlugIn : PlugIn

    {
        public Mesh painted_object_ { set; get; }
        public bool if_painted_object_set_;
        public DijkstraGraph graph;
        public List<Brep> my_objects_list;

        public MRhinoUiDockBar UIContiner = null;
        public MRhinoUiDockBar UIContiner_serialport = null;

        public MyRhino5rs11project8PlugIn()
        {
            Instance = this;

            UIContiner = new MRhinoUiDockBar(new Guid("{6A9552A6-B6AF-4795-A829-59C3AB49761E}"),
                "UIContainer", new My_Rhino_Usercontrol());

            UIContiner_serialport = new MRhinoUiDockBar(new Guid("{D333EA5E-7BF0-4A68-9E57-9DA2755B1920}"),
                "UIContiner_serialport", new SerialPort_UserControl());

            if_painted_object_set_ = false;
            my_objects_list = new List<Brep>();
        }

        ///<summary>Gets the only instance of the MyRhino5rs11project8PlugIn plug-in.</summary>
        public static MyRhino5rs11project8PlugIn Instance
        {
            get; private set;
        }

        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            MRhinoDockBarManager.CreateRhinoDockBar(
                this, UIContiner, true,
                MRhinoUiDockBar.DockLocation.floating,
                MRhinoUiDockBar.DockStyle.any,
                new System.Drawing.Point(200, 200));

            MRhinoDockBarManager.CreateRhinoDockBar(
                this, UIContiner_serialport, true,
                MRhinoUiDockBar.DockLocation.floating,
                MRhinoUiDockBar.DockStyle.any,
                new System.Drawing.Point(200, 200));

            return base.OnLoad(ref errorMessage);
        }

        public void LoadUI()
        {
            Rhino.RhinoApp.WriteLine("Add UI");
            if (!MRhinoDockBarManager.IsDockBarVisible(UIContiner.DockBarID))
            {
                MRhinoDockBarManager.ShowDockBar(UIContiner.DockBarID, true, true);
            }
        }

        public void LoadUI_SerialPort()
        {
            if (!MRhinoDockBarManager.IsDockBarVisible(UIContiner_serialport.DockBarID))
            {
                MRhinoDockBarManager.ShowDockBar(UIContiner_serialport.DockBarID, true, true);
            }
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and mantain plug-in wide options in a document.
    }
}