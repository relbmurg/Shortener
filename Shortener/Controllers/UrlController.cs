using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Shortener.Core;
using Shortener.Models;

namespace Shortener.Controllers
{
    [Authorize]
    public class UrlController : ApiController
    {
        private readonly UrlManager _manager;
        private readonly string _roteFormat;

        string CreateUrl(string key, string host)
        {
            return String.Format(_roteFormat, host, key);
        }

        public string Host
        {
            get { return Request.RequestUri.GetLeftPart(UriPartial.Authority); }
        }

        public UrlController(UrlManager manager)
        {
            _manager = manager;
            _roteFormat = "{0}/s/{1}";
        }

        [Route("s/{key}")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Trsansfer(string key)
        {
            var url = _manager.Find(key, true);
            if (String.IsNullOrWhiteSpace(url))
                return NotFound();

            return Redirect(new Uri(url));
        }

        [Route("api/create")]
        [HttpPost]
        public HttpResponseMessage Create([FromBody] string url)
        {
            var result = _manager.Save(new Uri(url));
            return Request.CreateResponse(CreateUrl(result, Host));
        }

        [Route("api/links")]
        [HttpGet]
        public HttpResponseMessage Links(int index, int size)
        {
            var host = Host;
            int total;
            var result = _manager.GetUrls(index, size, out total).Select(x => new ShortUrlModel
            {
                Created = x.Created,
                Redirects = x.Redirects,
                Short = CreateUrl(x.Short, host),
                Url = x.Url
            });
            return Request.CreateResponse(new { result, total } );
        }
    }
}
