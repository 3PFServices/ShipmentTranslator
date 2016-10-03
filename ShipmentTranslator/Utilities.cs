using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentTranslator
{
	public enum LogLevel
	{
		Error = 0,
		Warn = 1,
		Info = 2
	}


    public class Utilities
    {
		private LogLevel _logLevel = LogLevel.Error;
	    public CommandLine.ParserResult<CommandLineOptions> _args;
		

		public void WriteLog(string message, LogLevel logLevel, string lineData = "", Exception exception = null)
	    {
		    if ((int) logLevel <= (int) _logLevel)
		    {
			    Console.WriteLine(message );

		    }



			if (logLevel == LogLevel.Error)
		    {
				var logfilepath = ConfigurationManager.AppSettings["ErrorLogFilePath"] + "ShipmentTranslator_" +
		                      DateTime.Now.ToString("yyyMMdd") + ".txt";

			    if (!File.Exists(logfilepath))
			    {
				    File.CreateText(logfilepath).Close();
					
			    }
			    using (var log = File.AppendText(logfilepath))
			    {
				    log.Write(DateTime.Now.ToString("s") + "|" + message + "|" + (exception?.ToString() ?? "") +"|Data:" + lineData );
					log.Flush();
					log.Close();
				}

		    }
	    }

	    public Utilities(CommandLine.ParserResult<CommandLineOptions> args)
	    {
		    _args = args;


		    switch (args.Value.LogLevel)
		    {
				case "Warn":
					_logLevel = LogLevel.Warn;
				    break;
				case "Info":
					_logLevel = LogLevel.Info;
				    break;
				default:
					_logLevel = LogLevel.Error;
				    break;
		    }
	    }
    }

	#region Config file section definitions
	public class FileImportExportDefinition : ConfigurationSection
	{
		[ConfigurationProperty("DisplayName", IsRequired = true)]
		public string DisplayName
		{
			get { return this["DisplayName"].ToString(); }
			set { this["DisplayName"] = value; }
		}
		[ConfigurationProperty("ImportFileName",  IsRequired = true)]
		public string ImportFileName
		{
			get
			{
				return this["ImportFileName"].ToString();
			}
			set
			{
				this["ImportFileName"] = value;
			}
		}
		[ConfigurationProperty("ExportFileName", IsRequired = true)]
		public string ExportFileName
		{
			get
			{
				return this["ExportFileName"].ToString();
			}
			set
			{
				this["ExportFileName"] = value;
			}
		}
		[ConfigurationProperty("ImportFilePath", IsRequired = true)]
		public string ImportFilePath
		{
			get
			{
				return this["ImportFilePath"].ToString();
			}
			set
			{
				this["ImportFilePath"] = value;
			}
		}
		[ConfigurationProperty("ExportFilePath", IsRequired = true)]
		public string ExportFilePath
		{
			get
			{
				return this["ExportFilePath"].ToString();
			}
			set
			{
				this["ExportFilePath"] = value;
			}
		}
		[ConfigurationProperty("ArchiveFilePath", IsRequired = false,DefaultValue = "")]
		public string ArchiveFilePath
		{
			get
			{
				return this["ArchiveFilePath"].ToString();
			}
			set
			{
				this["ArchiveFilePath"] = value;
			}
		}
		[ConfigurationProperty("CheckForHeaderRecord", IsRequired = false, DefaultValue = false)]
		public bool CheckForHeaderRecord
		{
			get { return (bool)this["CheckForHeaderRecord"]; }
			set { this["CheckForHeaderRecord"] = value; }
		}


		[ConfigurationProperty("FreightMarkupPercent", IsRequired = false, DefaultValue = 0)]
		public int FreightMarkupPercent
		{
			get
			{
				return (int) this["FreightMarkupPercent"];
				
			}
			set { this["FreightMarkupPercent"] = value; }
		}



		[ConfigurationProperty("ImportFileDefinition")]
		public FileDefinitionElement ImportFileDefinition
		{
			get
			{
				return (FileDefinitionElement)this["ImportFileDefinition"];
			}
			set
			{ this["ImportFileDefinition"] = value; }
		}

		[ConfigurationProperty("ExportFileDefinition")]
		public FileDefinitionElement ExportFileDefinition
		{
			get
			{
				return (FileDefinitionElement)this["ExportFileDefinition"];
			}
			set
			{ this["ExportFileDefinition"] = value; }
		}
	}



	public class FileDefinitionElement : ConfigurationElement
	{
		[ConfigurationProperty("FileDefinition")]
		public string FileDefinition
		{
			get { return (string) this["FileDefinition"]; }
			set { this["FileDefinition"] = value; }
		}
	}
	#endregion

}
