using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Kia.KomakYad.Common.Configurations
{
    public class EmailConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
    }
}
