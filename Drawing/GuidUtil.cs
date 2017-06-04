using System;

namespace Drawing
{
    public static class GuidUtil
    {
        public static string ToUpperString(this Guid guid)
        {
            return guid.ToString("D").ToUpper();
        }
    }
}
