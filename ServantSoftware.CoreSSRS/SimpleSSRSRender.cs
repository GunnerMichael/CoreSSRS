using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServantSoftware.CoreSSRS
{
    /// <summary>
    /// Class to render an SSRS report to a binary format that can be saved to a file
    /// Very simple - uses a URL string to render rather than the full SOAP web-service
    /// </summary>
    public class SimpleSSRSRender : ISimpleSSRSRender
    {
        private string _serverUrl;

        /// <summary>
        /// Constructs a new instance of the SimpleSSRSRender class
        /// </summary>
        /// <param name="serverUrl">URL of the Report Server</param>
        public SimpleSSRSRender(string serverUrl)
        {
            this._serverUrl = serverUrl;
        }

        /// <summary>
        /// Render an SSRS report the specified format
        /// </summary>
        /// <param name="reportPath">Path to the SSRS report</param>
        /// <param name="reportParameters">Collection of parameters for the report</param>
        /// <param name="outputFormat">Output format e.g PDF or EXCEL</param>
        /// <returns>A byte array of the output that can be saved to a file</returns>
        public async Task<byte[]> RenderReport(string reportPath, NameValueCollection reportParameters, string outputFormat = "PDF")
        {
            string url = BuildFullRenderUrl(reportPath, reportParameters, outputFormat);
            HttpClient client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });

            return await client.GetByteArrayAsync(url);

        }

        /// <summary>
        /// Build the URL for the report
        /// </summary>
        /// <param name="reportPath">Path to the SSRS report</param>
        /// <param name="reportParameters">Collection of parameters for the report</param>
        /// <param name="outputFormat">Output format e.g PDF or EXCEL</param>
        /// <returns>Returns the url for rendering the report</returns>
        private string BuildFullRenderUrl(string reportPath, NameValueCollection reportParameters, string outputFormat)
        {
            string parameterString = BuildParameterQueryString(reportParameters);
            string fullPath = $"{_serverUrl}?{reportPath}&rs:Command=Render{parameterString}&rs:Format={outputFormat}";

            return fullPath;
        }

        /// <summary>
        /// Builds the query string from the supplied parameters
        /// </summary>
        /// <param name="reportParameters">Collection of parameters for the report</param>
        /// <returns>A string of the parameters</returns>
        private string BuildParameterQueryString(NameValueCollection reportParameters)
        {
            string parameterString = string.Empty;

            var items = reportParameters.AllKeys.SelectMany(reportParameters.GetValues, (k, v) => new { key = k, value = v });
            foreach (var item in items)
            {
                parameterString += $"&{item.key}={item.value}";
            }

            return parameterString;
        }
    }
}