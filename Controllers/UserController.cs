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
                HttpContext.Session.SetInt32("UserId",lender.LendersId);
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
                HttpContext.Session.SetInt32("UserId",borrower.BorrowersId);
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
        // Show Lender

        [HttpGet]
        [Route("lender/show")]
        public IActionResult ShowLender()
        {
            ViewBag.errors= new List<string>();
            int? Id=HttpContext.Session.GetInt32("UserId");
            Lenders lender=_context.Lenders
                .OrderByDescending(a=>a.CreatedAt).
                SingleOrDefault(b=>b.LendersId==(int)Id);
            ViewBag.Lender= lender;
            List<UserLoans> userLoans= new List<UserLoans>();
            userLoans = _context.UserLoans
                                .Include(a=>a.Borrowers)
                                .Include(a=>a.Lenders)
                                .Where(a=>a.LendersId==lender.LendersId)
                                .ToList();
            List<Borrowers> borrowers= _context.Borrowers.ToList();
            ViewBag.Borrowers= borrowers;
            ViewBag.UserLoans= userLoans;
            if(TempData["Errors"]!=null){
                ViewBag.errors.Add(TempData["Errors"]);
            }
            
            return View("lender");

        }


        // Show Borrower

        [HttpGet]
        [Route("Borrower/show")]
        public IActionResult ShowBorrower()
        {
            ViewBag.errors= new List<string>();
            int? Id=HttpContext.Session.GetInt32("UserId");
            Borrowers borrower=_context.Borrowers
                .OrderByDescending(a=>a.CreatedAt).
                SingleOrDefault(b=>b.BorrowersId==(int)Id);
            ViewBag.Borrower= borrower;
            List<UserLoans> userLoans = _context.UserLoans
                                            .Include(a=>a.Borrowers)
                                            .Include(a=>a.Lenders)
                                            .Where(a=>a.BorrowersId==borrower.BorrowersId)
                                            .ToList();
            ViewBag.UserLoans= userLoans;
            ViewBag.UserLoans= userLoans;
            if(TempData["Errors"]!=null){
                ViewBag.errors.Add(TempData["Errors"]);
            }
            
            return View("borrower");

        }




        [HttpPost]
        [Route("lend/{BId}")]
        public IActionResult Lend(int BId, int Money)
        {
            
            int? Id=HttpContext.Session.GetInt32("UserId");
            
            Lenders lender=_context.Lenders
                .OrderByDescending(a=>a.CreatedAt).
                SingleOrDefault(b=>b.LendersId==(int)Id);
        
            if (lender.Bank < Money){
                TempData["Errors"]="Insufficient Funds!";
                
            }
            else{

                Borrowers borrower=_context.Borrowers
                .OrderByDescending(a=>a.CreatedAt).
                SingleOrDefault(b=>b.BorrowersId==BId);
                
                UserLoans checkLoan= _context.UserLoans
                                            .Include(a=>a.Borrowers)
                                            .Include(a=>a.Lenders)
                                            .SingleOrDefault(a=>(a.BorrowersId==BId) && (a.LendersId==(int)Id));
                if(checkLoan!=null){
                    checkLoan.Amount=checkLoan.Amount+Money;
                }
                else{

                    UserLoans newLoan= new UserLoans{
                    Amount= Money,
                    BorrowersId=BId,
                    LendersId=(int)Id,
                    CreatedAt=DateTime.Now,
                    UpdatedAt= DateTime.Now
                    };
                     _context.UserLoans.Add(newLoan);

                }

                borrower.Raised=borrower.Raised+Money;
                lender.Lent=lender.Lent+Money;
                lender.Bank=lender.Bank-Money;
                _context.SaveChanges();


            }

            return RedirectToAction("ShowLender");
          
            
        }
        // To Login 

        [HttpGet]
        [Route("user/login")]
        public IActionResult ToLogin(){
            ViewBag.errors=new List<string>();
            return View("login");
        }

        [HttpPost]
        [Route("member/Login")]
        public IActionResult Login(string Email, string Password){
            Borrowers borrower= new Borrowers();
            Lenders lender = new Lenders();
             borrower= _context.Borrowers.SingleOrDefault(a=>a.Email==Email);
           lender= _context.Lenders.SingleOrDefault(a=>a.Email==Email);

            if(borrower==null&& lender==null){
                ViewBag.errors.Add("Email doesn't exist please register");
                return View("Index");
            }
            else{    
                    if (borrower!=null){
                         if (borrower.Password==Password){
                            HttpContext.Session.SetInt32("UserId",borrower.BorrowersId);
                            return RedirectToAction("ShowBorrower");
                        }
                    }  
                    else if (lender!=null){
                        if(lender.Password==Password){
                            HttpContext.Session.SetInt32("UserId",lender.LendersId);
                            return RedirectToAction("ShowLender");
                        }
                    }
                     
                    else{
                        ViewBag.errors.Add("Email or password is incorrect");
                    }
                   
                    return View("Index");
                }
        

        }

        [HttpGet]
        [Route("user/logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }



    }
}
