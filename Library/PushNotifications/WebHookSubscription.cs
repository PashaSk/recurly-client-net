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
    public class WebHookSubscription : RecurlyWebHookEntity
    {

        public class WebHookPlan: RecurlyWebHookEntity
        {
            public string PlanCode { get; private set; }
            public string Name { get; private set; }
            public int? Version { get; private set; }

            internal override void ReadXml(XmlTextReader reader)
            {
                while (reader.Read())
                {
                    if (reader.Name == "plan" && reader.NodeType == XmlNodeType.EndElement)
                        break;

                    if (reader.NodeType != XmlNodeType.Element) continue;

                    switch (reader.Name)
                    {
                        case "plan_code":
                            PlanCode = reader.ReadElementContentAsString();
                            break;
                        case "name":
                            Name = reader.ReadElementContentAsString();
                            break;
                        case "version":
                            Version = reader.ReadElementContentAsInt();
                            break;
                    }
                }
            }

            public override string ToString()
            {
                return "Recurly WebHookPlan: " + PlanCode;
            }

        }
     
        public WebHookPlan Plan { get; private set; }
        public string Uuid { get; private set; }
        public Subscription.SubscriptionState State { get; private set; }
        public int Quantity { get; private set; }
        public int TotalAmountInCents { get; private set; }
        public DateTime? ActivatedAt { get; private set; }
        public DateTime? CanceledAt { get; private set; }
        public DateTime? ExpiresAt { get; private set; }
        public DateTime? CurrentPeriodStartedAt { get; private set; }
        public DateTime? CurrentPeriodEndsAt { get; private set; }
        public DateTime? TrialPeriodStartedAt { get; private set; }
        public DateTime? TrialPeriodEndsAt { get; private set; }
        public string CollectionMethod { get; set; }

        internal WebHookSubscription(XmlTextReader xmlReader)
        {
            ReadXml(xmlReader);
        }

        internal override void ReadXml(XmlTextReader reader)
        {
            while (reader.Read())
            {
                if (reader.Name == "subscription" && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType != XmlNodeType.Element) continue;

                DateTime dateVal;                

                switch (reader.Name)
                {
                    case "plan":
                        Plan = new WebHookPlan();
                        Plan.ReadXml(reader);
                        break;

                    case "uuid":
                        Uuid = reader.ReadElementContentAsString();
                        break;

                    case "state":
                        State = reader.ReadElementContentAsString().ParseAsEnum<Subscription.SubscriptionState>();
                        break;

                    case "quantity":
                        Quantity = reader.ReadElementContentAsInt();
                        break;

                    case "total_amount_in_cents":
                        TotalAmountInCents = reader.ReadElementContentAsInt();
                        break;

                    case "activated_at":
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out dateVal))
                            ActivatedAt = dateVal;
                        break;

                    case "canceled_at":
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out dateVal))
                            CanceledAt = dateVal;
                        break;

                    case "expires_at":
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out dateVal))
                            ExpiresAt = dateVal;
                        break;

                    case "current_period_started_at":
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out dateVal))
                            CurrentPeriodStartedAt = dateVal;
                        break;

                    case "current_period_ends_at":
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out dateVal))
                            CurrentPeriodEndsAt = dateVal;
                        break;

                    case "trial_started_at":
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out dateVal))
                            TrialPeriodStartedAt = dateVal;
                        break;

                    case "trial_ends_at":
                        if (DateTime.TryParse(reader.ReadElementContentAsString(), out dateVal))
                            TrialPeriodEndsAt = dateVal;
                        break;

                    case "collection_method":
                        CollectionMethod = reader.ReadElementContentAsString();
                        break;
                }
            }
        }

        public override string ToString()
        {
            return "Recurly WebHookSubscription: " + Uuid;
        }
    }    
}
