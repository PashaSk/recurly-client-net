/**********************************************************************
* Copyright (c) 1999-2014 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Infratel.Utils.HttpRetry;
using Infratel.Utils.Text;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Recurly;

namespace Infratel.RecurlyLibrary
{
    public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }
    }

    public class RetryStrategy : HttpRetryStrategy
    {
        public RetryStrategy()
        {
            Name = GetType().Name;
        }

        public override bool IsTransient(Exception ex)
        {
            if (ex is Recurly.NotFoundException)
            {
                return false;
            }
            if (ex is Recurly.TemporarilyUnavailableException)
            {
                return true;
            }
            return base.IsTransient(ex);
        }
    }    

    public static class MightyCallExtensions
    {
        public static RetryPolicy<RetryStrategy> RecurlyWithRetries()
        {
            var retryPolicy = new RetryPolicy<RetryStrategy>(5, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200));
            retryPolicy.Retrying += (sender, args) =>
            {
                Trace.TraceError("{0}: RecurlyLibrary.RetryPolicy: Retry {1} due to exception: {2}", DateTime.UtcNow.ToLog(), args.CurrentRetryCount, args.LastException);
            };
            return retryPolicy;
        }

        public static Subscription GetFirstSubscription(this Account account, Subscription.SubscriptionState state = Subscription.SubscriptionState.All)
        {
            var subscriptions = account.GetSubscriptions(state);
            return subscriptions.FirstOrDefault();
        }

        public static List<Subscription> GetAllSubscriptions(this Account account, Subscription.SubscriptionState state = Subscription.SubscriptionState.All)
        {
            List<Subscription> result = new List<Subscription>();
            var subscriptions = account.GetSubscriptions(state);
            while (subscriptions.Any())
            {
                result.AddRange(subscriptions);
                subscriptions = subscriptions.Next;
            }
            return result;
        }

        public static List<Adjustment> GetAllAdjustments(this Account account, Adjustment.AdjustmentType type, Adjustment.AdjustmentState state)
        {
            List<Adjustment> result = new List<Adjustment>();
            var adjustments = account.GetAdjustments(type, state);
            while (adjustments.Any())
            {
                result.AddRange(adjustments);
                adjustments = adjustments.Next;
            }
            return result;
        }

        public static List<Invoice> GetAllInvoices(this Account account)
        {
            List<Invoice> result = new List<Invoice>();
            var invoices = account.GetInvoices();
            while (invoices.Any())
            {
                result.AddRange(invoices);
                invoices = invoices.Next;
            }
            return result;
        }

        public static int GetBalanceAmountInCents(this Account account, string currencyId)
        {            
            var adjs = account.GetAllAdjustments(Adjustment.AdjustmentType.All, Adjustment.AdjustmentState.Pending).Where(a => a.Currency == currencyId);
            var invoices = account.GetAllInvoices().Where(i => i.Currency == currencyId && i.State == Invoice.InvoiceState.PastDue).ToList();

            int sumAdjs = adjs.Sum(a => a.TotalInCents);
            int sumInvs = invoices.Sum(i => i.TotalInCents);
            int balanceInCents = sumAdjs - sumInvs;

            Debug.WriteLine(string.Format("{0}: GetBalanceAmountInCents({1}, {2}): sumAdj={3}, sumInvs={4}, balance={5}", DateTime.UtcNow.ToLog(), account.AccountCode, currencyId, sumAdjs, sumInvs, balanceInCents));

            return balanceInCents;
        }

        public static int GetAllAdjustmentsInCentsByDescription(this Account account, string currencyId, string description)
        {
            var adjs = account.GetAllAdjustments(Adjustment.AdjustmentType.All, Adjustment.AdjustmentState.Pending).Where(a => a.Currency == currencyId);
            int sumAdjs = adjs.Where(a => a.Description == description).Sum(a => a.TotalInCents);

            Debug.WriteLine(string.Format("{0}: GetAllAdjustmentsInCentsByDescription({1}, {2}): sumAdj={3}", DateTime.UtcNow.ToLog(), account.AccountCode, currencyId, sumAdjs));

            return sumAdjs;
        }

        public static Subscription GetFirstSubscription(string accountCode, Subscription.SubscriptionState state = Subscription.SubscriptionState.All)
        {
            var account = new Account(accountCode);
            return account.GetFirstSubscription(state);
        }

        public static int GetBalanceAmountInCents(string accountCode, string currencyId)
        {
            var account = new Account(accountCode);
            return account.GetBalanceAmountInCents(currencyId);
        }

        public static KeyValueList<string, string> GetBillingInfoCountries()
        {
            return new KeyValueList<string, string>
                {
                    { "AF", "Afghanistan" },
                    { "AL", "Albania" },
                    { "DZ", "Algeria" },
                    { "AS", "American Samoa" },
                    { "AD", "Andorra" },
                    { "AO", "Angola" },
                    { "AI", "Anguilla" },
                    { "AQ", "Antarctica" },
                    { "AG", "Antigua and Barbuda" },
                    { "AR", "Argentina" },
                    { "AM", "Armenia" },
                    { "AW", "Aruba" },
                    { "AC", "Ascension Island" },
                    { "AU", "Australia" },
                    { "AT", "Austria" },
                    { "AZ", "Azerbaijan" },
                    { "BS", "Bahamas" },
                    { "BH", "Bahrain" },
                    { "BD", "Bangladesh" },
                    { "BB", "Barbados" },
                    { "BY", "Belarus" },
                    { "BE", "Belgium" },
                    { "BZ", "Belize" },
                    { "BJ", "Benin" },
                    { "BM", "Bermuda" },
                    { "BT", "Bhutan" },
                    { "BO", "Bolivia" },
                    { "BA", "Bosnia and Herzegovina" },
                    { "BW", "Botswana" },
                    { "BV", "Bouvet Island" },
                    { "BR", "Brazil" },
                    { "BQ", "British Antarctic Territory" },
                    { "IO", "British Indian Ocean Territory" },
                    { "VG", "British Virgin Islands" },
                    { "BN", "Brunei" },
                    { "BG", "Bulgaria" },
                    { "BF", "Burkina Faso" },
                    { "BI", "Burundi" },
                    { "KH", "Cambodia" },
                    { "CM", "Cameroon" },
                    { "CA", "Canada" },
                    { "IC", "Canary Islands" },
                    { "CT", "Canton and Enderbury Islands" },
                    { "CV", "Cape Verde" },
                    { "KY", "Cayman Islands" },
                    { "CF", "Central African Republic" },
                    { "EA", "Ceuta and Melilla" },
                    { "TD", "Chad" },
                    { "CL", "Chile" },
                    { "CN", "China" },
                    { "CX", "Christmas Island" },
                    { "CP", "Clipperton Island" },
                    { "CC", "Cocos [Keeling] Islands" },
                    { "CO", "Colombia" },
                    { "KM", "Comoros" },
                    { "CD", "Congo [DRC]" },
                    { "CK", "Cook Islands" },
                    { "CR", "Costa Rica" },
                    { "HR", "Croatia" },
                    { "CU", "Cuba" },
                    { "CY", "Cyprus" },
                    { "CZ", "Czech Republic" },
                    { "DK", "Denmark" },
                    { "DG", "Diego Garcia" },
                    { "DJ", "Djibouti" },
                    { "DM", "Dominica" },
                    { "DO", "Dominican Republic" },
                    { "NQ", "Dronning Maud Land" },
                    { "DD", "East Germany" },
                    { "TL", "East Timor" },
                    { "EC", "Ecuador" },
                    { "EG", "Egypt" },
                    { "SV", "El Salvador" },
                    { "EE", "Estonia" },
                    { "ET", "Ethiopia" },
                    { "EU", "European Union" },
                    { "FK", "Falkland Islands [Islas Malvinas]" },
                    { "FO", "Faroe Islands" },
                    { "FJ", "Fiji" },
                    { "FI", "Finland" },
                    { "FR", "France" },
                    { "GF", "French Guiana" },
                    { "PF", "French Polynesia" },
                    { "TF", "French Southern Territories" },
                    { "FQ", "French Southern and Antarctic Territories" },
                    { "GA", "Gabon" },
                    { "GM", "Gambia" },
                    { "GE", "Georgia" },
                    { "DE", "Germany" },
                    { "GH", "Ghana" },
                    { "GI", "Gibraltar" },
                    { "GR", "Greece" },
                    { "GL", "Greenland" },
                    { "GD", "Grenada" },
                    { "GP", "Guadeloupe" },
                    { "GU", "Guam" },
                    { "GT", "Guatemala" },
                    { "GG", "Guernsey" },
                    { "GW", "Guinea-Bissau" },
                    { "GY", "Guyana" },
                    { "HT", "Haiti" },
                    { "HM", "Heard Island and McDonald Islands" },
                    { "HN", "Honduras" },
                    { "HK", "Hong Kong" },
                    { "HU", "Hungary" },
                    { "IS", "Iceland" },
                    { "IN", "India" },
                    { "ID", "Indonesia" },
                    { "IE", "Ireland" },
                    { "IM", "Isle of Man" },
                    { "IL", "Israel" },
                    { "IT", "Italy" },
                    { "JM", "Jamaica" },
                    { "JP", "Japan" },
                    { "JE", "Jersey" },
                    { "JT", "Johnston Island" },
                    { "JO", "Jordan" },
                    { "KZ", "Kazakhstan" },
                    { "KE", "Kenya" },
                    { "KI", "Kiribati" },
                    { "KW", "Kuwait" },
                    { "KG", "Kyrgyzstan" },
                    { "LA", "Laos" },
                    { "LV", "Latvia" },
                    { "LS", "Lesotho" },
                    { "LY", "Libya" },
                    { "LI", "Liechtenstein" },
                    { "LT", "Lithuania" },
                    { "LU", "Luxembourg" },
                    { "MO", "Macau" },
                    { "MK", "Macedonia [FYROM]" },
                    { "MG", "Madagascar" },
                    { "MW", "Malawi" },
                    { "MY", "Malaysia" },
                    { "MV", "Maldives" },
                    { "ML", "Mali" },
                    { "MT", "Malta" },
                    { "MH", "Marshall Islands" },
                    { "MQ", "Martinique" },
                    { "MR", "Mauritania" },
                    { "MU", "Mauritius" },
                    { "YT", "Mayotte" },
                    { "FX", "Metropolitan France" },
                    { "MX", "Mexico" },
                    { "FM", "Micronesia" },
                    { "MI", "Midway Islands" },
                    { "MD", "Moldova" },
                    { "MC", "Monaco" },
                    { "MN", "Mongolia" },
                    { "ME", "Montenegro" },
                    { "MS", "Montserrat" },
                    { "MA", "Morocco" },
                    { "MZ", "Mozambique" },
                    { "NA", "Namibia" },
                    { "NR", "Nauru" },
                    { "NP", "Nepal" },
                    { "NL", "Netherlands" },
                    { "AN", "Netherlands Antilles" },
                    { "NT", "Neutral Zone" },
                    { "NC", "New Caledonia" },
                    { "NZ", "New Zealand" },
                    { "NI", "Nicaragua" },
                    { "NE", "Niger" },
                    { "NG", "Nigeria" },
                    { "NU", "Niue" },
                    { "NF", "Norfolk Island" },
                    { "VD", "North Vietnam" },
                    { "MP", "Northern Mariana Islands" },
                    { "NO", "Norway" },
                    { "OM", "Oman" },
                    { "QO", "Outlying Oceania" },
                    { "PC", "Pacific Islands Trust Territory" },
                    { "PK", "Pakistan" },
                    { "PW", "Palau" },
                    { "PS", "Palestinian Territories" },
                    { "PA", "Panama" },
                    { "PZ", "Panama Canal Zone" },
                    { "PY", "Paraguay" },
                    { "PE", "Peru" },
                    { "PH", "Philippines" },
                    { "PN", "Pitcairn Islands" },
                    { "PL", "Poland" },
                    { "PT", "Portugal" },
                    { "PR", "Puerto Rico" },
                    { "QA", "Qatar" },
                    { "YD", "Republic of Yemen" },
                    { "RO", "Romania" },
                    { "RU", "Russia" },
                    { "RW", "Rwanda" },
                    { "RE", "Réunion" },
                    { "BL", "Saint Barthélemy" },
                    { "SH", "Saint Helena" },
                    { "KN", "Saint Kitts and Nevis" },
                    { "LC", "Saint Lucia" },
                    { "MF", "Saint Martin" },
                    { "PM", "Saint Pierre and Miquelon" },
                    { "VC", "Saint Vincent and the Grenadines" },
                    { "WS", "Samoa" },
                    { "SM", "San Marino" },
                    { "SA", "Saudi Arabia" },
                    { "SN", "Senegal" },
                    { "RS", "Serbia" },
                    { "CS", "Serbia and Montenegro" },
                    { "SC", "Seychelles" },
                    { "SL", "Sierra Leone" },
                    { "SG", "Singapore" },
                    { "SK", "Slovakia" },
                    { "SI", "Slovenia" },
                    { "SB", "Solomon Islands" },
                    { "ZA", "South Africa" },
                    { "GS", "South Georgia and the South Sandwich Islands" },
                    { "KR", "South Korea" },
                    { "ES", "Spain" },
                    { "LK", "Sri Lanka" },
                    { "SR", "Suriname" },
                    { "SJ", "Svalbard and Jan Mayen" },
                    { "SZ", "Swaziland" },
                    { "SE", "Sweden" },
                    { "CH", "Switzerland" },
                    { "ST", "São Tomé and Príncipe" },
                    { "TW", "Taiwan" },
                    { "TJ", "Tajikistan" },
                    { "TZ", "Tanzania" },
                    { "TH", "Thailand" },
                    { "TG", "Togo" },
                    { "TK", "Tokelau" },
                    { "TO", "Tonga" },
                    { "TT", "Trinidad and Tobago" },
                    { "TA", "Tristan da Cunha" },
                    { "TN", "Tunisia" },
                    { "TR", "Turkey" },
                    { "TM", "Turkmenistan" },
                    { "TC", "Turks and Caicos Islands" },
                    { "TV", "Tuvalu" },
                    { "UM", "U.S. Minor Outlying Islands" },
                    { "PU", "U.S. Miscellaneous Pacific Islands" },
                    { "VI", "U.S. Virgin Islands" },
                    { "UG", "Uganda" },
                    { "UA", "Ukraine" },
                    { "AE", "United Arab Emirates" },
                    { "GB", "United Kingdom" },
                    { "US", "United States" },
                    { "UY", "Uruguay" },
                    { "UZ", "Uzbekistan" },
                    { "VU", "Vanuatu" },
                    { "VA", "Vatican City" },
                    { "VE", "Venezuela" },
                    { "VN", "Vietnam" },
                    { "WK", "Wake Island" },
                    { "WF", "Wallis and Futuna" },
                    { "EH", "Western Sahara" },
                    { "YE", "Yemen" },
                    { "ZM", "Zambia" },
                    { "AX", "Åland Islands" },
                };
        }

        public static KeyValueList<string, string> GetBillingInfoUsStates()
        {
            return new KeyValueList<string, string>
                {
                    { "AK", "Alaska" },
                    { "AL", "Alabama" },
                    { "AP", "Armed Forces Pacific" },
                    { "AR", "Arkansas" },
                    { "AS", "American Samoa" },
                    { "AZ", "Arizona" },
                    { "CA", "California" },
                    { "CO", "Colorado" },
                    { "CT", "Connecticut" },
                    { "DC", "District of Columbia" },
                    { "DE", "Delaware" },
                    { "FL", "Florida" },
                    { "FM", "Federated States of Micronesia" },
                    { "GA", "Georgia" },
                    { "GU", "Guam" },
                    { "HI", "Hawaii" },
                    { "IA", "Iowa" },
                    { "ID", "Idaho" },
                    { "IL", "Illinois" },
                    { "IN", "Indiana" },
                    { "KS", "Kansas" },
                    { "KY", "Kentucky" },
                    { "LA", "Louisiana" },
                    { "MA", "Massachusetts" },
                    { "MD", "Maryland" },
                    { "ME", "Maine" },
                    { "MH", "Marshall Islands" },
                    { "MI", "Michigan" },
                    { "MN", "Minnesota" },
                    { "MO", "Missouri" },
                    { "MP", "Northern Mariana Islands" },
                    { "MS", "Mississippi" },
                    { "MT", "Montana" },
                    { "NC", "North Carolina" },
                    { "ND", "North Dakota" },
                    { "NE", "Nebraska" },
                    { "NH", "New Hampshire" },
                    { "NJ", "New Jersey" },
                    { "NM", "New Mexico" },
                    { "NV", "Nevada" },
                    { "NY", "New York" },
                    { "OH", "Ohio" },
                    { "OK", "Oklahoma" },
                    { "OR", "Oregon" },
                    { "PA", "Pennsylvania" },
                    { "PR", "Puerto Rico" },
                    { "PW", "Palau" },
                    { "RI", "Rhode Island" },
                    { "SC", "South Carolina" },
                    { "SD", "South Dakota" },
                    { "TN", "Tennessee" },
                    { "TX", "Texas" },
                    { "UT", "Utah" },
                    { "VA", "Virginia" },
                    { "VI", "Virgin Islands" },
                    { "VT", "Vermont" },
                    { "WA", "Washington" },
                    { "WV", "West Virginia" },
                    { "WI", "Wisconsin" },
                    { "WY", "Wyoming" },
                };
        }
    }
}
