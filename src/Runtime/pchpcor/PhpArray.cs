﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pchp.Core
{
    [DebuggerDisplay("array(length = {Count})", Type = PhpArray.PhpTypeName)]
    public class PhpArray : PhpHashtable, IPhpConvertible
    {
        /// <summary>
        /// Used in all PHP functions determining the type name. (var_dump, ...)
        /// </summary>
        public const string PhpTypeName = "array";

        /// <summary>
        /// Used in print_r function.
        /// </summary>
        public const string PrintablePhpTypeName = "Array";

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="PhpArray"/> with specified capacities for integer and string keys respectively.
        /// </summary>
        public PhpArray() : base() { }

        /// <summary>
        /// Creates a new instance of <see cref="PhpArray"/> with specified capacities for integer and string keys respectively.
        /// </summary>
        /// <param name="capacity"></param>
        public PhpArray(int capacity) : base(capacity) { }

        /// <summary>
        /// Creates a new instance of <see cref="PhpArray"/> with specified capacities for integer and string keys respectively.
        /// </summary>
        /// <param name="intCapacity"></param>
        /// <param name="stringCapacity"></param>
        public PhpArray(int intCapacity, int stringCapacity) : base(intCapacity + stringCapacity) { }

        /// <summary>
        /// Creates a new instance of <see cref="PhpArray"/> initialized with all values from <see cref="System.Array"/>.
        /// </summary>
        /// <param name="values"></param>
        public PhpArray(Array values) : base(values) { }

        /// <summary>
        /// Creates a new instance of <see cref="PhpArray"/> initialized with a portion of <see cref="System.Array"/>.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        public PhpArray(Array values, int index, int length) : base(values, index, length) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhpArray"/> class filled by values from specified array.
        /// </summary>
        /// <param name="values">An array of values to be added to the table.</param>
        /// <param name="start">An index of the first item from <paramref name="values"/> to add.</param>
        /// <param name="length">A number of items to add.</param>
        /// <param name="value">A value to be filtered.</param>
        /// <param name="doFilter">Wheter to add all items but <paramref name="value"/> (<b>true</b>) or 
        /// all items with the value <paramref name="value"/> (<b>false</b>).</param>
        public PhpArray(int[] values, int start, int length, int value, bool doFilter)
            : base(values, start, length, value, doFilter) { }

        /// <summary>
        /// Creates a new instance of <see cref="PhpArray"/> filled by data from an enumerator.
        /// </summary>
        /// <param name="data">The enumerator containing values added to the new instance.</param>
        public PhpArray(IEnumerable data)
            : base((data is ICollection) ? ((ICollection)data).Count : 0)
        {
            if (data != null)
            {
                foreach (object value in data)
                {
                    AddToEnd(PhpValue.FromClr(value));
                }
            }
        }

        /// <summary>
        /// Copy constructor. Creates <see cref="PhpArray"/> that shares internal data table with another <see cref="PhpArray"/>.
        /// </summary>
        /// <param name="array">Table to be shared.</param>
        /// <param name="preserveMaxInt">True to copy the <see cref="PhpHashtable.MaxIntegerKey"/> from <paramref name="array"/>.
        /// Otherwise the value will be recomputed when needed. See http://phalanger.codeplex.com/workitem/31484 for more details.</param>
        public PhpArray(PhpArray/*!*/array, bool preserveMaxInt)
            : base(array, preserveMaxInt)
        {

        }

        /// <summary>
        /// Creates an instance of <see cref="PhpArray"/> filled by given values.
        /// </summary>
        /// <param name="values">Values to be added to the new instance. 
        /// Keys will correspond order of values in the array.</param>
        public static PhpArray New(params object[] values)
        {
            PhpArray result = new PhpArray(values.Length, 0);
            foreach (object value in values)
                result.Add(value);
            return result;
        }

        /// <summary>
        /// Creates an instance of <see cref="PhpArray"/> filled by given entries.
        /// </summary>
        /// <param name="keysValues">Keys and values (alternating) or values only.</param>
        /// <remarks>If the length of <paramref name="keysValues"/> is odd then its last item is added without a key.</remarks>
        public static PhpArray Keyed(params object[] keysValues)
        {
            PhpArray result = new PhpArray();
            int length = keysValues.Length;
            int remainder = length % 2;

            for (int i = 0; i < length - remainder; i += 2)
                result.Add(keysValues[i], keysValues[i + 1]);

            if (remainder > 0)
                result.Add(keysValues[length - 1]);

            return result;
        }

        /// <summary>
        /// Cast given <paramref name="arrayobj"/> to <see cref="PhpArray"/>. Depends on current implementation of <see cref="PhpArray"/>.
        /// </summary>
        /// <param name="arrayobj"><see cref="Object"/> to be casted to <see cref="PhpArray"/>.</param>
        /// <returns>Casted object or <c>null</c>.</returns>
        public static PhpArray AsPhpArray(object arrayobj)
        {
            return arrayobj as PhpArray;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Creates copy of this instance using shared underlaying hashtable.
        /// </summary>
        public PhpArray DeepCopy() => new PhpArray(this);

        #endregion

        #region IPhpConvertible

        public PhpTypeCode TypeCode => PhpTypeCode.PhpArray;

        public double ToDouble() => Count;

        public long ToLong() => Count;

        public bool ToBoolean() => Count != 0;

        public Convert.NumberInfo ToNumber(out PhpNumber number)
        {
            number = PhpNumber.Create(Count);
            return Convert.NumberInfo.IsPhpArray | Convert.NumberInfo.LongInteger;
        }

        public string ToString(Context ctx)
        {
            return PrintablePhpTypeName;
        }

        public string ToStringOrThrow(Context ctx)
        {
            //PhpException.Throw(PhpError.Notice, CoreResources.GetString("array_to_string_conversion"));
            
            return ToString(ctx);
        }

        public object ToClass(Context ctx)
        {
            return new stdClass()
            {
                // TODO: RuntimeFields
            };
        }

        #endregion
    }
}