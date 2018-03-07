using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web.Mvc;
using System.Xml;
using TVController.BE1.Models;

namespace TVController.BE1.Controllers
{
    public class FilesController : ApiController
    {
        private readonly string StartVlcTemplate = ConfigurationManager.AppSettings["vlcOptions"];
        private readonly string CommandVlcApiTemplate = ConfigurationManager.AppSettings["vlcApi"];

        private readonly string StartTime = ConfigurationManager.AppSettings["startTime"];
        private readonly string Host = ConfigurationManager.AppSettings["host"];
        private readonly string Port = ConfigurationManager.AppSettings["port"];
        private readonly string Password = ConfigurationManager.AppSettings["password"];
        private readonly string VlcPath = ConfigurationManager.AppSettings["vlcPath"];
        private readonly string LastPathFile = ConfigurationManager.AppSettings["lastPathFile"];

        private int? _length;
        private async Task<int> Length()
        {
            if (!_length.HasValue)
            {
                _length = await GetLength();
            }
            return _length.Value;
        }

        //private int? _volume;
        //private async Task<int> Volume()
        //{
        //    if (!_volume.HasValue)
        //    {
        //        _volume = await GetVolume();
        //    }
        //    return _volume.Value;
        //}

        //private int? _time;
        //private async Task<int> Time()
        //{
        //    if (!_time.HasValue)
        //    {
        //        _time = await GetTime();
        //    }
        //    return _time.Value;
        //}
        
        private static HttpClient client = new HttpClient();
        XmlDocument doc = new XmlDocument();

        [HttpGet]
        public EntriesData ListEntries(string requestDir = null)
        {
            if (string.IsNullOrEmpty(requestDir))
            {
                requestDir = GetLastPath();
            }
            if (System.IO.Directory.Exists(requestDir))
            {
                var dirs = System.IO.Directory.GetDirectories(requestDir);
                var files = System.IO.Directory.GetFiles(requestDir);
                var parent = System.IO.Directory.GetParent(requestDir);

                var entriesData = new EntriesData();
                entriesData.EntryPath = requestDir;
                entriesData.Entries = new System.Collections.Generic.List<Entry>();

                entriesData.Entries.Add(new Entry { EntryType = EntryType.Parent, Extension = "", Name = "..", FullName = parent.FullName });
                dirs.ToList().ForEach(d => entriesData.Entries.Add(new Entry { EntryType = EntryType.Directory, Extension = "", FullName = d, Name = System.IO.Path.GetFileName(d) }));
                files.ToList().ForEach(f => entriesData.Entries.Add(new Entry
                {
                    EntryType = GetEntryTypeByName(f),
                    Extension = System.IO.Path.GetExtension(f),
                    Name = System.IO.Path.GetFileNameWithoutExtension(f),
                    FullName = f
                }
                ));
                SetLastPath(requestDir);
                return entriesData;
            }
            return null;
        }

        //C:\Program Files(x86)\VLC_SRV
        [HttpGet]
        public async Task StartVlc()
        {
            Process.Start(@"C:\Work\start.bat");

        }

        [HttpGet]
        public async Task<State> GetState()
        {
            System.Threading.Thread.Sleep(100);
            var s = await Status();
            var xs = new State();

            try { xs.Volume = int.Parse(GetStatusByNodeName(s, "volume")); } catch (Exception){}
            try { xs.Time = int.Parse(GetStatusByNodeName(s, "time")); } catch (Exception) { }
            try { xs.Length = int.Parse(GetStatusByNodeName(s, "length")); } catch (Exception) { }
            try { xs.MovieState = GetStatusByNodeName(s, "state"); } catch (Exception) { }
            try { xs.Fullscreen = bool.Parse(GetStatusByNodeName(s, "fullscreen")); } catch (Exception) { }
            try { xs.MovieTitle = GetStatusByNodeName(s, "info", "filename"); } catch (Exception) { }
            
            return xs;
        }

        [HttpGet]
        public async Task<State> Play(string moviePath, string subtitlePath)
        {
            //http://localhost:8081/requests/status.xml?command=in_play&input=G:\Filmek\xxx\Homeland.S05.HDTV.x264-MIXGROUP\Homeland.S05E02.HDTV.x264-FLEET.mp4
            //var state = await GetPlayState();
            var resp = await VlcApiCommand(string.Format("?command=in_play&input={0}",moviePath));
            resp = await VlcApiCommand("?command=fullscreen");
            var state = await GetState();
            if (!state.Fullscreen)
            {
                resp = await VlcApiCommand("?command=fullscreen");
            }
            return state;
        }

        [HttpGet]
        public async Task<State> Continue()
        {
            var resp = await VlcApiCommand(string.Format("?command=pl_play"));
            var state = await GetState();
            if (!state.Fullscreen)
            {
                resp = await VlcApiCommand("?command=fullscreen");
            }
            return state;
        }

        [HttpGet]
        public async Task<State> Pause()
        {
            var resp = await VlcApiCommand("?command=pl_pause");
            return await GetState();
        }

        [HttpGet]
        public async Task<State> Forward()
        {
            var l = await Length();
            var t = await GetTime();
            int onePercent = l / 100;
            var resp = await VlcApiCommand(string.Format("?command=seek&val={0}",t+onePercent <= l ? t + onePercent : l));
            return await GetState();
        }

        [HttpGet]
        public async Task<State> Backward()
        {
            var l = await Length();
            var t = await GetTime();
            int onePercent = l / 100;
            var resp = await VlcApiCommand(string.Format("?command=seek&val={0}", t - onePercent >= 0 ? t - onePercent : 0));
            return await GetState();
        }

        [HttpGet]
        public async Task<State> Stop()
        {
            var resp = await VlcApiCommand("?command=pl_stop");
            var state = await GetState();
            state.MovieState = "stopped";
            state.MovieTitle = null;
            return state;
        }
                
        private async Task<int> GetTime()
        {
            string l = await GetValue("time");
            return int.Parse(l);
        }
        private async Task<int> GetVolume()
        {
            string l = await GetValue("volume");
            return int.Parse(l);
        }
        private async Task<int> GetLength()
        {
            string l = await GetValue("length");
            return int.Parse(l);
        }
        private async Task<string> GetPlayState() {
            try
            {
                var s = await GetValue("state");
                return s;
            } catch (Exception e)
            {
                return string.Empty;
            }
        }

        private async Task<string> GetValue(string n)
        {
            var s = await Status();
            return GetStatusByNodeName(s, n);
        }

        private string GetStatusByNodeName(string statusXML, string name, string nameAttribnute = null)
        {
            doc.LoadXml(statusXML);
            XmlNodeList elemList = null;
            elemList = doc.GetElementsByTagName(name);
            if (!string.IsNullOrEmpty(nameAttribnute))
            {
                return elemList.Cast<XmlNode>().Where(n => n.Attributes["name"].InnerText == nameAttribnute).First().InnerText;
            }
            else
            {
                return elemList[0].InnerXml;
            }
        }

        private async Task<string> Status()
        {
            var res = await VlcApiCommand("");
            return await res.Content.ReadAsStringAsync();
        }

        private async Task<HttpResponseMessage> VlcApiCommand(string command)
        {
            var vlcApiCommand = string.Format(CommandVlcApiTemplate, Host, Port, command);
                
            var byteArray = Encoding.ASCII.GetBytes(":"+Password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.GetAsync(vlcApiCommand);
            
            return response;
        }

        private string GetVlcOptions()
        {
            return string.Format(StartVlcTemplate, StartTime, Host, Port, Password);
        }
        
        private EntryType GetEntryTypeByName(string f)
        {
            var ext = System.IO.Path.GetExtension(f);
            switch (ext.ToLower()) {
                case ".srt":
                    return EntryType.Subtitle;
                case ".avi":
                case ".mp4":
                case ".mpg":
                case ".mkv":
                    return EntryType.Movie;
                default:
                    return EntryType.OtherFile;
            }
        }

        private string GetLastPath()
        {
            var lastPathFile = LastPathFile;
            if (string.IsNullOrEmpty(lastPathFile))
            {
                lastPathFile = @".\lastPath.txt";
            }
            if (!System.IO.File.Exists(lastPathFile))
            {
                System.IO.File.WriteAllText(lastPathFile, @"G:\Filmek\xxx\");
            }
            var lastPath = System.IO.File.ReadAllText(lastPathFile);
            return lastPath;
        }

        private void SetLastPath(string requestDir)
        {
            var lastPathFile = LastPathFile;
            if (string.IsNullOrEmpty(lastPathFile))
            {
                lastPathFile = @".\lastPath.txt";
            }
            System.IO.File.WriteAllText(lastPathFile, requestDir);
        }
    }
}
