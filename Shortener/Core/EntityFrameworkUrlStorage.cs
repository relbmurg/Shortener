using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shortener.Core
{
    public class EntityFrameworkUrlStorage : DbContext, IUrlStorage
    {
        public DbSet<ShortUrl> Urls { get; set; }

        public EntityFrameworkUrlStorage() : this("Default")
        {
            
        }

        public EntityFrameworkUrlStorage(string connectionStringName) 
            : base(connectionStringName)
        {
            
        }

        public IEnumerable<ShortUrl> GetAll()
        {
            return Urls;
        }

        public ShortUrl GetByUrl(string url)
        {
            return Urls.FirstOrDefault(x => x.Url == url);
        }

        public ShortUrl GetByKey(string key)
        {
            return Urls.FirstOrDefault(x => x.Short == key);
        }

        public void Add(ShortUrl url)
        {
            Urls.Add(url);
            SaveChanges();
        }

        public void Update(ShortUrl url)
        {
            SaveChanges();
        }
    }
}