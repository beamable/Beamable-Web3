using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nethereum.Web3;
using Nethereum.Util;
using System.Collections.Generic;
using System.Text;
using Beamable.Server.Clients;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.Encoders;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;

public class EthereumTest : MonoBehaviour
{
    public string address;
    public string privateKey;
    
    // Start is called before the first frame update
    async void Start()
    {
        var msg1 = "wee test message 18/09/2017 02:55PM";
        var signer1 = new EthereumMessageSigner();
        var signature1 = signer1.EncodeUTF8AndSign(msg1, new EthECKey(privateKey));

        var client = new EthereumExampleClient();
        var resolvedAddress = await client.AssociateWallet(msg1, signature1);
        
        Debug.Log($"Original Address: {address}, Resolved Address: {resolvedAddress}");

        StartCoroutine(GetWalletBalance());
    }

    public IEnumerator GetWalletBalance()
    {
        var balanceRequest = new EthGetBalanceUnityRequest(
            "https://mainnet.infura.io/v3/ab915718ee34490e81e1f62360fee96f");
        
        yield return balanceRequest.SendRequest(address, BlockParameter.CreateLatest());
        
        Debug.Log("Balance of account:" + UnitConversion.Convert.FromWei(balanceRequest.Result.Value));
    }
}
