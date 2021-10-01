﻿using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class GendersMethods
    {
        private ApiServerConnection _server;
        private const string _methodsAddress = "genders/";

        public GendersMethods(ApiServerConnection server)
        {
            _server = server;
        }
    }
}
