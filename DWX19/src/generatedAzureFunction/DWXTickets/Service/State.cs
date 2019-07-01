
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunction
{
	public static class Function_State
	{
		private static string _functionName = "State";

		[FunctionName( "Function_State" )]
		public static async Task<HttpResponseMessage> Run( [HttpTrigger( AuthorizationLevel.Anonymous, "get", "post", Route = null )]HttpRequestMessage req, TraceWriter log )
		{
			log.Info( "C# HTTP trigger function processed a request." );

			return await Blockchain.CreateRespone(req, _functionName);
		}
	
	}
}
                       