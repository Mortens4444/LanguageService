using System.Collections.Generic;
using System.Xml;

namespace LanguageService.Extensions
{
	public static class XmlDocumentExtensions
	{
		public static XmlNamespaceManager InitializeXmlNamespaceManager(this XmlDocument xmlDocument, List<(string prefix, string uri)> namespaces)
		{
			var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			foreach (var (prefix, uri) in namespaces)
			{
				xmlNamespaceManager.AddNamespace(prefix, uri);
			}
			return xmlNamespaceManager;
		}
	}
}
