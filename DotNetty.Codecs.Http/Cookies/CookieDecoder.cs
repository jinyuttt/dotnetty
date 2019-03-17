// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Codecs.Http.Cookies
{
    using DotNetty.Common.Utilities;
    using DotNetty.Logging;
    using static CookieUtil;

    public abstract class CookieDecoder
    {
        static readonly Logger Logger = Logger.Singleton;
        protected readonly bool Strict;

        protected CookieDecoder(bool strict)
        {
            this.Strict = strict;
        }

        protected DefaultCookie InitCookie(string header, int nameBegin, int nameEnd, int valueBegin, int valueEnd)
        {
            if (nameBegin == -1 || nameBegin == nameEnd)
            {
                Logger.Debug("Skipping cookie with null name");
                return null;
            }

            if (valueBegin == -1)
            {
                Logger.Debug("Skipping cookie with null value");
                return null;
            }

            var sequence = new StringCharSequence(header, valueBegin, valueEnd - valueBegin);
            ICharSequence unwrappedValue = UnwrapValue(sequence);
            if (unwrappedValue == null)
            {
                Logger.DebugFormat("Skipping cookie because starting quotes are not properly balanced in '{0}'", sequence);
                return null;
            }

            string name = header.Substring(nameBegin, nameEnd - nameBegin);

            int invalidOctetPos;
            if (this.Strict && (invalidOctetPos = FirstInvalidCookieNameOctet(name)) >= 0)
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.DebugFormat("Skipping cookie because name '{0}' contains invalid char '{1}'", 
                        name, name[invalidOctetPos]);
                }
                return null;
            }

            bool wrap = unwrappedValue.Count != valueEnd - valueBegin;

            if (this.Strict && (invalidOctetPos = FirstInvalidCookieValueOctet(unwrappedValue)) >= 0)
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.DebugFormat("Skipping cookie because value '{0}' contains invalid char '{1}'",
                        unwrappedValue, unwrappedValue[invalidOctetPos]);
                }

                return null;
            }

            var cookie = new DefaultCookie(name, unwrappedValue.ToString());
            cookie.Wrap = wrap;
            return cookie;
        }
    }
}
