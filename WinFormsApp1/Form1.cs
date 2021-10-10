using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MockRobotDDI;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public MockBotDDI newBot;
        public String message;

        public Form1()
        {
            InitializeComponent();
            newBot = new MockBotDDI();
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            message = newBot.OpenConnection("127.0.0.1");
            if (String.IsNullOrEmpty(message))
            {
                richTextBox1.Text += "\n" + "Connection was open succefully connected.";
            }
            else
            {
                richTextBox1.Text += "\n" + message;

            }
        }

        private async void button2_Click(Object sender, EventArgs e)
        {
            richTextBox1.Text += "\n" + "Please wait while MockBot is initalizing";
            message = await Task.Run(() => newBot.Initialize());
            if (String.IsNullOrEmpty(message))
            {
                richTextBox1.Text += "\n" + "MockBot was successfully initialized.";
            }
            else 
            { 
                richTextBox1.Text += "\n" + message;
            }

        }

        private async void button3_Click(Object sender, EventArgs e)
        {
            richTextBox1.Text += "\n" + "Please wait while MockBot is receving comemnd";
            message = await Task.Run(() => newBot.ExecuteOperation("pick", new String[] {"Source Location"}, new String[] { "9" }));
            if (String.IsNullOrEmpty(message))
            {
                richTextBox1.Text += "\n" + "Pick commend was successfully initialized";
            }
            else
            {
                richTextBox1.Text += "\n" + message;
            }
        }

        private void button4_Click(Object sender, EventArgs e)
        {
            newBot.Abort();
            richTextBox1.Text += "\n" + "Connection is Close";
        }


        private void richTextBox1_TextChanged(Object sender, EventArgs e)
        {

        }

        private async void button5_Click(Object sender, EventArgs e)
        {
            richTextBox1.Text += "\n" + "Please wait while MockBot is receving comemnd";
            message = await Task.Run(() => newBot.ExecuteOperation("place", new String[] { "Destination Location" }, new String[] { "5" }));
            if (String.IsNullOrEmpty(message))
            {
                richTextBox1.Text += "\n" + "Place commend was successfully initialized";
            }
            else
            {
                richTextBox1.Text += "\n" + message;
            }
        }

        private async void button6_Click(Object sender, EventArgs e)
        {
            richTextBox1.Text += "\n" + "Please wait while MockBot is receving comemnd";
            message = await Task.Run(() => newBot.ExecuteOperation("transfer", new String[] { "Destination Location", "Source Location" }, new String[] { "5", "9" }));
            if (String.IsNullOrEmpty(message))
            {
                richTextBox1.Text += "\n" + "Transfer commend was successfully initialized";
            }
            else
            {
                richTextBox1.Text += "\n" + message;
            }
        }
    }
}
