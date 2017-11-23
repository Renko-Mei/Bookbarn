using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using BookBarn.Data;
using Microsoft.EntityFrameworkCore;


namespace BookBarn.Controllers
{
    public class EmailController : Controller
    {

        private readonly AuthenticationContext _context;

        public EmailController(AuthenticationContext context)
        {
            _context = context;
        }

        public string EmailInfo()
        {
            string userEmail = _context.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).Email;                         
            return userEmail;
        }

        public string NameInfo()
        {
            return _context.Users.FirstOrDefault(c => c.UserName == User.Identity.Name).UserName;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["userEmail"] = EmailInfo();
            //ViewData["userName"] = NameInfo();

            return View("Email_Input");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Index(string customerEmail, string customerRequest, string customerPhoneNumber)
        {
            string customerName = "Customer Name: " + NameInfo();
            //string customerDetail = "CustomerName: " + customerName;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BookBarn", "info@bookbarncanada.com"));
            message.To.Add(new MailboxAddress("mark", customerEmail));//this should be changed to seller email
            message.Subject = "test mail in asp.net core";
            var builder = new BodyBuilder();
            builder.TextBody = @"Dear seller,";
            if(customerPhoneNumber ==null){
                customerPhoneNumber = "not provided";
            }
            builder.HtmlBody = "<em>"+customerName+"<br>Customer Email: "+customerEmail+ "<br>Customer Phone Number: "+ customerPhoneNumber + "</em><br><br>Customer concern is: <br>"+customerRequest;

            message.Body = builder.ToMessageBody();
            //message.Body = new TextPart("plain")
            //{
            //    Text = "CustomerName: " + customerName +
            //        customerRequest
            //};
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("info@bookbarncanada.com", "InfoBookBarn");
                client.Send(message);

                client.Disconnect(true);
            }

            return View("Index");
        }
    }
}