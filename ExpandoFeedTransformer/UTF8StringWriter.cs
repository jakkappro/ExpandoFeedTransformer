﻿using System.Text;

namespace ExpandoFeedTransformer
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.GetEncoding("windows-1250");
    }
}