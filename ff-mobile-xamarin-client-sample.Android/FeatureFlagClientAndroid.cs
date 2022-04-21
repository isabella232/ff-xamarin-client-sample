using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using ff_mobile_xamarin_client_sample;
using IO.Harness.Cfsdk;
using Android.Content;
using IO.Harness.Cfsdk.Cloud.Model;
using IO.Harness.Cfsdk.Cloud.Events;
using Xamarin.Essentials;

[assembly: Dependency(typeof(ff_mobile_xamarin_client_sample.Droid.FeatureFlagClientAndroid))]
namespace ff_mobile_xamarin_client_sample.Droid
{
    public class FeatureFlagClientAndroid : IFeatureFlagClient
    {
        private static readonly string API_KEY = "f544d0a3-6497-4c44-a0cc-c263a446df6b";
        private Context context = Android.App.Application.Context;
        private CfListener listener = null;

        public event NotifyEvaluationChanged EvaluationChanged;
        public event EventHandler<bool> InitializationStatus;
        public event EventHandler<string> Error;

        public void Authenticate(string account)
        {

            listener = new CfListener();

            listener.InitializationStatus += (sender, status) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    InitializationStatus?.Invoke(this, status);
                });
                
            };
            listener.ErrorReceived += (sender, error) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Error?.Invoke(this, error.Message);
                });
            };
            listener.EvaluationReceived += (sender, e) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    EvaluationChanged.Invoke(e.Evaluation.From());
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
                .InvokeName(account)
                .InvokeIdentifier(account);


            CfClient.Instance.Initialize(this.context, API_KEY, configuration, target, listener);
        }
    }
    public static class StringExtension
    {
        public static bool? TryBool(this string s)
        {
            if( bool.TryParse(s, out bool result))
            {
                return result;
            }
            return null;
        }
        public static int? TryInt(this string s)
        {
            if (int.TryParse(s, out int result))
            {
                return result;
            }
            return null;
        }
        public static string TryString(this string s)
        {
            if (TryInt(s) == null && TryBool(s) == null)
            {
                return s;
            }
            return null;
        }
    }

    public static class Extensions
    {
        public static List<Evaluation> From(this IO.Harness.Cfsdk.Cloud.Core.Model.Evaluation[] e)
        {
            return e.ToList().Select(a => new Evaluation
            {
                Flag = a.Flag,
                Identifier = a.Identifier,
                Value = a.Value.From()
            }).ToList();
        }
        public static ValueType From(this Java.Lang.Object value) => new ValueType
        {
            BoolValue = value.ToString().TryBool(),
            IntValue = value.ToString().TryInt(),
            StringValue = value.ToString().TryString()
        };
    }
}
