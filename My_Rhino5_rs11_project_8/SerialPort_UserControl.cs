using System;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace My_Rhino5_rs11_project_8
{
    public partial class SerialPort_UserControl : UserControl
    {

        My_Rhino_SerialPort serial_port;
        bool _continue;
        Thread labelX_thread;
        Thread labelY_thread;
        Thread labelZ_thread;

        public delegate void change_x_delegate();
        public change_x_delegate change_x_delegate_method;
        public delegate void change_y_delegate();
        public change_y_delegate change_y_delegate_method;
        public delegate void change_z_delegate();
        public change_z_delegate change_z_delegate_method;


        public SerialPort_UserControl()
        {
            InitializeComponent();
            
            string[] available_ports = My_Rhino_SerialPort.GetAvailablePorts();
            comboBox_PortName.BeginUpdate();
            foreach (string port in available_ports)
            {
                comboBox_PortName.Items.Add(port);
            }
            comboBox_PortName.EndUpdate();
            
            change_x_delegate_method = new change_x_delegate(change_labelX);
            change_y_delegate_method = new change_y_delegate(change_labelY);
            change_z_delegate_method = new change_z_delegate(change_labelZ);

            _continue = false;
        }

        private void RefreshPort_Click(object sender, EventArgs e)
        {
            string[] available_ports = My_Rhino_SerialPort.GetAvailablePorts();
            comboBox_PortName.BeginUpdate();
            comboBox_PortName.Items.Clear();
            foreach (string port in available_ports)
            {
                comboBox_PortName.Items.Add(port);
            }
            comboBox_PortName.EndUpdate();
        }

        //start port button
        private void button1_Click(object sender, EventArgs e)
        {
            string portname = comboBox_PortName.SelectedItem.ToString();
            serial_port = new My_Rhino_SerialPort();
            if (!serial_port.SetPortName(portname))
            {
                MessageBox.Show("Wrong Port Name");
                return;
            }
            serial_port.StartPort();
            _continue = true;

            labelX_thread = new Thread(Change_X_Thread_Function);
            labelX_thread.IsBackground = true;
            labelX_thread.Start();
            labelY_thread = new Thread(Change_Y_Thread_Function);
            labelY_thread.IsBackground = true;
            labelY_thread.Start();
            labelZ_thread = new Thread(Change_Z_Thread_Function);
            labelZ_thread.IsBackground = true;
            labelZ_thread.Start();
            
            Rhino.RhinoApp.WriteLine("Start Port");
        }
        //end port button
        private void button1_Click_1(object sender, EventArgs e)
        {
            _continue = false;
            labelX_thread.Abort();
            labelY_thread.Abort();
            labelZ_thread.Abort();
            Rhino.RhinoApp.WriteLine("End XYZ");
            //lock (serial_port)
            //{
            serial_port.EndPort();
            //}
            Rhino.RhinoApp.WriteLine("End Port");
        }

        private void Change_X_Thread_Function()
        {
            while (_continue)
            {
                lock(serial_port.send_message_list)
                {
                    serial_port.GetPosition();
                }
                Thread.Sleep(20);
                this.Invoke(this.change_x_delegate_method);
                Thread.Sleep(20);
            }
        }
        private void Change_Y_Thread_Function()
        {
            while (_continue)
            {
                lock (serial_port.send_message_list)
                {
                    serial_port.GetPosition();
                }
                Thread.Sleep(20);
                this.Invoke(this.change_y_delegate_method);
                Thread.Sleep(20);
            }
        }
        private void Change_Z_Thread_Function()
        {
            while (_continue)
            {
                lock (serial_port.send_message_list)
                {
                    serial_port.GetPosition();
                }
                Thread.Sleep(20);
                this.Invoke(this.change_z_delegate_method);
                Thread.Sleep(20);
            }
        }

        private void change_labelX()
        {
            //Rhino.RhinoApp.WriteLine("Change X thread working");
            int position = serial_port.position_x;
            labelX.Text = position.ToString();
        }
        private void change_labelY()
        {
            int position = serial_port.position_y;
            labelY.Text = position.ToString();
        }
        private void change_labelZ()
        {
            int position = serial_port.position_z;
            labelZ.Text = position.ToString();
        }

        private void GoToPosition_Click(object sender, EventArgs e)
        {
            if(textBoxX.TextLength == 0 || textBoxY.TextLength == 0 || textBoxZ.TextLength == 0) { return; }
            int target_x = int.Parse(textBoxX.Text);
            int target_y = int.Parse(textBoxY.Text);
            int target_z = int.Parse(textBoxZ.Text);
            lock (serial_port.send_message_list)
            {
                serial_port.GoToPosition(target_x, target_y, target_z);
            }
        }

        private void Rotate_TurnTable_Click(object sender, EventArgs e)
        {
            if (textBox_TurnTable.TextLength == 0) { return; }
            float angle = float.Parse(textBox_TurnTable.Text);
            serial_port.RotateTurnTable(angle);
        }

        private void Rotate_Servo_Click(object sender, EventArgs e)
        {
            if (textBox_Servo.TextLength == 0) { return; }
            float angle = float.Parse(textBox_Servo.Text);
            serial_port.RotateServo(angle);
        }

        private void DecreaseX_MouseDown(object sender, MouseEventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingX(-1);
            //}
            watch.Stop();
            Rhino.RhinoApp.WriteLine("decrease x mouse down time: {0}", watch.Elapsed);
        }
        private void DecreaseX_MouseUp(object sender, MouseEventArgs e)
        {
            //lock(serial_port.send_message_list)
            //{
                serial_port.KeepMovingX(0);
            //}
        }
        private void IncreaseX_MouseDown(object sender, MouseEventArgs e)
        {
            //lock(serial_port.send_message_list)
            //{
                serial_port.KeepMovingX(1);
            //}
        }
        private void IncreaseX_MouseUp(object sender, MouseEventArgs e)
        {
            //lock(serial_port.send_message_list)
            //{
                serial_port.KeepMovingX(0);
            //}
        }
        private void DecreaseY_MouseDown(object sender, MouseEventArgs e)
        {
            //lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingY(-1);
            //}
        }
        private void DecreaseY_MouseUp(object sender, MouseEventArgs e)
        {
            //lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingY(0);
            //}
        }
        private void IncreaseY_MouseDown(object sender, MouseEventArgs e)
        {
            //lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingY(1);
            //}
        }
        private void IncreaseY_MouseUp(object sender, MouseEventArgs e)
        {
            //lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingY(0);
           // }
        }
        private void DecreaseZ_MouseDown(object sender, MouseEventArgs e)
        {
            //lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingZ(-1);
           // }
        }
        private void DecreaseZ_MouseUp(object sender, MouseEventArgs e)
        {
           // lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingZ(0);
           // }
        }
        private void IncreaseZ_MouseDown_1(object sender, MouseEventArgs e)
        {
            //lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingZ(1);
            //}
        }
        private void IncreaseZ_MouseUp(object sender, MouseEventArgs e)
        {
            //lock (serial_port.send_message_list)
            //{
                serial_port.KeepMovingZ(0);
            //}
        }

        private void Speed_Slow_Click(object sender, EventArgs e)
        {
            serial_port.SetSpeed(1);
        }
        private void Speed_Medium_Click(object sender, EventArgs e)
        {
            serial_port.SetSpeed(2);
        }
        private void Speed_Fast_Click(object sender, EventArgs e)
        {
            serial_port.SetSpeed(3);
        }

        //Run File button
        private void button2_Click(object sender, EventArgs e)
        {
            string address = textBox2.Text;
            serial_port.ReadOrdersFromFile(address);
        }

        private void PrintQueue_Click(object sender, EventArgs e)
        {
            serial_port.PrintQueue();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxX_TextChanged(object sender, EventArgs e)
        {

        }

        private void IncreaseX_Click(object sender, EventArgs e)
        {

        }
    }
}
