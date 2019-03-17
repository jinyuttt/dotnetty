// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Echo.Client
{
    internal class ClientSettings
    {
        public static bool IsSsl { get; internal set; }
        public static string Host { get; internal set; }
        public static int Port { get; internal set; }
        public static int Size { get; internal set; }
    }
}