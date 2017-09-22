﻿Fairlay provides a simple and powerful API to check globally accessible market data via the [Public API](https://github.com/Fairlay/FairlayPublicAPI) and work with your account data and integrate external providers easily via the [Private API](https://github.com/Fairlay/FairlayPrivateAPI).

# Overview
* Go to [Fairlay.com](https://Fairlay.com) for more information about Fairlay
* [Public API](https://github.com/Fairlay/FairlayPublicAPI) to grab all markets or competitions on Fairlay (updated every 5s). No account needed.
* [Private API](https://github.com/Fairlay/FairlayPrivateAPI) for POST requests (creating/changing markets and orders, transfering funds, etc.), costs 0.1mBTC per 100.000 requests, [see below](#private-api) on how to create your developer API account
* Other supported libraries:
  * [Python Sample Client](https://github.com/Fairlay/PythonSampleClient)
  * [Old CSharp API](https://github.com/Fairlay/FairlayDotNetClient/tree/old-sample-client) (.NET Desktop, written in .NET 4, outdated, but still works fine, does not use .NET Standard 2.0 and no async/await code)

# Getting Started
The easiest way to get started is to simply grab the FairlayDotNetClient via nuget:
> PM> Install-Package FairlayDotNetClient

Or simply create a new solution or project in Visual Studio and right click on the references, select "Manage NuGet Packages..." and type in "FairlayDotNetClient" into the Browse search box.

The [Public API](https://github.com/Fairlay/FairlayPublicAPI) is very easy to use, you don't need a developer API key. You can simply instantiate and call any method you like on FairlayPublicApi:
```csharp
var serverTime = new FairlayPublicApi().GetServerTime();
```
returns the current server time as a DateTime object

```csharp
var markets = new FairlayPublicApi().GetMarkets(new MarketsFilter { OnlyActive = true, Category = MarketCategory.Boxing });
```
will get all active markets in the boxing category. For details see [Public API](https://github.com/Fairlay/FairlayPublicAPI).

# Private API
If you know how to create your private/public RSA keys, you can skip this step-by-step guide. Just paste your public key into the textbox when creating the [new api account on Fairlay](https://fairlay.com/user/dev/).

### Generate RSA key pair using .NET
This library includes two static helper classes to generate a private/public RSA key pair and serialize them to XML in the `FairlayDotNetClient.Private` namespace:
```csharp
var rsaKeyPair = RsaKeyPairGenerator.GenerateNewRsaKeyPair();
// Call RsaParametersExtensions.ToXmlString extension to get a XML string representation of the key 
string privateRsaXml = rsaKeyPair.PrivateKeyParameters.ToXmlString();
string publicRsaXml = rsaKeyPair.PublicKeyParameters.ToXmlString();
```

### Generate RSA key pair using OpenSSL
1. Download [OpenSSL](https://www.openssl.org/), on Windows it is easiest to just install the [Win32 OpenSSL v1.1.0f Light installer from here](https://slproweb.com/products/Win32OpenSSL.html).
2. Follow the guide on [this wiki](https://en.wikibooks.org/wiki/Cryptography/Generate_a_keypair_using_OpenSSL) to generate a new private and public keypair:
> openssl genpkey -algorithm RSA -out private_key.pem -pkeyopt rsa_keygen_bits:2048
3. Keep your private key backed up and in a save location, they should never leave your possession.
4. Extract the public key from your private key for your developer api account on [Fairlay.com](https://fairlay.com/user/dev/)
> openssl rsa -pubout -in private_key.pem -out public_key.pem
5. To get the public key in xml format, use openssl commands or this [handy RSA Key Converter website](https://superdry.apphb.com/tools/online-rsa-key-converter):
> openssl rsa -text -in private_key.pem
6. Now paste the public key in xml format when [creating a new api account](https://fairlay.com/user/dev/), also write down your user id and api id, which are needed below. You will also need your private key to sign all communication with the Fairlay Private API server in the same xml format.

### Instantiate the PrivateApi
Once you have your [developer API key](https://fairlay.com/user/dev/) you only need to instantiate the FairlayClient. Make sure you use the private RSA key just created in `PrivateApiCredentials.PrivateRsaParameters` together with your user id and api account id (see ConsoleSample for an example):
```csharp
var privateApi = new FairlayPrivateApiBuilder(new PrivateApiCredentials(1004056)
{
	PrivateRsaParameters = RsaParametersExtensions.FromXmlString(ClientPrivateRsaXml)
}).Build();
```

Now you can use the client and get your balances, send payments, interact with Fairlay, etc.
```csharp
var balances = await client.GetBalances();
Console.WriteLine("Your current available balance: " +
	balances[CurrencyIds.Mbtc].AvailableFunds.ToCoinString());
```

For more details check the [source code](https://github.com/Fairlay/FairlayDotNetClient/tree/master/) and see the [Private API](https://github.com/Fairlay/FairlayPrivateAPI) documentation.

# Console Sample
The [FairlayDotNetClient.ConsoleSample](https://github.com/Fairlay/FairlayDotNetClient/tree/master/ConsoleSample/) is a very simple example that tells you everything you need to setup and use the private or public api.

```csharp
public class Program
{
	public static void Main()
	{
		UsePrivateApi().GetAwaiter().GetResult();
		UsePublicApi().GetAwaiter().GetResult();
	}
	...
}
```

UsePrivateApi creates the credentials and passes it into the FairlayPrivateApiBuilder to create an easy to use PrivateApi instance, then it calls a bunch of methods on it to query some data on the given fairlay account.

```csharp
private static async Task UsePrivateApi()
{
	var privateApi = new FairlayPrivateApiBuilder(new PrivateApiCredentials(1004056)
	{
		PrivateRsaParameters = RsaParametersExtensions.FromXmlString(ClientPrivateRsaXml)
	}).Build();
	var balances = await privateApi.GetBalances();
	Console.WriteLine("Your current available balance: " +
		balances[CurrencyIds.Mbtc].AvailableFunds.ToCoinString());
	Console.WriteLine("Your balances details: " + JsonConvert.SerializeObject(balances));
	var statements = await privateApi.GetStatement(DateTime.UtcNow.AddMonths(-1));
	Console.WriteLine("Your statements of the past month are: " +
		JsonConvert.SerializeObject(statements));
}
```

And finally the UsePublicApi method calls the GetMarkets and GetCompetitions methods and spits out the first result of each:

```csharp
private static async Task UsePublicApi()
{
	var publicApi = new FairlayPublicApi();
	var markets = await publicApi.GetMarkets(MarketX.Category.BITCOIN);
	Console.WriteLine("First bitcoin market: " + markets[0]);
	var competitions = await publicApi.GetCompetitions(MarketX.Category.SOCCER);
	Console.WriteLine("First competition: " + competitions[0]);
}
```

# Android Sample
Just for fun there is also a [FairlayDotNetClient.AndroidSample](https://github.com/Fairlay/FairlayDotNetClient/tree/master/AndroidSample/) included in the source code, which you can compile and run using the free Xamarin MonoDroid via Visual Studio. It features similar code to the ConsoleSample, but is a bit more pretty and provides a dropdown selection box for the category and some interactive buttons to grab some data.

![Fairlay Android Sample](https://github.com/Fairlay/FairlayDotNetClient/blob/master/AndroidSample/FairlayAndroidScreenshot.png)

```csharp
[Activity(Label = "Fairlay Android Sample", MainLauncher = true, Icon = "@mipmap/icon")]
public class MainActivity : Activity
{
 private readonly string[] categories =
 {
 	"Soccer",
 	"Tennis",
 	"Golf",
 	"RugbyUnion",
 	"Boxing",
 	"HorseRacing",
 	"MotorSport",
 	"RugbyLeague",
 	"Basketball",
 	"AmericanFootball",
 	"Baseball",
 	"Hockey",
 	"Politics",
 	"Financial",
 	"Esports",
 	"Bitcoin"
 };
 
 protected override void OnCreate(Bundle savedInstanceState)
 {
 	base.OnCreate(savedInstanceState);
 	SetContentView(Resource.Layout.Main);
 	var spinner = FindViewById<Spinner>(Resource.Id.categorySpinner);
 	spinner.Adapter = new ArrayAdapter<string>(this, Resource.Layout.MarketItem, categories);
 	var clickMarketsButton = FindViewById<Button>(Resource.Id.clickMarketsButton);
 	clickMarketsButton.Click += OnClickMarketsButton;
 	var clickBalanceButton = FindViewById<Button>(Resource.Id.clickBalanceButton);
 	clickBalanceButton.Click += OnClickBalanceButton;
 }
 
 private async void OnClickMarketsButton(object sender, EventArgs e)
 {
 	var markets = await new FairlayPublicApi().GetMarkets(GetSelectedCategory());
 	var marketTexts = markets.Select(m => m.Title).ToArray();
 	var marketsList = FindViewById<ListView>(Resource.Id.marketsList);
 	marketsList.Adapter = new ArrayAdapter<string>(this, Resource.Layout.MarketItem, marketTexts);
 	marketsList.TextFilterEnabled = true;
 	marketsList.ItemClick +=
 		(s, a)=> Toast.MakeText(Application, ((TextView)a.View).Text, ToastLength.Short).Show();
 }
 
 private int GetSelectedCategory()
 {
 	var spinner = FindViewById<Spinner>(Resource.Id.categorySpinner);
 	switch (spinner.SelectedItem.ToString())
 	{
 	case "Soccer": return MarketX.Category.SOCCER;
 	case "Tennis": return MarketX.Category.TENNIS;
 	case "Golf": return MarketX.Category.GOLF;
 	case "RugbyUnion": return MarketX.Category.RUGBYUNION;
 	case "Boxing": return MarketX.Category.BOXING;
 	case "HorseRacing": return MarketX.Category.HORSERACING;
 	case "MotorSport": return MarketX.Category.MOTORSPORT;
 	case "RugbyLeague": return MarketX.Category.RUGBYLEAGUE;
 	case "Basketball": return MarketX.Category.BASKETBALL;
 	case "AmericanFootball": return MarketX.Category.AMERICANFOOTBALL;
 	case "Baseball": return MarketX.Category.BASEBALL;
 	case "Hockey": return MarketX.Category.HOCKEY;
 	case "Politics": return MarketX.Category.POLITICS;
 	case "Financial": return MarketX.Category.FINANCIAL;
 	case "Esports": return MarketX.Category.ESPORTS;
 	default: return MarketX.Category.BITCOIN;
 	}
 }
 
 private async void OnClickBalanceButton(object sender, EventArgs e)
 {
 	var balanceLabel = FindViewById<TextView>(Resource.Id.balanceLabel);
	var privateApi = new FairlayPrivateApiBuilder(new PrivateApiCredentials(1004056)
	{
		PrivateRsaParameters = RsaParametersExtensions.FromXmlString(ClientPrivateRsaXml)
	}).Build();
 	var balances = await privateApi.GetBalances();
 	balanceLabel.Text = "TestAccount Balance: " + balances[0].AvailableFunds + " mBTC";
 }
}
```

# Source Code
You can also download or clone the source code from [here](https://github.com/Fairlay/FairlayDotNetClient/archive/master.zip), check out the tests to understand how each part is working or use the included [Sample](https://github.com/Fairlay/FairlayDotNetClient/tree/master/ConsoleSample) to get started with a working sample. If you have any question feel free to [open an issue](https://github.com/Fairlay/FairlayDotNetClient/issues).

The source code is written in .NET Core 2 and fully compatible with Windows, Linux, MacOS, Android and iOS via Xamarin, so you can run it pretty much on any platform you like. Feel free to use it in any way you like, for details check the [license](https://github.com/Fairlay/FairlayDotNetClient/tree/master/License.md).