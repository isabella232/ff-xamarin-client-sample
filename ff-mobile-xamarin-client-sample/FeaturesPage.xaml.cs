using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using ff_mobile_xamarin_client_sample.Model;
using ff_mobile_xamarin_client_sample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ff_mobile_xamarin_client_sample
{
    public partial class FeaturesPage : ContentPage
    {
        private IFeatureFlagClient client  = DependencyService.Get<IFeatureFlagClient>();

        private FeatureCardsViewModel viewModel = new FeatureCardsViewModel();

        public FeaturesPage(string account)
        { 
            InitializeComponent();


            client.InitializationStatus += (sender, status) =>
            {
                if (status)
                {
                    BindingContext = viewModel;
                    client.EvaluationChanged += Client_EvaluationChanged;
                }

            };

            client.Error += async (sender, error) =>
            {
                await DisplayAlert("ERROR", error, "OK");
            };
            client.Authenticate(account);

        }
        private void UpdateVisibility(bool? enabled, string identifier)
        {
            if (enabled is bool isAvailable)
            {
                viewModel.SetAvailable(isAvailable, identifier);
            }
        }
        private void SetTrial(int? value, string identifier)
        {
            if (value is int trialPeriod)
            {
                viewModel.SetTrial(trialPeriod, identifier);
            }
        }
        private void Client_EvaluationChanged(List<Evaluation> ev)
        {
            foreach(Evaluation e in ev )
            {
                switch(e.Flag)
                {
                    case FeatureIdentifiers.harnessappdemocfribbon:
                        bool? ribbon = e.Value.BoolValue;
                        if( ribbon is bool isRibbon)
                        {
                            viewModel.FeatureCards.First(f => f.Name == "Features").ShowNewBadge = isRibbon;
                        }
                        break;
                    case FeatureIdentifiers.harnessappdemodarkmode:
                        bool? dark = e.Value.BoolValue;
                        if( dark is bool isDark)
                        {
                            Content.BackgroundColor = isDark ? Color.Black : Color.White;
                            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = isDark ? Color.Black : Color.White;
                            ((NavigationPage)Application.Current.MainPage).BarTextColor = isDark ? Color.White  : Color.Black;

                            viewModel.SetDark(isDark);
                        }
                        break;
                    case FeatureIdentifiers.harnessappdemocetriallimit:
                        SetTrial(e.Value.IntValue, "Efficiency");
                        break;
                    case FeatureIdentifiers.harnessappdemocftriallimit:
                        SetTrial(e.Value.IntValue, "Features");
                        break;
                    case FeatureIdentifiers.harnessappdemocitriallimit:
                        SetTrial(e.Value.IntValue, "Integration");
                        break;
                    case FeatureIdentifiers.harnessappdemocvtriallimit:
                        SetTrial(e.Value.IntValue, "Verification");
                        break;
                    case FeatureIdentifiers.harnessappdemoenablecemodule:
                        UpdateVisibility(e.Value.BoolValue, "Efficiency");
                        break;
                    case FeatureIdentifiers.harnessappdemoenablecfmodule:
                        UpdateVisibility(e.Value.BoolValue, "Features");
                        break;
                    case FeatureIdentifiers.harnessappdemoenablecimodule:
                        UpdateVisibility(e.Value.BoolValue, "Integration");
                        break;
                    case FeatureIdentifiers.harnessappdemoenablecvmodule:
                        UpdateVisibility(e.Value.BoolValue, "Verification");
                        break;
                    case FeatureIdentifiers.harnessappdemoenableglobalhelp:
                        break;
                }
                    
            }
            viewModel.Update();
        }
    }
}
