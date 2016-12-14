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
                List<Tuple<User, User>> toEdit = new List<Tuple<User, User>>();
                try
                {
                    foreach (User u in dataBase.Users)
                    { 
                        if (DateTime.UtcNow >= u.Time)
                        {
                            User newUser = GetReward(u);
                            if (newUser != null)
                            {
                                toEdit.Add(Tuple.Create(u, newUser));
                            }
                        }
                    }
                }
                catch (Exception err) { Console.WriteLine($"Error in sending request.\nCheck connection! MSG: {err.Message}\n"); }
                foreach (var obj in toEdit)
                {
                    obj.Item1.Update(obj.Item2);
                }
                if (toEdit.Count != 0)
                    dataBase.SaveChanges();
                Thread.Sleep(120000);
            }
        }

        public static User GetReward(User user)
        {
            string html = Internet.GetData(Internet.URLs.CSGO500, user.Cookie);
            string token = Internet.Parse.CSRF(html);
            int status = Internet.SendData(Internet.URLs.CSGO500Reward, user.Cookie, token);


            html = Internet.GetData(Internet.URLs.CSGO500, user.Cookie);
            User newUser = Internet.Parse.User(html);

            if (status == 200)
            {
                Console.WriteLine($"Succesfull send request to CSGO500.com. Nick: {user.Name}");
                if (user.Withdraw)
                {
                    status = Internet.SendData(Internet.URLs.CSGO500Transfer, user.Cookie, token,
                        new Dictionary<string, string> { { "value", newUser.Balance.ToString() } });
                    if (status == 200)
                        newUser.Balance = 0;
                    else { Console.WriteLine($"Error in sending request. HTTPERROR: {status}"); }
                }
            }
            else { Console.WriteLine($"Error in sending request. Check cookie. HTTPERROR: {status}"); }
            return newUser;
        }
    }
}
