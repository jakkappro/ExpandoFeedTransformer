﻿using System.Text;

namespace ExpandoFeedTransformer
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => new UTF8Encoding(false);
    }
}