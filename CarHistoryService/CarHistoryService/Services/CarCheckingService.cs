using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CarHistoryService.Models;

namespace CarHistoryService.Services
{
    public class CarCheckingService : Service
    {
        public CarCheckingService(IConfiguration conf) : base(conf.GetSection("Addresses")["CarCheckingService"]) { }

        public async Task<AccuListWithTime> GetAccuState(int IdCar)
        {
            var httpResponseMessage = await Get($"check/cars/{IdCar}/accus/");
            if (httpResponseMessage == null || httpResponseMessage.Content == null)
                return null;

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            try
            {
                var a = JsonConvert.DeserializeObject<AccuListWithTime>(response);
                return a;
            }
            catch
            {
                return null;
            }
        }

        public async Task<AccuListWithTime> GetAccuState(int IdCar, int IdAccu)
        {
            var httpResponseMessage = await Get($"check/cars/{IdCar}/accus/{IdAccu}/");
            if (httpResponseMessage == null || httpResponseMessage.Content == null)
                return null;

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            try
            {
                var a = JsonConvert.DeserializeObject<AccuListWithTime>(response);
                return a;
            }
            catch
            {
                return null;
            }
        }
    }
}
