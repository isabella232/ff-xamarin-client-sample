using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ff_mobile_xamarin_client_sample.Model
{
	public class FeatureCard
	{
		public string FeatureImageName { get; set; }
		public string FeatureName { get; set; }
		public string FeatureDescription { get; set; }
		public int FeatureTrialPeriod { get; set;  }
		public bool Enabled { get; set; }
		public bool Available { get; set; }
		public bool HasRibbon { get; set; }
	}


	public static class FeatureIdentifiers
    {
		public const string harnessappdemocfribbon = "harnessappdemocfribbon";
		public const string harnessappdemodarkmode = "harnessappdemodarkmode";
		public const string harnessappdemocetriallimit = "harnessappdemocetriallimit";
		public const string harnessappdemocftriallimit = "harnessappdemocftriallimit";
		public const string harnessappdemocitriallimit = "harnessappdemocitriallimit";
		public const string harnessappdemocvtriallimit = "harnessappdemocvtriallimit";
		public const string harnessappdemoenablecemodule = "harnessappdemoenablecemodule";
		public const string harnessappdemoenablecfmodule = "harnessappdemoenablecfmodule";
		public const string harnessappdemoenablecimodule = "harnessappdemoenablecimodule";
		public const string harnessappdemoenablecvmodule = "harnessappdemoenablecvmodule";
		public const string harnessappdemoenableglobalhelp = "harnessappdemoenableglobalhelp";
	}
}
