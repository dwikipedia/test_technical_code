using BookMeetingRoom.Data;
using BookMeetingRoom.Web.App_Start;
using BookMeetingRoom.Web.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookMeetingRoom.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly HttpClient _httpClient;

        public BookController() => _httpClient = ApiClient.Instance;

        public ActionResult Index() => View(new BookViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Book(BookViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", vm);
            }

            try
            {
                var dto = new BookDto
                {
                    Id = vm.Id,
                    Name = vm.Name,
                    NumOfPeople = vm.NumOfPeople,
                    Time = vm.Time,
                    Duration = TimeSpan.FromMinutes(vm.Duration)

                };

                var jsonContent = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("book", content);
                if (response.IsSuccessStatusCode)
                {
                    return View("Index", new BookViewModel());
                }
                else
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<BookingData>>(json);

                    ViewData["Message"] = result.Message;
                    var suggestion = result.Data
                     .Where(s => s != null)
                     .Select(s => new SuggestionViewModel
                     {
                         Id = s.Id,
                         RoomName = s.RoomName,
                         StartTime = s.StartTime,
                         EndTime = s.EndTime
                     })
                     .ToList();

                    ViewData["Suggestions"] = suggestion;

                    ModelState.AddModelError("", "Failed to book a meeting room");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error Occurred: " + ex.Message);
            }

            return View("Index", vm);
        }
    }
}