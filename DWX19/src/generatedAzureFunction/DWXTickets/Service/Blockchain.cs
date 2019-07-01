using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureFunction
{
	public class SmartContract
	{
		public static string Abi => "[{\"constant\":true,\"inputs\":[],\"name\":\"InstanceOwner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"OfferPrice\",\"outputs\":[{\"name\":\"\",\"type\":\"int256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"Description\",\"outputs\":[{\"name\":\"\",\"type\":\"string\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"InstanceBuyer\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"AskingPrice\",\"outputs\":[{\"name\":\"\",\"type\":\"int256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"State\",\"outputs\":[{\"name\":\"\",\"type\":\"uint8\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"name\":\"description\",\"type\":\"string\"},{\"name\":\"price\",\"type\":\"int256\"}],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"constant\":false,\"inputs\":[{\"name\":\"offerPrice\",\"type\":\"int256\"}],\"name\":\"MakeOffer\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[],\"name\":\"Reject\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[],\"name\":\"AcceptOffer\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
		public static string ContractAddress = "https://dwx19.blockchain.azure.com:3200/H8mYTyhdkumn3GmyqS7SPwrp";
		public static string BlockchainRpcEndpoint = "";
	}

	public static class Blockchain
	{
		public static async Task<HttpResponseMessage> CreateRespone(HttpRequestMessage req, string functionName)
		{
			// Get request body
			var jsonBody = await req.Content.ReadAsStringAsync();
			if (string.IsNullOrWhiteSpace(jsonBody)) jsonBody = @"{}";
			var body = JsonConvert.DeserializeObject<JObject>(jsonBody);

			// Get parameters
			var inputParameters = body.Values();
			var arguments = new object[inputParameters.Count()];
			var i = 0;
			foreach (var p in inputParameters.Values())
				arguments[i++] = p.Value<string>();

			var web3 = new Nethereum.Web3.Web3(SmartContract.BlockchainRpcEndpoint);
			var contract = web3.Eth.GetContract(SmartContract.Abi, SmartContract.ContractAddress);

			var functionABI = contract.ContractBuilder.ContractABI.Functions
				.FirstOrDefault(f => f.Name == functionName);

			if (functionABI == null)
				return req.CreateResponse(HttpStatusCode.BadRequest, "Function not found!");

			var functionParameters = functionABI.InputParameters;
			if (functionParameters?.Count() != inputParameters.Count())
				return req.CreateResponse(HttpStatusCode.BadRequest, "Parameters do not match!");

			Function function = contract.GetFunction(functionName);
			Type returnType = GetFunctionReturnType(functionABI);
			IEthCall ethCall = contract.Eth.Transactions.Call;
			var result = await ethCall.SendRequestAsync(function.CreateCallInput(arguments), contract.Eth.DefaultBlock)
				.ConfigureAwait(false);

			FunctionBase functionBase = function;
			PropertyInfo builderBaseProperty = functionBase.GetType()
				.GetProperty("FunctionBuilderBase", BindingFlags.Instance | BindingFlags.NonPublic);
			if (builderBaseProperty != null)
			{
				FunctionBuilderBase builderBase = (FunctionBuilderBase) builderBaseProperty.GetValue(functionBase);
				PropertyInfo funcCallDecoderProperty = builderBase.GetType()
					.GetProperty("FunctionCallDecoder", BindingFlags.Instance | BindingFlags.NonPublic);
				if (funcCallDecoderProperty != null)
				{
					ParameterDecoder decoder = (ParameterDecoder) funcCallDecoderProperty.GetValue(builderBase);
					var results = decoder.DecodeDefaultData(result, functionABI.OutputParameters);

					if (results.Count == 1)
					{
						var resultValue = JsonConvert.SerializeObject(results[0].Result);
						return req.CreateResponse(HttpStatusCode.OK, resultValue);
					}

					var resultMultiValue = Activator.CreateInstance(returnType, results.Select(r => r.Result).ToArray());
					return req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(resultMultiValue));
				}
			}

			return req.CreateResponse(HttpStatusCode.InternalServerError);
		}

		private static Type GetFunctionReturnType( FunctionABI functionABI )
		{
			if ( functionABI == null )
				return typeof( object );

			Parameter[] parameters = functionABI.OutputParameters;

			if ( parameters == null || parameters.Length == 0 )
				return typeof( object );

			if ( parameters.Length == 1 )
				return parameters[0].ABIType.GetDefaultDecodingType();

			Type taskType = Type.GetType( "System.Tuple`" + parameters.Length );
			List<Type> typeArgs = new List<Type>();

			foreach ( var param in parameters )
			{
				typeArgs.Add( param.ABIType.GetDefaultDecodingType() );
			}

			if (taskType != null)
			{
				Type genericType = taskType.MakeGenericType( typeArgs.ToArray() );

				return genericType;
			}

			return null;
		}
	}
}
                       