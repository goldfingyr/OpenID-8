using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebAppMVC.Models;
using WebAppMVC.Services;

namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        public WeatherDataList weatherData { get; set; }

        private readonly ILogger<HomeController> _logger;
        private readonly WeatherService _myService;


        public HomeController(ILogger<HomeController> logger, WeatherService myService)
        {
            _logger = logger;
            _myService = myService;
        }

        public IActionResult Index()
        {
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            try
            {
                var data = _myService.GetProtectedDataAsync(accessToken).Result;
                List<WeatherData>  weatherDataList = JsonConvert.DeserializeObject<List<WeatherData>>(data);
                weatherData = new();
                weatherData.data = new();
                foreach (var item in weatherDataList)
                {
                    weatherData.data.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ViewBag.weatherData = weatherData;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class WeatherData
    {
        public string date { get; set; }
        public string temperatureC { get; set; }
        public string temperatureF { get; set; }
        public string summary { get; set; }

    }

    public class WeatherDataList
    {
        public List<WeatherData> data { get; set; }
    }
}
