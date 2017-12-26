using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHistoryService.Models
{
    public class AccuHistoryResponce
    {
        public int time = 0;
        public int idAccu = 0;
        public string nameAccu = "";
        public double voltage = 0.0;
        public double current = 0.0;
        public int charge = 0;

        public AccuHistoryResponce()
        {

        }

        public AccuHistoryResponce(int time, int idAccu, string nameAccu, double voltage, double current, int charge)
        {
            this.time = time;
            this.idAccu = idAccu;
            this.nameAccu = nameAccu;
            this.voltage = voltage;
            this.current = current;
            this.charge = charge;
        }
    }
}
