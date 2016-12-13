using System;
using CommandLine.Text;
using CommandLine;
using Arguments = FesteloNetBOT.Arguments;
using static FesteloNetBOT.DataBase;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        dataBase.Database.EnsureCreated();
        var parser = Parser.Default.ParseArguments<Arguments.WorkVerb, Arguments.SetVerb, Arguments.ShowVerb>(args);
        int exitcode = parser.MapResult(
            (Arguments.WorkVerb arg) => { new Arguments.Parser.Work(arg); return 0; },
            (Arguments.SetVerb arg) => { new Arguments.Parser.Set(arg); return 0; },
            (Arguments.ShowVerb arg) => { new Arguments.Parser.Show(arg); return 0; },
            errors => 1
            );
    }
}