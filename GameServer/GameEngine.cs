using GameServer.Game_Objects;
using GameServer.Network;
using GameServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static GameServer.Enums;

namespace GameServer {
    public static class GameEngine {

        private static readonly List<Player> PlayerList = new List<Player>();
        public static void AddPlayer(Player player) {
            if(PlayerList.Count > Constants.MAX_PLAYERS) {
                Console.WriteLine("Attempt to add a player beyond MAX_PLAYERS.");
                return;
            }
  
            PlayerList.Add(player);
        }

        // Not yet implemented
        //private static List<Card> _cardsOnBoard1 = new List<Card>();
        //private static List<Card> _cardsOnBoard2 = new List<Card>();

        private static List<Card> _cardsOnHand1 = new List<Card>();
        private static List<Card> _cardsOnHand2 = new List<Card>();

        private static List<Card> _cardsOnDeck1;
        private static List<Card> _cardsOnDeck2;

        #region Events

        public static void CardPlayed(long playerIndex, int cardId, int boardIndex)
        {
            if (playerIndex != _playerOnTurn)
                throw new IllegalMessageReceivedException("Carta Jogada por jogador não permitido.");

            ServerSendData.SendCreateCard(_playerOnTurn == 0 ? 1 : 0, cardId, CardPlace.ENEMY_BOARD, boardIndex);
        }

        public static void TurnEnded(long index) {

            if(index != _playerOnTurn) {
                throw new IllegalMessageReceivedException("EndTurn recebido por jogador não permitido.");
            }

            _playerOnTurn = _playerOnTurn == 0 ? 1 : 0;

            Console.WriteLine("Starting Player " + _playerOnTurn + " turn.");

            ServerSendData.SendSetTurn(0, _playerOnTurn == 0 ? 1 : 0);
            ServerSendData.SendSetTurn(1, _playerOnTurn == 0 ? 0 : 1);
        }

        public static void DrawCardTo(long index, long qtd) {
            //TODO: Cards are going to be server-identifier, redo code.

            if (index == 0) { 
                if (_cardsOnDeck1.Count == 0)
                    return;

                for (int i = 0; i < qtd; i++) {
                    Card toDrawn = _cardsOnDeck1[0];
                    ServerSendData.SendCreateCard(0, toDrawn.CardId, CardPlace.HAND, -1);
                    _cardsOnDeck1.Remove(toDrawn);
                    _cardsOnHand1.Add(toDrawn);
                }
            } else {
                if (_cardsOnDeck2.Count == 0)
                    return;

                for (int i = 0; i < qtd; i++) {
                    Card toDrawn = _cardsOnDeck2[0];
                    ServerSendData.SendCreateCard(1, toDrawn.CardId, CardPlace.HAND, -1);
                    _cardsOnDeck2.Remove(toDrawn);
                    _cardsOnHand2.Add(toDrawn);
                }
            }
        }

        #endregion

        #region Game State

        private static int _playerOnTurn;    

        public static void InitializeGame() {
            _playerOnTurn = 0;
            ServerSendData.SendSetTurn(0, 1);
            ServerSendData.SendSetTurn(1, 0);

            //FIXME: Temp code for testing purposes
            _cardsOnDeck1 = new List<Card> {
                    new Card(0, 0),
                    new Card(1, 0),
                    new Card(0, 0),
                    new Card(1, 0),
                    new Card(0, 0),
                    new Card(1, 0)
            };

            _cardsOnDeck2 = new List<Card> {
                    new Card(0, 1),
                    new Card(1, 1),
                    new Card(0, 1),
                    new Card(1, 1),
                    new Card(0, 1),
                    new Card(1, 1)
            };

        }

        #endregion
    }
}
