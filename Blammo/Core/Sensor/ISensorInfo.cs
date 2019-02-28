using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Core.Sensor
{
    public interface ISensorInfo
    {
        Guid SensorId { get; set; }

        Guid HostId { get; set; }

        Guid NetworkId { get; set; }

        string Name { get; }

        sensorType Type { get; set; }

        double[] Coefficients { get; set; }
    }

    public enum sensorType
    {
        Piezo,
        Stick,
        LoadCell,
        Mems,
        Rebar,
        Tiltmeter,
        HighTemp
    }
}
