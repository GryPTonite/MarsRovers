using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MarsRover.Lib
{
    public static class Utils
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        internal class StringValueAttribute : Attribute
        {

            #region Properties

            /// <summary>
            /// Holds the stringvalue for a value in an enum.
            /// </summary>
            public string StringValue { get; protected set; }

            #endregion

            #region Constructor

            /// <summary>
            /// Constructor used to init a StringValue Attribute
            /// </summary>
            /// <param name="value"></param>
            public StringValueAttribute(string value)
            {
                this.StringValue = value;
            }

            #endregion

           

        }
       
        /// <summary>
        /// Allow string support using attributes on enums : http://weblogs.asp.net/stefansedich/archive/2008/03/12/enum-with-string-values-in-c.aspx 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
}
