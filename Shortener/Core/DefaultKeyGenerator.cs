using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shortener.Core
{
    public class DefaultKeyGenerator : IKeyGenerator
    {
        public string Create(string value)
        {
            return value;
        }
    }
}