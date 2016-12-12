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
        parser.WithParsed(
            (Arguments.SetVerb arg) => new Arguments.Parser.Set(arg)
            );
    }
}