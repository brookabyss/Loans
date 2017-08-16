using System;
using System.Collections.Generic;
namespace Loans.Models
{
    public class Lenders
    {
        public int LendersId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public int Bank {get;set;}
        public int Lent {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<UserLoans> UserLoans  {get;set;}

        public Lenders(){
           UserLoans= new List<UserLoans>();
        }
    }
}