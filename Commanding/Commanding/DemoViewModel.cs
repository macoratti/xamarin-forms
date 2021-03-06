﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Commanding
{
	public class DemoViewModel : INotifyPropertyChanged
	{
		bool canDownload = true;
		string simulatedDownloadResult;

		public int Number { get; set; }

		public double SquareRootResult { get; private set; }

		public double SquareRootWithParameterResult { get; private set; }

		public string SimulatedDownloadResult {
			get { return simulatedDownloadResult; }
			private set {
				if (simulatedDownloadResult != value) {
					simulatedDownloadResult = value;
					OnPropertyChanged ("SimulatedDownloadResult");
				}
			}
		}

		public ICommand SquareRootCommand { get; private set; }

		public ICommand SquareRootWithParameterCommand { get; private set; }

		public ICommand SimulateDownloadCommand { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public DemoViewModel ()
		{
			Number = 25;
			SquareRootCommand = new Command (CalculateSquareRoot);
			SquareRootWithParameterCommand = new Command<string> (CalculateSquareRoot);
			SimulateDownloadCommand = new Command (async () => await SimulateDownloadAsync (), () => canDownload);
		}

		void CalculateSquareRoot ()
		{
			SquareRootResult = Math.Sqrt (Number);
			OnPropertyChanged ("SquareRootResult");			
		}

		void CalculateSquareRoot (string value)
		{
			double num = Convert.ToDouble (value);
			SquareRootWithParameterResult = Math.Sqrt (num);
			OnPropertyChanged ("SquareRootWithParameterResult");			
		}

		async Task SimulateDownloadAsync ()
		{
			CanInitiateNewDownload (false);
			SimulatedDownloadResult = string.Empty;
			await Task.Run (() => SimulateDownload ());
			SimulatedDownloadResult = "Simulated download complete";
			CanInitiateNewDownload (true);
		}

		void CanInitiateNewDownload (bool value)
		{
			canDownload = value;
			((Command)SimulateDownloadCommand).ChangeCanExecute ();
		}

		void SimulateDownload ()
		{
			// Simulate a 5 second pause
			var endTime = DateTime.Now.AddSeconds (5);
			while (true) {
				if (DateTime.Now >= endTime) {
					break;
				}
			}
		}

		protected virtual void OnPropertyChanged (string propertyName)
		{
			var changed = PropertyChanged;
			if (changed != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
			}
		}
	}
}

