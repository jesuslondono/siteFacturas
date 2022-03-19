using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using siteFacturas.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using wsFacturas.Models;
using wsFacturas.Models.Entities;

namespace siteFacturas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            //var json = await httpClient.GetStringAsync("https://localhost:44373/api/Factura");
            String Token = string.Empty;
            List<Factura> lstFac = null;
            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44373/Users/authenticate"))
            {
                request.Content = new StringContent("{\"username\":\"prueba\",\"password\":\"prueba\"}", Encoding.UTF8, "application/json"); 
                var response = await httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                var authRespon = JsonConvert.DeserializeObject<AuthenticateResponse>(json);
                Token = authRespon.Token;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44373/api/Factura"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var jsonFac = await response.Content.ReadAsStringAsync();
                lstFac = JsonConvert.DeserializeObject<List<Factura>>(jsonFac);
            }

            return View(lstFac);
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
}
