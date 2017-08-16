using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Loans.Models;
using System.Linq;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Loans.Controllers
{
    public class UserController : Controller
    {
        private LoanContext _context;
        public UserController(LoanContext context){
            _context=context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ModelBundle newBundle= new ModelBundle();
            ViewBag.Bundle= newBundle;
            return View(newBundle);
        }

        // Create Lender
        [HttpPost]
        [Route("Lender/new")]
        public IActionResult LenderRegister(LendersView LenView)
        {
            if(ModelState.IsValid){
                Lenders newLend= new Lenders {
                    FirstName=LenView.FirstName,
                    LastName=LenView.LastName,
                    Email=LenView.Email,
                    Password=LenView.Password,
                    Bank=LenView.Bank,
                    CreatedAt= DateTime.Now,
                    UpdatedAt= DateTime.Now,
                };
                _context.Lenders.Add(newLend);
                _context.SaveChanges();
                List<Borrowers> borrowers= _context.Borrowers.ToList();
                ViewBag.Borrowers= borrowers;

                Lenders lender=_context.Lenders
                .OrderByDescending(a=>a.CreatedAt).
                SingleOrDefault(b=>b.Email==LenView.Email);
                ViewBag.Lender= lender;

                 List<UserLoans> userLoans = _context.UserLoans
                                            .Include(a=>a.Borrowers)
                                            .Include(a=>a.Lenders)
                                            .Where(a=>a.LendersId==lender.LendersId)
                                            .ToList();
                ViewBag.UserLoans= userLoans;
                return View("lender");
            }
            else{
                ViewBag.Errors= ModelState.Values;
                return RedirectToAction("Index;");
            }
           
        }
       

        [HttpPost]
        [Route("Borrower/new")]
        public IActionResult BorrowerRegister(BorrowersView BorrView)
        {
             if(ModelState.IsValid){
                 Borrowers newBor= new Borrowers {
                    FirstName=BorrView.FirstName,
                    LastName=BorrView.LastName,
                    Email=BorrView.Email,
                    Password=BorrView.Password,
                    LoanReason=BorrView.LoanReason,
                    Description= BorrView.Description,
                    LoanAmount= BorrView.LoanAmount,
                    CreatedAt= DateTime.Now,
                    UpdatedAt= DateTime.Now,
                };
                _context.Borrowers.Add(newBor);
                _context.SaveChanges();
               
               
                Borrowers borrower=_context.Borrowers
                .OrderByDescending(a=>a.CreatedAt).
                SingleOrDefault(b=>b.Email==BorrView.Email);
                ViewBag.borrower=borrower;

                 List<UserLoans> userLoans = _context.UserLoans
                                            .Include(a=>a.Borrowers)
                                            .Include(a=>a.Lenders)
                                            .Where(a=>a.BorrowersId==borrower.BorrowersId)
                                            .ToList();
                 ViewBag.UserLoans= userLoans;
                 return View("borrower");
            }
            else{
                ViewBag.Errors= ModelState.Values;
                return RedirectToAction("Index");
            }
        }
    }
}
