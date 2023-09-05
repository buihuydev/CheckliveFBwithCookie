using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckliveFBwithCookie
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string cookie = textBox1.Text;
            if(cookie == null)
            {
                MessageBox.Show("Vui lòng nhập cookie!");
                return;
            }
            string result = await Check_live(cookie);
            if(result == "Checkpoint")
            {
                textBox2.Text = "Tài Khoản Bị Checkpoint!";
            }    
            if(result == "live")
            {
                textBox2.Text = "Tài Khoản Live!";
            }
        }
        private async Task<string> Check_live(string cookie)
        {
            string check = string.Empty;
            var options = new RestClientOptions("https://mbasic.facebook.com")
            {
                MaxTimeout = -1,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36 Edg/116.0.1938.69",
            };
            var client = new RestClient(options);
            var request = new RestRequest("/login/device-based/regular/login/?refsrc=deprecated&lwv=100&refid=8", Method.Post);
            request.AddHeader("authority", "mbasic.facebook.com");
            request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            request.AddHeader("accept-language", "en-US,en;q=0.9");
            request.AddHeader("cache-control", "max-age=0");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("cookie", cookie);
            request.AddHeader("dpr", "1");
            request.AddHeader("origin", "https://mbasic.facebook.com");
            request.AddHeader("referer", "https://mbasic.facebook.com/");
            request.AddHeader("sec-fetch-dest", "document");
            request.AddHeader("sec-fetch-mode", "navigate");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-user", "?1");
            request.AddHeader("upgrade-insecure-requests", "1");
            request.AddHeader("viewport-width", "658");
            request.AddParameter("lsd", "AVoxpJobVHc");
            request.AddParameter("jazoest", "21034");
            request.AddParameter("m_ts", "1693895711");
            request.AddParameter("li", "H8z2ZPOHdE0XM-jNFcBeOJmG");
            request.AddParameter("try_number", "0");
            request.AddParameter("unrecognized_tries", "0");
            request.AddParameter("login", "Log In");
            request.AddParameter("bi_xrwh", "0");
            RestResponse response = await client.ExecuteAsync(request);
            //check chuỗi
            string responseHtml = response.Content.ToString();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseHtml);
            string targetText = "Chúng tôi đã đình chỉ tài khoản của bạn";
            var nodes = doc.DocumentNode.SelectNodes($"//*[text()[contains(., '{targetText}')]]");

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    check = "Checkpoint";
                }
            }
            else
            {
                check = "live";
            }
            return check;
        }
    }
}
