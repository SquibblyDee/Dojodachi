using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
                HttpContext.Session.SetObjectAsJson("OurPetString", ourPet);
                return View(ourPet);
            }
            DojodachiModel ourSessionPet = HttpContext.Session.GetObjectFromJson<DojodachiModel>("OurPetString");
            return View(ourSessionPet);
        }

        [HttpGet("feed")]
        public IActionResult Feed()
        {
            DojodachiModel ourSessionPet = HttpContext.Session.GetObjectFromJson<DojodachiModel>("OurPetString");
            if(ourSessionPet.Meals > 0)
            {
                Random random = new Random();
                int howMuchFood = random.Next(5,11);
                ourSessionPet.Fullness+=howMuchFood;
                ourSessionPet.Meals-=1;
                HttpContext.Session.SetString("Message", $"You fed your Dojodachi! Fullness +{howMuchFood}, Meals -1");
                HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("Message", "You need at least 1 meal to feed your Dojodochi");
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

    public static class SessionExtensions
    {
        // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // This helper function simply serializes theobject to JSON and stores it as a string in session
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // generic type T is a stand-in indicating that we need to specify the type on retrieval
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            // Upon retrieval the object is deserialized based on the type we specified
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
