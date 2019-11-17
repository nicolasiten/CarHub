using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Utils
{
    public class Base64Utils
    {
        public static string ConvertToBase64String(byte[] image)
        {
            return $"data:image;base64,{System.Convert.ToBase64String(image)}";
        }
    }
}
