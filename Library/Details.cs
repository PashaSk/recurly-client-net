using System;
using System.Xml;

namespace Recurly
{
    /// <summary>
    /// This class should not be parsed. It is used to avoid XML issues
    /// </summary>
    class Details : RecurlyEntity
    {
        internal override void ReadXml(XmlTextReader reader)
        {
            while (reader.Read())
            {
                if (reader.Name == "details" && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Element) continue;

                switch (reader.Name)
                {
                    case "account":
                        break;
                }
            }
        }

        internal override void WriteXml(XmlTextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
