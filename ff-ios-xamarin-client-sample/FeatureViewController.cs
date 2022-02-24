using System;
using ff_ios_client_sdk_proxy;
using UIKit;

namespace ff_ios_xamarin_client_sample
{
    public class FeatureViewController : UIViewController
    {
        string identifier;
        CfListener listener;
        UITextView textView;

        public FeatureViewController(string identifier) 
        {
            this.identifier = identifier;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            textView = new UITextView(View.Bounds)
            {
                AutoresizingMask = UIViewAutoresizing.All
            };

            View.BackgroundColor = UIColor.Red;
            Add(textView);

            Start();
        }
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
        }

        private void Start()
        {
            this.listener = new CfListener();

            this.listener.MessageReceived += (sender, m) =>
            {
                textView.Text += $"MESSAGE: domain: {m.Message.Domain} identifier: {m.Message.Identifier} version: {m.Message.Version} event: {m.Message.Event}\n";
            };
            this.listener.EvaluationReceived += (sender, e) =>
            {
                textView.Text += $"EVALUATION: {e.Evaluation} \n";
            };
            this.listener.ErrorReceived += (sender, e) =>
            {
                textView.Text += $"ERROR: {e.Error.Title}. {e.Error.LocalizedMessage} \n";
            };
            this.listener.SystemMessageReceived += (sender, m) =>
            {
                textView.Text += $"Info: {m.Information} \n";
            };

            this.listener.InitializeSuccess += (sender, e) =>
            {
                textView.Text += $"Initialize Success \n";
                CfClientProxy.Shared.BoolVariationWithEvaluationId("bool_flag", Foundation.NSNumber.FromBoolean(false), (evaluation) =>
                {
                    textView.Text += $"BOOL variation {evaluation.Identifier}: {evaluation.Value.BoolValue?.BoolValue} \n";
                });
                CfClientProxy.Shared.StringVariationWithEvaluationId("string_flag", "default_value", (evaluation) =>
                {
                    textView.Text += $"String variation {evaluation.Identifier}: {evaluation.Value.StringValue} \n";
                });
                CfClientProxy.Shared.NumberVariationWithEvaluationId("Number_flag", Foundation.NSNumber.FromInt32(0), (evaluation) =>
                {
                    textView.Text += $"Number variation {evaluation.Identifier}: {evaluation.Value.IntValue?.Int32Value} \n";
                });


                var k = new[] { new Foundation.NSString("key1"), new Foundation.NSString("key2") };
                var v = new Foundation.NSObject[] { new Foundation.NSString("object1"), new Foundation.NSString("object2") };
   
                CfClientProxy.Shared.JsonVariationWithEvaluationId("jsonFlag_flag", new Foundation.NSDictionary<Foundation.NSString, Foundation.NSObject>(k,v), (evaluation) =>
                {
                    textView.Text += $"Number variation {evaluation.Identifier}: {evaluation.Value.StringValue} \n";
                });
            };

            string API_KEY = "27bb973c-5069-4406-9973-941c53478e7e";

            // set endpoint URI
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
                Identifier = this.identifier,
                Name = this.identifier
            };

            // Initialize authentication
            CfClientProxy.Shared.InitializeWithApiKey(API_KEY, config, target);
        }
    }
}

