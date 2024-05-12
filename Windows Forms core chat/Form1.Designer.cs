namespace Windows_Forms_Chat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            MyPortTextBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            serverPortTextBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            ServerIPTextBox = new System.Windows.Forms.TextBox();
            ChatTextBox = new System.Windows.Forms.TextBox();
            TypeTextBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            HostButton = new System.Windows.Forms.Button();
            JoinButton = new System.Windows.Forms.Button();
            SendButton = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            button6 = new System.Windows.Forms.Button();
            button7 = new System.Windows.Forms.Button();
            button8 = new System.Windows.Forms.Button();
            button9 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 10);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(49, 15);
            label1.TabIndex = 0;
            label1.Text = "My Port";
            // 
            // MyPortTextBox
            // 
            MyPortTextBox.Location = new System.Drawing.Point(11, 28);
            MyPortTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MyPortTextBox.Name = "MyPortTextBox";
            MyPortTextBox.Size = new System.Drawing.Size(110, 23);
            MyPortTextBox.TabIndex = 1;
            MyPortTextBox.Text = "6666";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(220, 10);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(64, 15);
            label2.TabIndex = 2;
            label2.Text = "Server Port";
            // 
            // serverPortTextBox
            // 
            serverPortTextBox.Location = new System.Drawing.Point(220, 28);
            serverPortTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            serverPortTextBox.Name = "serverPortTextBox";
            serverPortTextBox.Size = new System.Drawing.Size(110, 23);
            serverPortTextBox.TabIndex = 3;
            serverPortTextBox.Text = "6666";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(370, 10);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(51, 15);
            label3.TabIndex = 4;
            label3.Text = "server IP";
            // 
            // ServerIPTextBox
            // 
            ServerIPTextBox.Location = new System.Drawing.Point(370, 28);
            ServerIPTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ServerIPTextBox.Name = "ServerIPTextBox";
            ServerIPTextBox.Size = new System.Drawing.Size(140, 23);
            ServerIPTextBox.TabIndex = 5;
            ServerIPTextBox.Text = "127.0.0.1";
            // 
            // ChatTextBox
            // 
            ChatTextBox.Location = new System.Drawing.Point(10, 125);
            ChatTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            ChatTextBox.Multiline = true;
            ChatTextBox.Name = "ChatTextBox";
            ChatTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ChatTextBox.Size = new System.Drawing.Size(530, 155);
            ChatTextBox.TabIndex = 6;
            ChatTextBox.Text = "\r\n";
            // 
            // TypeTextBox
            // 
            TypeTextBox.Location = new System.Drawing.Point(52, 292);
            TypeTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            TypeTextBox.Name = "TypeTextBox";
            TypeTextBox.Size = new System.Drawing.Size(392, 23);
            TypeTextBox.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(10, 292);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(35, 15);
            label4.TabIndex = 8;
            label4.Text = "Chat:";
            // 
            // HostButton
            // 
            HostButton.Location = new System.Drawing.Point(11, 68);
            HostButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HostButton.Name = "HostButton";
            HostButton.Size = new System.Drawing.Size(82, 22);
            HostButton.TabIndex = 9;
            HostButton.Text = "Host Server";
            HostButton.UseVisualStyleBackColor = true;
            HostButton.Click += HostButton_Click;
            // 
            // JoinButton
            // 
            JoinButton.Location = new System.Drawing.Point(220, 68);
            JoinButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            JoinButton.Name = "JoinButton";
            JoinButton.Size = new System.Drawing.Size(82, 22);
            JoinButton.TabIndex = 10;
            JoinButton.Text = "Join Server";
            JoinButton.UseVisualStyleBackColor = true;
            JoinButton.Click += JoinButton_Click;
            // 
            // SendButton
            // 
            SendButton.Location = new System.Drawing.Point(458, 292);
            SendButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SendButton.Name = "SendButton";
            SendButton.Size = new System.Drawing.Size(82, 22);
            SendButton.TabIndex = 11;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(151, 26);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(23, 15);
            label5.TabIndex = 12;
            label5.Text = "OR";
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.Violet;
            button1.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button1.Location = new System.Drawing.Point(640, 26);
            button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(82, 74);
            button1.TabIndex = 13;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = System.Drawing.Color.Violet;
            button2.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button2.Location = new System.Drawing.Point(727, 26);
            button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(82, 74);
            button2.TabIndex = 13;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = System.Drawing.Color.Violet;
            button3.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button3.Location = new System.Drawing.Point(815, 26);
            button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(82, 74);
            button3.TabIndex = 13;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = System.Drawing.Color.Violet;
            button4.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button4.Location = new System.Drawing.Point(640, 104);
            button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(82, 74);
            button4.TabIndex = 13;
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackColor = System.Drawing.Color.Violet;
            button5.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button5.Location = new System.Drawing.Point(727, 104);
            button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(82, 74);
            button5.TabIndex = 13;
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.BackColor = System.Drawing.Color.Violet;
            button6.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button6.Location = new System.Drawing.Point(815, 104);
            button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(82, 74);
            button6.TabIndex = 13;
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.BackColor = System.Drawing.Color.Violet;
            button7.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button7.Location = new System.Drawing.Point(640, 182);
            button7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(82, 74);
            button7.TabIndex = 13;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.BackColor = System.Drawing.Color.Violet;
            button8.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button8.Location = new System.Drawing.Point(727, 182);
            button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(82, 74);
            button8.TabIndex = 13;
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.BackColor = System.Drawing.Color.Violet;
            button9.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            button9.Location = new System.Drawing.Point(815, 182);
            button9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(82, 74);
            button9.TabIndex = 13;
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1009, 418);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(SendButton);
            Controls.Add(JoinButton);
            Controls.Add(HostButton);
            Controls.Add(label4);
            Controls.Add(TypeTextBox);
            Controls.Add(ChatTextBox);
            Controls.Add(ServerIPTextBox);
            Controls.Add(label3);
            Controls.Add(serverPortTextBox);
            Controls.Add(label2);
            Controls.Add(MyPortTextBox);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MyPortTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverPortTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ServerIPTextBox;
        private System.Windows.Forms.TextBox ChatTextBox;
        private System.Windows.Forms.TextBox TypeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button HostButton;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
    }
}

