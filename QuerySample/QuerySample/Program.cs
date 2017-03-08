using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QuerySample
{
    class Program
    {
        static void Main(string[] args)
        {
            var reservations = new List<Reservation>();
            for (int i = 0; i < 20; i++)
            {
                reservations.Add(
                    new Reservation
                    {
                        StartDateTime = DateTime.Today.AddHours(i),
                        EndDateTime = DateTime.Today.AddHours(i + 1),
                    });
            }

            DateTime? startDateTime = null;
            //DateTime? startDateTime = DateTime.Parse("2017/03/08 2:00:00");
            //DateTime? endDateTime = null;
            DateTime? endDateTime = DateTime.Parse("2017/03/08 6:00:00");



            var results = Search(reservations, startDateTime, endDateTime);
            foreach (var reservation in results)
            {
                Console.WriteLine($"StartDateTime:{reservation.StartDateTime} EndDateTime:{reservation.EndDateTime}");
            }
            Console.ReadLine();
        }

        private static IList<Reservation> Search(IList<Reservation> reservations, DateTime? startDateTime,
            DateTime? endDateTime)
        {
            return 
                (from x in reservations
                 where (startDateTime == null || startDateTime.Value <= x.StartDateTime)
                    && (endDateTime == null || x.EndDateTime <= endDateTime.Value)
                 select x).ToList();
        }
    }

    public class Reservation

    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
