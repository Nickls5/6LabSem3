using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Lab6
{

    struct Weather
    {
        public string Country { get; set; }
        public string Name { get; set; }

        public double Temp { get; set; }

        public string Description { get; set; }


        public void init()
        {
            bool hasName = false;

            while (!hasName)
            {
                Random random = new Random();
                double latitude = random.Next(-90, 91);
                double longitude = random.Next(-180, 181);
                string apiKey = "4684c129508faf1a91054864cde77335";

                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}";
                HttpClient client = new HttpClient();

                string responseBody = client.GetStringAsync(apiUrl).Result;

                if (IsFilled(responseBody))
                {
                    JObject jsonObject = JObject.Parse(responseBody);
                 
                    Country = (string)jsonObject["sys"]["country"];
                    Name = (string)jsonObject["name"];
                    Temp = (double)jsonObject["main"]["temp"];
                    Description = (string)jsonObject["weather"][0]["description"];
                    hasName = true;
                }

            }
        }

        private bool IsFilled(string responseBody)
        {
            JObject jsonObject = JObject.Parse(responseBody);
            return (string)jsonObject["name"] != "";
        }

    }

    class Program
    {
        static void Main()
        {
             
            List<Weather> list = new List<Weather>(50);

            for (int i = 0; i < 50; i++)
            {
                Weather weather = new Weather();
                weather.init();
                list.Add(weather);
            }

            var min = list.OrderBy(w => w.Temp).First();
            Console.WriteLine($"Min temp {min.Country}");
            var max = list.OrderBy(w => w.Temp).Last();
            Console.WriteLine($"Max temp {max.Country}");
            var avr = list.Average(w => w.Temp);
            Console.WriteLine($"Average temp {avr}");
            var count = list.Select(w => w.Country).Distinct().Count();
            Console.WriteLine($"Count {count}");
            var result = list.FirstOrDefault(w => w.Description == "clear sky" || w.Description == "rain" || w.Description == "few clouds");
            Console.WriteLine($"Result {result.Country}");
            Console.ReadLine();
        }
    }

}