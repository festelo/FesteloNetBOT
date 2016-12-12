using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;
using static FesteloNetBOT.DataBase;

namespace FesteloNetBOT.Arguments
{
    [Verb("work", HelpText = "Run script.")]
    public class WorkVerb
    {
        [Option('m', "manually", HelpText = "Run script for accounts manually.")]
        public IList<int> RemoveArg { get; set; }
    }


    [Verb("show", HelpText = "Show info about script and data.")]
    public class ShowVerb
    {
        [Option('f', "full", HelpText = "Show full information.")]
        public bool FullArg { get; set; }

        [Option('l', "live", HelpText = "Show live information from web.")]
        public IList<int> LiveArg { get; set; }
    }


    [Verb("set", HelpText = "Set the settings and data.")]
    public class SetVerb
    {

        [Option('n', "new", HelpText = "Add new account to DB.")]
        public IList<string> NewArg { get; set; }

        [Option('r', "remove", HelpText = "Remove account from DB.")]
        public IList<int> RemoveArg { get; set; }

        [Option('f', "refresh", HelpText = "Refresh account from web.")]
        public IList<int> RefreshArg { get; set; }

        [Option('w', "withdraw", HelpText = "Change withdraw to SkinX for account.")]
        public IList<int> WithdrawArg { get; set; }

        [Option('m', "main", HelpText = "Change main account to transfer on SkinX.", Default = -1)]
        public int MainArg { get; set; }
    }

    public static class Parser
    {
        public class Set
        {
            SetVerb args;
            public Set(SetVerb args)
            {
                this.args = args;
                if (args.MainArg != -1) Main();
                if (args.NewArg.Count != 0) New();
                if (args.RemoveArg.Count != 0) Remove();
                if (args.RefreshArg.Count != 0) Refresh();
                if (args.WithdrawArg.Count != 0) Withdraw();
            }

            private void Main()
            {
            }

            private void New()
            {
                foreach (string s in args.NewArg)
                {
                    dataBase.Users.Add(new User { Cookie = s, Balance = 0, Time = DateTime.UtcNow.ToString() , Withdraw = false });
                }
                dataBase.SaveChanges();
            }

            private void Remove()
            {
            }

            private void Refresh()
            {
            }

            private void Withdraw()
            {
            }
        }
    }
}
