using System;
using System.Collections.Generic;
using System.Linq;
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
            return uri.OriginalString;
        }

        public IEnumerable<ShortUrlModel> GetUrls()
        {
            return Enumerable.Empty<ShortUrlModel>();
        } 
    }
}