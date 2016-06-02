using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Shortener.Models;

namespace Shortener.Core
{
    public class UrlManager
    {
        private readonly IUrlStorage _storage;
        private readonly IKeyGenerator _generator;

        public UrlManager(IUrlStorage storage, IKeyGenerator generator)
        {
            _storage = storage;
            _generator = generator;
        }

        public string Save(Uri uri)
        {
            var stored = _storage.GetByUrl(uri.ToString());
            if (stored != null)
                return stored.Short;

            var url = new ShortUrl
            {
                Created = DateTime.UtcNow,
                Url = uri.ToString()
            };

            using (var trans = new TransactionScope())
            {
                _storage.Add(url);
                url.Short = _generator.Create(url.Id);
                _storage.Update(url);

                trans.Complete();
            }

            return url.Short;
        }

        public IEnumerable<ShortUrl> GetUrls()
        {
            return _storage.GetAll();
        }

        public string Find(string key, bool updateRedirects)
        {
            var url = _storage.GetByKey(key);
            if (url == null || !updateRedirects) return url?.Url;

            url.Redirects++;
            _storage.Update(url);

            return url.Url;
        }
    }
}