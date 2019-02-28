using System;

namespace Agent.Core.Misc
{
    //server uses SI units only
    //we need to specify enum values because clients maintains a duplicate list
    public enum Units
    {

        None,
        //Temperature
        degreesC,
        degreesF,
        degreesK,
        //Default
        Volts,
        AuxillaryVolts,
        Percent,
        Hertz,
        Digits,
        //Pressure
        psi,
        inchesH2O,
        feetH2O,
        millimetersH2O,
        centimetersH2O,
        metersH2O,
        mbar,
        bar,
        kPa,
        MPa,
        //Load
        pounds,
        kilograms,
        kips,
        tons,
        metricTons,
        kilonewtons,
        //Distance
        inches,
        feet,
        millimeters,
        centimeters,
        meters,
        //Strain
        microstrainC,
        microstrainD,
        microstrainE,
        //Tilt
        Degrees,
        Radians,
        Arcseconds

    }

    public enum UnitCategory
    {
        Default = 0,
        Pressure = 1,
        Load = 2,
        Distance = 3,
        Strain = 4,
        Temperature = 5,
        Tilt = 6
    }

    public static class UnitsExtensions
    {
        public static Units ToUnitValue(string unit)
        {
            switch (unit)
            {
                case "-":
                    return Units.None;
                //temperature
                case "°C":
                    return Units.degreesC;
                case "°F":
                    return Units.degreesF;
                case "°K":
                    return Units.degreesK;
                //Default
                case "V":
                    return Units.Volts;
                case "Volts":
                    return Units.Volts;
                case "AuxV":
                    return Units.AuxillaryVolts;
                case "%":
                    return Units.Percent;
                case "Hz":
                    return Units.Hertz;
                case "Dig":
                    return Units.Digits;
                //Pressure
                case "psi":
                    return Units.psi;
                case "inH2O":
                    return Units.inchesH2O;
                case "ftH2O":
                    return Units.feetH2O;
                case "mmH2O":
                    return Units.millimetersH2O;
                case "cmH2O":
                    return Units.centimetersH2O;
                case "mH2O":
                    return Units.metersH2O;
                case "mbar":
                    return Units.mbar;
                case "bar":
                    return Units.bar;
                case "kPa":
                    return Units.kPa;
                case "MPa":
                    return Units.MPa;
                //Load
                case "Lb":
                    return Units.pounds;
                case "Kg":
                    return Units.kilograms;
                case "Kips":
                    return Units.kips;
                case "tons":
                    return Units.tons;
                case "M/T":
                    return Units.metricTons;
                case "kN":
                    return Units.kilonewtons;
                //Distance
                case "in":
                    return Units.inches;
                case "ft":
                    return Units.feet;
                case "mm":
                    return Units.millimeters;
                case "cm":
                    return Units.centimeters;
                case "m":
                    return Units.meters;
                //Strain
                case "µstrain (C)":
                    return Units.microstrainC;
                case "µstrain (D)":
                    return Units.microstrainD;
                case "µstrain (E)":
                    return Units.microstrainE;
                //Tilt
                case "°":
                    return Units.Degrees;
                case "Radians":
                    return Units.Radians;
                case "Arcseconds":
                    return Units.Arcseconds;


                default:
                    return Units.None;//Enum.GetValues(typeof(Units));
            }
        }

        public static string ToDisplayString(this Units unit)
        {
            switch (unit)
            {
                case Units.None:
                    return "-";
                //temperature
                case Units.degreesC:
                    return "°C";
                case Units.degreesF:
                    return "°F";
                case Units.degreesK:
                    return "°K";
                //Default
                case Units.Volts:
                    return "V";
                case Units.AuxillaryVolts:
                    return "AuxV";
                case Units.Percent:
                    return "%";
                case Units.Hertz:
                    return "Hz";
                case Units.Digits:
                    return "Dig";
                //Pressure
                case Units.psi:
                    return "psi";
                case Units.inchesH2O:
                    return "inH2O";
                case Units.feetH2O:
                    return "ftH2O";
                case Units.millimetersH2O:
                    return "mmH2O";
                case Units.centimetersH2O:
                    return "cmH2O";
                case Units.metersH2O:
                    return "mH2O";
                case Units.mbar:
                    return "mbar";
                case Units.bar:
                    return "bar";
                case Units.kPa:
                    return "kPa";
                case Units.MPa:
                    return "MPa";
                //Load
                case Units.pounds:
                    return "Lb";
                case Units.kilograms:
                    return "Kg";
                case Units.kips:
                    return "Kips";
                case Units.tons:
                    return "tons";
                case Units.metricTons:
                    return "M/T";
                case Units.kilonewtons:
                    return "kN";
                //Distance
                case Units.inches:
                    return "in";
                case Units.feet:
                    return "ft";
                case Units.millimeters:
                    return "mm";
                case Units.centimeters:
                    return "cm";
                case Units.meters:
                    return "m";
                //Strain
                case Units.microstrainC:
                    return "µstrain (C)";
                case Units.microstrainD:
                    return "µstrain (D)";
                case Units.microstrainE:
                    return "µstrain (E)";
                //Tilt
                case Units.Degrees:
                    return "°";
                case Units.Radians:
                    return "Radians";
                case Units.Arcseconds:
                    return "Arcseconds";

                default:
                    return Enum.GetName(typeof(Units), unit);
            }
        }

        //public static double? ConvertTo(this double? valueToConvert, Units sourceUnits, Units targetUnits)
        //{
        //    switch (sourceUnits)
        //    {
        //        //
        //        case Units.degreesC:

        //            switch (targetUnits)
        //            {
        //                case Units.degreesF:
        //                    return (valueToConvert*1.8) + 32;

        //                default:
        //                    throw new NotImplementedException();
        //            }

        //        //
        //        case Units.psi:

        //            switch (targetUnits)
        //            {
        //                case Units.inchesH2O:
        //                    return (valueToConvert * 27.730);
        //                case Units.feetH2O:
        //                    return (valueToConvert * 2.3108);
        //                case Units.millimetersH2O:
        //                    return (valueToConvert * 704.32);
        //                case Units.centimetersH2O:
        //                    return (valueToConvert * 70.432);
        //                case Units.metersH2O:
        //                    return (valueToConvert * 0.70432);
        //                case Units.mbar:
        //                    return (valueToConvert * 68.947);
        //                case Units.bar:
        //                    return (valueToConvert * 0.068947);
        //                case Units.kPa:
        //                    return (valueToConvert * 0.068947);

        //                default:
        //                    throw new NotImplementedException();
        //            }

        //        //
        //        case Units.pounds:

        //            switch (targetUnits)
        //            {
        //                case Units.kilograms:
        //                    return (valueToConvert * 0.4535);
        //                case Units.kips:
        //                    return (valueToConvert * 0.001);
        //                case Units.tons:
        //                    return (valueToConvert * 0.0005);
        //                case Units.metricTons:
        //                    return (valueToConvert * 0.0004535);
        //                case Units.kilonewtons:
        //                    return (valueToConvert * 0.00444898);

        //                default:
        //                    throw new NotImplementedException();
        //            }

        //        //
        //        case Units.inches:

        //            switch (targetUnits)
        //            {
        //                case Units.feet:
        //                    return (valueToConvert * 0.0833);
        //                case Units.millimeters:
        //                    return (valueToConvert * 25.4);
        //                case Units.centimeters:
        //                    return (valueToConvert * 2.54);
        //                case Units.meters:
        //                    return (valueToConvert * 0.0254);

        //                default:
        //                    throw new NotImplementedException();
        //            }

        //        //
        //        case Units.Digits:

        //            switch (targetUnits)
        //            {
        //                case Units.microstrainC:
        //                    return (valueToConvert * 4.062);
        //                case Units.microstrainD:
        //                    return (valueToConvert * 3.304);
        //                case Units.microstrainE:
        //                    return (valueToConvert * 0.39102);

        //                default:
        //                    throw new NotImplementedException();
        //            }

        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        public static string Conversion(Units fromUnits, Units targetUnits)
        {
            string equation = "";

            switch (fromUnits)
            {
                case Units.Volts:
                    break;

                case Units.AuxillaryVolts:
                    break;

                case Units.Hertz:
                    break;

                case Units.Percent:
                    break;

                case Units.degreesC:

                    switch (targetUnits)
                    {
                        case Units.degreesK:
                            equation = "°C + 273.15";
                            break;
                        case Units.degreesF:
                            equation = "(°C * 1.8) + 32";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case Units.degreesF:

                    switch (targetUnits)
                    {
                        case Units.degreesK:
                            equation = "(°F + 459.67) * .6";
                            break;
                        case Units.degreesC:
                            equation = "(°F / 1.8) - 32";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case Units.degreesK:

                    switch (targetUnits)
                    {
                        case Units.degreesF:
                            equation = "(°K * 1.8) + 32";
                            break;
                        case Units.degreesC:
                            equation = "(°K * 1.8) + 32";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case Units.psi:

                    switch (targetUnits)
                    {
                        case Units.inchesH2O:
                            equation = "psi * 27.730";
                            break;
                        case Units.feetH2O:
                            equation = "psi * 2.3108";
                            break;
                        case Units.millimetersH2O:
                            equation = "psi * 704.32";
                            break;
                        case Units.centimetersH2O:
                            equation = "psi * 70.432";
                            break;
                        case Units.metersH2O:
                            equation = "psi * 0.70432";
                            break;
                        case Units.mbar:
                            equation = "psi * 68.947";
                            break;
                        case Units.bar:
                            equation = "psi * 0.068947";
                            break;
                        case Units.kPa:
                            equation = "psi * 0.00068947";
                            break;
                        case Units.MPa:
                            equation = "psi * 0.00689476";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case Units.inchesH2O:

                    switch (targetUnits)
                    {
                        case Units.psi:
                            equation = "inchesH2O * 0.03612629";
                            break;
                        case Units.feetH2O:
                            equation = "inchesH2O * 0.08333344";
                            break;
                        case Units.millimetersH2O:
                            equation = "inchesH2O * 25.3999947";
                            break;
                        case Units.centimetersH2O:
                            equation = "inchesH2O * 2.539999983";
                            break;
                        case Units.metersH2O:
                            equation = "inchesH2O * 0.02540650";
                            break;
                        case Units.mbar:
                            equation = "inchesH2O * 2.4884";
                            break;
                        case Units.bar:
                            equation = "inchesH2O * 0.0024884";
                            break;
                        case Units.kPa:
                            equation = "inchesH2O * 0.24884";
                            break;
                        case Units.MPa:
                            equation = "inchesH2O * 0.00024884";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case Units.feetH2O:

                    switch (targetUnits)
                    {
                        case Units.psi:
                            equation = "feetH2O * 0.43352750";
                            break;
                        case Units.inchesH2O:
                            equation = "feetH2O * 12";
                            break;
                        case Units.millimetersH2O:
                            equation = "feetH2O * 304.8";
                            break;
                        case Units.centimetersH2O:
                            equation = "feetH2O * 30.48";
                            break;
                        case Units.metersH2O:
                            equation = "feetH2O * 0.3048";
                            break;
                        case Units.mbar:
                            equation = "feetH2O * 29.890669";
                            break;
                        case Units.bar:
                            equation = "feetH2O * 0.029890669";
                            break;
                        case Units.kPa:
                            equation = "feetH2O * 22.98607999";
                            break;
                        case Units.MPa:
                            equation = "feetH2O * 0.00298607";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case Units.millimetersH2O:

                    switch (targetUnits)
                    {
                        case Units.psi:
                            equation = "millimetersH2O * 0.00142233";
                            break;
                        case Units.inchesH2O:
                            equation = "millimetersH2O * 0.0393701";
                            break;
                        case Units.feetH2O:
                            equation = "millimetersH2O * 0.00328084";
                            break;
                        case Units.centimetersH2O:
                            equation = "millimetersH2O * 0.1";
                            break;
                        case Units.metersH2O:
                            equation = "millimetersH2O * 0.001";
                            break;
                        case Units.mbar:
                            equation = "millimetersH2O * 0.0980665";
                            break;
                        case Units.bar:
                            equation = "millimetersH2O * 0.0000980665";
                            break;
                        case Units.kPa:
                            equation = "millimetersH2O * 0.00980665";
                            break;
                        case Units.MPa:
                            equation = "millimetersH2O * 0.00000980665";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case Units.centimetersH2O:

                    switch (targetUnits)
                    {
                        case Units.psi:
                            equation = "centimetersH2O * 0.01422295";
                            break;
                        case Units.inchesH2O:
                            equation = "centimetersH2O * 0.393701";
                            break;
                        case Units.feetH2O:
                            equation = "centimetersH2O * 0.0328084";
                            break;
                        case Units.millimetersH2O:
                            equation = "centimetersH2O * 10";
                            break;
                        case Units.metersH2O:
                            equation = "centimetersH2O * 0.01";
                            break;
                        case Units.mbar:
                            equation = "centimetersH2O * 0.980638";
                            break;
                        case Units.bar:
                            equation = "centimetersH2O * 0.000980638";
                            break;
                        case Units.kPa:
                            equation = "millimetersH2O * 0.0980638";
                            break;
                        case Units.MPa:
                            equation = "millimetersH2O * 0.00009806";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case Units.pounds:
                    return "";

                case Units.kilograms:
                    return "(Lbs * 0.4535)";

                case Units.kips:
                    return "(Lbs * 0.001)";

                case Units.tons:
                    return "(Lbs * 0.0005)";

                case Units.metricTons:
                    return "(Lbs * 0.0004535)";

                case Units.kilonewtons:
                    return "(Lbs * 0.00444898)";

                case Units.inches:
                    return "";

                case Units.feet:
                    return "(in * 0.0833)";

                case Units.millimeters:
                    return "(in * 25.4)";

                case Units.centimeters:
                    return "(in * 2.54)";

                case Units.meters:
                    return "(in * 0.0254)";

                case Units.Digits:
                    return "";

                case Units.microstrainC:
                    return "(Digits * 4.062)";

                case Units.microstrainD:
                    return "(Digits * 3.304)";

                case Units.microstrainE:
                    return "(Digits * 0.39102)";


                default:
                    break;
            }

            return equation;
        }
    }
}