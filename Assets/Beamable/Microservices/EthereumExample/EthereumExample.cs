using Beamable.Server;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.Signer;

namespace Beamable.Microservices
{
	[Microservice("EthereumExample")]
	public class EthereumExample : Microservice
	{
		/// <summary>
		/// It's used to get a wallet address linked to the given private key.
		/// </summary>
		/// <param name="msg">A message you want to send</param>
		/// <param name="signature">A signature made using the private key through <see cref="EthereumMessageSigner"/></param>
		/// <returns>A wallet address</returns>
		private string GetWalletAddress(string msg, string signature)
		{
			var signer1 = new EthereumMessageSigner();
			return signer1.EncodeUTF8AndEcRecover(msg, signature);
		}
		
		/// <summary>
		/// Associate wallet address with a player using Stats.
		/// </summary>
		/// <param name="msg1">A message you want to send</param>
		/// <param name="signature1">A signature made using the private key through <see cref="EthereumMessageSigner"/></param>
		/// <returns>Resolved wallet address</returns>
		[ClientCallable]
		public async Task<string> AssociateWallet(string msg1, string signature1)
		{
			// This code executes on the server.
			string walletAddress = GetWalletAddress(msg1, signature1);
			// prepare a dictionary with a wallet address stat
			var stats = new Dictionary<string, string>
			{
				{"walletAddr", walletAddress}
			};

			// write wallet address stat to the backend
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
