using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shortener.Core
{
    public class ShortUrl
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Short { get; set; }
        public DateTime Created { get; set; }
        public int Redirects { get; set; }
        public string UserId { get; set; }
    }
}