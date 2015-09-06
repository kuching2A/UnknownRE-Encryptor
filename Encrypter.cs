using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using UnknownREEncrypter.Properties;

namespace UnknownREEncrypter
{
    public class UnknownREEncrypter : Form
    {
        private Button _button1;
        private Button _button2;
        private Button _button3;

        private int _cursorXPositionDifference;
        private int _cursorYPositionDifference;
        private Label _exitLabel;
        private GroupBox _groupBox1;
        private GroupBox _groupBox2;
        private GroupBox _groupBox3;
        private Label _label1;
        private bool _leftMouseDown;
        private PictureBox _menuBar;
        private Label _menuBarTitleLabel;
        private OpenFileDialog _openFile;
        private Panel _panel1;
        private PictureBox _pictureBox1;
        private RadioButton _radioButtonNormal;
        private RadioButton _radioButtonPortable;
        private SaveFileDialog _saveFile;
        private TextBox _statusBox;
        private TextBox _textBox1;
        private TextBox _textBox2;
        private ToolTip _toolTipNormal;
        private ToolTip _toolTipPortable;
        private IContainer components;

        public UnknownREEncrypter()
        {
            InitializeComponent();
        }

        public string Status
        {
            get { return _statusBox.Text; }
            set { _statusBox.Text = value; }
        }

        private void menuBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_leftMouseDown)
            {
                if (ActiveForm != null)
                {
                    ActiveForm.Location = new Point(MousePosition.X - _cursorXPositionDifference,
                        MousePosition.Y - _cursorYPositionDifference);
                }
            }
        }

        private void menuBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (ActiveForm != null)
            {
                _cursorXPositionDifference = MousePosition.X - ActiveForm.Location.X;
                _cursorYPositionDifference = MousePosition.Y - ActiveForm.Location.Y;
                _leftMouseDown = true;
            }
        }

        private void menuBar_MouseUp(object sender, MouseEventArgs e)
        {
            _leftMouseDown = false;
        }

        private void exitLabel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void exitLabel_MouseEnter(object sender, EventArgs e)
        {
            _exitLabel.ForeColor = Color.Gold;
        }

        private void exitLabel_MouseLeave(object sender, EventArgs e)
        {
            _exitLabel.ForeColor = Color.White;
        }

        private void minimizeLabel_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void menuBarTitleLabel_MouseEnter(object sender, EventArgs e)
        {
            _menuBarTitleLabel.ForeColor = Color.Gold;
        }

        private void menuBarTitleLabel_MouseLeave(object sender, EventArgs e)
        {
            _menuBarTitleLabel.ForeColor = Color.White;
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                var text = _textBox2.Text;

                if (File.Exists(text) && (_textBox1.Text != string.Empty))
                {
                    var extension = ".*";
                    var startIndex = text.LastIndexOf('.');
                    extension = text.Substring(startIndex);

                    Status = "Encrypting bytes...";
                    var fileContent = Encryption.Encrypt(File.ReadAllBytes(text), _textBox1.Text);

                    // Read the resource to a byte array
                    var soundFile = Resources.soundFile;

                    Status = "Done encrypting bytes.";

                    _saveFile.Filter = extension + @" Files|*" + extension;

                    if (_radioButtonPortable.Checked)
                    {
                        _saveFile.Filter = @"Executable File|*.exe";
                    }

                    _saveFile.Title = @"Select where to save your encrypted file";

                    if (_saveFile.ShowDialog() == DialogResult.OK)
                    {
                        if (_radioButtonPortable.Checked)
                        {
                            var source = Resources.Portable.Replace("[output-replace]",
                                "+@\"" + text.Substring(text.LastIndexOf('\\')) + "\"");
                            var fileName = Path.Combine(Application.StartupPath, "Encrypted.resources");
                            var icon = Resources.icon;

                            using (var writer = new ResourceWriter(fileName))
                            {
                                writer.AddResource("file", fileContent);
                                writer.AddResource("UnknownREIntro", soundFile);
                                writer.AddResource("extension", extension);
                                writer.Generate();
                            }

                            Status = Compiler.CompileFromSource(source, _saveFile.FileName, fileName, icon)
                                ? "Portable file compiled successfully: " + _saveFile.FileName
                                : "Portable file could not be compiled";

                            File.Delete(fileName);
                        }
                        else
                        {
                            File.WriteAllBytes(_saveFile.FileName, fileContent);
                            Status = "Encrypted file saved to: " + _saveFile.FileName;
                        }
                    }
                }
                else
                {
                    Status = "Error: Please enter a password and a valid filename!";
                }
            }
            catch
            {
                // Do nothing
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            Status = "";
            var text = _textBox2.Text;

            if (File.Exists(text) && (_textBox1.Text != string.Empty))
            {
                var startIndex = text.LastIndexOf('.');

                var str2 = text.Substring(startIndex);
                Status = "Attempting to decrypt bytes...";

                var bytes = Encryption.Decrypt(File.ReadAllBytes(text), _textBox1.Text);

                if (bytes == null)
                {
                    Status = "Error: Wrong decryption key!";
                }
                else
                {
                    Status = "Done decrypting bytes.";
                    _saveFile.Filter = str2 + @" Files|*" + str2;
                    _saveFile.Title = @"Select where to save your decrypted file";

                    if (_saveFile.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(_saveFile.FileName, bytes);
                        Status = "Decrypted file saved to: " + _saveFile.FileName;
                    }
                }
            }
            else
            {
                Status = "Error: Please enter a password and a valid filename!";
            }
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            _openFile.Title = @"Select File To Be Encrypted/Decrypted";
            _openFile.Filter = @"All Files|*.*";

            if (_openFile.ShowDialog() == DialogResult.OK)
            {
                _textBox2.Text = _openFile.FileName;
            }
        }

        private void InitializeComponent()
        {
            components = new Container();
            var resources = new ComponentResourceManager(typeof (UnknownREEncrypter));
            _statusBox = new TextBox();
            _panel1 = new Panel();
            _label1 = new Label();
            _toolTipNormal = new ToolTip(components);
            _radioButtonNormal = new RadioButton();
            _toolTipPortable = new ToolTip(components);
            _radioButtonPortable = new RadioButton();
            _groupBox3 = new GroupBox();
            _groupBox2 = new GroupBox();
            _textBox1 = new TextBox();
            _textBox2 = new TextBox();
            _button3 = new Button();
            _groupBox1 = new GroupBox();
            _pictureBox1 = new PictureBox();
            _button2 = new Button();
            _saveFile = new SaveFileDialog();
            _button1 = new Button();
            _openFile = new OpenFileDialog();
            _menuBarTitleLabel = new Label();
            _exitLabel = new Label();
            _menuBar = new PictureBox();
            _panel1.SuspendLayout();
            _groupBox3.SuspendLayout();
            _groupBox2.SuspendLayout();
            _groupBox1.SuspendLayout();
            ((ISupportInitialize) (_pictureBox1)).BeginInit();
            ((ISupportInitialize) (_menuBar)).BeginInit();
            SuspendLayout();
            // 
            // _statusBox
            // 
            _statusBox.BorderStyle = BorderStyle.FixedSingle;
            _statusBox.Font = new Font("Calibri", 9.75F, FontStyle.Bold);
            _statusBox.HideSelection = false;
            _statusBox.Location = new Point(6, 0);
            _statusBox.Name = "_statusBox";
            _statusBox.ReadOnly = true;
            _statusBox.Size = new Size(537, 27);
            _statusBox.TabIndex = 1;
            // 
            // _panel1
            // 
            _panel1.BackColor = Color.WhiteSmoke;
            _panel1.Controls.Add(_statusBox);
            _panel1.Location = new Point(6, 279);
            _panel1.Name = "_panel1";
            _panel1.Size = new Size(543, 30);
            _panel1.TabIndex = 5;
            // 
            // _label1
            // 
            _label1.AutoSize = true;
            _label1.Font = new Font("Calibri", 9.75F, FontStyle.Bold);
            _label1.ForeColor = SystemColors.ControlDark;
            _label1.Location = new Point(367, 101);
            _label1.Name = "_label1";
            _label1.Size = new Size(177, 21);
            _label1.TabIndex = 2;
            _label1.Text = "Created by UnknownRE";
            // 
            // _toolTipNormal
            // 
            _toolTipNormal.ToolTipTitle = "Output: Same filetype as the original file";
            // 
            // _radioButtonNormal
            // 
            _radioButtonNormal.AutoSize = true;
            _radioButtonNormal.Checked = true;
            _radioButtonNormal.Location = new Point(9, 23);
            _radioButtonNormal.Name = "_radioButtonNormal";
            _radioButtonNormal.Size = new Size(83, 25);
            _radioButtonNormal.TabIndex = 0;
            _radioButtonNormal.TabStop = true;
            _radioButtonNormal.Text = "Normal";
            _toolTipNormal.SetToolTip(_radioButtonNormal, "Must use PhoenixCrypt to decrypt.");
            _radioButtonNormal.UseVisualStyleBackColor = true;
            // 
            // _toolTipPortable
            // 
            _toolTipPortable.ToolTipTitle = "Create an executable file";
            // 
            // _radioButtonPortable
            // 
            _radioButtonPortable.AutoSize = true;
            _radioButtonPortable.Location = new Point(98, 23);
            _radioButtonPortable.Name = "_radioButtonPortable";
            _radioButtonPortable.Size = new Size(91, 25);
            _radioButtonPortable.TabIndex = 1;
            _radioButtonPortable.Text = "Portable";
            _toolTipPortable.SetToolTip(_radioButtonPortable,
                "Enter password to decrypt and save your file with specified name.");
            _radioButtonPortable.UseVisualStyleBackColor = true;
            // 
            // _groupBox3
            // 
            _groupBox3.Controls.Add(_radioButtonNormal);
            _groupBox3.Controls.Add(_radioButtonPortable);
            _groupBox3.Font = new Font("Calibri", 9.75F, FontStyle.Bold);
            _groupBox3.Location = new Point(355, 43);
            _groupBox3.Name = "_groupBox3";
            _groupBox3.Size = new Size(194, 55);
            _groupBox3.TabIndex = 2;
            _groupBox3.TabStop = false;
            _groupBox3.Text = " Output Method ";
            // 
            // _groupBox2
            // 
            _groupBox2.Controls.Add(_textBox1);
            _groupBox2.Font = new Font("Calibri", 9.75F, FontStyle.Bold);
            _groupBox2.Location = new Point(6, 180);
            _groupBox2.Name = "_groupBox2";
            _groupBox2.Size = new Size(543, 55);
            _groupBox2.TabIndex = 1;
            _groupBox2.TabStop = false;
            _groupBox2.Text = "Password";
            // 
            // _textBox1
            // 
            _textBox1.BorderStyle = BorderStyle.FixedSingle;
            _textBox1.Font = new Font("Calibri", 9.75F, FontStyle.Bold);
            _textBox1.Location = new Point(6, 22);
            _textBox1.Name = "_textBox1";
            _textBox1.PasswordChar = '*';
            _textBox1.Size = new Size(532, 27);
            _textBox1.TabIndex = 1;
            _textBox1.UseSystemPasswordChar = true;
            // 
            // _textBox2
            // 
            _textBox2.AllowDrop = true;
            _textBox2.BorderStyle = BorderStyle.FixedSingle;
            _textBox2.Location = new Point(7, 22);
            _textBox2.Name = "_textBox2";
            _textBox2.Size = new Size(435, 27);
            _textBox2.TabIndex = 0;
            // 
            // _button3
            // 
            _button3.Location = new Point(445, 18);
            _button3.Name = "_button3";
            _button3.Size = new Size(93, 31);
            _button3.TabIndex = 1;
            _button3.Text = "Browse";
            _button3.UseVisualStyleBackColor = true;
            _button3.Click += selectFileButton_Click;
            // 
            // _groupBox1
            // 
            _groupBox1.Controls.Add(_button3);
            _groupBox1.Controls.Add(_textBox2);
            _groupBox1.Font = new Font("Calibri", 9.75F, FontStyle.Bold);
            _groupBox1.Location = new Point(5, 121);
            _groupBox1.Name = "_groupBox1";
            _groupBox1.Size = new Size(544, 57);
            _groupBox1.TabIndex = 0;
            _groupBox1.TabStop = false;
            _groupBox1.Text = " Input File ";
            // 
            // _pictureBox1
            // 
            _pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            _pictureBox1.Image = ((Image) (resources.GetObject("_pictureBox1.Image")));
            _pictureBox1.Location = new Point(24, 42);
            _pictureBox1.Name = "_pictureBox1";
            _pictureBox1.Size = new Size(76, 76);
            _pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            _pictureBox1.TabIndex = 8;
            _pictureBox1.TabStop = false;
            // 
            // _button2
            // 
            _button2.Font = new Font("Calibri", 9.75F, FontStyle.Bold);
            _button2.Location = new Point(312, 235);
            _button2.Name = "_button2";
            _button2.Size = new Size(237, 38);
            _button2.TabIndex = 4;
            _button2.Text = "Decrypt";
            _button2.UseVisualStyleBackColor = true;
            _button2.Click += decryptButton_Click;
            // 
            // _button1
            // 
            _button1.Font = new Font("Calibri", 9.75F, FontStyle.Bold);
            _button1.Location = new Point(12, 235);
            _button1.Name = "_button1";
            _button1.Size = new Size(294, 38);
            _button1.TabIndex = 3;
            _button1.Text = "Encrypt";
            _button1.UseVisualStyleBackColor = true;
            _button1.Click += encryptButton_Click;
            // 
            // _menuBarTitleLabel
            // 
            _menuBarTitleLabel.AutoSize = true;
            _menuBarTitleLabel.BackColor = Color.FromArgb(255, 128, 0);
            _menuBarTitleLabel.Font = new Font("Calibri", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _menuBarTitleLabel.ForeColor = Color.White;
            _menuBarTitleLabel.Location = new Point(133, -2);
            _menuBarTitleLabel.Name = "_menuBarTitleLabel";
            _menuBarTitleLabel.Size = new Size(297, 37);
            _menuBarTitleLabel.TabIndex = 11;
            _menuBarTitleLabel.Text = "UnknownRE Encrypter";
            _menuBarTitleLabel.MouseDown += menuBar_MouseDown;
            _menuBarTitleLabel.MouseEnter += menuBarTitleLabel_MouseEnter;
            _menuBarTitleLabel.MouseLeave += menuBarTitleLabel_MouseLeave;
            _menuBarTitleLabel.MouseMove += menuBar_MouseMove;
            _menuBarTitleLabel.MouseUp += menuBar_MouseUp;
            // 
            // _exitLabel
            // 
            _exitLabel.AutoSize = true;
            _exitLabel.BackColor = Color.FromArgb(255, 128, 0);
            _exitLabel.Cursor = Cursors.Hand;
            _exitLabel.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            _exitLabel.ForeColor = Color.White;
            _exitLabel.Location = new Point(521, 0);
            _exitLabel.Name = "_exitLabel";
            _exitLabel.Size = new Size(37, 36);
            _exitLabel.TabIndex = 10;
            _exitLabel.Text = "X";
            _exitLabel.Click += exitLabel_Click;
            _exitLabel.MouseEnter += exitLabel_MouseEnter;
            _exitLabel.MouseLeave += exitLabel_MouseLeave;
            // 
            // _menuBar
            // 
            _menuBar.BackColor = Color.FromArgb(255, 128, 0);
            _menuBar.Location = new Point(-1, -2);
            _menuBar.Name = "_menuBar";
            _menuBar.Size = new Size(559, 39);
            _menuBar.TabIndex = 9;
            _menuBar.TabStop = false;
            _menuBar.MouseDown += menuBar_MouseDown;
            _menuBar.MouseMove += menuBar_MouseMove;
            _menuBar.MouseUp += menuBar_MouseUp;
            // 
            // UnknownREEncrypter
            // 
            BackColor = Color.White;
            ClientSize = new Size(557, 314);
            Controls.Add(_menuBarTitleLabel);
            Controls.Add(_exitLabel);
            Controls.Add(_menuBar);
            Controls.Add(_label1);
            Controls.Add(_panel1);
            Controls.Add(_groupBox3);
            Controls.Add(_groupBox2);
            Controls.Add(_groupBox1);
            Controls.Add(_pictureBox1);
            Controls.Add(_button2);
            Controls.Add(_button1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = ((Icon) (resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            Name = "UnknownREEncrypter";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UnknownRE Encrypter";
            _panel1.ResumeLayout(false);
            _panel1.PerformLayout();
            _groupBox3.ResumeLayout(false);
            _groupBox3.PerformLayout();
            _groupBox2.ResumeLayout(false);
            _groupBox2.PerformLayout();
            _groupBox1.ResumeLayout(false);
            _groupBox1.PerformLayout();
            ((ISupportInitialize) (_pictureBox1)).EndInit();
            ((ISupportInitialize) (_menuBar)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}