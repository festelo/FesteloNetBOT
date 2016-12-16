using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static FesteloNetBOT.DataBase;

namespace FesteloNetBOT
{
    class Work
    {
        static ILogger log = Log.CreateLogger<Work>();
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
                catch (Exception err) { log.LogWarning($"Error in sending requests to reward. ERROR: {err.Message}"); }
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
            log.LogInformation($"Getting reward. ID: {user.Id}, Nick: {user.Name}");
            string html = Internet.GetData(Internet.URLs.CSGO500, user.Cookie);
            string token = Internet.Parse.CSRF(html);
            int status = Internet.SendData(Internet.URLs.CSGO500Reward, user.Cookie, token);


            html = Internet.GetData(Internet.URLs.CSGO500, user.Cookie);
            User newUser = Internet.Parse.User(html);

            if (status == 200)
            {
                log.LogInformation($"Succesfull getted reward. ID: {user.Id}, Nick: {user.Name}");
                if (user.Withdraw)
                {
                    log.LogInformation($"Sending reward to SkinX. ID: {user.Id}, Nick: {user.Name}");
                    status = Internet.SendData(Internet.URLs.CSGO500Transfer, user.Cookie, token,
                        new Dictionary<string, string> { { "value", newUser.Balance.ToString() } });
                    if (status == 200)
                        newUser.Balance = 0;
                    else { log.LogWarning($"Error in sending reward. HTTPERROR: {status}, ID: {user.Id}, Nick: {user.Name}"); }
                }
            }
            else { log.LogWarning($"Error in getting reward. HTTPERROR: {status}, ID: {user.Id}, Nick: {user.Name}"); }
            return newUser;
        }
    }
}
