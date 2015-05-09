using System;

namespace NXKit.Imaging
{

    [Serializable]
    public struct Unit
    {

        /// <summary>
        /// Returns a <see cref="Unit"/> for the given string representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Parse(string value)
        {
            throw new NotImplementedException();
        }

        readonly UnitType unitType;
        readonly double value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="value"></param>
        public Unit(UnitType unitType, double value)
        {
            this.unitType = unitType;
            this.value = value;
        }

        /// <summary>
        /// Gets the type of the <see cref="Unit"/>.
        /// </summary>
        public UnitType UnitType
        {
            get { return unitType; }
        }

        /// <summary>
        /// Gets the value of the <see cref="Unit"/>.
        /// </summary>
        public double Value
        {
            get { return value; }
        }

        public override string ToString()
        {
            switch (unitType)
            {
                case UnitType.Pixels:
                    return value + "px";
                case UnitType.Inches:
                    return value + "in";
            }

            throw new InvalidOperationException();
        }

    }

}
