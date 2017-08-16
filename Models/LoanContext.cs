
using Microsoft.EntityFrameworkCore;
 
namespace Loans.Models
{
    public class LoanContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public LoanContext(DbContextOptions<LoanContext> options) : base(options) { }
        public DbSet<Lenders> Lenders {get;set;}
        public DbSet<Borrowers> Borrowers {get;set;}
        public DbSet<UserLoans> UserLoans {get;set;}
    }
}