using System;
using System.Linq;
using System.Net;
using Android.App;
using Android.OS;
using Android.Widget;
using FairlayDotNetClient.Private;
using FairlayDotNetClient.Public;

namespace FairlayDotNetClient.AndroidSample
{
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

		public const string ClientPrivateRsaXml =
			"<RSAKeyValue><Modulus>5ZuQEg2qtTrYvehQTxdBJiOSyNiBpBW+fVlk7HJ9+6OQmGCNhnjLfcGFMA1ODxsiC8ILlI5++l2H8SCtDLNMGi2O5aDBsY9Bs+QetQAJVKyFtsoDRtBvy4vlR/joMP+jic/Mt2r92riFGWunZliPI47qiPVJd8JycXR2Kf8VbCU=</Modulus><Exponent>AQAB</Exponent><P>/b6BwxydWyLh1RAaJtO4ROiRNYUB14TAzjfleylapPfT64HlDyLbMzolg0zbH1FDq6D17mEB+Ac3m9+cKHWpzQ==</P><Q>56Yfum0pgh4Yi0WKzmiu/rKYVkVqjz0acjczKtgaUjLEsE2vUM4B9N+8S2EXci5TZ/dCsYktMuD157Ea5sGTuQ==</Q><DP>sR4zka/9nsoAfSraNlP/AgqcoZEQMhH2S3v08T1yikh7Yp6u9xvLijyCRt507a1Z4Qlf4V9RcoIHLQSvCgbn2Q==</DP><DQ>01Bv0RiTrObXbPTbUr+cIyu4W7qnIlOTNG22d3b7S9CULGAxdXz3u/H9SqYfRUGNRGICrQF+AdPFfr3I1IfxwQ==</DQ><InverseQ>dA/bkeMYF3YO1+2ZQftJ5M1tcyBocrYu30q3ztNwqegWwKmBNQc+GxM/OY0ybTQBkuwX3IMo80KUgHj7puOgfw==</InverseQ><D>oAyvAZabPxcVDFPK30bTd/VmFTCuNaWekhvlONiaLvWWDlGHdYvwNOnXoGY12lvMgKuzjMtDgdv+rbtcRTUaPvbW14ZBLQ0FakuNtOwCax8uGGSeFsmBoLXJXTWJdtrlCyV1N581X+7yfeKjE+xXT1KLT0ojUCOqEq/yEQtZMeE=</D></RSAKeyValue>";
	}
}