using System;
using System.Collections.Generic;
using System.Linq;
using EnVarTool.Core;

namespace EnVarTool.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && (args[0] == "--help" || args[0] == "-h"))
            {
                ShowHelp();
                return;
            }

            IList<EnvVarInfo> varsWithScope;
            if (args.Length > 1 && args[0] == "--find")
            {
                varsWithScope = EnvVarService.FindVariablesWithScope(args[1]);
            }
            else if (args.Length > 1 && args[0] == "--regex")
            {
                varsWithScope = EnvVarService.RegexVariablesWithScope(args[1]);
            }
            else
            {
                varsWithScope = EnvVarService.GetAllVariablesWithScope();
            }

            DisplayVariablesWithScope(varsWithScope);
        }

        static void DisplayVariablesWithScope(IList<EnvVarInfo> vars)
        {
            const int pageSize = 20;
            int count = 0;
            var ordered = vars.OrderBy(v => v.Key).ThenBy(v => v.Scope).ToList();
            foreach (var kv in ordered)
            {
                var prefix = count == ordered.Count - 1 ? "\u2514\u2500" : "\u251C\u2500";
                // Color by scope
                switch (kv.Scope)
                {
                    case EnvVarScope.Process:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case EnvVarScope.User:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case EnvVarScope.Machine:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                }
                System.Console.Write(prefix + " " + kv.Key);
                Console.ResetColor();
                System.Console.Write(" = ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.Write(kv.Value);
                Console.ResetColor();
                System.Console.Write(" [");
                switch (kv.Scope)
                {
                    case EnvVarScope.Process:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        System.Console.Write("Process");
                        break;
                    case EnvVarScope.User:
                        Console.ForegroundColor = ConsoleColor.Green;
                        System.Console.Write("User");
                        break;
                    case EnvVarScope.Machine:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        System.Console.Write("Machine");
                        break;
                }
                Console.ResetColor();
                System.Console.WriteLine("]");
                count++;
                if (count % pageSize == 0)
                {
                    System.Console.Write("-- More -- Press Enter to continue, q to quit --");
                    var key = System.Console.ReadKey();
                    System.Console.WriteLine();
                    if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                        break;
                }
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("EnVarTool - Environment Variable Viewer\n");
            Console.WriteLine("Usage:");
            Console.WriteLine("  envartool            Display all environment variables");
            Console.WriteLine("  envartool --find TERM  Filter variables by TERM (case-insensitive)");
            Console.WriteLine("  envartool --regex PAT  Filter variables by regex pattern");
            Console.WriteLine("  envartool --help       Show this help message\n");
            Console.WriteLine("Examples:");
            Console.WriteLine("  envartool --find path");
            Console.WriteLine("  envartool --regex ^USER");
        }
    }
}

