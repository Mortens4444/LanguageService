using LanguageService.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace LanguageService.Ods
{
    public class OdsReader
	{
		private static readonly List<(string Prefix, string Uri)> Namespaces = new List<(string, string)>
		{
			("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0"),
			("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0"),
			("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0"),
			("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0"),
			("draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0"),
			("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0"),
			("dc", "http://purl.org/dc/elements/1.1/"),
			("meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0"),
			("number", "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0"),
			("presentation", "urn:oasis:names:tc:opendocument:xmlns:presentation:1.0"),
			("svg", "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0"),
			("chart", "urn:oasis:names:tc:opendocument:xmlns:chart:1.0"),
			("dr3d", "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0"),
			("math", "http://www.w3.org/1998/Math/MathML"),
			("form", "urn:oasis:names:tc:opendocument:xmlns:form:1.0"),
			("script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0"),
			("ooo", "http://openoffice.org/2004/office"),
			("ooow", "http://openoffice.org/2004/writer"),
			("oooc", "http://openoffice.org/2004/calc"),
			("dom", "http://www.w3.org/2001/xml-events"),
			("xforms", "http://www.w3.org/2002/xforms"),
			("xsd", "http://www.w3.org/2001/XMLSchema"),
			("xsi", "http://www.w3.org/2001/XMLSchema-instance"),
			("rpt", "http://openoffice.org/2005/report"),
			("of", "urn:oasis:names:tc:opendocument:xmlns:of:1.2"),
			("rdfa", "http://docs.oasis-open.org/opendocument/meta/rdfa#"),
			("config", "urn:oasis:names:tc:opendocument:xmlns:config:1.0")
		};

		public DataSet ReadFile(string filePath)
		{
            using (var zipArchive = ZipFile.OpenRead(filePath))
            {
                var contentXml = zipArchive.GetXmlDocument("content.xml");

                var xmlNamespaceManager = contentXml.InitializeXmlNamespaceManager(Namespaces);
                var odsFile = new DataSet(Path.GetFileName(filePath));
                var xmlNodeList = contentXml.SelectNodes("/office:document-content/office:body/office:spreadsheet/table:table", xmlNamespaceManager);
                foreach (XmlNode tableNode in xmlNodeList)
                {
                    odsFile.Tables.Add(GetSheet(tableNode, xmlNamespaceManager));
                }

                return odsFile;
            }
		}

		private DataTable GetSheet(XmlNode tableNode, XmlNamespaceManager xmlNamespaceManager)
		{
			var sheet = new DataTable(tableNode.Attributes["table:name"].Value);
			XmlNodeList rowNodes = tableNode.SelectNodes("table:table-row", xmlNamespaceManager);

			int rowIndex = 0;
			foreach (XmlNode rowNode in rowNodes)
			{
				rowIndex = GetRow(rowNode, sheet, xmlNamespaceManager, rowIndex);
			}

			return sheet;
		}

		private int GetRow(XmlNode rowNode, DataTable sheet, XmlNamespaceManager xmlNamespaceManager, int rowIndex)
		{
			XmlAttribute rowsRepeated = rowNode.Attributes["table:number-rows-repeated"];
			if (rowsRepeated == null || Convert.ToInt32(rowsRepeated.Value, CultureInfo.InvariantCulture) == 1)
			{
				while (sheet.Rows.Count < rowIndex)
				{
					sheet.Rows.Add(sheet.NewRow());
				}

				var row = sheet.NewRow();
				var cellNodes = rowNode.SelectNodes("table:table-cell", xmlNamespaceManager);

				int cellIndex = 0;
				foreach (XmlNode cellNode in cellNodes)
				{
					cellIndex = GetCell(cellNode, row, cellIndex);
				}

				sheet.Rows.Add(row);

				return rowIndex + 1;
			}
			if (sheet.Rows.Count == 0)
			{
				sheet.Rows.Add(sheet.NewRow());
				sheet.Columns.Add();
			}
			return rowIndex + Convert.ToInt32(rowsRepeated.Value, CultureInfo.InvariantCulture);
		}

		private int GetCell(XmlNode cellNode, DataRow row, int cellIndex)
		{
			var cellRepeated = cellNode.Attributes["table:number-columns-repeated"];
			if (cellRepeated == null)
			{
				var sheet = row.Table;
				while (sheet.Columns.Count <= cellIndex)
				{
					sheet.Columns.Add();
				}
				row[cellIndex] = ReadCellValue(cellNode);
				return cellIndex + 1;
			}
			return cellIndex + Convert.ToInt32(cellRepeated.Value, CultureInfo.InvariantCulture);
		}

		private string ReadCellValue(XmlNode cell)
		{
			var cellValue = cell.Attributes["office:value"];
			return cellValue?.Value ?? cell.InnerText;
		}
	}
}
