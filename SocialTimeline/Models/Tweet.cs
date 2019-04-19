using System;

namespace SocialTimeline.Models
{
    public class Tweet
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public User TwitterUser { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
    }
}