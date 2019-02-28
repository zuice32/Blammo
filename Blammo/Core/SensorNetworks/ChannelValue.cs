using System;

namespace Agent.Core.SensorNetworks
{
    public class ChannelValue
    {
        private double? _value;
        private int _index;

        public ChannelValue(int index)
        {
            this._index = index;
        }

        public double? Value
        {
            get { return this._value; }
            set
            {
                if (this.Validator != null)
                {
                    if (!this.Validator(value))
                    {
                        throw new InvalidChannelValueException(this._index,value);
                    }
                }
                this._value = value;
            }
        }

        public Func<double?, bool> Validator { get; set; }
 
        public bool ShouldAverage { get; set; }

        public uint AggregateReadingCount { get; set; }
   }
}