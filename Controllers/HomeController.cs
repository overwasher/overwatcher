using System;
using Microsoft.AspNetCore.Mvc;

namespace Overwatcher.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        private readonly Random _rnd = new Random();
        
        public string Index()
        {
            var variants = new[]
            {
                "What are you looking for, internet traveller?",
                "See you, space cowboy...",
                "A sound soul dwells within a sound mind and a sound body.",
                "MOTDs are hard...",
            };

            var i = _rnd.Next(variants.Length);
            return variants[i];
        }
    }
}