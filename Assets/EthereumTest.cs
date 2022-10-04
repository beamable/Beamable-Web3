using System.Collections;
using UnityEngine;
using Nethereum.Util;
using Beamable.Server.Clients;
using Nethereum.Signer;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;

public class EthereumTest : MonoBehaviour
{
    public string address;
    public string privateKey;
    
    private async void Start()
    {
        var msg1 = "wee test message 18/09/2017 02:55PM";
        var signer1 = new EthereumMessageSigner();
        // get wallet's signature using it's private key
        var signature1 = signer1.EncodeUTF8AndSign(msg1, new EthECKey(privateKey));

        // associate wallet address with a player using a microservice
        var client = new EthereumExampleClient();
        var resolvedAddress = await client.AssociateWallet(msg1, signature1);
        
        Debug.Log($"Original Address: {address}, Resolved Address: {resolvedAddress}");

        StartCoroutine(GetWalletBalance());
    }

    /// <summary>
    /// This method sends a request to check the balance of the associated wallet
    /// </summary>
    private IEnumerator GetWalletBalance()
    {
        // create a balance request passing a URL to the Ethereum client
        var balanceRequest = new EthGetBalanceUnityRequest(
            "https://mainnet.infura.io/v3/ab915718ee34490e81e1f62360fee96f");
        
        // send the request and wait for response
        yield return balanceRequest.SendRequest(address, BlockParameter.CreateLatest());
        
        // convert the value from requests result
        Debug.Log("Balance of account:" + UnitConversion.Convert.FromWei(balanceRequest.Result.Value));
    }
}
