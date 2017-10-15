namespace My_Rhino5_rs11_project_8
{
    partial class SerialPort_UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            this.comboBox_PortName = new System.Windows.Forms.ComboBox();
            this.StartPort = new System.Windows.Forms.Button();
            this.labelX = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelZ = new System.Windows.Forms.Label();
            this.DecreaseX = new System.Windows.Forms.Button();
            this.IncreaseX = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.DecreaseY = new System.Windows.Forms.Button();
            this.IncreaseY = new System.Windows.Forms.Button();
            this.DecreaseZ = new System.Windows.Forms.Button();
            this.IncreaseZ = new System.Windows.Forms.Button();
            this.RefreshPort = new System.Windows.Forms.Button();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxZ = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.GoToPosition = new System.Windows.Forms.Button();
            this.Speed_Slow = new System.Windows.Forms.Button();
            this.Speed_Medium = new System.Windows.Forms.Button();
            this.Speed_Fast = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.PrintQueue = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_TurnTable = new System.Windows.Forms.TextBox();
            this.Rotate_TurnTable = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_Servo = new System.Windows.Forms.TextBox();
            this.Rotate_Servo = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(36, 42);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(154, 33);
            label1.TabIndex = 0;
            label1.Text = "Port Name";
            // 
            // comboBox_PortName
            // 
            this.comboBox_PortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PortName.FormattingEnabled = true;
            this.comboBox_PortName.Location = new System.Drawing.Point(221, 42);
            this.comboBox_PortName.Name = "comboBox_PortName";
            this.comboBox_PortName.Size = new System.Drawing.Size(269, 33);
            this.comboBox_PortName.TabIndex = 1;
            // 
            // StartPort
            // 
            this.StartPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartPort.Location = new System.Drawing.Point(42, 103);
            this.StartPort.Name = "StartPort";
            this.StartPort.Size = new System.Drawing.Size(258, 46);
            this.StartPort.TabIndex = 2;
            this.StartPort.Text = "Start Port";
            this.StartPort.UseVisualStyleBackColor = true;
            this.StartPort.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX.Location = new System.Drawing.Point(108, 201);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(29, 31);
            this.labelX.TabIndex = 3;
            this.labelX.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(47, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 31);
            this.label2.TabIndex = 4;
            this.label2.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(46, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 31);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelY.Location = new System.Drawing.Point(108, 264);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(29, 31);
            this.labelY.TabIndex = 6;
            this.labelY.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(46, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 31);
            this.label4.TabIndex = 7;
            this.label4.Text = "Z";
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZ.Location = new System.Drawing.Point(108, 324);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(29, 31);
            this.labelZ.TabIndex = 8;
            this.labelZ.Text = "0";
            // 
            // DecreaseX
            // 
            this.DecreaseX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DecreaseX.Location = new System.Drawing.Point(199, 193);
            this.DecreaseX.Name = "DecreaseX";
            this.DecreaseX.Size = new System.Drawing.Size(108, 47);
            this.DecreaseX.TabIndex = 9;
            this.DecreaseX.Text = "<-";
            this.DecreaseX.UseVisualStyleBackColor = true;
            this.DecreaseX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DecreaseX_MouseDown);
            this.DecreaseX.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DecreaseX_MouseUp);
            // 
            // IncreaseX
            // 
            this.IncreaseX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncreaseX.Location = new System.Drawing.Point(337, 193);
            this.IncreaseX.Name = "IncreaseX";
            this.IncreaseX.Size = new System.Drawing.Size(113, 47);
            this.IncreaseX.TabIndex = 10;
            this.IncreaseX.Text = "->";
            this.IncreaseX.UseVisualStyleBackColor = true;
            this.IncreaseX.Click += new System.EventHandler(this.IncreaseX_Click);
            this.IncreaseX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IncreaseX_MouseDown);
            this.IncreaseX.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IncreaseX_MouseUp);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(337, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(243, 46);
            this.button1.TabIndex = 11;
            this.button1.Text = "End Port";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // DecreaseY
            // 
            this.DecreaseY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DecreaseY.Location = new System.Drawing.Point(199, 257);
            this.DecreaseY.Name = "DecreaseY";
            this.DecreaseY.Size = new System.Drawing.Size(108, 45);
            this.DecreaseY.TabIndex = 12;
            this.DecreaseY.Text = "<-";
            this.DecreaseY.UseVisualStyleBackColor = true;
            this.DecreaseY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DecreaseY_MouseDown);
            this.DecreaseY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DecreaseY_MouseUp);
            // 
            // IncreaseY
            // 
            this.IncreaseY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncreaseY.Location = new System.Drawing.Point(337, 257);
            this.IncreaseY.Name = "IncreaseY";
            this.IncreaseY.Size = new System.Drawing.Size(113, 45);
            this.IncreaseY.TabIndex = 13;
            this.IncreaseY.Text = "->";
            this.IncreaseY.UseVisualStyleBackColor = true;
            this.IncreaseY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IncreaseY_MouseDown);
            this.IncreaseY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IncreaseY_MouseUp);
            // 
            // DecreaseZ
            // 
            this.DecreaseZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DecreaseZ.Location = new System.Drawing.Point(199, 324);
            this.DecreaseZ.Name = "DecreaseZ";
            this.DecreaseZ.Size = new System.Drawing.Size(108, 44);
            this.DecreaseZ.TabIndex = 14;
            this.DecreaseZ.Text = "<-";
            this.DecreaseZ.UseVisualStyleBackColor = true;
            this.DecreaseZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DecreaseZ_MouseDown);
            this.DecreaseZ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DecreaseZ_MouseUp);
            // 
            // IncreaseZ
            // 
            this.IncreaseZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncreaseZ.Location = new System.Drawing.Point(337, 324);
            this.IncreaseZ.Name = "IncreaseZ";
            this.IncreaseZ.Size = new System.Drawing.Size(113, 44);
            this.IncreaseZ.TabIndex = 15;
            this.IncreaseZ.Text = "->";
            this.IncreaseZ.UseVisualStyleBackColor = true;
            this.IncreaseZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IncreaseZ_MouseDown_1);
            this.IncreaseZ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IncreaseZ_MouseUp);
            // 
            // RefreshPort
            // 
            this.RefreshPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshPort.Location = new System.Drawing.Point(521, 26);
            this.RefreshPort.Name = "RefreshPort";
            this.RefreshPort.Size = new System.Drawing.Size(143, 49);
            this.RefreshPort.TabIndex = 16;
            this.RefreshPort.Text = "Refresh";
            this.RefreshPort.UseVisualStyleBackColor = true;
            this.RefreshPort.Click += new System.EventHandler(this.RefreshPort_Click);
            // 
            // textBoxX
            // 
            this.textBoxX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxX.Location = new System.Drawing.Point(82, 420);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(100, 38);
            this.textBoxX.TabIndex = 17;
            this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
            // 
            // textBoxY
            // 
            this.textBoxY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxY.Location = new System.Drawing.Point(238, 420);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(100, 38);
            this.textBoxY.TabIndex = 18;
            // 
            // textBoxZ
            // 
            this.textBoxZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxZ.Location = new System.Drawing.Point(390, 420);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.Size = new System.Drawing.Size(100, 38);
            this.textBoxZ.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(44, 423);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 31);
            this.label5.TabIndex = 20;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(200, 423);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 31);
            this.label6.TabIndex = 21;
            this.label6.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(353, 423);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 31);
            this.label7.TabIndex = 22;
            this.label7.Text = "Z";
            // 
            // GoToPosition
            // 
            this.GoToPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GoToPosition.Location = new System.Drawing.Point(531, 411);
            this.GoToPosition.Name = "GoToPosition";
            this.GoToPosition.Size = new System.Drawing.Size(148, 54);
            this.GoToPosition.TabIndex = 23;
            this.GoToPosition.Text = "Go To";
            this.GoToPosition.UseVisualStyleBackColor = true;
            this.GoToPosition.Click += new System.EventHandler(this.GoToPosition_Click);
            // 
            // Speed_Slow
            // 
            this.Speed_Slow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Speed_Slow.Location = new System.Drawing.Point(597, 204);
            this.Speed_Slow.Name = "Speed_Slow";
            this.Speed_Slow.Size = new System.Drawing.Size(151, 45);
            this.Speed_Slow.TabIndex = 24;
            this.Speed_Slow.Text = "Slow";
            this.Speed_Slow.UseVisualStyleBackColor = true;
            this.Speed_Slow.Click += new System.EventHandler(this.Speed_Slow_Click);
            // 
            // Speed_Medium
            // 
            this.Speed_Medium.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Speed_Medium.Location = new System.Drawing.Point(597, 264);
            this.Speed_Medium.Name = "Speed_Medium";
            this.Speed_Medium.Size = new System.Drawing.Size(151, 45);
            this.Speed_Medium.TabIndex = 25;
            this.Speed_Medium.Text = "Medium";
            this.Speed_Medium.UseVisualStyleBackColor = true;
            this.Speed_Medium.Click += new System.EventHandler(this.Speed_Medium_Click);
            // 
            // Speed_Fast
            // 
            this.Speed_Fast.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Speed_Fast.Location = new System.Drawing.Point(597, 323);
            this.Speed_Fast.Name = "Speed_Fast";
            this.Speed_Fast.Size = new System.Drawing.Size(151, 45);
            this.Speed_Fast.TabIndex = 26;
            this.Speed_Fast.Text = "Fast";
            this.Speed_Fast.UseVisualStyleBackColor = true;
            this.Speed_Fast.Click += new System.EventHandler(this.Speed_Fast_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(601, 161);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 31);
            this.label8.TabIndex = 27;
            this.label8.Text = "Speed";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(25, 683);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(165, 31);
            this.label9.TabIndex = 28;
            this.label9.Text = "File Address";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(206, 683);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(317, 38);
            this.textBox2.TabIndex = 30;
            this.textBox2.Text = "C:/TestFile.txt";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(541, 683);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 42);
            this.button2.TabIndex = 31;
            this.button2.Text = "Run File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // PrintQueue
            // 
            this.PrintQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrintQueue.Location = new System.Drawing.Point(35, 733);
            this.PrintQueue.Name = "PrintQueue";
            this.PrintQueue.Size = new System.Drawing.Size(159, 51);
            this.PrintQueue.TabIndex = 32;
            this.PrintQueue.Text = "PrintQueue";
            this.PrintQueue.UseVisualStyleBackColor = true;
            this.PrintQueue.Click += new System.EventHandler(this.PrintQueue_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(52, 486);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(145, 31);
            this.label10.TabIndex = 33;
            this.label10.Text = "Turn Table";
            // 
            // textBox_TurnTable
            // 
            this.textBox_TurnTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_TurnTable.Location = new System.Drawing.Point(206, 486);
            this.textBox_TurnTable.Name = "textBox_TurnTable";
            this.textBox_TurnTable.Size = new System.Drawing.Size(132, 38);
            this.textBox_TurnTable.TabIndex = 34;
            // 
            // Rotate_TurnTable
            // 
            this.Rotate_TurnTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rotate_TurnTable.Location = new System.Drawing.Point(359, 480);
            this.Rotate_TurnTable.Name = "Rotate_TurnTable";
            this.Rotate_TurnTable.Size = new System.Drawing.Size(133, 48);
            this.Rotate_TurnTable.TabIndex = 35;
            this.Rotate_TurnTable.Text = "Rotate";
            this.Rotate_TurnTable.UseVisualStyleBackColor = true;
            this.Rotate_TurnTable.UseWaitCursor = true;
            this.Rotate_TurnTable.Click += new System.EventHandler(this.Rotate_TurnTable_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(58, 549);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(161, 31);
            this.label11.TabIndex = 36;
            this.label11.Text = "Servo Motor";
            // 
            // textBox_Servo
            // 
            this.textBox_Servo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Servo.Location = new System.Drawing.Point(221, 549);
            this.textBox_Servo.Name = "textBox_Servo";
            this.textBox_Servo.Size = new System.Drawing.Size(155, 38);
            this.textBox_Servo.TabIndex = 37;
            // 
            // Rotate_Servo
            // 
            this.Rotate_Servo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rotate_Servo.Location = new System.Drawing.Point(390, 543);
            this.Rotate_Servo.Name = "Rotate_Servo";
            this.Rotate_Servo.Size = new System.Drawing.Size(133, 48);
            this.Rotate_Servo.TabIndex = 38;
            this.Rotate_Servo.Text = "Rotate";
            this.Rotate_Servo.UseVisualStyleBackColor = true;
            this.Rotate_Servo.Click += new System.EventHandler(this.Rotate_Servo_Click);
            // 
            // SerialPort_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Rotate_Servo);
            this.Controls.Add(this.textBox_Servo);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Rotate_TurnTable);
            this.Controls.Add(this.textBox_TurnTable);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.PrintQueue);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Speed_Fast);
            this.Controls.Add(this.Speed_Medium);
            this.Controls.Add(this.Speed_Slow);
            this.Controls.Add(this.GoToPosition);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxZ);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.RefreshPort);
            this.Controls.Add(this.IncreaseZ);
            this.Controls.Add(this.DecreaseZ);
            this.Controls.Add(this.IncreaseY);
            this.Controls.Add(this.DecreaseY);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.IncreaseX);
            this.Controls.Add(this.DecreaseX);
            this.Controls.Add(this.labelZ);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.StartPort);
            this.Controls.Add(this.comboBox_PortName);
            this.Controls.Add(label1);
            this.Name = "SerialPort_UserControl";
            this.Size = new System.Drawing.Size(769, 815);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_PortName;
        private System.Windows.Forms.Button StartPort;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.Button DecreaseX;
        private System.Windows.Forms.Button IncreaseX;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button DecreaseY;
        private System.Windows.Forms.Button IncreaseY;
        private System.Windows.Forms.Button DecreaseZ;
        private System.Windows.Forms.Button IncreaseZ;
        private System.Windows.Forms.Button RefreshPort;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxZ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button GoToPosition;
        private System.Windows.Forms.Button Speed_Slow;
        private System.Windows.Forms.Button Speed_Medium;
        private System.Windows.Forms.Button Speed_Fast;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button PrintQueue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_TurnTable;
        private System.Windows.Forms.Button Rotate_TurnTable;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_Servo;
        private System.Windows.Forms.Button Rotate_Servo;
    }
}
