using System.Collections.Generic;


namespace Agent.Core.Misc
{
    public class DataSeriesCore
    {
        public int Id;

        public uint SerialNumber;

        public List<DataPoint> History;

        public DataSeriesCore()
        {
            this.History = new List<DataPoint>();
        }
    }
}
