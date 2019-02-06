/**********************************************************************
* Copyright (c) 1999-2016 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System.Configuration;

namespace Infratel.RecurlyLibrary.Configuration
{
    public class Plan : ConfigurationElement
    {

        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("MinutesLimit", IsRequired = false, DefaultValue = 0)]
        public int MinutesLimit
        {
            get { return (int)this["MinutesLimit"]; }
            set { this["MinutesLimit"] = value; }
        }

        [ConfigurationProperty("UsersLimit", IsRequired = false, DefaultValue = 100)]
        public int UsersLimit
        {
            get { return (int)this["UsersLimit"]; }
            set { this["UsersLimit"] = value; }
        }


        [ConfigurationProperty("VoiceMailsLimit", IsRequired = false, DefaultValue = 100)]
        public int VoiceMailsLimit
        {
            get { return (int)this["VoiceMailsLimit"]; }
            set { this["VoiceMailsLimit"] = value; }
        }

        [ConfigurationProperty("PhonesLimit", IsRequired = false, DefaultValue = 0)]
        public int PhonesLimit
        {
            get { return (int)this["PhonesLimit"]; }
            set { this["PhonesLimit"] = value; }
        }

        [ConfigurationProperty("AllowTollFree", IsRequired = false, DefaultValue = false)]
        public bool AllowTollFree
        {
            get { return (bool)this["AllowTollFree"]; }
            set { this["AllowTollFree"] = value; }
        }
        [ConfigurationProperty("AllowCrmApi", IsRequired = false, DefaultValue = false)]
        public bool AllowCrmApi
        {
            get { return (bool)this["AllowCrmApi"]; }
            set { this["AllowCrmApi"] = value; }
        }
        [ConfigurationProperty("AllowTeamLiveStatus", IsRequired = false, DefaultValue = false)]
        public bool AllowTeamLiveStatus
        {
            get { return (bool)this["AllowTeamLiveStatus"]; }
            set { this["AllowTeamLiveStatus"] = value; }
        }
        [ConfigurationProperty("ApiGatewayPlan", IsRequired = false, DefaultValue = "")]
        public string ApiGatewayPlan
        {
            get { return (string)this["ApiGatewayPlan"]; }
            set { this["ApiGatewayPlan"] = value; }
        }

        [ConfigurationProperty("ClickConnectLimit", IsRequired = false, DefaultValue = 0)]
        public int ClickConnectLimit
        {
            get { return (int)this["ClickConnectLimit"]; }
            set { this["ClickConnectLimit"] = value; }
        }

        [ConfigurationProperty("CallbackLimit", IsRequired = false, DefaultValue = 0)]
        public int CallbackLimit
        {
            get { return (int)this["CallbackLimit"]; }
            set { this["CallbackLimit"] = value; }
        }

        [ConfigurationProperty("ContactUsLimit", IsRequired = false, DefaultValue = 0)]
        public int ContactUsLimit
        {
            get { return (int)this["ContactUsLimit"]; }
            set { this["ContactUsLimit"] = value; }
        }

        [ConfigurationProperty("AllowInternationalCalls", IsRequired = false, DefaultValue = false)]
        public bool AllowInternationalCalls
        {
            get { return (bool)this["AllowInternationalCalls"]; }
            set { this["AllowInternationalCalls"] = value; }
        }

        [ConfigurationProperty("SmsLimit", IsRequired = false, DefaultValue = 0)]
        public int SmsLimit
        {
            get { return (int)this["SmsLimit"]; }
            set { this["SmsLimit"] = value; }
        }

        [ConfigurationProperty("AllowAcd", IsRequired = false, DefaultValue = false)]
        public bool AllowAcd
        {
            get { return (bool)this["AllowAcd"]; }
            set { this["AllowAcd"] = value; }
        }


        [ConfigurationProperty("AllowEmailInbox", IsRequired = false, DefaultValue = false)]
        public bool AllowEmailInbox
        {
            get { return (bool)this["AllowEmailInbox"]; }
            set { this["AllowEmailInbox"] = value; }
        }

        [ConfigurationProperty("AllowFacebook", IsRequired = false, DefaultValue = false)]
        public bool AllowFacebook
        {
            get { return (bool)this["AllowFacebook"]; }
            set { this["AllowFacebook"] = value; }
        }

        [ConfigurationProperty("AllowTwitter", IsRequired = false, DefaultValue = false)]
        public bool AllowTwitter
        {
            get { return (bool)this["AllowTwitter"]; }
            set { this["AllowTwitter"] = value; }
        }

        [ConfigurationProperty("AllowDashboardStatistics", IsRequired = false, DefaultValue = false)]
        public bool AllowDashboardStatistics
        {
            get { return (bool)this["AllowDashboardStatistics"]; }
            set { this["AllowDashboardStatistics"] = value; }
        }

        [ConfigurationProperty("WiFiCallsEnabled", IsRequired = false, DefaultValue = false)]
        public bool WiFiCallsEnabled
        {
            get { return (bool)this["WiFiCallsEnabled"]; }
            set { this["WiFiCallsEnabled"] = value; }
        }

        [ConfigurationProperty("SipPhonesEnabled", IsRequired = false, DefaultValue = false)]
        public bool SipPhonesEnabled
        {
            get { return (bool)this["SipPhonesEnabled"]; }
            set { this["SipPhonesEnabled"] = value; }
        }

        [ConfigurationProperty("CallRecording", IsRequired = false, DefaultValue = false)]
        public bool CallRecording
        {
            get { return (bool)this["CallRecording"]; }
            set { this["CallRecording"] = value; }
        }

        [ConfigurationProperty("VoiceToText", IsRequired = false, DefaultValue = false)]
        public bool VoiceToText
        {
            get { return (bool)this["VoiceToText"]; }
            set { this["VoiceToText"] = value; }
        }

        [ConfigurationProperty("Price", IsRequired = false, DefaultValue = null)]
        public int? Price
        {
            get { return (int?)this["Price"]; }
            set { this["Price"] = value; }
        }

        [ConfigurationProperty("DisplayName", IsRequired = false, DefaultValue = null)]
        public string DisplayName
        {
            get { return (string)this["DisplayName"]; }
            set { this["DisplayName"] = value; }
        }

        [ConfigurationProperty("AutoAttendant", IsRequired = false, DefaultValue = true)]
        public bool AutoAttendant
        {
            get { return (bool)this["AutoAttendant"]; }
            set { this["AutoAttendant"] = value; }
        }

        [ConfigurationProperty("ActivityQueue", IsRequired = false, DefaultValue = true)]
        public bool ActivityQueue
        {
            get { return (bool)this["ActivityQueue"]; }
            set { this["ActivityQueue"] = value; }
        }

        [ConfigurationProperty("Position", IsRequired = false, DefaultValue = null)]
        public int? Position
        {
            get { return (int?)this["Position"]; }
            set { this["Position"] = value; }
        }

        [ConfigurationProperty("AllowedAddons", IsRequired = false, DefaultValue = null)]
        private string _AllowedAddons
        {
            get { return (string)this["AllowedAddons"]; }
            set { this["AllowedAddons"] = value; }
        }
        public string[] AllowedAddons
        {
            get {
                var v = _AllowedAddons;
                if (!string.IsNullOrEmpty(v))
                {
                    return v.Split(new char[] { ';', ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                }

                return null;
            }
            set { _AllowedAddons = string.Join(";",value); }
        }

        [ConfigurationProperty("IsTrial", IsRequired = false, DefaultValue = false)]
        public bool IsTrial
        {
            get { return (bool)this["IsTrial"]; }
            set { this["IsTrial"] = value; }
        }

        [ConfigurationProperty("MaxUsersPerOneGroup", IsRequired = true)]
        public int MaxUsersPerOneGroup {

            get { return (int)this["MaxUsersPerOneGroup"]; }
            set { this["MaxUsersPerOneGroup"] = value;  }
        }

        [ConfigurationProperty("CallflowsLimit", IsRequired = false, DefaultValue = null)]
        public int? CallflowsLimit
        {

            get { return (int?)this["CallflowsLimit"]; }
            set { this["CallflowsLimit"] = value; }

        }

    }
}