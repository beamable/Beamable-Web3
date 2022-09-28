using Beamable.Server;
using Nethereum.Util;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Beamable.Common.Api.Stats;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.Encoders;

namespace Beamable.Microservices
{
	[Microservice("EthereumExample")]
	public class EthereumExample : Microservice
	{
		private string GetWalletAddress(string msg, string signature)
		{
			var signer1 = new EthereumMessageSigner();
			return signer1.EncodeUTF8AndEcRecover(msg, signature);
		}
		
		[ClientCallable]
		public async Task<string> AssociateWallet(string msg1, string signature1)
		{
			// This code executes on the server.
			string walletAddress = GetWalletAddress(msg1, signature1);
			var stats = new Dictionary<string, string>()
			{
				{"walletAddr", walletAddress}
			};

			await Services.Stats.SetStats(
				"game", 
				"private", 
				"player", 
				Context.UserId, 
				stats
			);
			
			return walletAddress;
		}
	}
}
