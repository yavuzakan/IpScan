using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace IpScan
{
    public partial class Form1 : Form
    {
        public static string ipgovde = "";
        public static string ipkuyruk1 = "";
        public static string ipkuyruk2 = "";
        public static string sorip = "";
        public static string sonuc = "";
        public static string sonuc2 = "";
        public static string sonuc3 = "";
        public static string newline = Environment.NewLine;
        BackgroundWorker worker;
       DataTable dt = new DataTable();

      
        public Form1()
        {
            InitializeComponent();
           
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            dt.Columns.Add("IP"); 
            dt.Columns.Add("HostName");
           
            dataGridView1.DataSource = dt;

            ipgovde = ipbul();
           dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            textBox1.Text = ipbul()+"1";
            textBox2.Text = ipbul()+"255";
            textBox3.Enabled = false;
        }



            public string kuyruk11()
        {
            string ip1 = textBox1.Text;
            string ip11 = textBox1.Text;
            char nokta = '.';
            char last = ip1[ip1.Length-1];
            int i = 0;
            while (last != nokta)
            {
                ip1 = ip1.Remove(ip1.Length-1, 1);
                last = ip1[ip1.Length-1];
                i++;
            }
            string son = ip11.Substring(ip11.Length-i);

            return son;

        }

        public string kuyruk22()
        {
            string ip1 = textBox2.Text;
            string ip11 = textBox2.Text;
            char nokta = '.';
            char last = ip1[ip1.Length-1];
            int i = 0;
            while (last != nokta)
            {
                ip1 = ip1.Remove(ip1.Length-1, 1);
                last = ip1[ip1.Length-1];
                i++;
            }
            string son = ip11.Substring(ip11.Length-i);

            return son;
        }





        public static string ipbul()
        {
            String mac = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    mac = ip.ToString();
                    char nokta = '.';
                    char last = mac[mac.Length-1];
                    while (last != nokta)
                    {

                        mac = mac.Remove(mac.Length-1, 1);

                        last = mac[mac.Length-1];
                    }



                    return mac;

                }
            }
            return mac;




        }

       

        public void pingg(string gelenip)
        {
            //int timeout = 10;
            Ping ping = new Ping();
            PingReply cevap = ping.Send(gelenip);

            if (cevap.Status==IPStatus.Success)
            {
              //  textBox3.Text += gelenip + newline;
            }
            else
            {

                // textBox3.Text += gelenip + newline;

            }


        }

 
        private void button2_Click(object sender, EventArgs e)
        {


            if (backgroundWorker1.IsBusy != true)
            {
                dt.Rows.Clear();
                dataGridView1.Refresh();
             
                // Start the asynchronous operation.
               
                backgroundWorker1.RunWorkerAsync();
                button2.Text = "STOP";

            }
            else 
            {
                backgroundWorker1.CancelAsync();
                button2.Text = "SCAN";
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            String ipkuyruk1 = kuyruk11();
            String ipkuyruk2 = kuyruk22();
            int sayi = int.Parse(ipkuyruk1);
            int sayi2 = int.Parse(ipkuyruk2);
            int sayi3 = sayi2 + 1;

            if (sayi < 256)
            {

                for (int i = sayi; i < sayi3; i++)
                {
                    if (worker.CancellationPending==true)
                    { break; }

                  sorip = ipgovde+i.ToString();
                    // textBox3.Text += ipgovde+i.ToString() + newline;
                    string time = DateTime.Now.ToString("h:mm:ss tt");
                    try
                    {

                        Ping ping = new Ping();
         
                        int timeout = 10;
                      



                        PingReply cevap = ping.Send(sorip, timeout);

                        if (cevap.Status==IPStatus.Success)
                        {
                            sonuc =   sorip;
                            try
                            {
                                IPHostEntry entry = Dns.GetHostEntry(sorip);
                                if (entry != null)
                                {
                                     sonuc3 = entry.HostName;
                                 
                                }

                            }
                            catch
                            {
                            }
                        }
                        else
                        {

                            sonuc = "";

                        }
                        sonuc2 = sorip;

                    }
                    catch (Exception exp)
                    {


                    }

                    System.Threading.Thread.Sleep(1);
                    worker.ReportProgress(1);

                }

            }
            else
            {

            }
      


        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            textBox3.Text = sonuc2;
            DataRow dr = dt.NewRow();
            if(sonuc != "")
            {            
                dr[0]= sonuc;
                dr[1]= sonuc3;
                dt.Rows.Add(dr);
                sonuc = "";
                sonuc3 = "";

            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //textBox3.Text += "end";
        }
    }
}
