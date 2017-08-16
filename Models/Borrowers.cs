using System;
using System.Collections.Generic;
namespace Loans.Models
{
    public class Borrowers
    {
        public int BorrowersId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public string LoanReason {get;set;}
        public string Description {get;set;}
        public int LoanAmount {get;set;}
        public int Raised {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<UserLoans> UserLoans  {get;set;}

        public Borrowers(){
           UserLoans= new List<UserLoans>();
        }
    }
}