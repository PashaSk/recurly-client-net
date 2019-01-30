/**********************************************************************
* Copyright (c) 1999-2014 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System.Collections.Generic;
using System.Configuration;

namespace Infratel.RecurlyLibrary.Configuration
{
    public class PlansCollection : ConfigurationElementCollection, IEnumerable<Plan>
    {
        public Plan this[int index]
        {
            get { return (Plan)BaseGet(index); }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Plan();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Plan)element).Name;
        }

        protected override string ElementName
        {
            get { return "plan"; }
        }

        public new IEnumerator<Plan> GetEnumerator()
        {
            int count = base.Count;

            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as Plan;
            }
        }
    }

}