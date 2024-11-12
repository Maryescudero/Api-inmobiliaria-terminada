using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliaria.Models
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        public string? email { get; set; }
        [DataType(DataType.Password)]
        public string? password{ get; set; }
    }
}