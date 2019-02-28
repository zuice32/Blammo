using System;

namespace Agent.Core.SensorNetworks
{
    public class InvalidChannelValueException : Exception
    {
        public InvalidChannelValueException(int channelIndex, double? value)
            : base(string.Format("Channel value at index {0}:{1} is invalid.", channelIndex,value))
        {
        }
    }
}