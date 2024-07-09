using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using api.Models;

namespace api.Validations
{
    public class ValidateUserAdd: ValidationAttribute
    //validationContext is whats passed in the parameter of the function being validated
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext){
            var user = validationContext.ObjectInstance as User;
            if (user == null || string.IsNullOrWhiteSpace(user.firstname) || string.IsNullOrWhiteSpace(user.lastname)  ||string.IsNullOrWhiteSpace(user.password)){
                return new ValidationResult("Incorrect parameters");
            }
            else{
                return ValidationResult.Success;
            }

        }    
    }
}