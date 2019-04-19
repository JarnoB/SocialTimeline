using Newtonsoft.Json.Linq;
using SocialTimeline.Models;
using SocialTimeline.OAuth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SocialTimeline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Tweets");
        }
        
        /// <summary>
        /// Retrieves Tweets via the Twitter API.
        /// </summary>
        /// <returns>Returns a view with tweets</returns>
        public async Task<ActionResult> Tweets()
        {
            try
            {
                // Default user to show
                string screenName = "DevJarno";
                // Declaring routeId
                string routeId;
                // Declaring and initializing a list with Tweet models. 
                List<Tweet> tweets = new List<Tweet>();

                HttpClient client = new HttpClient(new OAuthMessageHandler(new HttpClientHandler()))
                {
                    BaseAddress = new Uri("http://localhost")
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Checks if an username is present in the URL. If so, the application will try to request those tweets.
                if (RouteData.Values["id"] != null)
                {
                    routeId = RouteData.Values["id"].ToString();
            
                    // if the username is different than the default username, change the API-url to this username
                    if (routeId != screenName)
                    {
                        screenName = routeId;
                    }
                }

                // Requests twitter API for tweets
                HttpResponseMessage response = await client.GetAsync("https://api.twitter.com/1.1/statuses/user_timeline.json?include_rts=true&exclude_replies=true&count=40&screen_name=" + screenName);

                if (response.IsSuccessStatusCode)
                {
                    JToken result = await response.Content.ReadAsAsync<JToken>();
                    
                    // gathers the information of each tweet and puts it in a 
                    foreach (var tweet in result)
                    {
                        // Declaring and initializing local Tweet model
                        Tweet model = new Tweet();

                        // Gathering user information and putting this information in Tweet model.
                        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        model.TwitterUser = serializer.Deserialize<User>(tweet["user"].ToString());

                        // Gathering Tweet/message information and putting this information in model.
                        model.Message = tweet["text"].ToString();

                        // Gathering date of post, putting it in the model in the correct local-set format
                        string dateStr = tweet["created_at"].ToString();
                        model.Date = DateTime.ParseExact(dateStr, "ddd MMM dd HH:mm:ss zzz yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        // Adding tweet to list
                        tweets.Add(model);
                    }
                    //Returning view with list of tweets. 
                    return View(tweets);
                }   
            }
            catch (Exception ex)
            {
                // Redirects you to the Error page when an error occurs.
                ViewBag.Error = ex.Message;
                Response.Redirect("Error.cshtml");
            }
            return View();
        }
    }
}