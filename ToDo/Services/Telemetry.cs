using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace ToDo.Services
{
    internal class Telemetry : ITelemetry
    {
        private readonly TelemetryClient _telemetryClient  = new TelemetryClient();
        
        public void RecordExecutionTime(Type commandType, double executionTime)
        {
            _telemetryClient.TrackEvent(commandType.Name, metrics: new Dictionary<string, double>
            {
                { "executionTimeMs",  executionTime}
            });
        }
    }
}