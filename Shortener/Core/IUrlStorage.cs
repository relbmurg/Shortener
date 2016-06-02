using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core
{
    public interface IUrlStorage
    {
        IEnumerable<ShortUrl> GetAll();
        ShortUrl GetByUrl(string url);
        ShortUrl GetByKey(string key);
        void Add(ShortUrl url);
        void Update(ShortUrl url);
    }
}
