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
        public IList<int> ManuallyArg { get; set; }
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

        [Option('t', "transfer", HelpText = "Change main account to transfer on SkinX.", Default = -1)]
        public int TransferArg { get; set; }
    }

    public static class Parser
    {
        public class Work
        {
            WorkVerb args;
            public Work(WorkVerb args)
            {
                this.args = args;
                if (args.ManuallyArg.Count != 0) ManuallyArg();
                else new FesteloNetBOT.Work();
            }

            public void ManuallyArg()
            { }
        }
        public class Show
        {
            ShowVerb args;
            public Show(ShowVerb args)
            {
                this.args = args;
                if (args.LiveArg.Count != 0) Live();
                else Main();
            }
            public void Main()
            {
                foreach (User u in dataBase.Users)
                {
                    string message = $"{u.Id}: Name: {u.Name} | Balance: {u.Balance} | Reward Date: {u.Time} UTC";
                    if (args.FullArg) message += $"\nWithdraw: {u.Withdraw} | Cookie: {u.Cookie}\n";
                    Console.WriteLine(message);
                }
                ;
            }
            public void Live()
            {
            }
        }
        public class Set
        {
            SetVerb args;
            public Set(SetVerb args)
            {
                this.args = args;
                if (args.TransferArg != -1) Transfer();
                if (args.NewArg.Count != 0) New();
                if (args.RemoveArg.Count != 0) Remove();
                if (args.RefreshArg.Count != 0) Refresh();
                if (args.WithdrawArg.Count != 0) Withdraw();
            }

            private void Transfer()
            {
            }

            private void New()
            {
                bool save = false;
                foreach (string s in args.NewArg)
                {
                    string html = Internet.GetData(Internet.URLs.CSGO500, s);
                    User usr = Internet.Parse.User(html);
                    if (usr == null)
                    {
                        Console.WriteLine("Error. Cookie: " + s);
                        continue;
                    }
                    else
                    {
                        usr.Cookie = s;
                        dataBase.Users.Add(usr);
                        save = true;
                    }
                }
                if(save)
                    dataBase.SaveChanges();
            }

            private void Remove()
            {
                List<User> toRemove = new List<User>();
                foreach (int i in args.RemoveArg)
                    toRemove.Add(new User { Id = i });
                dataBase.Users.RemoveRange(toRemove);
                int rows = dataBase.SaveChanges();
                Console.WriteLine($"Removed {rows} rows");
            }

            private void Refresh()
            {
            }

            private void Withdraw()
            {
                bool save = false;
                foreach (var u in dataBase.Users)
                {
                    if (args.WithdrawArg.Contains(u.Id))
                    {
                        save = true;
                        u.Withdraw = !u.Withdraw;
                    }
                }
                if (save)
                {
                    int rows = dataBase.SaveChanges();
                    Console.WriteLine($"Changed {rows} rows");
                }
            }
        }
    }
}
