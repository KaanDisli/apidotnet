using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using api.Models;


namespace api.Validations
{
    public class ValidateBookAdd: ValidationAttribute
    {
         protected override ValidationResult IsValid(object? value, ValidationContext validationContext){
            var book = value as Book;
            if (book == null || string.IsNullOrWhiteSpace(book.category)  ||string.IsNullOrWhiteSpace(book.title) || string.IsNullOrWhiteSpace(book.price) ||string.IsNullOrWhiteSpace(book.serialNumber) ){
                return new ValidationResult("Incorrect parameters");
            }
            else{
                return ValidationResult.Success;
            }

         }
    }
}