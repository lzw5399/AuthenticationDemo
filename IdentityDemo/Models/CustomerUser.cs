using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Models
{
    public class CustomerUser : IdentityUser// IdentityUser<T> 可以指定表的主键
    {
        [MaxLength(20)]
        public string CustomerTag { get; set; }
    }
}
