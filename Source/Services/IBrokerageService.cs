using System;

namespace FastBuild.Dashboard.Services
{
	internal interface IBrokerageService
    {
		string[] WorkerNames { get; }
		string BrokeragePath { get; set; }
		string BrokerageRelativePath { get; set; }

		event EventHandler WorkerCountChanged;
    }
}
