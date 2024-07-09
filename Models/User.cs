using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class User
    {
        public int id {get;set;}
        [Required]
        public string ?firstname {get;set;}
        [Required]
        public string ?lastname{get;set;}
        [Required]
        public string ?password{get;set;}
        
        public string ?gender {get;set;}
    }
}