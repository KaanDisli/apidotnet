using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class Book
    {
    
        public int id {get;set;}
        [Required]
        public string ?title {get;set;}
        [Required]
        public string ?author {get;set;}
        [Required]
        public string ?price {get;set;}
        [Required]
        public string ?category {get;set;}
        [Required]
        public string ?serialNumber {get;set;}
    }
}