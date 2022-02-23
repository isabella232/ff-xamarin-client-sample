using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using ff_ios_client_sdk_proxy;
using ff_mobile_xamarin_client_sample;
using Foundation;

[assembly: Dependency(typeof(ff_mobile_xamarin_client_sample.iOS.FeatureFlagClientiOS))]
namespace ff_mobile_xamarin_client_sample.iOS
{
    public class FeatureFlagClientiOS : IFeatureFlagClient
    {
        private static readonly string API_KEY = "27bb973c-5069-4406-9973-941c53478e7e";
        private CfListener listener = null;

        public event NotifyEvaluationChanged EvaluationChanged;
        public event EventHandler<bool> InitializationStatus;
        public event EventHandler<string> Error;
        public bool IsInitialized { get; set; }

        public void Authenticate(string account)
        {
            // subscribe to events
            listener = new CfListener();
            IsInitialized = false;

            listener.PollingReceived += (sender, e) =>
            {
                EvaluationChanged.Invoke(e.Evaluation.From());
            };

            listener.EvaluationReceived += (sender, e) =>
            {
                EvaluationChanged.Invoke(e.Evaluation.From());
            };
            listener.MessageReceived += (sender, m) =>
            {
               Console.WriteLine($"MESSAGE: domain: {m.Message.Domain} identifier: {m.Message.Identifier} version: {m.Message.Version} event: {m.Message.Event}");
            };
            listener.ErrorReceived += (sender, e) =>
            {
                Console.WriteLine($"ERROR: {e.Error.Title}. {e.Error.LocalizedMessage}");
                if( !IsInitialized)
                {
                    InitializationStatus?.Invoke(this, false);
                }
                Error?.Invoke(this, e.Error.Title ?? "");
            };
            listener.SystemMessageReceived += (sender, m) =>
            {
                Console.WriteLine($"Info: {m.Information}");
            };
            listener.InitializeSuccess += (sender, m) =>
            {
                IsInitialized = true;
                InitializationStatus?.Invoke(this, true);
            };


            //create configuration
            var config = new CfConfigurationProxy
            {
                StreamEnabled = true,
                AnalyticsEnabled = true,
                ConfigUrl = "https://config.feature-flags.uat.harness.io/api/1.0",
                EventUrl = "https://event.feature-flags.uat.harness.io/api/1.0",
                StreamUrl = "https://config.feature-flags.uat.harness.io/api/1.0/stream"
            };

            // set selected identifer
            var target = new CfTargetProxy
            {
                Identifier = account,
                Name = account
            };

            // Initialize authentication
            CfClientProxy.Shared.InitializeWithApiKey(API_KEY, config, target);
        }
    }

    public static class Extensions
    {
        public static List<Evaluation> From(this CxEvaluation[] e)
        {
            return e.ToList().Select(a => new Evaluation
            {
                Flag = a.Flag,
                Identifier = a.Identifier,
                Value = a.Value.From()
            }).ToList();
        }
        public static ValueType From(this CxValue value) => new ValueType
        {
            BoolValue = value.BoolValue?.BoolValue,
            IntValue = value.IntValue?.Int32Value,
            StringValue = value.StringValue,
            ObjectValue = value.ObjectValue.Convert()
          
        };
        private static Dictionary<string, ValueType> Convert(this NSDictionary nativeDict)
        {
            return nativeDict == null ?  new Dictionary<string, ValueType>() : nativeDict.ToDictionary<KeyValuePair<NSObject, NSObject>, string, ValueType>(item => (NSString)item.Key, item => ((CxValue)item.Value).From());
        }
    }
}
