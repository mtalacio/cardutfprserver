using GameServer.Utils;
using System;

namespace GameServer.Network {
    public static class AccountManager {

        public static void CreateAccount(string username, string password) {

            if (MySQL.RecordExists("select * from players where login = '" + username + "'"))
                throw new AccountExistsException();

            Console.WriteLine("Creating new account. Username: " + username + " Password: " + password);
            MySQL.InsertAccount(username, password);
        }

        public static void Login(long playerIndex, string username, string password) {
            try {
                object[,] account = MySQL.GetRows("select * from players where login = '" + username + "'");
                string dbPass = account[2, 0].ToString();

                if (!dbPass.Equals(password))
                    throw new IncorrectPasswordException();

                Player newPlayer = new Player(playerIndex, username);
                GameEngine.AddPlayer(newPlayer);


            } catch (EndOfRecordsetException) {
                throw new AccountDoesNotExist();
            } 
        }

    }
}
