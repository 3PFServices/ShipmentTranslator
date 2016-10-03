using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace ShipmentTranslator
{
	public class ProcessFile
	{
		private Utilities _utilities;
		private FileImportExportDefinition _ups = null;
		private FileImportExportDefinition _endicia = null;

		public ProcessFile(Utilities utilities)
		{
			_utilities = utilities;
			try
			{
				_ups = (FileImportExportDefinition) ConfigurationManager.GetSection("UpsShipments");
			}
			catch (Exception err)
			{
				_utilities.WriteLog("UPS Shipment configuration Issue. " + err.Message, LogLevel.Error,null,err);
			}
			try
			{
				_endicia = (FileImportExportDefinition) ConfigurationManager.GetSection("EndiciaShipments");
			}
			catch (Exception err)
			{

				_utilities.WriteLog("Endicia Shipment configuration issue." + err.Message, LogLevel.Error,null, err);
			}

		}

		public void ProcessShipments()
		{
			if (_ups == null && _endicia == null)
			{
				_utilities.WriteLog("No shipment files defined.", LogLevel.Info);
				return;
			}

			if (_ups != null)
			{
				ImportExportData(_ups);
			}

			if (_endicia != null)
			{
				ImportExportData(_endicia);
			}
		}


		private void ImportExportData(FileImportExportDefinition fileImportExport)
		{
			_utilities.WriteLog("Processing "+ fileImportExport.DisplayName + " file: " + fileImportExport.ImportFileName, LogLevel.Info);


			//find import file
			if (!File.Exists(fileImportExport.ImportFilePath + fileImportExport.ImportFileName))
			{
				_utilities.WriteLog("File: " + fileImportExport.ImportFileName + " doesn't exist.", LogLevel.Info);
				return;
			}

			if (!Directory.Exists(fileImportExport.ExportFilePath))
			{
				Directory.CreateDirectory(fileImportExport.ExportFilePath);
				_utilities.WriteLog("Creating Export Directory: " + fileImportExport.ExportFilePath, LogLevel.Info);
			}

			if (!string.IsNullOrWhiteSpace(fileImportExport.ArchiveFilePath) && !Directory.Exists(fileImportExport.ArchiveFilePath))
			{
				Directory.CreateDirectory(fileImportExport.ArchiveFilePath);
				_utilities.WriteLog("Creating Archive Directory: " + fileImportExport.ArchiveFilePath, LogLevel.Info);
			}

			#region Get Import Definition
			_utilities.WriteLog("Processing File Import definition", LogLevel.Info);
			var importdef = fileImportExport.ImportFileDefinition.FileDefinition.Split('|');

			Dictionary<string, int> importDefinition = new Dictionary<string, int>();
			foreach (var column in importdef.Where(t => !string.IsNullOrWhiteSpace(t)))
			{
				var columnDef = column.Split(',');
				importDefinition.Add(columnDef[1], Convert.ToInt32(columnDef[0]));
			}
			_utilities.WriteLog("Import definition has " + importDefinition.Count.ToString() + " number of columns defined.", LogLevel.Info);
			#endregion

			#region Get Export Definition
			_utilities.WriteLog("Processing File Import definition", LogLevel.Info);
			var exportdef = fileImportExport.ExportFileDefinition.FileDefinition.Split('|');

			Dictionary<int, string> exportDefinition = new Dictionary<int, string>();
			foreach (var column in exportdef.Where(t => !string.IsNullOrWhiteSpace(t)))
			{
				var columnDef = column.Split(',');
				exportDefinition.Add(Convert.ToInt32(columnDef[0]), columnDef[1]);
			}
			_utilities.WriteLog("Export definition has " + exportDefinition.Count.ToString() + " number of columns defined.", LogLevel.Info);
			#endregion

			bool foundOrder = false;

			string updatedExportFileName = fileImportExport.ExportFileName;

			if (updatedExportFileName.Contains("["))
			{

				string dateMask = updatedExportFileName.Substring(updatedExportFileName.IndexOf("["),
					(updatedExportFileName.IndexOf("]") )- updatedExportFileName.IndexOf("[")+1);
				updatedExportFileName = updatedExportFileName.Replace(dateMask, DateTime.Now.ToString(dateMask.Replace("[","").Replace("]","")));
				_utilities.WriteLog("Export filename has been updated to: "+ updatedExportFileName, LogLevel.Info);
			}

			string _enteredStartingOrderNumber = string.Empty;

			Console.WriteLine("==========================================");
			Console.WriteLine("Please enter the Starting Order number for the " + fileImportExport.DisplayName + " file.");
			Console.WriteLine("Note: If you want to process the entire file, don't enter any number.");
			Console.WriteLine("==========================================");
			_enteredStartingOrderNumber = Console.ReadLine();

			
			

			int lineCount = 1;
			//read file from order number on
			//discard any with blank order #s
			using (var import = File.OpenText(fileImportExport.ImportFilePath + fileImportExport.ImportFileName))
			using (var export = File.CreateText(fileImportExport.ExportFilePath + updatedExportFileName))
			{

				do
				{

					var line = import.ReadLine();
					_utilities.WriteLog("Processing line #" + lineCount, LogLevel.Info);
					try
					{
						var data = line.Split(',');

						if (string.IsNullOrWhiteSpace(data[0])
						    || (!foundOrder
						        && !string.IsNullOrWhiteSpace(_enteredStartingOrderNumber)
						        && data[0].Replace("\"","") != _enteredStartingOrderNumber))
						{
							lineCount++;
							continue;
						}

						if (fileImportExport.CheckForHeaderRecord)
						{
							if (data[0].Contains("Order"))
							{
								lineCount++;
								continue;
							}
						}
						if (!string.IsNullOrWhiteSpace(_enteredStartingOrderNumber) && data[0].Replace("\"","") == _enteredStartingOrderNumber)
						{
							_utilities.WriteLog("Found starting order number. Proceeding to export remaining lines in file.", LogLevel.Info);
							foundOrder = true;
						}

						//write export file
						//with markup

						StringBuilder exportLine = new StringBuilder();

						foreach (var key in exportDefinition.Keys.OrderBy(t => t))
						{
							

							if ((!importDefinition.ContainsKey(exportDefinition[key]) &&
								!exportDefinition[key].Contains("PlusMarkup"))
							    || data.Length < key)
							{
								exportLine.Append(",");
								continue;

							}
							decimal markup = Convert.ToDecimal(fileImportExport.FreightMarkupPercent);

							//exportLine.Append("\"");
							if (exportDefinition[key].Contains("PlusMarkup"))
							{
								if (markup >= 0)
								{
									var baseFreightColumn = importDefinition.Keys.First(t => t.Contains("BaseFreight"));
									decimal freight = Convert.ToDecimal(data[importDefinition[baseFreightColumn]].Replace("\"",""));

									freight += ((markup/100)*freight);

									exportLine.Append(freight.ToString("F2"));
								}
							}
							else
							{
								exportLine.Append(data[importDefinition[exportDefinition[key]]].Replace("\"", ""));
							}

							exportLine.Append(",");

						}

						//remove trailing comma
						exportLine.Remove(exportLine.Length - 1, 1);
						export.WriteLine(exportLine.ToString());
						export.Flush();
						lineCount++;
					}
					catch (Exception err)
					{
						_utilities.WriteLog(
							"Error during import file read. Reading line: \"" + line +
							"\". Skipping line please manually process this shipment.",
							LogLevel.Error, line, err);
						lineCount++;
						continue;

					}





				} while (!import.EndOfStream);

				_utilities.WriteLog("Completed processing import file.", LogLevel.Info);
				export.Close();

				import.Close();
			}

			//Don't archive file if not archive file path defined.
			//This is to allow the UPS export to append to the file instead of creating a new file.
			//As we are having issues with getting the export to properly create the export.
			if (!string.IsNullOrWhiteSpace(fileImportExport.ArchiveFilePath))
			{
				FileInfo f = new FileInfo(fileImportExport.ImportFilePath + fileImportExport.ImportFileName);

				string archiveFileName = f.Name.Replace(f.Extension, "") + DateTime.Now.ToString("_yyyyMMdd_HHmmss") + f.Extension;
				_utilities.WriteLog("Archiving Import file to " + fileImportExport.ArchiveFilePath + archiveFileName, LogLevel.Info);
				try
				{
					File.Move(fileImportExport.ImportFilePath + fileImportExport.ImportFileName,
						fileImportExport.ArchiveFilePath + archiveFileName);
				}
				catch (Exception err)
				{

					_utilities.WriteLog("Unable to move Import file to archive location.", LogLevel.Error, "", err);
				}

			}
			_utilities.WriteLog("Completed processing " + fileImportExport.DisplayName , LogLevel.Info);
		}
	}
}
