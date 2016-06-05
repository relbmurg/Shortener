using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Shortener.Core
{
    public class DefaultKeyGenerator : IKeyGenerator
    {
        static readonly int baseNum = 62;
        static readonly String baseDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public string Create(ShortUrl url)
        {
            var id = url.Id;
            var toValue = new StringBuilder(id == 0 ? "0" : "");

            while (id != 0)
            {
                int mod = (int)(id % baseNum);
                toValue.Insert(0, baseDigits.Substring(mod, 1));
                id = id / baseNum;
            }

            return toValue.ToString();
        }
    }
}