// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Echo.Server
{
    internal class ServerSettings
    {
        public static bool UseLibuv { get; internal set; }
        public static bool IsSsl { get; internal set; }
        public static int Port { get; internal set; }
    }
}