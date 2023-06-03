using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Weterynarze.Data;
using Weterynarze.Models;
using System.Data;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace Weterynarze.Controllers
{
    public class VetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vets
        public async Task<IActionResult> Index()
        {
            return _context.Vet != null ?
                        View(await _context.Vet.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Vet'  is null.");
        }

        public async Task<IActionResult> Location()
        {
            return _context.Vet != null ?
                        View(await _context.Vet.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Vet'  is null.");
        }

        public async Task<IActionResult> ClinicLocation()
        {
            return _context.Vet != null ?
                        View(await _context.Vet.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Vet'  is null.");
        }

        public async Task<IActionResult> New()
        {
            return _context.Vet != null ?
                        View(await _context.Vet.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Vet'  is null.");
        }

        [HttpGet]
        public IActionResult GetCoordinates(string searchText)
        {
            string apiKey = "chDMQRlDRv0KngLYo3sXcOtNBESgx1eEU199e4Z1B7U";
            //Console.WriteLine($"searchText: {searchText}");
            string apiUrl = $"https://geocoder.ls.hereapi.com/6.2/geocode.json?searchtext={searchText}&gen=9&apiKey={apiKey}";

            

            try
            {
                // Wykonanie zapytania HTTP do usługi API
                using (var client = new System.Net.WebClient())
                {
                                

                    string responseJson = client.DownloadString(apiUrl);
                    JObject responseObject = JObject.Parse(responseJson);
                    JToken view = responseObject["Response"]["View"][0];
                    JToken result = view["Result"][0];
                    JToken location = result["Location"]["DisplayPosition"];

                    double latitude = location.Value<double>("Latitude");
                    double longitude = location.Value<double>("Longitude");

                    //Console.WriteLine($"latitude: {latitude}");
                    //Console.WriteLine($"longitude: {longitude}");

                    // Zwrócenie wyników jako JSON
                    var coordinates = new { latitude, longitude };

                    //Console.WriteLine($"coordinates: {coordinates}");

                    return Json(coordinates);
                }
            }
            catch
            {
                return Json(null); // W przypadku błędu zwróć null lub odpowiednią informację
            }
        }

        /*
        public string getCoordinatesFromAddress(string addr)
        {
            var url = "https://geocoder.ls.hereapi.com/6.2/geocode.json?searchtext=" +
                    addr +
                    "&gen=" + 9 +
                    "&apiKey=chDMQRlDRv0KngLYo3sXcOtNBESgx1eEU199e4Z1B7U";


            var request = WebRequest.Create(url);

            using (WebResponse wrs = request.GetResponse())
            {
                using (Stream stream = wrs.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        var obj = JObject.Parse(json);
                        string Latitude = (string)obj["Latitude"];
                        string Longitude = (string)obj["Longitude"];

                       
                        return (Latitude + Longitude);
                    }
                }
            }

        }*/






        public static string CityStateCountByIp(string IP)
        {
            //var url = "http://freegeoip.net/json/" + IP;
            //var url = "http://freegeoip.net/json/" + IP;
            string url = "http://api.ipstack.com/" + IP + "?access_key=[7eb55775f303365894f741db574c7eaa]";
            var request = System.Net.WebRequest.Create(url);

            using (WebResponse wrs = request.GetResponse())
            {
                using (Stream stream = wrs.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        var obj = JObject.Parse(json);
                        string City = (string)obj["city"];
                        string Country = (string)obj["region_name"];
                        string CountryCode = (string)obj["country_code"];

                        return (CountryCode + " - " + Country + "," + City);
                    }
                }
            }


            return "";

        }

        // GET: Vets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vet == null)
            {
                return NotFound();
            }

            var vet = await _context.Vet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vet == null)
            {
                return NotFound();
            }

            return View(vet);
        }

        // GET: Vets/Create
       


        public IActionResult Create()
        {
            return View();
        }

        // POST: Vets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,Zwierze,Objawy,Adres,Priorytet")] Vet vet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vet);
        }

        // GET: Vets/Edit/5


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vet == null)
            {
                return NotFound();
            }

            var vet = await _context.Vet.FindAsync(id);
            if (vet == null)
            {
                return NotFound();
            }
            return View(vet);
        }

        // POST: Vets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,Zwierze,Objawy,Adres,Priorytet")] Vet vet)
        {
            if (id != vet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VetExists(vet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vet);
        }

        // GET: Vets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vet == null)
            {
                return NotFound();
            }

            var vet = await _context.Vet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vet == null)
            {
                return NotFound();
            }

            return View(vet);
        }

        // POST: Vets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vet'  is null.");
            }
            var vet = await _context.Vet.FindAsync(id);
            if (vet != null)
            {
                _context.Vet.Remove(vet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VetExists(int id)
        {
          return (_context.Vet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
