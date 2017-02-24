using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace BulbDigitalChangelog.Controllers
{
    public class SlackWebhookController : Controller
    {
        // GET: SlackWebhook
        public void PostToSOMRepoActivity(repoPost post)
        {
            //byte[] data = Encoding.ASCII.GetBytes(input);

            //WebRequest request = WebRequest.Create("https://hooks.slack.com/services/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");
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = data.Length;
            //using (Stream stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hooks.slack.com/services/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");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var fields = new List<Field>();
            fields.Add(new Field(){ title = post.username, value = post.message, @short = false });

            var attachments = new List<Object>();

            if (post.type == "changelog")
            {
                attachments.Add(new { pretext = "New changelog item for " + post.framework, fallback = post.fallback, color = "#2980b9", fields = fields });
            }
            else if(post.type == "pull")
            {
                attachments.Add(new { pretext = "New Provisioning Activity", fallback = post.fallback, color = "#FB005D", fields = fields });
            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    attachments = attachments
                });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}