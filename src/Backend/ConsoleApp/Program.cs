using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp
{
    class Program
    {
        private static HttpClient client = new HttpClient();
        private static string StartVlcTemplate = ConfigurationManager.AppSettings["vlcOptions"];
        private static string CommandVlcApiTemplate = ConfigurationManager.AppSettings["vlcApi"];
        private static string StartTime = ConfigurationManager.AppSettings["startTime"];
        private static string Host = ConfigurationManager.AppSettings["host"];
        private static string Port = ConfigurationManager.AppSettings["port"];
        private static string Password = ConfigurationManager.AppSettings["password"];
        private static string VlcPath = ConfigurationManager.AppSettings["vlcPath"];
        private static string LastPathFile = ConfigurationManager.AppSettings["lastPathFile"];

        static void Main(string[] args)
        {
            //SeekTo(10);
            //Stop();
            //Pause();
            //Play();
            //var v = Status();
            X();
            Console.ReadKey();
        }
        public static void X()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Work\TVController\Backend\TVController.Backend\ConsoleApp\ApiStatus.xml");

            //Display all the book titles.
            XmlNodeList elemList = doc.GetElementsByTagName("time");
            for (int i = 0; i < elemList.Count; i++)
            {
                Console.WriteLine(elemList[i].InnerXml);
            }
        }
        public static async void SeekTo(int toSecond)
        {
            VlcApiCommand(string.Format("?command=seek&val={0}", toSecond));
        }
        
        public static async void Stop()
        {
            VlcApiCommand("?command=pl_stop");
        }

        public static async void Pause()
        {
            VlcApiCommand("?command=pl_pause");
        }

        public static async void Play()
        {
            VlcApiCommand("?command=pl_play");
        }

        public static async Task<string> Status()
        {
            var res = await VlcApiCommand("");
            var s = await res.Content.ReadAsStringAsync();
            return s;
        }

        private static async Task<HttpResponseMessage> VlcApiCommand(string command)
        {
            var vlcApiCommand = string.Format(CommandVlcApiTemplate, Host, Port, command);

            var byteArray = Encoding.ASCII.GetBytes(":" + Password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.GetAsync(vlcApiCommand);

            return response;
        }
    }
}
