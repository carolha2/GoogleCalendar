using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace GoogleCalendar.Controllers
{
    public class OAuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public void CallBack(string code, string error, string state)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                this.GetTokens(code);
            }
        }
        public ActionResult GetTokens(string code)
        {
            var tokenFile = "C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\tokens.json";
            var credentialsFile = "C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\credentials.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\credentials.json"));

            RestClient restClient = new RestClient();
            RestRequest restRequest = new RestRequest();

            restRequest.AddQueryParameter("client_id", credentials["client_id"].ToString());
            restRequest.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            restRequest.AddQueryParameter("code", code);
            restRequest.AddQueryParameter("grant_type", "authorization_code");
            restRequest.AddQueryParameter("redirect_uri", "https://localhost:5000/oauth/callback");

            restClient.BaseUrl = new System.Uri("https://oauth2.googleapis.com/token");
            var response = restClient.Post(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\tokens.json", response.Content);
                return RedirectToAction("Index", "Home");
            }
            return View("Error");

        }

        public ActionResult RefreshToken()
        {
            var credentials = JObject.Parse(System.IO.File.ReadAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\credentials.json"));
            var tokens = JObject.Parse(System.IO.File.ReadAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\tokens.json"));

            RestClient restClient = new RestClient();
            RestRequest restRequest = new RestRequest();

            restRequest.AddQueryParameter("client_id", credentials["client_id"].ToString());
            restRequest.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            restRequest.AddQueryParameter("grant_type", "refresh_token");
            restRequest.AddQueryParameter("refresh_token", tokens["refresh_token"].ToString());

            restClient.BaseUrl = new System.Uri("https://oauth2.googleapis.com/token");
            var response = restClient.Post(restRequest);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject newTokens = JObject.Parse(response.Content);
                newTokens["refresh_token"] = tokens["refresh_token"].ToString();
                System.IO.File.WriteAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\tokens.json", newTokens.ToString());
                return RedirectToAction("Index", "Home", new { status = "success" });
            }
            return View("Error");
        }

        public ActionResult RevokeToken()
        {
            var tokens = JObject.Parse(System.IO.File.ReadAllText("C:\\Users\\hakim\\source\\repos\\GoogleCalendar\\GoogleCalendar\\Files\\tokens.json"));
            RestClient restClient = new RestClient();
            RestRequest restRequest = new RestRequest();

            restRequest.AddQueryParameter("token", tokens["access_token"].ToString());

            restClient.BaseUrl = new System.Uri("https://oauth2.googleapis.com/revoke");
            var response = restClient.Post(restRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home", new { status = "success" });
            }
            return View("Error");

        }
    }
}
