using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace My_Rhino5_rs11_project_8
{
    class Turntable
    {
        public bool has_reached;
        public bool should_rotate;
        public bool _continue;
        public Thread rotateThread;
        public int interval;
        public float angle;
        public float rotate_position;


        public Turntable()
        {
            _continue = true;
            has_reached = true;
            should_rotate = false;
            rotateThread = new Thread(this.rotate);
            rotateThread.IsBackground = true;
            rotateThread.Start();
            rotate_position = 0;
        }

        public void TurntableStop()
        {
            has_reached = true;
            should_rotate = false;
            _continue = false;
            rotateThread.Abort();
        }

        public void RotateTo(int i, float a)
        {
            float angle = a - rotate_position;
            SetRotation(i, angle);
        }

        public void SetRotation(int i, float a)
        {
            if(should_rotate)
            {
                Rhino.RhinoApp.WriteLine("Rotation Error: already set rotation parameters");
                return;
            }
            if (a == 0) { return; }
            interval = i;
            angle = a;
            rotate_position = rotate_position + a;
            should_rotate = true;
            has_reached = false;
        }

        public void rotate()
        {
            while(_continue)
            {
                if(should_rotate)
                {
                    Rhino.RhinoApp.WriteLine("current angle: {0}", rotate_position);
                    should_rotate = false;
                    string url = "http://10.0.1.15/cal1";
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    float degree = 0;
                    if (angle > 0) { degree = interval * 1000 + angle; }
                    else { degree = -1 * (interval * 1000 - angle); }
                    string degree_str = degree.ToString();
                    string postData = "cmd=rotateTable&degrees=" + degree_str;
                    Rhino.RhinoApp.WriteLine("rotate angle: {0}", degree_str);
                    var data = Encoding.ASCII.GetBytes(postData);

                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    if (responseString != null)
                    {
                        has_reached = true;
                    }
                    else
                    {
                        Rhino.RhinoApp.WriteLine("did not get response");
                    }
                } 
            }
        }
    }
    
    class TurnTableMessage
    {

    }
}
