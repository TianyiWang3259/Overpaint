using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace My_Rhino5_rs11_project_8
{
    class MotionMessage
    {
        // 0: give order to arduino and pi
        // 1: acquire position from arduino
        // 2: zero
        // 3: x-1
        // 4: x0
        // 5: x1
        // 6: y-1
        // 7: y0
        // 8: y1
        // 9: z-1
        // 10: z0
        // 11: z1
        // 12: acquire range from arduino

        public int category; 
        public int step_x;
        public int step_y;
        public int step_z;
        // the unit for all time is microseconds(us)
        public int step_time;
        public int total_time;
        public float rotate_angle;
        public int rotate_interval_time;
        public float servo_angle;
        public bool is_punching;

        public MotionMessage(int c)
        {
            category = c;
            step_x = 0;
            step_y = 0;
            step_z = 0;
            step_time = 0;
            rotate_interval_time = 0;
            rotate_angle = 0;
            is_punching = false;
            servo_angle = 361;
            total_time = 0;
        }

        public MotionMessage(string message)
        {
            category = 0;
            string[] str_list = message.Split('_');
            string step_x_str = str_list[0];
            string step_y_str = str_list[1];
            string step_z_str = str_list[2];
            string step_time_str = str_list[3];
            string rotate_angle_str = str_list[4];
            string servo_angle_str = str_list[5];
            string is_punching_str = str_list[6];
            step_x = Convert.ToInt32(step_x_str);
            step_y = Convert.ToInt32(step_y_str);
            step_z = Convert.ToInt32(step_z_str);
            step_time = Convert.ToInt32(step_time_str);
            rotate_angle = Convert.ToSingle(rotate_angle_str);
            servo_angle = Convert.ToSingle(servo_angle_str);
            is_punching = Convert.ToBoolean(is_punching_str);
            int max_step_number = Math.Max(Math.Max(step_x, step_y), step_z);
            total_time = max_step_number * step_time;
            int rotate_step_number = (int)((rotate_angle / 360.0) * 8000.0);
            rotate_interval_time = (int)((total_time * 1.0) / (rotate_step_number * 1.0));
        }
        public MotionMessage(int s_x, int s_y, int s_z, int s_time, float r_angle, float s_angle, bool i_p)
        {
            category = 0;
            step_x = s_x;
            step_y = s_y;
            step_z = s_z;
            step_time = s_time;
            rotate_angle = r_angle;
            servo_angle = s_angle;
            int max_step_number = Math.Max(Math.Max(step_x, step_y), step_z);
            total_time = max_step_number * step_time;
            int rotate_step_number = (int)((rotate_angle / 360.0) * 8000.0);
            rotate_interval_time = (int)((total_time * 1.0) / (rotate_step_number * 1.0));
            is_punching = i_p;
        }

        //only move x y z
        public MotionMessage(int s_x, int s_y, int s_z, int s_time, bool i_p)
        {
            category = 0;
            step_x = s_x;
            step_y = s_y;
            step_z = s_z;
            step_time = s_time;
            int max_step_number = Math.Max(Math.Max(step_x, step_y), step_z);
            total_time = max_step_number * step_time;
            rotate_angle = 0;
            servo_angle = 361;
            is_punching = i_p;
            rotate_interval_time = 0;

        }

        //only rotate turntable
        public MotionMessage(int r_interval, float r_angle, bool i_p)
        {
            category = 0;
            step_x = 0;
            step_y = 0;
            step_z = 0;
            step_time = 0;
            rotate_interval_time = r_interval;
            rotate_angle = r_angle;
            is_punching = i_p;
            servo_angle = 361;
            total_time = (int)(r_interval * (r_angle / 360) * 8000);
        }

        //only rotate servo
        public MotionMessage(float s_angle, bool i_p)
        {
            category = 0;
            step_x = 0;
            step_y = 0;
            step_z = 0;
            step_time = 0;
            rotate_interval_time = 0;
            rotate_angle = 0;
            is_punching = i_p;
            servo_angle = s_angle;
            total_time = (int)((servo_angle * 332/60) * 1000);
        }

        //only punch
        public MotionMessage(bool i_p)
        {
            category = 0;
            step_x = 0;
            step_y = 0;
            step_z = 0;
            step_time = 0;
            rotate_interval_time = 0;
            rotate_angle = 0;
            is_punching = i_p;
            servo_angle = 361;
            total_time = 0;
        }


        public override string ToString()
        {
            string step_x_str = step_x.ToString();
            string step_y_str = step_y.ToString();
            string step_z_str = step_z.ToString();
            string step_time_str = step_time.ToString();
            //string total_time_str = total_time.ToString();
            string rotate_angle_str = rotate_angle.ToString();
            string servo_angle_str = servo_angle.ToString();
            string is_punching_str;
            if (is_punching) { is_punching_str = "1"; }
            else { is_punching_str = "0"; }
            //string is_punching_str = is_punching.ToString();
            string _ = "_";
            string message = step_x_str + _ + step_y_str + _ + step_z_str + _ + rotate_angle_str + _ + servo_angle_str + _ + is_punching_str;
            return message;
        }
        public string GetSerialString()
        {
            if(category == 0)
            {
                string serial_message = "";
                if (step_x != 0) { serial_message = serial_message + string.Format("X{0}", step_x); }
                if (step_y != 0) { serial_message = serial_message + string.Format("Y{0}", step_y); }
                if (step_z != 0) { serial_message = serial_message + string.Format("Z{0}", step_z); }
                if (step_time != 0) { serial_message = serial_message + string.Format("C{0}", step_time); }
                if (servo_angle < 360)
                {
                    serial_message = serial_message + string.Format("S{0}", servo_angle);
                    if (total_time != 0) { serial_message = serial_message + string.Format("T{0}", total_time); }
                }
                string is_punching_str;
                if (is_punching) { is_punching_str = "1"; }
                else { is_punching_str = "0"; }
                serial_message = serial_message + string.Format("P{0}", is_punching_str);
                return serial_message;
            }
            else
            {
                string serial_message;
                switch (category)
                {
                    case 1:
                        serial_message = "G";
                        break;
                    case 2:
                        serial_message = "O";
                        break;
                    case 3:
                        serial_message = "x-1";
                        break;
                    case 4:
                        serial_message = "x0";
                        break;
                    case 5:
                        serial_message = "x1";
                        break;
                    case 6:
                        serial_message = "y-1";
                        break;
                    case 7:
                        serial_message = "y0";
                        break;
                    case 8:
                        serial_message = "y1";
                        break;
                    case 9:
                        serial_message = "z-1";
                        break;
                    case 10:
                        serial_message = "z0";
                        break;
                    case 11:
                        serial_message = "z1";
                        break;
                    case 12:
                        serial_message = "R";
                        break;
                    default:
                        serial_message = "G";
                        break;
                }
                return serial_message;
            }
            
        }

        public float GetRotateAngle()
        {
            return rotate_angle;
        }
        public int GetRotateIngerval()
        {
            return rotate_interval_time;
        }
    }
}
