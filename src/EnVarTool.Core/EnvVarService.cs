using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace EnVarTool.Core
{
    public enum EnvVarScope
    {
        Process,
        User,
        Machine
    }

    public class EnvVarInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public EnvVarScope Scope { get; set; }
    }

    public static class EnvVarService
    {
        public static IDictionary<string, string> GetAllVariables()
        {
            return Environment.GetEnvironmentVariables()
                .Cast<System.Collections.DictionaryEntry>()
                .ToDictionary(e => (string)e.Key, e => (string)e.Value);
        }

        public static IDictionary<string, string> FindVariables(string search)
        {
            var all = GetAllVariables();
            return all.Where(kv => kv.Key.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                   kv.Value.Contains(search, StringComparison.OrdinalIgnoreCase))
                      .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static IDictionary<string, string> RegexVariables(string pattern)
        {
            var all = GetAllVariables();
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return all.Where(kv => regex.IsMatch(kv.Key) || regex.IsMatch(kv.Value))
                      .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static IList<EnvVarInfo> GetAllVariablesWithScope()
        {
            var result = new List<EnvVarInfo>();
            var processVars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            var userVars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);
            var machineVars = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);

            foreach (System.Collections.DictionaryEntry entry in processVars)
            {
                result.Add(new EnvVarInfo { Key = (string)entry.Key, Value = (string)entry.Value, Scope = EnvVarScope.Process });
            }
            foreach (System.Collections.DictionaryEntry entry in userVars)
            {
                result.Add(new EnvVarInfo { Key = (string)entry.Key, Value = (string)entry.Value, Scope = EnvVarScope.User });
            }
            foreach (System.Collections.DictionaryEntry entry in machineVars)
            {
                result.Add(new EnvVarInfo { Key = (string)entry.Key, Value = (string)entry.Value, Scope = EnvVarScope.Machine });
            }
            return result;
        }

        public static IList<EnvVarInfo> FindVariablesWithScope(string search)
        {
            var all = GetAllVariablesWithScope();
            return all.Where(kv => kv.Key.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                   kv.Value.Contains(search, StringComparison.OrdinalIgnoreCase))
                      .ToList();
        }

        public static IList<EnvVarInfo> RegexVariablesWithScope(string pattern)
        {
            var all = GetAllVariablesWithScope();
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return all.Where(kv => regex.IsMatch(kv.Key) || regex.IsMatch(kv.Value))
                      .ToList();
        }
    }
}
