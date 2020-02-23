using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncProgressBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var progress = new Progress<int>(percent =>
            {
                textBox1.Text = percent + "%";
                progressBar1.Value = percent;
            });

            await Task.Run(() =>
                //DoProcessing(progress)
                DownloadFile(
                    "link", 
                    @"kaydedilecek yer"
                )
            );

            textBox1.Text = "Done!";
        }

        public void DoProcessing(IProgress<int> progress)
        {
            for (int i = 0; i != 100; ++i)
            {
                Thread.Sleep(100); // CPU-bound work
                if (progress != null)
                    progress.Report(i);
            }
        }




        public void DownloadFile(string address, string location)
        {
            WebClient client = new WebClient();
            Uri Uri = new Uri(address);

            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);

            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
            client.DownloadFileAsync(Uri, location);

        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            listBox1.Items.Insert(0, string.Format(
                "{0} downloaded {1} of {2} bytes. {3} % complete...",
            (string)e.UserState,
            e.BytesReceived,
            e.TotalBytesToReceive,
            e.ProgressPercentage));


            progressBar1.Value = e.ProgressPercentage;

        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                textBox1.Text = "Download has been canceled.";
            }
            else
            {
                textBox1.Text = "Download completed!";
            }
        }
    }

}
