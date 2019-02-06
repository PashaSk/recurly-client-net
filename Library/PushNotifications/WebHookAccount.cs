/**********************************************************************
* Copyright (c) 1999-2014 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System.Xml;

namespace Infratel.RecurlyLibrary
{

    public class WebHookAccount : RecurlyWebHookEntity
    {
        public string AccountCode { get; private set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        internal WebHookAccount(XmlTextReader xmlReader)
        {
            ReadXml(xmlReader);
        }

        internal override void ReadXml(XmlTextReader reader)
        {
            while (reader.Read())
            {
                if (reader.Name == "account" && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Element) continue;

                switch (reader.Name)
                {
                    case "account_code":
                        AccountCode = reader.ReadElementContentAsString();
                        break;

                    case "username":
                        Username = reader.ReadElementContentAsString();
                        break;

                    case "email":
                        Email = reader.ReadElementContentAsString();
                        break;

                    case "first_name":
                        FirstName = reader.ReadElementContentAsString();
                        break;

                    case "last_name":
                        LastName = reader.ReadElementContentAsString();
                        break;

                    case "company_name":
                        CompanyName = reader.ReadElementContentAsString();
                        break;
                }
            }
        }

        public override string ToString()
        {
            return "Recurly WebHookAccount: " + AccountCode;
        }

    }
}
