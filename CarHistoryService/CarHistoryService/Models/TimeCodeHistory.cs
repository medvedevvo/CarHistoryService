using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHistoryService.Models
{
    public class TimeCodeHistory
    {
        public int Id { get; set; }
        public int IdCar { get; set; }
        public int Time { get; set; }

        public TimeCodeHistory()
        {

        }

        public TimeCodeHistory(int IdCar, int Time)
        {
            this.IdCar = IdCar;
            this.Time = Time;
        }
    }
}
