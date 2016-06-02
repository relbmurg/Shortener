using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shortener.Core
{
    public class MemoryUrlStorage : IUrlStorage
    {
        private readonly List<ShortUrl> _list = new List<ShortUrl>();

        public IEnumerable<ShortUrl> GetAll()
        {
            return _list.ToArray();

        }

        public ShortUrl GetByUrl(string url)
        {
            return _list.FirstOrDefault(x => x.Url == url);
        }

        public ShortUrl GetByKey(string key)
        {
            return _list.FirstOrDefault(x => x.Short == key);
        }

        public void Add(ShortUrl url)
        {
            url.Id = _list.Count + 1;
            _list.Add(url);
        }

        public void Update(ShortUrl url)
        {
            var stored = _list.FirstOrDefault(x => x.Id == url.Id);
            if (stored == null)
                return;
            stored.Short = url.Short;
            stored.Url = url.Url;
            stored.Redirects = url.Redirects;
            stored.Created = url.Created;
        }
    }
}