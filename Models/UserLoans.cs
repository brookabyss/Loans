using System;
namespace Loans.Models
{
    public class UserLoans
    {
        public int UserLoansId {get;set;}
        public int Amount {get;set;}
        
        public int LendersId {get;set;}
        public Lenders Lenders {get;set;}

        public int BorrowersId {get;set;}
        public Borrowers Borrowers {get;set;}


        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
    }
}