using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmachiRerollBannerClicker
{
    public class StatusCodeArgs : EventArgs
    {
        public string StatusCode;
        public string ExternalFunctionName;
        public string APIFunctionName;

        public StatusCodeArgs(string apiFunctionName, string externalFunctionname, string StatusCode)
        {
            this.ExternalFunctionName = externalFunctionname;
            this.APIFunctionName = apiFunctionName;
            this.StatusCode = StatusCode;
        }

        public override string ToString()
        {
            return $"Funktion {this.APIFunctionName} returned StatusCode: {this.StatusCode}.";
        }
    }
}
