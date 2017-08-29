using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
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
			case "Soccer": return Market.Category.SOCCER;
			case "Tennis": return Market.Category.TENNIS;
			case "Golf": return Market.Category.GOLF;
			case "RugbyUnion": return Market.Category.RUGBYUNION;
			case "Boxing": return Market.Category.BOXING;
			case "HorseRacing": return Market.Category.HORSERACING;
			case "MotorSport": return Market.Category.MOTORSPORT;
			case "RugbyLeague": return Market.Category.RUGBYLEAGUE;
			case "Basketball": return Market.Category.BASKETBALL;
			case "AmericanFootball": return Market.Category.AMERICANFOOTBALL;
			case "Baseball": return Market.Category.BASEBALL;
			case "Hockey": return Market.Category.HOCKEY;
			case "Politics": return Market.Category.POLITICS;
			case "Financial": return Market.Category.FINANCIAL;
			case "Esports": return Market.Category.ESPORTS;
			default: return Market.Category.BITCOIN;
			}
		}

		private async void OnClickBalanceButton(object sender, EventArgs e)
		{
			await Task.Delay(1);
			var balanceLabel = FindViewById<TextView>(Resource.Id.balanceLabel);
			balanceLabel.Text = "TestAccount Balance: 0 mBTC";
		}
	}
}