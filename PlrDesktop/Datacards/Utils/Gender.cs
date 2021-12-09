using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Datacards.Utils
{
    public class Gender : IApiDataCard
    {
        public int? Id { get; set; } = null;

        public string Name { get; set; } = null;
    }
}
