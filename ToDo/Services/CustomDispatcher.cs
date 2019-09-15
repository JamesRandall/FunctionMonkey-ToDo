using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureFromTheTrenches.Commanding.Abstractions.Model;

namespace ToDo.Services
{
    internal class CustomDispatcher : ICommandDispatcher
    {
        private readonly IFrameworkCommandDispatcher _underlyingDispatcher;
        private readonly ITelemetry _telemetry;

        public CustomDispatcher(
            IFrameworkCommandDispatcher underlyingDispatcher,
            ITelemetry telemetry
            )
        {
            _underlyingDispatcher = underlyingDispatcher;
            _telemetry = telemetry;
        }
        
        public async Task<CommandResult<TResult>> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = new CancellationToken())
        {
            Stopwatch sw = Stopwatch.StartNew();
            CommandResult<TResult> result = await _underlyingDispatcher.DispatchAsync(command, cancellationToken);
            sw.Stop();
            _telemetry.RecordExecutionTime(command.GetType(), sw.ElapsedMilliseconds);
            return result;
        }

        public async Task<CommandResult> DispatchAsync(ICommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            Stopwatch sw = Stopwatch.StartNew();
            CommandResult result = await _underlyingDispatcher.DispatchAsync(command, cancellationToken);
            sw.Stop();
            _telemetry.RecordExecutionTime(command.GetType(), sw.ElapsedMilliseconds);
            return result;
        }

        public ICommandExecuter AssociatedExecuter => null;
    }
}