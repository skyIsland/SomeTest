using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IsMatch.Models;
using IsMatch.Helper;
using Microsoft.Extensions.Options;

namespace IsMatch.Controllers
{
    public class HomeController : Controller
    {
        private Param _param;

        public HomeController(IOptions<Param> param)
        {
            _param = param.Value;
        }
        public IActionResult Index()
        {
            ViewBag.Options = _param;
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

        public IActionResult Gallery()
        {
            var factory = new DouBan();
            //var factory=new Mm131();
            ViewBag.List = factory.GetListBelle();
            return View();
        }
    }
}
