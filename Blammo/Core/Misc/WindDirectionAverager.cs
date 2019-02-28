using System;

namespace Agent.Core.Misc
{
    public static class WindDirectionAverager
    {
        public static double AverageFromArray(double[] directionDegrees)
        {
            double cosineSum = 0.0 ;
            double sineSum = 0.0 ;

            foreach (double angle in directionDegrees)
            {
                if(angle > 360 || angle < 0)
                {
                    throw new ArgumentOutOfRangeException(string.Format("angle={0}",angle),"Angle must be >= 0 and <= 360 degrees");
                }

                cosineSum += Math.Cos(angle * (Math.PI/180));

                sineSum += Math.Sin(angle * (Math.PI/180));
            }
            
            // Cosines are x, Sines are y
            double radians = Math.Atan2(sineSum, cosineSum);

            //radians to degrees
            double degrees = radians * (180 /Math.PI);

            //convert from -PI/2 to PI/2 to 0-360 degrees
            degrees = degrees < 0 ? 360 + degrees : degrees;

            return degrees;

        }
    }
}
