using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Remorse.Protocol
{
    internal class DispatcherData
    {
        [STAThread]
        static void Main(string[] args)
        {
            string argument = Regex.Match(args[0], @"\/\/(.*?)$").Groups[1].Value;
            string state = Regex.Match(argument, @"state=(.*?)&").Groups[1].Value;
            ProcessingArgument(argument, state);
        }
        private static void ProcessingArgument(string argument, string state)
        {
            if (state == "VkAuth")
            {
                string access_token = Regex.Match(argument, @"access_token=(.*?)&").Groups[1].Value;
                string email = Regex.Match(argument, @"email=(.*?)&").Groups[1].Value;
                long userId = long.Parse(Regex.Match(argument, @"user_id=(.*?)&").Groups[1].Value);
                DispatchData(state, access_token, email, userId);
            }
        }

        private static void DispatchData(string state, params object[] data)
        {
            Clipboard.SetData(state, data);
        }
    }
}
