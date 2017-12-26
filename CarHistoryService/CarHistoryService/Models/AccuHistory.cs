using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHistoryService.Models
{
    public class AccuHistory
    {
        public int Id { get; set; }
        public int IdTimeCode { get; set; }
        public int IdAccu{ get; set; }
        public string NameAccu { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
        public int Charge { get; set; }

        public AccuHistory()
        {

        }

        public AccuHistory(int IdTimeCode, int IdAccu, string NameAccu, double Voltage, double Current, int Charge)
        {
            this.IdTimeCode = IdTimeCode;
            this.IdAccu = IdAccu;
            this.NameAccu = NameAccu;
            this.Voltage = Voltage;
            this.Current = Current;
            this.Charge = Charge;
        }
    }
}
