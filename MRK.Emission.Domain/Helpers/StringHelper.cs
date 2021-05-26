using System;
using System.Text;

namespace MRK.Emission.Domain.Helpers
{
    public class StringHelper
    {
        public static string StringToBase64(string normalString)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(normalString), Base64FormattingOptions.None);
        
    }
}
