using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HitmulCore
{
    public struct Hitmul
    {
        private readonly string _format;
        private string Format => _format ?? "";

        private readonly object[] _arguments;
        private object[] Arguments => _arguments ?? new object[] { };

        public Hitmul(FormattableString str)
        {
            _format = str.Format;
            _arguments = str.GetArguments();
        }

        private Hitmul(string fmt, object[] args)
        {
            _format = fmt;
            _arguments = args;
        }

        public Hitmul Append(FormattableString str)
        {
            string fmt = Format + "\n" + IncrementFormatArgs(str.Format, str.ArgumentCount, Arguments.Length);
            object[] args = Arguments.Concat(str.GetArguments()).ToArray();
            return new Hitmul(fmt, args);
        }

        public Hitmul AppendRaw(string str, params object[] args)
        {
            string fmt = Format + "\n" + IncrementFormatArgs(str, args.Length, Arguments.Length);
            return new Hitmul(fmt, Arguments.Concat(args).ToArray());
        }

        public static Hitmul Raw(string rawHtml)
        {
            return new Hitmul(rawHtml, new object[] { });
        }

        private static string IncrementFormatArgs(string fmt, int numArgs, int start)
        {
            return string.Format(fmt, Enumerable.Range(start, numArgs).Select(n => "{" + n + "}").ToArray());
        }

        public string ToHtml()
        {
            List<string> formatArgs = new List<string>();

            foreach (object arg in Arguments)
            {
                if (arg is Hitmul inner)
                {
                    string nestedHtml = inner.ToHtml();
                    formatArgs.Add(nestedHtml);
                }
                else if (arg is string str)
                {
                    formatArgs.Add(WebUtility.HtmlEncode(str));
                }
                else if (arg is IFormattable fmt)
                {
                    formatArgs.Add(WebUtility.HtmlEncode(fmt.ToString()));
                }
                else
                {
                    throw new ArgumentException($"cannot convert argument to html: {arg}");
                }
            }

            return string.Format(Format, formatArgs.ToArray());
        }
    }
}
