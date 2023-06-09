using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;

namespace Sample_GUI_PBL
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private StreamWriter logFile;

        public Form1()
        {
            InitializeComponent();

            // Initialize video devices
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }

            // Populate the camera selection toolbox
            foreach (FilterInfo device in videoDevices)
            {
                comboBox1.Items.Add(device.Name);
            }

            // Select the first camera by default
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

            // Create or open the log file
            string logFilePath = "log.txt";
            logFile = File.AppendText(logFilePath);
            Log("Application started");
            LogSystemInfo();
        }

        private void StartCapture()
        {
            // Retrieve the selected camera
            FilterInfo selectedDevice = videoDevices[comboBox1.SelectedIndex];

            videoSource = new VideoCaptureDevice(selectedDevice.MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            videoSource.Start();

            Log("Video capture started");
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Update the PictureBox with the new frame
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void StopCapture()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }

            Log("Video capture stopped");
        }

        private void Log(string message)
        {
            string logEntry = $"{DateTime.Now}: {message}";
            logFile.WriteLine(logEntry);
        }

        private void LogSystemInfo()
        {
            string systemInfo = $"Operating System: {Environment.OSVersion.VersionString}";
            Log(systemInfo);
        }

        private void CaptureFrame()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                string captureTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string captureFilePath = $"capture_{captureTime}.jpg";
                Bitmap currentFrame = (Bitmap)pictureBox1.Image.Clone();
                currentFrame.Save(captureFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                Log($"Frame captured: {captureFilePath}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartCapture();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopCapture();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CaptureFrame();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logFile.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Perform login validation here
            // Example: Check if the username and password are valid

            if (IsValidLogin(username, password))
            {
                // Hide the login UI and show the main form
                groupBox1.Visible = false;
                this.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private bool IsValidLogin(string username, string password)
        {
            // Implement your own logic for validating the username and password
            // Return true if the login is valid, false otherwise
            // Example: Check against a predefined list of valid usernames and passwords

            // Replace this example logic with your own validation
            string validUsername = "admin";
            string validPassword = "password";

            return username == validUsername && password == validPassword;
        }
    }
}
