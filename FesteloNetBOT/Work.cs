using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static FesteloNetBOT.DataBase;

namespace FesteloNetBOT
{
    class Work
    {
        public Work()
        {
            CheckToReward();
        }

        private void CheckToReward()
        {
            while(true)
            {
                bool save = false;
                foreach (User u in dataBase.Users)
                {
                    if (DateTime.UtcNow >= u.Time)
                    {
                        User newUser = GetReward(u);
                        if (newUser != null)
                        {
                            u.Update(newUser);
                        }
                        save = true;
                    }
                }
                if (save)
                    dataBase.SaveChanges();
                Thread.Sleep(2000);
            }
        }

        public static User GetReward(User user)
        {
            string html = Internet.GetData(Internet.URLs.CSGO500, user.Cookie);
            string token = Internet.Parse.CSRF(html);
            int status = Internet.SendData(Internet.URLs.CSGO500Reward, user.Cookie, token);
            if (status == 200)
            {
                Console.WriteLine($"Succesfull send request to CSGO500.com. Nick: {user.Name}");
                html = Internet.GetData(Internet.URLs.CSGO500, user.Cookie);
                User newUser = Internet.Parse.User(html);
                if (user.Withdraw)
                {
                    status = Internet.SendData(Internet.URLs.CSGO500Transfer, user.Cookie, token, 
                        new Dictionary<string, string> { { "value", newUser.Balance.ToString() }});
                    if (status == 200)
                        newUser.Balance = 0;
                }
                return newUser;
            }
            return null;
        }
    }
}
