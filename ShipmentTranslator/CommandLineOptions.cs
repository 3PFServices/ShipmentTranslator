using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ShipmentTranslator
{
	public class CommandLineOptions
	{
		[Option('s',"StartOrderNumber",DefaultValue = "", Required = false, 
			HelpText = "Starting Order Number in the shipment export file. Will discard all records before this number.")]
		public string StartOrderNumber { get; set; }


		[Option("UPSInputFilePath", Required = false,
			HelpText = "UPS Input File Path overriding the default in the configuration.")]
		public string UPSInputFilePath { get; set; }

		[Option( "UPSInputFilePath", Required = false,
			HelpText = "UPS Export File Path overriding the default in the configuration.")]
		public string UPSExportFilePath { get; set; }

		
		[Option('l',"LogLevel",DefaultValue = "Error", Required = false,
		  HelpText = "Prints informational messages to the screen based on given Log Level.")]
		public string LogLevel { get; set; }

		[Option("EndiciaInputFilePath", Required = false,
			HelpText = "Endicia Input File Path overriding the default in the configuration.")]
		public string EndiciaInputFilePath { get; set; }

		[Option("EndiciaInputFilePath", Required = false,
			HelpText = "Endicia Export File Path overriding the default in the configuration.")]
		public string EndiciaExportFilePath { get; set; }
	}

}
