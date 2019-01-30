/**********************************************************************
* Copyright (c) 1999-2014 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System;
using System.Xml;
using Recurly;

namespace Infratel.RecurlyLibrary
{
    public class WebHookInvoice: RecurlyWebHookEntity
    {
        public string Uuid { get; protected set; }
        public string SubscriptionId { get; private set; }
        public Invoice.InvoiceState State { get; private set; }
        public int InvoiceNumber { get; private set; }
        public string PoNumber { get; private set; }
        public string VatNumber { get; private set; }
        public int TotalInCents { get; protected set; }
        public string Currency { get; protected set; }
        public DateTime? Date { get; private set; }
        public DateTime? ClosedAt { get; private set; }
        public int? NetTerms { get; private set; }
        public string CollectionMethod { get; private set; }

        internal WebHookInvoice(XmlTextReader xmlReader)
        {
            ReadXml(xmlReader);
        }

        internal override void ReadXml(XmlTextReader reader)        
        {
            while (reader.Read())
            {
                // End of invoice element, get out of here
                if (reader.Name == "invoice" && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Element) continue;

                switch (reader.Name)
                {
                    case "uuid":
                        Uuid = reader.ReadElementContentAsString();
                        break;

                    case "subscription_id":
                        SubscriptionId = reader.ReadElementContentAsString();
                        break;

                    case "state":
                        State = reader.ReadElementContentAsString().ParseAsEnum<Invoice.InvoiceState>();
                        break;

                    case "invoice_number":
                        int invNumber;
                        if (Int32.TryParse(reader.ReadElementContentAsString(), out invNumber))
                            InvoiceNumber = invNumber;
                        break;

                    case "po_number":
                        PoNumber = reader.ReadElementContentAsString();
                        break;

                    case "vat_number":
                        VatNumber = reader.ReadElementContentAsString();
                        break;                    

                    case "total_in_cents":
                        TotalInCents = reader.ReadElementContentAsInt();
                        break;

                    case "currency":
                        Currency = reader.ReadElementContentAsString();
                        break;

                    case "date":
                        DateTime createdAt;
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out createdAt))
                            Date = createdAt;
                        break;

                    case "closed_at":
                        DateTime closedAt;
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out closedAt))
                            ClosedAt = closedAt;
                        break;

                    case "net_terms":
                        NetTerms = reader.ReadElementContentAsInt();
                        break;

                    case "collection_method":
                        CollectionMethod = reader.ReadElementContentAsString();
                        break;
                }
            }
        }

        public override string ToString()
        {
            return "Recurly WebHookInvoice: " + Uuid;
        }
    }
}
