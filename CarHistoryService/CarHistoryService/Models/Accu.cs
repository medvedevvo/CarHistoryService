using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHistoryService.Models
{
    public class AccuListWithTime
    {
        public int idCar = 1;
        public int time = 0;
        public List<Accu> accu;

        public AccuListWithTime()
        {

        }

        public AccuListWithTime(int time, List<Accu> accu)
        {
            this.time = time;
            this.accu = accu;
        }

        public AccuListWithTime(int time, Accu accu)
        {
            this.time = time;
            if(accu != null)
            {
                this.accu = new List<Accu>();
                this.accu.Add(accu);
            }
                
        }
    }


    public class Accu
    {
        public int id = 0;
        public string name = "";
        public double voltage = 0.0;
        public double current = 0.0;
        public int charge = 0;

        public Accu()
        {

        }
        public Accu(int id, string name, double voltage, double current, int charge)
        {
            this.name = name;
            this.id = id;
            this.voltage = voltage;
            this.current = current;
            this.charge = charge;
        }
    }
}
