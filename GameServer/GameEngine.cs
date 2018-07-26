using GameServer.Game_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameServer.Enums;

namespace GameServer {
    public static class GameEngine {

        private static List<Player> playerList = new List<Player>();
        public static void AddPlayer(Player player) {
            if(playerList.Count > Constants.MAX_PLAYERS) {
                Console.WriteLine("Attempt to add a player beyond MAX_PLAYERS.");
                return;
            }
  
            playerList.Add(player);
        }


        private static List<Card> cardOnBoard1 = new List<Card>();
        private static List<Card> cardOnBoard2 = new List<Card>();

        private static List<Card> cardOnHand1 = new List<Card>();
        private static List<Card> cardOnHand2 = new List<Card>();

        #region Events

        public static void CardPlayed(int cardId, int boardIndex) {

            if (playerOnTurn == 0) {
                ServerSendData.SendCreateCard(1, cardId, CardPlace.ENEMY_BOARD, boardIndex);
            } else {
                ServerSendData.SendCreateCard(0, cardId, CardPlace.ENEMY_BOARD, boardIndex);
            }
        }

        #endregion

        #region Game State

        private static int playerOnTurn = 0;    

        public static void InitializeGame() {
            playerOnTurn = 0;
        }

        #endregion
    }
}
