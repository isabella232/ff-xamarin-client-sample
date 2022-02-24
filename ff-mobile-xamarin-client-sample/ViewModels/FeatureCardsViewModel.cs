using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ff_mobile_xamarin_client_sample.Model;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ff_mobile_xamarin_client_sample.ViewModels
{
    public class FeatureCardsViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		readonly IList<FeatureCardViewModel> source;

        public ObservableCollection<FeatureCardViewModel> FeatureCards { get; private set; }
		public ObservableCollection<FeatureCardViewModel> EnabledCards { get; private set; }

		public FeatureCardsViewModel()
        {
			source = new List<FeatureCardViewModel>();
			BuildFeatureCards();
		}


		private void BuildFeatureCards()
		{
			source.Add(new FeatureCardViewModel(new FeatureCard { FeatureImageName = "cd", FeatureName = "Delivery", FeatureDescription = "", FeatureTrialPeriod = 0, Enabled = true, Available = true, HasRibbon = false }));
			source.Add(new FeatureCardViewModel(new FeatureCard { FeatureImageName = "cv", FeatureName = "Verification", FeatureDescription = "Deploy in peace, verify activities that take place in the system. Identify risk early.", FeatureTrialPeriod = 0, Enabled = false, Available = true, HasRibbon = false }));
			source.Add(new FeatureCardViewModel(new FeatureCard { FeatureImageName = "ci", FeatureName = "Integration", FeatureDescription = "Commit, build, and test your code at a whole new level.", FeatureTrialPeriod = 0, Enabled = false, Available = true, HasRibbon = false }));
			source.Add(new FeatureCardViewModel(new FeatureCard { FeatureImageName = "ce", FeatureName = "Efficiency", FeatureDescription = "Efficiency Spot and quickly debug inefficiencies and optimize them to reduce costs.", FeatureTrialPeriod = 0, Enabled = false, Available = true, HasRibbon = false }));
			source.Add(new FeatureCardViewModel(new FeatureCard { FeatureImageName = "cf", FeatureName = "Features", FeatureDescription = "Decouple release from deployment and rollout features safely and quickly.", FeatureTrialPeriod = 0, Enabled = false, Available = true, HasRibbon = false }));


			FeatureCards = new ObservableCollection<FeatureCardViewModel>(source.Where(e => e.Available && e.HasEnableOption).ToList());
			EnabledCards = new ObservableCollection<FeatureCardViewModel>(source.Where(e => !e.HasEnableOption).ToList());
		}
		public void SetAvailable(bool available, string identifier)
        {
			source.First(f => f.Name == identifier).Available = available;
		}
		public void SetTrial(int trial, string identifier)
        {
			source.First(f => f.Name == identifier).TrialPeriod = trial;
		}
		public void SetDark(bool dark)
		{
			source.ToList().ForEach(f => f.DarkMode = dark);
		}

		public void Update()
        {
			FeatureCards = new ObservableCollection<FeatureCardViewModel>(source.Where(e => e.Available && e.HasEnableOption).ToList());
			NotifyPropertyChanged("FeatureCards");
		}
	}

	public class FeatureCardViewModel : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		private bool darkMode = false;

		private readonly FeatureCard featureCard;
		public FeatureCardViewModel(FeatureCard featureCard)
        {
			this.featureCard = featureCard;

			DarkMode = false;
        }

		public bool ShowNewBadge
		{
			get { return featureCard.HasRibbon; }
			set { featureCard.HasRibbon = value; }
		}
		public bool Available
        {
			get { return featureCard.Available; }
			set
			{
				featureCard.Available = value;
				NotifyPropertyChanged("Available");
			}
		}
		public bool HasEnableOption
		{
			get { return !featureCard.Enabled; }
			set
			{
				featureCard.Enabled = !value;
				NotifyPropertyChanged("Enabled");
			}
		}
		public bool DarkMode
        {
			get { return darkMode; }
			set {
				darkMode = value;
				NotifyPropertyChanged("CardBackgroundColor");
				NotifyPropertyChanged("EnableButtonColor");
				NotifyPropertyChanged("TrialLabelColor");
				NotifyPropertyChanged("FeatureImage");
			}
		}
		public int TrialPeriod
		{
			get { return featureCard.FeatureTrialPeriod; }
			set
			{
				featureCard.FeatureTrialPeriod = value;
				NotifyPropertyChanged("TrialPeriod");
			}
		}
		public string Description { get => featureCard.FeatureDescription; }

		public Color EnableButtonColor
        {
            get { return DarkMode ? Color.FromHex("59B3FF") :  Color.FromHex("327DFF"); }
        }
		public Color TrialLabelColor
        {
			get { return DarkMode ? Color.FromHex("909FAC") : Color.FromHex("6B7B89"); }
		}
		public Color CardBackgroundColor
        {
            get { return DarkMode ? Color.Black : Color.White; }
		}
		public Color DescriptionTextColor
		{
			get { return DarkMode ? Color.White : Color.Gray; }
		}
		public string Name {  get { return featureCard.FeatureName; } }
		public ImageSource NewImage {get { return ImageSource.FromResource("new.png");}}
		public ImageSource FeatureImage
		{
			get
			{
				return ImageSource.FromResource(featureCard.FeatureImageName + (DarkMode ? "_dark" : "") + ".png");
			}
		}

		
	}
}
