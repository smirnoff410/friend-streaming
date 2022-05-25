﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsumerTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			var port = 4100;
			var client = new UdpClient(port);
			while (true)
			{
				var data = await client.ReceiveAsync();
				
				using (var ms = new MemoryStream(data.Buffer))
				{
					//pictureBox1.Image = new Bitmap(ms);
					Text = $"Bytes recived: {data.Buffer.Length * sizeof(byte)}";
				}
			}
		}

		private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());

			MessageBox.Show(string.Join("\n",
				host.AddressList.Where(i => i.AddressFamily == AddressFamily.InterNetwork)));
		}
	}
}
