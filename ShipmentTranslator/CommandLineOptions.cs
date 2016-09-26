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
		[Option("UPSInputFilePath", Required = false,
			HelpText = "UPS Input File Path overriding the default in the configuration.")]
		public string UPSInputFilePath { get; set; }

		[Option( "UPSInputFilePath", Required = false,
			HelpText = "UPS Export File Path overriding the default in the configuration.")]
		public string UPSExportFilePath { get; set; }

		// omitting long name, default --verbose
		[Option(DefaultValue = false, Required = false,
		  HelpText = "Prints all informational messages to the screen.")]
		public bool Verbose { get; set; }

		[Option("EndiciaInputFilePath", Required = false,
			HelpText = "Endicia Input File Path overriding the default in the configuration.")]
		public string EndiciaInputFilePath { get; set; }

		[Option("EndiciaInputFilePath", Required = false,
			HelpText = "Endicia Export File Path overriding the default in the configuration.")]
		public string EndiciaExportFilePath { get; set; }
	}

}
