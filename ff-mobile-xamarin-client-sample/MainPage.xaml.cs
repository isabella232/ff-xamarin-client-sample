using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ff_mobile_xamarin_client_sample;
using Xamarin.Forms;

namespace ff_mobile_xamarin_client_sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {

            var layout = new StackLayout()
            {
                Padding = new Thickness(30),
                Spacing = 20
            };


            Model.Accounts.Names.ForEach(e =>
            {
                var btn = new Button
                {
                    Text = e,
                    BackgroundColor = Color.DeepSkyBlue,
                    Font = Font.SystemFontOfSize(18, FontAttributes.Bold),
                    TextColor = Color.White
                };
                btn.Clicked += Btn_Clicked;
                layout.Children.Add(btn);
            });

            this.Content = layout;

        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FeaturesPage( (sender as Button).Text ));
        }
    }
}
