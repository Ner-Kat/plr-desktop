﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Lib
{
    public interface IPlrCardWindow
    {
        public int? GetId();

        public void UpdateCardData();
    }
}
