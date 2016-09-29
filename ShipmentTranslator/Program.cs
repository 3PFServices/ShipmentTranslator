using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
	        var tmp = ConfigurationManager.GetSection("UpsShipments");

			Console.WriteLine("Press any Key to continue...");
	        Console.ReadKey();
			return;
        }
    }
}
