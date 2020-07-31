using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Common
{
    public static class ConnectionString
    {
        
        public static string getConnString()
        {
            return "DefaultEndpointsProtocol=https;AccountName=sahpexcalibur;AccountKey=BC0dpcC+SxBgFXjh1EBuma7mN4IBXE7jVDYC23k9eN97R4OKUOCShRPDLeFLSzf97yr8R2pfV57SpGO5h5bNzA==;EndpointSuffix=core.windows.net";
        }
    }
}
