using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Connection
{
    public class AuthInfo
    {
        public string Login { get; set; }
        public string Password { get; set; }


        public Dictionary<string, string> GetAsDictionary()
        {
            return new Dictionary<string, string>()
            {
                {"Login", Login},
                {"Password", Password}
            };
        }
    }
}
