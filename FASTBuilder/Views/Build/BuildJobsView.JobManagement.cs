using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using FastBuilder.Services;
using FastBuilder.Support;
using FastBuilder.ViewModels.Build;

namespace FastBuilder.Views.Build
{
	partial class BuildJobsView
	{
		// recoreds active (visible) jobs
		private readonly HashSet<BuildJobViewModel> _activeJobs
			= new HashSet<BuildJobViewModel>();

		private void Clear()
		{
			_activeJobs.Clear();
			_visibleCores.Clear();
			_coreTopMap.Clear();

			this.InvalidateVisual();
		}

		private void TryAddJob(BuildJobViewModel job)
		{
			_activeJobs.Add(job);
		}

		private void UpdateJobs()
		{
			var buildViewportService = IoC.Get<IBuildViewportService>();

			_startTimeOffset = buildViewportService.ViewStartTimeOffsetSeconds
							   + (_headerViewWidth - 8) / buildViewportService.Scaling; // minus 8px to make the jobs looks like being covered under the header panel

			_endTimeOffset = buildViewportService.ViewEndTimeOffsetSeconds;
			_wasNowInTimeFrame = _endTimeOffset >= _currentTimeOffset && _startTimeOffset <= _currentTimeOffset;

			var jobs = new HashSet<BuildJobViewModel>(_jobManager.EnumerateJobs(_startTimeOffset, _endTimeOffset, _visibleCores));

			// remove job that are no longer existed in current time frame
			var jobsToRemove = _activeJobs.Where(job => !jobs.Contains(job)).ToList();

			foreach (var job in jobsToRemove)
			{
				_activeJobs.Remove(job);
			}

			// create view for jobs which are new to current time frame
			foreach (var job in jobs)
			{
				this.TryAddJob(job);
			}

			this.InvalidateVisual();
		}

	}
}
