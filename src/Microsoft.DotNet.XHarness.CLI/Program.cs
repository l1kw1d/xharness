// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.DotNet.XHarness.CLI.Android;
using Microsoft.DotNet.XHarness.CLI.Commands;
using Microsoft.DotNet.XHarness.CLI.Commands.iOS;
using Microsoft.DotNet.XHarness.CLI.Commands.Wasm;
using Microsoft.DotNet.XHarness.Common.CLI.Commands;
using Mono.Options;

namespace Microsoft.DotNet.XHarness.CLI
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine($"XHarness command issued: {string.Join(' ', args)}");

            // Root command: will use the platform specific commands to perform the appropriate action.
            var commands = new CommandSet("xharness");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                commands.Add(new iOSCommandSet());
            }

            commands.Add(new AndroidCommandSet());
            commands.Add(new WasmCommandSet());
            commands.Add(new XHarnessHelpCommand());
            commands.Add(new XHarnessVersionCommand());            

            // Mono.Options wouldn't allow "--" and CommandSet parser will temporarily rename it
            int result = commands.Run(args.Select(a => a == "--" ? XHarnessCommand.VerbatimArgumentPlaceholder : a));
            Console.WriteLine($"XHarness exit code: {result}");
            return result;
        }
    }
}
