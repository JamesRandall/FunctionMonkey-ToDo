using System;

namespace ToDo.Services
{
    internal interface ITelemetry
    {
        void RecordExecutionTime(Type commandType, double executionTime);
    }
}