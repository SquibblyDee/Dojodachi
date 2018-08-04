using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("dojodachi")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Message") == null)
            {
                HttpContext.Session.SetString("Message", "Welcome to Dojodachi");
                DojodachiModel ourPet = new DojodachiModel()
                {
                    Fullness = 20,
                    Happiness = 20,
                    Meals = 3,
                    Energy = 50
                };
                return View(ourPet);
            }
            return View();
        }

        [HttpGet("feed")]
        public IActionResult Feed()
        {
            HttpContext.Session.SetString("Message", "You fed your Dojodachi");
            return RedirectToAction("Index");
        }

        [HttpGet("play")]
        public IActionResult Play()
        {
            HttpContext.Session.SetString("Message", "You played with your Dojodachi");

            return RedirectToAction("Index");
        }

        [HttpGet("work")]
        public IActionResult Work()
        {
            HttpContext.Session.SetString("Message", "You put your Dojodachi to work");

            return RedirectToAction("Index");
        }

        [HttpGet("sleep")]
        public IActionResult Sleep()
        {
            HttpContext.Session.SetString("Message", "You Dojodachi went to sleep");

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
