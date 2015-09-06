using System.ComponentModel;
using System.Windows.Forms;

namespace UnknownREEncrypter
{
    partial class StartupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupForm));
            this.menuBarTitle = new System.Windows.Forms.Label();
            this.pbTopBar = new System.Windows.Forms.PictureBox();
            this.pictureBoxBottomBar = new System.Windows.Forms.PictureBox();
            this.pbRightBar = new System.Windows.Forms.PictureBox();
            this.pbLeftBar = new System.Windows.Forms.PictureBox();
            this.exitLabel = new System.Windows.Forms.Label();
            this.menuBar = new System.Windows.Forms.PictureBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbTopBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBottomBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRightBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeftBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuBarTitle
            // 
            this.menuBarTitle.AutoSize = true;
            this.menuBarTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.menuBarTitle.Font = new System.Drawing.Font("Calibri", 17F, System.Drawing.FontStyle.Bold);
            this.menuBarTitle.ForeColor = System.Drawing.Color.White;
            this.menuBarTitle.Location = new System.Drawing.Point(10, 1);
            this.menuBarTitle.Name = "menuBarTitle";
            this.menuBarTitle.Size = new System.Drawing.Size(224, 28);
            this.menuBarTitle.TabIndex = 16;
            this.menuBarTitle.Text = "UnknownRE Encrypter";
            this.menuBarTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblMenuBarTitle_MouseDown);
            this.menuBarTitle.MouseEnter += new System.EventHandler(this.menuBarTitle_MouseEnter);
            this.menuBarTitle.MouseLeave += new System.EventHandler(this.menuBarTitle_MouseLeave);
            this.menuBarTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menuBarTitle_MouseMove);
            this.menuBarTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblMenuBarTitle_MouseUp);
            // 
            // pbTopBar
            // 
            this.pbTopBar.BackColor = System.Drawing.Color.Gainsboro;
            this.pbTopBar.Location = new System.Drawing.Point(0, 30);
            this.pbTopBar.Name = "pbTopBar";
            this.pbTopBar.Size = new System.Drawing.Size(500, 5);
            this.pbTopBar.TabIndex = 14;
            this.pbTopBar.TabStop = false;
            // 
            // pictureBoxBottomBar
            // 
            this.pictureBoxBottomBar.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBoxBottomBar.Location = new System.Drawing.Point(0, 184);
            this.pictureBoxBottomBar.Name = "pictureBoxBottomBar";
            this.pictureBoxBottomBar.Size = new System.Drawing.Size(500, 5);
            this.pictureBoxBottomBar.TabIndex = 13;
            this.pictureBoxBottomBar.TabStop = false;
            // 
            // pbRightBar
            // 
            this.pbRightBar.BackColor = System.Drawing.Color.Gainsboro;
            this.pbRightBar.Location = new System.Drawing.Point(495, 30);
            this.pbRightBar.Name = "pbRightBar";
            this.pbRightBar.Size = new System.Drawing.Size(5, 159);
            this.pbRightBar.TabIndex = 12;
            this.pbRightBar.TabStop = false;
            // 
            // pbLeftBar
            // 
            this.pbLeftBar.BackColor = System.Drawing.Color.Gainsboro;
            this.pbLeftBar.Location = new System.Drawing.Point(0, 30);
            this.pbLeftBar.Name = "pbLeftBar";
            this.pbLeftBar.Size = new System.Drawing.Size(5, 159);
            this.pbLeftBar.TabIndex = 11;
            this.pbLeftBar.TabStop = false;
            // 
            // exitLabel
            // 
            this.exitLabel.AutoSize = true;
            this.exitLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.exitLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitLabel.ForeColor = System.Drawing.Color.White;
            this.exitLabel.Location = new System.Drawing.Point(240, 0);
            this.exitLabel.Name = "exitLabel";
            this.exitLabel.Size = new System.Drawing.Size(31, 29);
            this.exitLabel.TabIndex = 10;
            this.exitLabel.Text = "X";
            this.exitLabel.Click += new System.EventHandler(this.exitLabel_Click);
            this.exitLabel.MouseEnter += new System.EventHandler(this.exitLabel_MouseEnter);
            this.exitLabel.MouseLeave += new System.EventHandler(this.exitLabel_MouseLeave);
            // 
            // menuBar
            // 
            this.menuBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(500, 30);
            this.menuBar.TabIndex = 9;
            this.menuBar.TabStop = false;
            this.menuBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuBar_MouseDown);
            this.menuBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menuBar_MouseMove);
            this.menuBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.menuBar_MouseUp);
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinue.ForeColor = System.Drawing.Color.White;
            this.btnContinue.Location = new System.Drawing.Point(58, 119);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(159, 34);
            this.btnContinue.TabIndex = 20;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::UnknownREEncrypter.Properties.Resources.Logo;
            this.pictureBox1.Location = new System.Drawing.Point(100, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(76, 76);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(271, 160);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.menuBarTitle);
            this.Controls.Add(this.pbTopBar);
            this.Controls.Add(this.pictureBoxBottomBar);
            this.Controls.Add(this.pbRightBar);
            this.Controls.Add(this.pbLeftBar);
            this.Controls.Add(this.exitLabel);
            this.Controls.Add(this.menuBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.Name = "StartupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UnknownRE Encrypter";
            ((System.ComponentModel.ISupportInitialize)(this.pbTopBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBottomBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRightBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeftBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label menuBarTitle;
        private PictureBox pbTopBar;
        private PictureBox pictureBoxBottomBar;
        private PictureBox pbRightBar;
        private PictureBox pbLeftBar;
        private Label exitLabel;
        private PictureBox menuBar;
        private Button btnContinue;
        private PictureBox pictureBox1;
    }
}