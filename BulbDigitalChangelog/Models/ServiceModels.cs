using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BulbDigitalChangelog
{
    public class BuildResponse
    {
        public string Framework;
        public List<ServiceChangelog> Changelogs;
        public string Version;
        public DateTime BuildDate = DateTime.Now;
    }

    public class ServiceChangelog
    {
        public string ChangeNote;
        public DateTime DateLogged;
        public string Username;
    }

    public class textResponse
    {
        public string text { get; set; }
        public List<Attachment> attachments { get; set; }
    }

    public class repoPost
    {
        public string framework;
        public string type;
        public string username;
        public string message;
        public string fallback;
    }

    public class Attachment
    {
        //Required plain-text summary of the attachment
        public string fallback { get; set; }
        //#36a64f
        public string color { get; set; }
        //Optional text that appears above the attachment block
        public string pretext { get; set; }
        //Slack API Documentation
        public string title { get; set; }

        //https://api.slack.com/
        public string title_link { get; set; }
        //Optional text that appears within the attachment
        public string text { get; set; }
        public List<Field> fields { get; set; }
        public string footer { get; set; }
        public List<string> mrkdwn_in { get; set; }
    }

    public class Field
    {
        public string title;
        public string value;
        public bool @short;
    }

    public class SlackPost
    {
        public string token { get; set; }
        public string team_id { get; set; }
        public string team_domain { get; set; }
        public string channel_id { get; set; }
        public string channel_name { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string command { get; set; }
        public string text { get; set; }
    }
}