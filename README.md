### Beamable Nethereum Demo

This project demonstrates basic interactions with an Ethereum blockchain.

# Important files

- SampleScene.unity
	The scene holding the demo.
- EthereumTest.cs
	This is the MonoBehaviour which demonstrates usage of Nethereum.
- EthereumExample.cs
	This is the microservice which handles association between a player and a wallet.

# How to add external dependencies to a microservice

In order to add external dependencies you first need to import all of them as assets. Then go to `Assets/Beamable/Microservices` and enter the folder of your microservice. Inside microservice directory there is an assembly definition asset. Select it and in the Inspector window you will see a list of Assembly Definition References. You can add other assembly definition assets here. In order to add dll libraries you need to check `Override References` setting on the top of the page. A list of Assembly References will appear. You can add/remove elements and choose them from a dropdown. After you make any changes remember to press `Apply` in the bottom of the Inspector. After a recompile the microservice will have access to attached dependencies.

# How to extract a wallet address from an Ethereum signed request

To receive a wallet address you need to use `EthereumMessageSigner` object. Then `EncodeUTF8AndEcRecover` method will return the address. To make it work you have to pass a message and a signature as parameters. The message is just anything you would like to send. A signature is a `EthECKey` object which takes wallet's private key as a constructor parameter.

# How to associate the wallet address in the player stats

To store the wallet address in the backend you will need a microservice. In a `ClientCallable` method you can use `Services.Stats.SetStats` method. It's parameters include:
- `access`: should be "private" which means only stat owner will be able to read the value.
- `domain`: "game" means that the stat will be stored on the backend as opposed to "client" which is stored on the Unity side.
- `id`: the user ID of the stat owner.
- `type`: the only value is "player" (exists for legacy purposes only).
- `stats`: a dictionary with actual stats.
The full code to store the wallet address would be:

```
var stats = new Dictionary<string, string>()
{
	{"walletAddr", walletAddress}
};

await Services.Stats.SetStats("game", "private", "player", Context.UserId, stats);
```

# How to query a wallet for its contents

Wallet's balance can be requested using `EthGetBalanceUnityRequest` object passing the URL of an Ethereum client in the constructor's parameter. Next you need to send the request from a coroutine using 
`yield return balanceRequest.SendRequest(address, BlockParameter.CreateLatest());`
Once this method finishes the balance will be stored in `balanceRequest.Result.Value`. However the value has to be converted using `UnitConversion.Convert.FromWei(balanceRequest.Result.Value)`.