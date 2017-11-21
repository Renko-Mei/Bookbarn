using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using BookBarn.Data;


namespace BookBarn.Controllers
{
    public class EmailController : Controller
    {

        private readonly InitialModelsContext _context;

        public EmailController(InitialModelsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //var Contact = _context.;

            return View("Email_Input");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Index(string customerEmail, string customerRequest)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("bookbarn", "info@bookbarncanada.com"));
            message.To.Add(new MailboxAddress("mark", customerEmail));
            message.Subject = "test mail in asp.net core";
            message.Body = new TextPart("plain")
            {
                Text = customerRequest
            };
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