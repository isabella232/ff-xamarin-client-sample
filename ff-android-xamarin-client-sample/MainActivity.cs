using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using ff_mobile_xamarin_client_sample.Droid;
using IO.Harness.Cfsdk;
using IO.Harness.Cfsdk.Cloud.Model;
using Xamarin.Essentials;

namespace ff_xamarin_client_sample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        CfListener listener;
        private static readonly string API_KEY = "78167c7d-329e-4704-9ef9-7f6e456825b6";
        private Context context = Android.App.Application.Context;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            TextView tv = FindViewById<TextView>(Resource.Id.textView1);
            tv.Text = "";

            this.listener = new CfListener();
            listener.InitializationStatus += (sender, status) =>
            {
                bool bRet = CfClient.Instance.BoolVariation("falg1", false);
                string sRet = CfClient.Instance.StringVariation("flag3", "none");
                double fRet = CfClient.Instance.NumberVariation("flag2", 1.0);
                var jRet = CfClient.Instance.JsonVariation("flag4", new Org.Json.JSONObject("{\"key\":\"value\"}"));

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    tv.Text += $"Initialize Success {status}\n";
                    if (status)
                    {
                        tv.Text += $"BOOL variation bool_flag : {bRet} \n";
                        tv.Text += $"String variation string_flag : {sRet} \n";
                        tv.Text += $"Number variation number_flag : {fRet} \n";
                        tv.Text += $"JSON variation json_flag : {jRet} \n";
                    }
      
                });

            };
            listener.ErrorReceived += (sender, error) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    tv.Text += $"ERROR received {error.Message} \n";
                });
            };
            listener.EvaluationReceived += (sender, e) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach( var v in e.Evaluation)
                        {
                            tv.Text += $"Flag received {v.Identifier} {v.Flag} {v.Value} \n";
                        }
                        
                    });
                });
            };
            listener.SystemMessageReceived += (sender, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    tv.Text += $"Message received {m.Information} \n";
                });
            };

            CfConfiguration configuration = new CfConfiguration.Builder()
                .EnableStream(true)
                .EnableAnalytics(true)
                .EventUrl("https://event.feature-flags.uat.harness.io/api/1.0")
                .StreamUrl("https://config.feature-flags.uat.harness.io/api/1.0/stream")
                .BaseUrl("https://config.feature-flags.uat.harness.io/api/1.0")
                .Build();

            Target target = new Target()
                .InvokeName("Harness")
                .InvokeIdentifier("Harness");


            CfClient.Instance.Initialize(this.context, API_KEY, configuration, target, listener);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
