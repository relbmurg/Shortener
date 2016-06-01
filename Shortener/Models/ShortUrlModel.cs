using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shortener.Models
{
    public class ShortUrlModel
    {
        public string Url { get; set; }
        public string Short { get; set; }
        public DateTime Created { get; set; }
        public int Redirects { get; set; }
    }
}