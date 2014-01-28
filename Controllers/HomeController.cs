using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcPersonalSite.Models;
using System.Net.Mail;

namespace MvcPersonalSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Thanks for Checking in";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Resume()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactData data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage m = new MailMessage("hello@greggrcarey.com",
                        "gregg.carey@outlook.com", "Email from MVCPersonalSite", string.Format(
                        "{0} has sent you the following message: {1}</br></br>" +
                        "respond to this address:{2}", data.Name, data.Comments, data.Email));

                    m.ReplyToList.Add(data.Email);
                    m.Priority = MailPriority.High;
                    m.IsBodyHtml = true;

                    SmtpClient client = new SmtpClient("relay-hosting.secureserver.net");

                    //uncomment for deploy
                    client.Send(m);

                    ViewBag.Message = "Your message has been sent.";



                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error occured and your message was unsent.";
                    ViewBag.ExceptionMessage = ex.StackTrace;

                }

                return View("ContactThanks", data);
            }//end if 
            else
            {
                return View();
            }
        }



    }
}
