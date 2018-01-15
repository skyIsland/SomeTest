using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IsMatch.Models;
using IsMatch.Helper;

namespace IsMatch.Controllers
{
    public class HomeController : Controller    
    {
        public IActionResult Index()
        {
            var db = new DouBan();
            ViewBag.List = db.GetListBelle();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "关于我";
            ViewData["Message"] = "克莱登大学南极洲文学史副教授.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = "联系方式";
            ViewData["Message"] = "暂无";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
