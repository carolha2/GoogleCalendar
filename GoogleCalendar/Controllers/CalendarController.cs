using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using GoogleCalendar.Models;


namespace GoogleCalendar.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ListEvents()
        {
            var tokens = JObject.Parse(System.IO.File.ReadAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\tokens.json"));
            var key = JObject.Parse(System.IO.File.ReadAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\key.json"));

            RestClient restClient = new RestClient();
            RestRequest restRequest = new RestRequest();

            restRequest.AddQueryParameter("key", key["key"].ToString());
            restRequest.AddHeader("Authorization", "Bearer " + tokens["access_token"]);
            restRequest.AddHeader("Accept", "application/json");

            restClient.BaseUrl = new System.Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events");
            var response = restClient.Get(restRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject Events = JObject.Parse(response.Content);
                IEnumerable<Event> ev= Events["items"].ToObject<IEnumerable<Event>>();
                return View(ev);
            }
            return View("Error");
        }
    }
}
