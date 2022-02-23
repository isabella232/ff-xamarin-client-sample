
using UIKit;

namespace ff_ios_xamarin_client_sample
{
    public class MainScreen : UIViewController
    {
        UITableView table;

        public MainScreen() 
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            table = new UITableView(View.Bounds)
            {
                AutoresizingMask = UIViewAutoresizing.All
            };

			// Add list of environment identifiers
			var tableItems = new string[]{
				"Aptiv",
				"Experian",
				"Fiserv",
				"Harness",
				"Palo Alto Networks"
			};

			var source = new TableSource(tableItems);
			table.Source = source;
			source.Selected += (sender, e) => {

				var vc = new FeatureViewController(e.Content);
				PresentViewController(vc, true, null);
			};

			Add(table);
        }
    }

}
