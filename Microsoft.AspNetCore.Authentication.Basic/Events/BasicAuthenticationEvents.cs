using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.Basic.Events
{
    public class BasicAuthenticationEvents
    {
        public Func<ValidateCredentialsContext, Task> OnValidateCredentials { get; set; } = context => Task.CompletedTask;

        public virtual Task ValidateCredentials(ValidateCredentialsContext context) => OnValidateCredentials(context);
    }
}
