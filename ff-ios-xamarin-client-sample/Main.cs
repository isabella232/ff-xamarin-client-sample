using System;
using UIKit;
using Foundation;

namespace ff_ios_xamarin_client_sample
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
			UIApplication.Main(args, null, typeof(AppDelegate));
		}
    }

	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		protected UIWindow window;
		protected MainScreen iPhoneHome;

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			window = new UIWindow(UIScreen.MainScreen.Bounds);
			window.MakeKeyAndVisible();

			iPhoneHome = new MainScreen();
			iPhoneHome.View.Frame = new CoreGraphics.CGRect(0
						, UIApplication.SharedApplication.StatusBarFrame.Height
						, UIScreen.MainScreen.ApplicationFrame.Width
						, UIScreen.MainScreen.ApplicationFrame.Height);

			window.RootViewController = iPhoneHome;

			return true;
		}
	}
}
