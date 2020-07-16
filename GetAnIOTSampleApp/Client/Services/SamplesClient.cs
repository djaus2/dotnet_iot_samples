using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using ProjectClasses;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetAnIOTSampleApp.Client.Services
{
        public class AlphaCount
        {
            public char Key { get; set; }
            public int Count { get; set; } = 0;
        }

        public class SampleCount
        {
            public string Key { get; set; }
            public int Count { get; set; } = 0;
        }
        public class SamplesClient
        {
            private readonly HttpClient client;
            private const string ServiceEndpoint = "/api/Samples";

            public static char CurrentIndex { get; set; } = '0';

            public static Dictionary<string, List<Project>> Projects { get; set; }
            public static List<IGrouping<char, KeyValuePair<string, List<Project>>>> Samples = null;
            public static Dictionary<char, List<SampleCount>> AlphaDict = null;
            public static List<AlphaCount> AlphaCount { get; set; }
            public static Project CurrentProject { get; set;}

        public SamplesClient(HttpClient client)
            {
                this.client = client;
            }

        public async Task<string> GetFile (string path)
        {
            string fileContents = "";
            var strn = await client.GetAsync(ServiceEndpoint + $"/{path}");
            fileContents = await strn.Content.ReadAsStringAsync();
            return fileContents;
        }

            public async Task<IEnumerable<IGrouping<char, KeyValuePair<string, List<Project>>>>> Get()
            {
                


                try
                {
                    var strn = await client.GetAsync(ServiceEndpoint);
                    string content = await strn.Content.ReadAsStringAsync();
                    Projects = JsonConvert.DeserializeObject< Dictionary<string, List<Project>> >(content);
                    var Devices = Projects.Keys.ToList();
                var DeviceCounts = (from p in Projects
                                select new Tuple<string, int>(p.Key, p.Value.Count()))
                                .ToList();

                 Samples = Projects.GroupBy(x => char.ToUpper(x.Key[0]))
                    .ToList();


                AlphaCount = new List<AlphaCount>();
                foreach (var c in Samples)
                {
                    var alp = new AlphaCount { Key = c.Key };
                    foreach (var b in c)
                    {
                        alp.Count += b.Value.Count();
                    }
                    AlphaCount.Add(alp);
                }

                AlphaDict = new Dictionary<char, List<SampleCount>>();
                foreach (var sample in Samples)
                {
                    List<SampleCount> projs = new List<SampleCount>();
                    foreach (var subSample in sample)
                    {
                        projs.Add(new SampleCount { Key = subSample.Key, Count = subSample.Value.Count() });
                    }
                    AlphaDict.Add(sample.Key, projs);
                }
            }
                catch (Newtonsoft.Json.JsonException ex2) // Invalid JSON
                {
                    System.Diagnostics.Debug.WriteLine("Invalid JSON.");
                    System.Diagnostics.Debug.WriteLine(ex2.Message);
                    System.Diagnostics.Debug.WriteLine(ex2.InnerException);
                }
                catch (HttpRequestException ex) // Non success
                {
                    System.Diagnostics.Debug.WriteLine("An Http error occurred.");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.InnerException);
                }
                catch (NotSupportedException ex1) // When content type is not valid
                {
                    System.Diagnostics.Debug.WriteLine("The content type is not supported.");
                    System.Diagnostics.Debug.WriteLine(ex1.Message);
                    System.Diagnostics.Debug.WriteLine(ex1.InnerException);
                }
                catch (Exception ex3)
                {
                Samples = null;
                    System.Diagnostics.Debug.WriteLine("Other error occured");
                    System.Diagnostics.Debug.WriteLine(ex3.Message);
                    System.Diagnostics.Debug.WriteLine(ex3.InnerException);
                }
                return Samples;
            }
            
        /*
            public async Task<List<BookingSlot>> GetSlotListFwd()// (int Id = 0)
            {
                var bookings = new BookingSlot[0];

                try
                {
                    bookings = await client.GetFromJsonAsync<BookingSlot[]>(
                        ServiceEndpoint);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.InnerException);
                }

                var books = from b in bookings where b.Date.Date >= DateTime.Now.Date select b;
                bookings = books.ToArray<BookingSlot>();
                var books2 = bookings.OrderByDescending(b => b.Date).ThenBy(b => b.Time);
                List<BookingSlot> slots = books2.ToList<BookingSlot>();

                return slots;
            }


            public async Task<BookingSlot> GetBookingSlot(DateTime date, TimeSpan time)
            {
                var bookings = new BookingSlot[0];

                try
                {

                    bookings = await client.GetFromJsonAsync<BookingSlot[]>(
                        ServiceEndpoint);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.InnerException);
                    return null;
                }

                var book = from b in bookings where (b.Date == date) && (b.Time == time) select b;
                return book.FirstOrDefault();
            }

            public async Task<List<BookingSlot>> GetBookingSlotsForDay(DateTime date)
            {
                List<TimeSpan> myBook = new List<TimeSpan>();
                var bookingsQ = new List<BookingSlot>();
                try
                {
                    var books1 = await GetSlotList();
                    var books = from b in books1 where (b._Date == date.ToString("yyyy-MM-dd")) select b;
                    //bookingsQ = books.ToList<BookingSlot>();
                    //int[] times = books.AsEnumerable().Select(s => s._Time).ToArray<int>();
                    //foreach (var time in times)
                    //{
                    //    var spots = from t in books where t._Time == time select t;
                    //    int count = spots.Count();
                    //    if (count != 0)
                    //    {
                    //        TimeSpan timeTS = new TimeSpan(0, time * 30, 0);
                    //        if (!myBook.Contains(timeTS))
                    //            myBook.Add(timeTS);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.InnerException);
                }

                return bookingsQ;
            }

            public async Task AddBookingSlot(BookingSlot bookingSlot)
            {
                bool exists = await Exists(bookingSlot);
                if (exists)
                    return;
                await client.PostAsJsonAsync(ServiceEndpoint, bookingSlot);
            }

            public async Task DeleteBookingSlot(int id)
            {
                await client.DeleteAsync($"{ServiceEndpoint}/{id}");
            }


            /// /////// Checks ///////////////////////////

            public async Task<bool> Exists(BookingSlot bookingSlot)
            {
                return await Exists(bookingSlot.Date, bookingSlot.Time);
            }

            public async Task<bool> Exists(DateTime date, TimeSpan time)
            {
                int id = await GetBookingSlotId(date, time);
                return (id > 0);
            }

            public async Task<int> GetBookingSlotId(DateTime date, TimeSpan time)
            {
                var bookings = new BookingSlot[0];

                try
                {

                    bookings = await client.GetFromJsonAsync<BookingSlot[]>(
                        ServiceEndpoint);
                    var book = from b in bookings where (b.Date == date.Date) && (b.Time == time) select b;
                    BookingSlot slot = book.FirstOrDefault();
                    if (slot == null)
                        return 0;
                    return slot.Id;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.InnerException);
                    return -1;
                }
            }

            public async Task<BookingSlot> GetBookingSlot(int id)
            {
                BookingSlot bookingSlot = await client.GetFromJsonAsync<BookingSlot>($"{ServiceEndpoint}/{id}");
                return bookingSlot;
            }
        */
        }
    }
