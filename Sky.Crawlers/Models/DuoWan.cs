using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky.Crawler.Models
{
    public class DuoWan
    {

        public class DwResult
        {
            public string html { get; set; }
            public bool more { get; set; }
            public int offset { get; set; }
            public bool enabled { get; set; }
        }

        public class DetailResult
        {
            public string gallery_title { get; set; }
            public Picinfo[] picInfo { get; set; }
            public string[] hiidoId { get; set; }
        }

        public class Picinfo
        {
            public string title { get; set; }
            public string pic_id { get; set; }
            public string ding { get; set; }
            public string cai { get; set; }
            public string old { get; set; }
            public string cover_url { get; set; }
            public object file_url { get; set; }
            public string file_width { get; set; }
            public string file_height { get; set; }
            public string url { get; set; }
            public string source { get; set; }
            public string mp4_url { get; set; }
            public string sort { get; set; }
            public string cmt_md5 { get; set; }
            public string cmt_url { get; set; }
            public string add_intro { get; set; }
        }

    }
}
