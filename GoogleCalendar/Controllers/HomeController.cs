using GoogleCalendar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace GoogleCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ActionResult OauthRedirect()
        {

            var credentialsFile = "C:\\Users\\hakim\\source\\repos\\GoogleCalanderDIsplayer\\GoogleCalanderDIsplayer\\Files\\credentials.json";
            JObject credentials = JObject.Parse(System.IO.File.ReadAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\credentials.json"));
            var client_id = credentials["client_id"];
            Console.WriteLine(credentials);
            var redirectURL = "https://accounts.google.com/o/oauth2/v2/auth?" +
                "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                "access_type=offline&" +
                "include_granted_scopes=true&" +
                "response_type=code&" +
                "state=hellothere&" +
                "redirect_uri=https://localhost:5000/oauth/callback&" +
                "client_id=" + client_id;
            return Redirect(redirectURL);
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
