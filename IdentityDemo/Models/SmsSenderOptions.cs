using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Models
{
    public class SmsSenderOptions
    {
        public int AppId { get; set; }

        public string AppKey { get; set; }

        public int TemplateId { get; set; }

        public string SmsSign { get; set; }
    }
}
