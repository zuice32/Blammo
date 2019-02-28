using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Core.Sensor
{
    public interface ISensorInfoAssignment
    {
        Guid SensorId { get; set; }

        Guid HostId { get; set; }

        Guid NetworkId { get; set; }

        DateTime Start { get; set; }

        DateTime End { get; set; }

        uint? Channel { get; set; }
    }
}
