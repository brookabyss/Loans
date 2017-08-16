using System.ComponentModel.DataAnnotations;
using System;
namespace Loans.Models
{
    public class LendersView
    {
        [Required]
        [MinLength(2, ErrorMessage="First name has to be 2 characters in length at least")]
        public string FirstName {get;set;}
        [Required]
        [MinLength(2, ErrorMessage="Last name has to be 2 characters in length at least")]
        public string LastName {get;set;}
        [Required]
        [EmailAddress (ErrorMessage="Incorrect email format")]
        public string Email {get;set;}
        [Required]
        [MinLength(8, ErrorMessage="Password needs to be at least 8 characters in length")]
        public string Password {get;set;}
        [Required (ErrorMessage="Bank field needs to be completed.")]
        public int Bank {get;set;}
        
    }
}