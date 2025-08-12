using System.Text;

namespace BookMeetingRoom.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HttpClient _httpClient;

        public BookingRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<BookingData>>> BookRoomAsync(BookDto dto)
        {
            var jsonContent = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("book", content);
            string json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApiResponse<List<BookingData>>>(json);
        }
    }

}
