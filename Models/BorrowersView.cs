using System.ComponentModel.DataAnnotations;
using System;
namespace Loans.Models
{
    public class BorrowersView
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
        [Required(ErrorMessage="Loan Reason needs to be specified")]
        public string LoanReason {get;set;}
        [Required(ErrorMessage="Please describe the purpose of the loan")]
        public string Description {get;set;}
        [Required(ErrorMessage="Loan Amount needs to be specified")]
        public int LoanAmount {get;set;}
        
    }
}