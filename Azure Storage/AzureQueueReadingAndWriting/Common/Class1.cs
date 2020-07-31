using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Common
{
    public static class ConnectionString
    {
        
        public static string getConnString()
        {
            return "DefaultEndpointsProtocol=https;AccountName=sahpexcalibur;AccountKey=;EndpointSuffix=core.windows.net";
        }
    }
}
