using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Datacards
{
    public interface IChangableApiCard
    {
        public object ForChanging();

        public object ForAdding();
    }
}
