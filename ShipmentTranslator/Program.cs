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
			 var cmdArgs = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args);
	        

	        if (cmdArgs.Errors.Any())
	        {
		        Console.WriteLine("There were errors in the command line arguements. Please correct these errors and rerun.");
		        foreach (var cmdArgsError in cmdArgs.Errors)
		        {
			        Console.WriteLine(cmdArgsError.ToString());
		        }
				Console.WriteLine("Press any Key to continue...");
				Console.ReadKey();
				return;
			}

	        var process = new ProcessFile(new Utilities(cmdArgs));
			process.ProcessShipments();

			 
			Console.WriteLine("Press any Key to continue...");
	        Console.ReadKey();
			return;
        }

    }
}
