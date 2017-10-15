using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace My_Rhino5_rs11_project_8
{
    class My_Rhino_SerialPort
    {
        bool _continue;
        SerialPort _serialPort;
        string serial_port_name;
        //string message;
        Thread readThread;
        //Thread sendThread;
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

        public int position_x { get; set; }
        public int position_y { get; set; }
        public int position_z { get; set; }

        public int step_time { get; set; }

        public int range_x { get; set; }
        public int range_y { get; set; }
        public int range_z { get; set; }

        public Turntable turntable { get; set; }

        bool has_reached;
        bool has_reached_end;
        bool is_reading_from_file;
        bool is_punching;
        //bool should_send;
        //string send_message;
        //public LinkedList<string> send_message_list;
        public LinkedList<MotionMessage> send_message_list;

        bool has_started_port;

        Stopwatch watch;

        public My_Rhino_SerialPort()
        {
            
            //sendThread = new Thread(this.Send);
            _serialPort = new SerialPort();
            //SetPortName(_serialPort.PortName);
            serial_port_name = _serialPort.PortName;
            _serialPort.BaudRate = 115200;
            _serialPort.ReadTimeout = 10;
            _serialPort.WriteTimeout = 10;

            position_x = 0;
            position_y = 0;
            position_z = 0;

            step_time = 500;
            turntable = new Turntable();
            range_x = 0;
            range_y = 0;
            range_z = 0;

            has_reached = true;
            has_reached_end = false;
            is_reading_from_file = false;
            is_punching = false;
            //should_send = false;
            //send_message = null;
            has_started_port = false;
            send_message_list = new LinkedList<MotionMessage>();

            watch = new Stopwatch();
        }

        public void StartPort()
        {
            if (has_started_port) { return; }
            _serialPort.Open();
            _continue = true;
            readThread = new Thread(this.Read);
            readThread.IsBackground = true;
            readThread.Start();
            has_started_port = true;
            //sendThread.Start();
        }

        public void EndPort()
        {
            if (!has_started_port) { return; }
            _continue = false;
            readThread.Abort();
            _serialPort.Close();
            has_started_port = false;
            send_message_list.Clear();
            turntable.TurntableStop();
        }

        
        public void Read()
        {
            while (_continue && has_started_port)
            {
                //Rhino.RhinoApp.WriteLine("serial port read thread working");
                lock(send_message_list)
                {
                    if (send_message_list.Count > 0)
                    {

                        if (has_reached && turntable.has_reached)
                        {
                            //string send_message = send_message_list.First.Value;
                            MotionMessage send_message = send_message_list.First.Value;

                            /*
                            if (send_message[0] == 'x')
                            {

                                LinkedListNode<string> send_message_list_node = send_message_list.First;
                                for (int i = 0; i < send_message_list.Count; i++)
                                {
                                    Rhino.RhinoApp.Write("{0}, ", send_message_list_node.Value);
                                    send_message_list_node = send_message_list_node.Next;
                                }
                                Rhino.RhinoApp.WriteLine(" ");

                                watch.Stop();
                                Rhino.RhinoApp.WriteLine("order queueing time: {0}", watch.Elapsed);
                                watch.Reset();
                            }
                            */

                            //send message to arduino
                            send_message_list.RemoveFirst();
                            _serialPort.WriteLine(send_message.GetSerialString());
                            if (send_message.GetSerialString() != "G")
                            {
                                Rhino.RhinoApp.WriteLine(send_message.GetSerialString());
                            }
                            turntable.SetRotation(send_message.GetRotateIngerval(), send_message.GetRotateAngle());

                            string sent_str = send_message.GetSerialString();
                            if (sent_str[0] == 'X' || sent_str[0] == 'Y' || sent_str[0] == 'Z')
                            {
                                has_reached = false;
                            }
                        }
                        else
                        {
                            string send_message = send_message_list.First.Value.GetSerialString();
                            if (send_message[0] == 'G') { send_message_list.RemoveFirst(); }
                        }
                        //should_send = false;
                    }
                }
                


                //bool keep_read = true;
                string message = null;
                //while (keep_read)
                //{
                    try
                    {
                        message = _serialPort.ReadLine();
                        //Console.WriteLine(message);
                        //keep_read = false;
                    }
                    catch (TimeoutException) { //keep_read = true; 
                }
                //}
                if(message != null)
                {
                    string submessage = message.Substring(1);
                    switch (message[0])
                    {
                        case 'E':
                            {
                                has_reached_end = true;
                                has_reached = true;
                                _serialPort.WriteLine("G");
                                break;
                            }
                        case 'F':
                            {
                                has_reached = true;
                                break;
                            }
                        case 'P':
                            {
                                int X, Y, Z;
                                ConvertPosition(submessage, out X, out Y, out Z);
                                position_x = X;
                                position_y = Y;
                                position_z = Z;
                                break;
                            }
                        case 'R':
                            {
                                int X, Y, Z;
                                ConvertPosition(submessage, out X, out Y, out Z);
                                range_x = X;
                                range_y = Y;
                                range_z = Z;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                
                Thread.Sleep(1);
            }
        }


        public bool SetPortName(string portName)
        {
            if(has_started_port) { return true; }
            string defaultPortName = _serialPort.PortName;
            
            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
                _serialPort.PortName = portName;
                return false;
            }
            _serialPort.PortName = portName;
            return true;
        }

        public void GoToPosition(int X, int Y, int Z)
        {
            //string send_message;
            if(!has_started_port) { return; }
            if (is_reading_from_file) { return; }
            int delta_x = X - position_x;
            int delta_y = Y - position_y;
            int delta_z = Z - position_z;
            //send_message = string.Format("X{0}Y{1}Z{2}", delta_x, delta_y, delta_z);
            MotionMessage send_message = new MotionMessage(delta_x, delta_y, delta_z, step_time, is_punching);
            string ss = send_message.GetSerialString();
            Rhino.RhinoApp.WriteLine(ss);
            lock (send_message_list)
            {
                send_message_list.AddLast(send_message);
            }
            //should_send = true;
        }

        public void RotateTurnTable(float angle)
        {
            if (!has_started_port) { return; }
            if (is_reading_from_file) { return; }
            MotionMessage send_message = new MotionMessage(step_time,angle,is_punching);
            lock (send_message_list)
            {
                send_message_list.AddLast(send_message);
            }
        }

        public void RotateServo(float angle)
        {
            if (!has_started_port) { return; }
            if (is_reading_from_file) { return; }
            MotionMessage send_message = new MotionMessage(angle, is_punching);
            lock (send_message_list)
            {
                send_message_list.AddLast(send_message);
            }
        }

        public void GetPosition()
        {
            if (!has_started_port) { return; }
            //if (has_reached) { return; }
            if (is_reading_from_file) { return; }
            if (send_message_list.Count != 0)
            {
                if (send_message_list.First.Value.GetSerialString() == "G" || send_message_list.Last.Value.GetSerialString() == "G") { return; }
            }
            MotionMessage send_message = new MotionMessage(1);
            lock(send_message_list)
            {
                send_message_list.AddLast(send_message);
            }
            
            //should_send = true;
        }

        public void KeepMovingX(int dir)
        {
            if (!has_started_port) { return; }
            if (is_reading_from_file) { return; }
            watch.Start();
            MotionMessage send_message;
            //string send_message;
            //if (dir > 0) { send_message = "x1"; }
            //else if (dir == 0) { send_message = "x0"; }
            //else { send_message = "x-1"; }
            if (dir > 0) { send_message = new MotionMessage(5); }
            else if (dir == 0) { send_message = new MotionMessage(4); }
            else { send_message = new MotionMessage(3); }
            lock (send_message_list)
            {
                send_message_list.AddLast(send_message);
            }
            //should_send = true;
        }
        public void KeepMovingY(int dir)
        {
            if (!has_started_port) { return; }
            if (is_reading_from_file) { return; }
            MotionMessage send_message;
            //string send_message;
            //if (dir > 0) { send_message = "y1"; }
            //else if (dir == 0) { send_message = "y0"; }
            //else { send_message = "y-1"; }
            if (dir > 0) { send_message = new MotionMessage(8); }
            else if (dir == 0) { send_message = new MotionMessage(7); }
            else { send_message = new MotionMessage(6); }
            lock (send_message_list)
            {
                send_message_list.AddLast(send_message);
            }
            //should_send = true;
        }
        public void KeepMovingZ(int dir)
        {
            if (!has_started_port) { return; }
            if (is_reading_from_file) { return; }
            //string send_message;
            MotionMessage send_message;
            //if (dir > 0) { send_message = "z1"; }
            //else if (dir == 0) { send_message = "z0"; }
            //else { send_message = "z-1"; }
            if (dir > 0) { send_message = new MotionMessage(11); }
            else if (dir == 0) { send_message = new MotionMessage(10); }
            else { send_message = new MotionMessage(9); }
            lock (send_message_list)
            {
                send_message_list.AddLast(send_message);
            }
            //should_send = true;
        }

        public void SetSpeed(int speed_level)
        {
            if (!has_started_port) { return; }
            if (is_reading_from_file) { return; }
            switch(speed_level)
            {
                case 1:
                    step_time = 6000;
                    break;
                case 2:
                    step_time = 2000;
                    break;
                case 3:
                    step_time = 400;
                    break;
                default:
                    step_time = 500;
                    break;
            }
            MotionMessage send_message = new MotionMessage(0, 0, 0, step_time, false);
            //string send_message = string.Format("C{0}", step_time);
            lock (send_message_list)
            {
                send_message_list.AddLast(send_message);
            }
        }

        public void ReadOrdersFromFile(string address)
        {
            if (!has_started_port) { return; }
            if (is_reading_from_file) { return; }
            string[] lines = System.IO.File.ReadAllLines(@address);
            is_reading_from_file = true;
            lock(send_message_list)
            {
                foreach (string line in lines)
                {
                    MotionMessage send_message = new MotionMessage(line);
                    send_message_list.AddLast(send_message);
                }
            }
            PrintQueue();
            is_reading_from_file = false;

        }


        public void PrintQueue()
        {
            LinkedListNode<MotionMessage> send_message_list_node = send_message_list.First;
            for (int i = 0; send_message_list_node != null; i++)
            {
                Rhino.RhinoApp.Write("{0}, ", send_message_list_node.Value);
                send_message_list_node = send_message_list_node.Next;
            }
            Rhino.RhinoApp.WriteLine(" ");
        }


        public static void ConvertPosition(string position_string, out int X, out int Y, out int Z)
        {
            string[] position_list = position_string.Split('_');
            X = int.Parse(position_list[0]);
            Y = int.Parse(position_list[1]);
            Z = int.Parse(position_list[2]);
        }
        
        public static string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }
    }
}
