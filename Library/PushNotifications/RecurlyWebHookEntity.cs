/**********************************************************************
* Copyright (c) 1999-2014 Infratel, Inc.
* All rights reserved.
* http://www.infratel.com
*
***********************************************************************/

using System.Xml;

namespace Infratel.RecurlyLibrary
{
    public abstract class RecurlyWebHookEntity
    {
        internal abstract void ReadXml(XmlTextReader reader);
    }
}
