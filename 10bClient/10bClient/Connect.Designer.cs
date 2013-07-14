namespace _10bClient
{
    partial class Connect
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(303, 168);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(63, 12);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(315, 20);
            this.txtHost.TabIndex = 1;
            this.txtHost.Text = "10bit.danopia.net";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(63, 38);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(315, 20);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "10817";
            // 
            // txtUser
            //
            this.txtUser.Location = new System.Drawing.Point(63, 64);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(315, 20);
            this.txtUser.TabIndex = 3;
            this.txtUser.Text = "ldunn";
            //
            // txtPass
            //
            this.txtPass.Location = new System.Drawing.Point(63, 90);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(315, 20);
            this.txtPass.TabIndex = 4;
            this.txtPass.Text = "SECRETPASSWORDOMG";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Host";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "User";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Pass";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Port";
            // 
            // Connect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 203);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.btnConnect);
            this.Name = "Connect";
            this.Text = "Connect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}