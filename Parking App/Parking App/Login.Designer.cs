namespace Parking_App
{
    partial class Login_form
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
            this.username_txtbx = new System.Windows.Forms.TextBox();
            this.password_txtbx = new System.Windows.Forms.TextBox();
            this.login_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // username_txtbx
            // 
            this.username_txtbx.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.username_txtbx.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username_txtbx.Location = new System.Drawing.Point(123, 194);
            this.username_txtbx.Name = "username_txtbx";
            this.username_txtbx.Size = new System.Drawing.Size(215, 43);
            this.username_txtbx.TabIndex = 0;
            this.username_txtbx.Text = "username";
            this.username_txtbx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.username_txtbx.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // password_txtbx
            // 
            this.password_txtbx.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.password_txtbx.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_txtbx.Location = new System.Drawing.Point(123, 262);
            this.password_txtbx.Name = "password_txtbx";
            this.password_txtbx.PasswordChar = '*';
            this.password_txtbx.Size = new System.Drawing.Size(215, 43);
            this.password_txtbx.TabIndex = 1;
            this.password_txtbx.Text = "password";
            this.password_txtbx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.password_txtbx.TextChanged += new System.EventHandler(this.password_txtbx_TextChanged);
            // 
            // login_btn
            // 
            this.login_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.login_btn.BackColor = System.Drawing.Color.Lime;
            this.login_btn.FlatAppearance.BorderSize = 0;
            this.login_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.login_btn.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.login_btn.ForeColor = System.Drawing.SystemColors.Window;
            this.login_btn.Location = new System.Drawing.Point(123, 332);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(215, 45);
            this.login_btn.TabIndex = 2;
            this.login_btn.Text = "Login";
            this.login_btn.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 81);
            this.label1.TabIndex = 3;
            this.label1.Text = "ParkingApp";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Login_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 497);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.login_btn);
            this.Controls.Add(this.password_txtbx);
            this.Controls.Add(this.username_txtbx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Login_form";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox username_txtbx;
        private System.Windows.Forms.TextBox password_txtbx;
        private System.Windows.Forms.Button login_btn;
        private System.Windows.Forms.Label label1;
    }
}

