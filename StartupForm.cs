using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Windows.Media;
using UnknownREEncrypter.Properties;
using Color = System.Drawing.Color;

namespace UnknownREEncrypter
{
    public partial class StartupForm : Form
    {
        private MediaPlayerFromResource _soundPlayer;

        public StartupForm()
        {
            InitializeComponent();

            // Play the soundFile from (embedded resource) memory
            _soundPlayer = new MediaPlayerFromResource();
            _soundPlayer.PlayResourceFile(new Uri("/UnknownREEncrypter;component/Resources/soundFile.mp3", UriKind.RelativeOrAbsolute));
            pictureBoxBottomBar.Location = new Point(0, Height - pictureBoxBottomBar.Height);
        }

        #region Custom Title Bar Effects

        int cursorXPositionDifference;
        int cursorYPositionDifference;
        bool leftMouseDown;

        private void menuBarTitle_MouseEnter(object sender, EventArgs e)
        {
            menuBarTitle.ForeColor = Color.Gold;
        }

        private void menuBarTitle_MouseLeave(object sender, EventArgs e)
        {
            menuBarTitle.ForeColor = Color.White;
        }

        private void exitLabel_MouseEnter(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.Gold;
        }

        private void exitLabel_MouseLeave(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.White;
        }

        private void exitLabel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void minimizeLabel_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void menuBar_MouseDown(object sender, MouseEventArgs e)
        {
            cursorXPositionDifference = MousePosition.X - ActiveForm.Location.X;
            cursorYPositionDifference = MousePosition.Y - ActiveForm.Location.Y;
            leftMouseDown = true;
        }

        private void lblMenuBarTitle_MouseDown(object sender, MouseEventArgs e)
        {
            cursorXPositionDifference = MousePosition.X - ActiveForm.Location.X;
            cursorYPositionDifference = MousePosition.Y - ActiveForm.Location.Y;
            leftMouseDown = true;
        }

        private void menuBar_MouseUp(object sender, MouseEventArgs e)
        {
            leftMouseDown = false;
        }

        private void lblMenuBarTitle_MouseUp(object sender, MouseEventArgs e)
        {
            leftMouseDown = false;
        }

        private void menuBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftMouseDown)
            {
                ActiveForm.Location = new Point(MousePosition.X - cursorXPositionDifference, MousePosition.Y - cursorYPositionDifference);
            }
        }

        private void menuBarTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftMouseDown)
            {
                ActiveForm.Location = new Point(MousePosition.X - cursorXPositionDifference, MousePosition.Y - cursorYPositionDifference);
            }
        }

        #endregion

        private void continueButton_Click(object sender, EventArgs e)
        {
            UnknownREEncrypter mainForm = new UnknownREEncrypter();

            _soundPlayer?.Stop();
            _soundPlayer = null;

            mainForm.Show();
            Hide();
        }         
    }
}
