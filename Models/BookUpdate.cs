using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
namespace api.Models
{
    public class BookUpdate
    {
    
        public string ?title {get;set;}
        
        public string ?author {get;set;}
  
        public string ?price {get;set;}
   
        public string ?category {get;set;}
        
        public string ?serialNumber {get;set;}
    }
}