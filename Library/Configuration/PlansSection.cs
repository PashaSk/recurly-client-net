/**********************************************************************
* Copyright (c) 1999-2014 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace Infratel.RecurlyLibrary.Configuration
{
    public class PlansSection : ConfigurationSection
    {
        private PlansSection()
        {
        }

        private static PlansSection _instance = null;

        public static PlansSection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (PlansSection)ConfigurationManager.GetSection("plans");
                }
                return _instance;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public PlansCollection Plans
        {
            get { return (PlansCollection)base[""]; }
        }
    }
    
}