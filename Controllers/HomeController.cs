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
                ourSessionPet.Meals-=1;
                int howMuchFood = random.Next(5,11);
                int Like = random.Next(4);
                if(Like == 1)
                {
                    HttpContext.Session.SetString("Message", "Your Dojodachi didn't like that!");
                    HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
                    return RedirectToAction("Index");
                }
                ourSessionPet.Fullness+=howMuchFood;
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
            DojodachiModel ourSessionPet = HttpContext.Session.GetObjectFromJson<DojodachiModel>("OurPetString");
            if(ourSessionPet.Energy > 4)
            {
                ourSessionPet.Energy-=5;
                Random random = new Random();
                int HowHappy = random.Next(5, 11);
                int Like = random.Next(4);
                if(Like == 1)
                {
                    HttpContext.Session.SetString("Message", "Your Dojodachi didn't like that!");
                    HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
                    return RedirectToAction("Index");
                }
                ourSessionPet.Happiness+=HowHappy;
                HttpContext.Session.SetString("Message", $"You played with your Dojodachi! Happiness +{HowHappy}, Energy -5");
                HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
                return RedirectToAction("Index");

            }
            HttpContext.Session.SetString("Message", "You need 5 energy to play with your Dojodachi");
            HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
            return RedirectToAction("Index");
        }

        [HttpGet("work")]
        public IActionResult Work()
        {
            DojodachiModel ourSessionPet = HttpContext.Session.GetObjectFromJson<DojodachiModel>("OurPetString");
            if(ourSessionPet.Energy > 4)
            {
                ourSessionPet.Energy-=5;
                Random random = new Random();
                int HowMany = random.Next(4);
                ourSessionPet.Meals+=HowMany;
                HttpContext.Session.SetString("Message", $"You put your Dojodachi to work! Meals {ourSessionPet.Meals}, Energy -5");
                HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("Message", "You need 5 energy to put your Dojodachi to work");
            HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
            return RedirectToAction("Index");
        }

        [HttpGet("sleep")]
        public IActionResult Sleep()
        {
            DojodachiModel ourSessionPet = HttpContext.Session.GetObjectFromJson<DojodachiModel>("OurPetString");
            ourSessionPet.Energy+=15;
            ourSessionPet.Fullness-=5;
            ourSessionPet.Happiness-=5;
            if(ourSessionPet.Happiness < 1 || ourSessionPet.Fullness < 1)
            {
                HttpContext.Session.SetString("Message", "You Dojodachi died in it's sleep!");
                HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("Message", "Your Dojodachi went to sleep");
            HttpContext.Session.SetObjectAsJson("OurPetString", ourSessionPet);
            return RedirectToAction("Index");
        }

        [HttpGet("clearsession")]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear();
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
