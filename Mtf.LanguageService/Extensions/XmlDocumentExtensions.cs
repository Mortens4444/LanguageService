using Mtf.LanguageService.Models;
using System.Collections.Generic;
using System.Xml;

namespace Mtf.LanguageService.Extensions
{
    public static class XmlDocumentExtensions
    {
        public static XmlNamespaceManager InitializeXmlNamespaceManager(this XmlDocument xmlDocument, List<XmlNamespace> namespaces)
        {
            var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            foreach (var xmlNamespace in namespaces)
            {
                xmlNamespaceManager.AddNamespace(xmlNamespace.Prefix, xmlNamespace.Uri);
            }
            return xmlNamespaceManager;
        }
    }
}
