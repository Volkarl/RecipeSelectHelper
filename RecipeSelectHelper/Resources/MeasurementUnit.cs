using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Resources
{
    public static class MeasurementUnit
    {
        public enum Unit 
        {
            Gram, KiloGram, MilliLiter, DeciLiter, Liter, Cup, TableSpoon, TeaSpoon, Pinch, Packet
        }

        public static double GetConversionRate(Unit convertFrom, Unit convertTo)
        {
            return ConvertToGram(convertFrom) / ConvertToGram(convertTo);
        }

        public static double ConvertToGram(Unit unit)
        {
            switch (unit)
            {
                case Unit.Gram:
                    return 1;
                case Unit.KiloGram:
                    return 1000;
                case Unit.MilliLiter:
                    return 1;
                case Unit.DeciLiter:
                    return 100;
                case Unit.Liter:
                    return 1000;
                case Unit.Cup:
                    return ConvertToGram(Unit.DeciLiter) * 2;
                case Unit.TableSpoon:
                    return ConvertToGram(Unit.TeaSpoon) * 3;
                case Unit.TeaSpoon:
                    return 3;
                case Unit.Pinch:
                    return 1;
                case Unit.Packet:
                    return 24;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }
    }
}
