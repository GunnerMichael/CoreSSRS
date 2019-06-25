using System.Collections.Specialized;
using System.Threading.Tasks;

namespace ServantSoftware.CoreSSRS
{
    public interface ISimpleSSRSRender
    {
        /// <summary>
        /// Render an SSRS report the specified format
        /// </summary>
        /// <param name="reportPath">Path to the SSRS report</param>
        /// <param name="reportParameters">Collection of parameters for the report</param>
        /// <param name="outputFormat">Output format e.g PDF or EXCEL</param>
        /// <returns>A byte array of the output that can be saved to a file</returns>
        Task<byte[]> RenderReport(string reportPath, NameValueCollection reportParameters, string outputFormat = "PDF");
    }
}