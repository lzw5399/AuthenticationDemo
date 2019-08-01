using Microsoft.AspNetCore.Authentication.Basic.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Authentication.Basic
{
    public class BasicAuthenticationOptions: AuthenticationSchemeOptions
    {
        public string Realm { get; set; }

        public new BasicAuthenticationEvents Events
        {
            get { return (BasicAuthenticationEvents)base.Events; }
            set { base.Events = value; }
        }
    }
}
