using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Serko_Msg.Models;

namespace Serko_Msg.Controllers
{
    [System.Web.Http.AllowAnonymous]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [ValidateInput(false)]
        [System.Web.Http.HttpPost]
        public JsonResult procemsg([FromBody]string messagecont)
        {
           
            Result _Result = new Result();
            RejectMsg _RejectMsg = new RejectMsg();

            string cost_centre = "", payment_method = "", val = "";
            bool message_rejected = false;
            double total = 0, total_excluding_GST = 0, GST = 0;

            string regularExpressionPattern = @"<(\{1})?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)>";
            Regex regex = new Regex(regularExpressionPattern, RegexOptions.Compiled);
            MatchCollection collection = regex.Matches(messagecont);

            foreach (Match match in collection)
            {
                int Start, End;
                if (messagecont.Contains(match.Value) && messagecont.Contains(match.Value.Replace(@"<", @"</")))
                {
                    Start = messagecont.IndexOf(match.Value, 0, StringComparison.CurrentCulture) + match.Value.Length;

                    End = messagecont.IndexOf(match.Value.Replace(@"<", @"</"), Start, StringComparison.CurrentCulture);

                    val = messagecont.Substring(Start, End - Start);

                    if (match.Value.Contains("<cost_centre>"))
                    {

                        cost_centre = val;

                    }
                    else if (match.Value.Contains("<payment_method>"))
                    {

                        payment_method = val;

                    }

                    else if (match.Value.Contains("<total>"))
                    {

                        total = Convert.ToDouble(val);

                    }

                }
                else
                {
                    message_rejected = true;

                    break;
                }
            }

            if (cost_centre.Length == 0)
            {
                cost_centre = "UNKNOWN";
            }

            if (total <= 0)
            {
                message_rejected = true;
            }
            if (message_rejected)
            {
                _RejectMsg = new RejectMsg()
                {
                    message_rejected = "message is rejected"
                };
                return Json(_RejectMsg, JsonRequestBehavior.AllowGet);


            }
            else
            {
                total_excluding_GST = total / 1.15;
                GST = total - total_excluding_GST;


                 _Result = new Result()
                {
                    cost_centre = cost_centre,
                    payment_method = payment_method,
                    total = total,
                    total_excluding_GST = total_excluding_GST,
                    GST = GST
                };
            }
            
            return Json(_Result, JsonRequestBehavior.AllowGet);


        }

    }
}
