using Agent.Core.Services;

namespace Agent.Core.SensorNetworks
{
    public interface ISensorRecordingService : IApplicationService
    {
        int RecordIntervalSetting { get; }

        void ResetRecordInterval(int newTimerIntervalMilliseconds);

        bool IsEnabled { get; }
    }
}