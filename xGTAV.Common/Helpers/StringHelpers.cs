/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xGTAV.Common.Helpers
{
    public static partial class StringHelpers
    {
        /// <summary>
        /// Checks if string is an integer
        /// </summary>
        /// <returns>if int</returns>
        public static bool IsInt(this string str)
        {
            int testNum = 0;
            return int.TryParse(str, out testNum);
        }
        /// <summary>
        /// Checks if string is a float
        /// </summary>
        /// <returns>is float</returns>
        public static bool IsFloat(this string str)
        {
            float flOut = 0.0F;
            return float.TryParse(str, out flOut);
        }
        /// <summary>
        /// Randomizes a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="size">size of outputstring</param>
        /// <returns>randomized string</returns>
        public static string RandomizeString(this string str, int size)
        {
            string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] buffer = new char[size];
            Random _rng = new Random();
            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }
        /// <summary>
        /// Makes a packaged ID
        /// </summary>
        /// <param name="id">string to package into an ID</param>
        /// <returns>integer of the ID</returns>
        public static int MakeID(this string id)
        {
            if (id.Length != 4)
                throw new ArgumentException("ID must be exactly 4 characters in length");
            byte[] idBytes = ASCIIEncoding.ASCII.GetBytes(id);
            return (idBytes[3] << 24) | (idBytes[2] << 16) | (idBytes[1] << 8) | idBytes[0];
        }
    }
}
