using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.Lib
{
    public static class ApiUtils
    {
        public static string ColorDecToHex(int color)
        {
            char[] result = "000000".ToCharArray();

            string value = color.ToString("X");
            for (int i = value.Length - 1; i >= 0; i--)
                result[i] = value[i];

            return "#FF" + new string(result);
        }

        public static int ColorHexToDec(string color)
        {
            char[] fColor = "000000FF".ToCharArray();
            var rColor = color.Reverse().ToArray();
            for (int i = 0; i < rColor.Length; i++)
                fColor[i] = color[i];
            fColor = fColor.Reverse().ToArray()[2..];

            return Convert.ToInt32(new String(fColor), 16);
        }

        public static DateTime? StrToDate(string d)
        {
            if (d.StartsWith('-'))
                d = d[1..];
            int separatorInd = d.IndexOf('-');
            if (separatorInd == -1)
                return null;

            string format = "-MM-dd";
            format = format.PadLeft(format.Length + separatorInd, 'y');
            return DateTime.ParseExact(d, format, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string StrDateToApiDate(string dText, bool after = true)
        {
            if (dText.EndsWith('.'))
                dText = dText[0..^1];

            string[] elems = dText.Split('.');
            if (elems.Length == 0)
                return null;

            string res = "";
            foreach (var elem in elems.Reverse())
            {
                res += elem + "-";
            }
            res = res[0..^1];

            if (!after)
                return "-" + res;
            return res;
        }

        public static string ApiDateToStrDate(string date)
        {
            var sign = "";
            if (date.StartsWith('-'))
            {
                date = date[1..];
                sign = "-";
            }

            string res = "";
            string[] elems = date.Split('-');
            elems[0] = sign + elems[0];
            foreach (var elem in elems.Reverse())
            {
                if (!ContainsOnly(elem, '0'))
                    res += elem + ".";
            }

            return res[0..^1];
        }

        public static bool ContainsOnly(string str, char s)
        {
            foreach (var c in str)
                if (c != s)
                    return false;

            return true;
        }

        public static int ContainsCount(string str, char s)
        {
            int count = 0;
            foreach (var c in str)
                if (c == s)
                    count++;

            return count;
        }
    }
}
