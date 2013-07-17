namespace _10bClient
{
    partial class Main
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
            this.tabRooms = new System.Windows.Forms.TabControl();
            this.tbpStatus = new System.Windows.Forms.TabPage();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lstDummy = new System.Windows.Forms.ListView();
            this.tabRooms.SuspendLayout();
            this.tbpStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabRooms
            // 
            this.tabRooms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabRooms.Controls.Add(this.tbpStatus);
            this.tabRooms.Location = new System.Drawing.Point(12, 12);
            this.tabRooms.Name = "tabRooms";
            this.tabRooms.SelectedIndex = 0;
            this.tabRooms.Size = new System.Drawing.Size(749, 549);
            this.tabRooms.TabIndex = 0;
            // 
            // tbpStatus
            // 
            this.tbpStatus.Controls.Add(this.lstDummy);
            this.tbpStatus.Controls.Add(this.txtStatus);
            this.tbpStatus.Location = new System.Drawing.Point(4, 22);
            this.tbpStatus.Name = "tbpStatus";
            this.tbpStatus.Size = new System.Drawing.Size(741, 523);
            this.tbpStatus.TabIndex = 0;
            this.tbpStatus.Text = "Status";
            this.tbpStatus.UseVisualStyleBackColor = true;
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(0, 0);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(611, 520);
            this.txtStatus.TabIndex = 0;
            this.txtStatus.Text = "";
            // 
            // txtMsg
            // 
            this.txtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMsg.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtMsg.Location = new System.Drawing.Point(12, 583);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(668, 20);
            this.txtMsg.TabIndex = 1;
            this.txtMsg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMsg_KeyDown);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(686, 581);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lstDummy
            // 
            this.lstDummy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDummy.Location = new System.Drawing.Point(617, 0);
            this.lstDummy.Name = "lstDummy";
            this.lstDummy.Size = new System.Drawing.Size(121, 520);
            this.lstDummy.TabIndex = 1;
            this.lstDummy.UseCompatibleStateImageBehavior = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 616);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.tabRooms);
            this.Name = "Main";
            this.Text = "10b.NET";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.tabRooms.ResumeLayout(false);
            this.tbpStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabRooms;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TabPage tbpStatus;
        private System.Windows.Forms.RichTextBox txtStatus;
        private System.Windows.Forms.ListView lstDummy;



    }
}

