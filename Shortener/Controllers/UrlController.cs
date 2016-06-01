using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Shortener.Models;

namespace Shortener.Controllers
{
    public class UrlController : ApiController
    {
        [Route("s/{key}")]
        [HttpGet]
        public RedirectResult Trsansfer(string key)
        {
            return Redirect(new Uri("https://www.google.com"));
        }

        [Route("api/create")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] string url)
        {
            return Request.CreateResponse(url);
        }

        [Route("api/links")]
        [HttpGet]
        public HttpResponseMessage Links()
        {
            return Request.CreateResponse(Enumerable.Empty<ShortUrlModel>());
        }
    }
}
