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

        public string Create(long value)
        {
            var toValue = new StringBuilder(value == 0 ? "0" : "");

            while (value != 0)
            {
                int mod = (int)(value % baseNum);
                toValue.Insert(0, baseDigits.Substring(mod, 1));
                value = value / baseNum;
            }

            return toValue.ToString();
        }
    }
}