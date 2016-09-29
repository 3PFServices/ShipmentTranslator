using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentTranslator
{
    public class Utilities
    {


    }
	#region Config file section definitions
	public class FileImportExportDefinition : ConfigurationSection
	{

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
