using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Xml;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Provides methods to assist in setting underlying binding values.
    /// </summary>
    public static class BindingUtil
    {

        /// <summary>
        /// Sets the value of the given binding.
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="value"></param>
        public static void Set(XFormsBinding binding, object value)
        {
            Contract.Requires<ArgumentNullException>(binding != null);

            if (binding.Type == XmlSchemaConstants.XMLSchema + "boolean")
                binding.SetValue(ToXsdBoolean((bool?)value));
            else if (binding.Type == XmlSchemaConstants.XMLSchema + "date")
                binding.SetValue(ToXsdDate((DateTime?)value));
            else if (binding.Type == XmlSchemaConstants.XMLSchema + "int")
                binding.SetValue(ToXsdDouble((double?)value));
            else if (binding.Type == XmlSchemaConstants.XMLSchema + "integer")
                binding.SetValue(ToXsdDouble((double?)value));
            else if (binding.Type == XmlSchemaConstants.XMLSchema + "long")
                binding.SetValue(ToXsdDouble((double?)value));
            else if (binding.Type == XmlSchemaConstants.XMLSchema + "short")
                binding.SetValue(ToXsdDouble((double?)value));
            else if (binding.Type == XmlSchemaConstants.XMLSchema + "double")
                binding.SetValue(ToXsdDouble((double?)value));
            else
                binding.SetValue((value ?? "").ToString());
        }

        /// <summary>
        /// Gets the value of the given binding.
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static object Get(XFormsBinding binding, Type type)
        {
            Contract.Requires<ArgumentNullException>(binding != null);
            Contract.Requires<ArgumentNullException>(type != null);

            if ((binding == null || string.IsNullOrEmpty(binding.Value)) && type.IsValueType)
                return Activator.CreateInstance(type);
            else if ((binding == null || string.IsNullOrEmpty(binding.Value)))
                return null;
            else if (type == typeof(string))
                return binding.Value;
            else
            {
                // obtain converter and check whether conversion is valid
                var c = TypeDescriptor.GetConverter(type);
                if (c == null || !c.IsValid(binding.Value))
                    throw new Exception();

                // convert to the specified type
                return c.ConvertFromString(binding.Value);
            }
        }

        /// <summary>
        /// Gets the value of the given binding.
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static T Get<T>(XFormsBinding binding)
        {
            return (T)Get(binding, typeof(T));
        }

        public static string ToXsdBoolean(bool? boolean)
        {
            if (boolean != null)
                return XmlConvert.ToString((bool)boolean);
            else
                return "";
        }

        public static bool? FromXsdBoolean(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return XmlConvert.ToBoolean(value);
            else
                return null;
        }

        /// <summary>
        /// Transforms a <see cref="DateTime"/> type to a 'xsd:date' value.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToXsdDate(DateTime? date)
        {
            if (date != null)
                return XmlConvert.ToString((DateTime)date, XmlDateTimeSerializationMode.Unspecified);
            else
                return "";
        }

        /// <summary>
        /// Transforms a 'xsd:date' value to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? FromXsdDate(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Unspecified);
            else
                return null;
        }

        public static string ToXsdDouble(double? value)
        {
            if (value != null)
                return XmlConvert.ToString((double)value);
            else
                return "";
        }

        public static double? FromXsdDouble(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return XmlConvert.ToDouble(value);
            else
                return null;
        }

    }

}
