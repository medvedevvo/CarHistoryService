using System;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarHistoryService.Services;
using CarHistoryService.Models;
using Microsoft.Extensions.Logging;

namespace CarHistoryService.Controllers
{
    [Produces("application/json")]
    [Route("api/history")]
    public class HistoryController : Controller
    {
        private ConnectionSettings connectionSettings = ConnectionSettings.getInstance();
        private CarCheckingService carCheckingService;
        private readonly StateHistoryDbContext _context;
        private ILogger<HistoryController> logger;

        public HistoryController(StateHistoryDbContext context, CarCheckingService carCheckingService, ILogger<HistoryController> logger)
        {
            this.carCheckingService = carCheckingService;
            logger.LogDebug($"Created SaveController");
            _context = context;
            this.logger = logger;
        }

        [HttpGet("cars/{IdCar}/accus/time/{TFrom}/{TTo}")]
        public async Task<IActionResult> GetCarAccus([FromRoute] int IdCar, [FromRoute] int TFrom, [FromRoute] int TTo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //UpdateStates(IdCar);

            List<AccuHistoryResponce> accuHistory;

            using (SqlConnection connection = new SqlConnection(connectionSettings.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT tc.Time, a.IdAccu, a.NameAccu, a.Voltage, a.[Current], a.Charge ");
                sb.Append("FROM AccuStates as a, TimeCodes as tc ");
                sb.Append("WHERE a.IdTimeCode = tc.Id AND ");
                sb.Append("tc.IdCar = " + IdCar.ToString() + " AND ");
                sb.Append("tc.Time >= " + TFrom.ToString() + " AND ");
                sb.Append("tc.Time <= " + TTo.ToString() + " ");
                sb.Append("ORDER BY tc.Time ");
                String sql = sb.ToString();

                accuHistory = GetAccuHistoryResponce(sql, connection);
            }

            if (accuHistory == null)
            {
                return NotFound();
            }
            if (accuHistory.Count == 0)
            {
                return NotFound();
            }

            return Ok(accuHistory);
        }

        [HttpGet("cars/{IdCar}/accus/{IdAccu}/time/{TFrom}/{TTo}")]
        public async Task<IActionResult> GetCarAccu([FromRoute] int IdCar, [FromRoute] int IdAccu, [FromRoute] int TFrom, [FromRoute] int TTo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //UpdateStates(IdCar);

            List<AccuHistoryResponce> accuHistory;

            using (SqlConnection connection = new SqlConnection(connectionSettings.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT tc.Time, a.IdAccu, a.NameAccu, a.Voltage, a.[Current], a.Charge ");
                sb.Append("FROM AccuStates as a, TimeCodes as tc ");
                sb.Append("WHERE a.IdTimeCode = tc.Id AND ");
                sb.Append("tc.IdCar = " + IdCar.ToString() + " AND ");
                sb.Append("a.IdAccu = " + IdAccu.ToString() + " AND ");
                sb.Append("tc.Time >= " + TFrom.ToString() + " AND ");
                sb.Append("tc.Time <= " + TTo.ToString() + " ");
                sb.Append("ORDER BY tc.Time ");
                String sql = sb.ToString();

                accuHistory = GetAccuHistoryResponce(sql, connection);
            }

            if (accuHistory == null)
            {
                return NotFound();
            }
            if (accuHistory.Count == 0)
            {
                return NotFound();
            }

            return Ok(accuHistory);
        }

        private async Task<IActionResult> UpdateStates(int IdCar)
        {
            AccuListWithTime accu = await carCheckingService.GetAccuState(IdCar);
            if (accu == null)
            {
                return NotFound();
            }
            logger.LogDebug($"Start saving time code");
            TimeCodeHistory timeCodeHistory = new TimeCodeHistory(accu.idCar, accu.time);
            _context.TimeCodes.Add(timeCodeHistory);
            await _context.SaveChangesAsync();
            int idTimeCode = timeCodeHistory.Id;
            logger.LogDebug($"Time code saved. Start saving accu history");

            foreach (Accu a in accu.accu)
            {
                _context.AccuStates.Add(new AccuHistory(idTimeCode, a.id, a.name, a.voltage, a.current, a.charge));
            }
            await _context.SaveChangesAsync();
            logger.LogDebug($"Accu history saved");

            return Ok();
        }

        private List<AccuHistoryResponce> GetAccuHistoryResponce(string sql, SqlConnection connection)
        {
            List<AccuHistoryResponce> accuHistory = new List<AccuHistoryResponce>();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AccuHistoryResponce accuHistoryResponce = new AccuHistoryResponce();
                        try
                        {
                            accuHistoryResponce.time = reader.GetInt32(0);
                            accuHistoryResponce.idAccu = reader.GetInt32(1);
                            accuHistoryResponce.nameAccu = reader.GetString(2);
                            accuHistoryResponce.voltage = reader.GetDouble(3);
                            accuHistoryResponce.current = reader.GetDouble(4);
                            accuHistoryResponce.charge = reader.GetInt32(5);

                            accuHistory.Add(accuHistoryResponce);
                        }
                        catch (Exception e)
                        {
                            int a = 0;
                            return null;
                        }
                    }
                }
            }
            return accuHistory;
        }
    }
}