using GameServer.Game_Objects;
using GameServer.Network;
using GameServer.Utils;
using System;
using System.Collections.Generic;
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

        private static readonly List<Card> CardsOnHand1 = new List<Card>();
        private static readonly List<Card> CardsOnHand2 = new List<Card>();

        private static List<Card> _cardsOnDeck1;
        private static List<Card> _cardsOnDeck2;

        #region Events

        public static void CardPlayed(long playerIndex, int serverId, int boardIndex)
        {
            if (playerIndex != _playerOnTurn)
                throw new IllegalMessageReceivedException("Carta Jogada por jogador não permitido.");

            Card cardPlayed;
            if (_playerOnTurn == 0) {
                cardPlayed = CardsOnHand1.Find(x => x.ServerId == serverId);
                CardsOnHand1.Remove(cardPlayed);
            }
            else {
                cardPlayed = CardsOnHand2.Find(x => x.ServerId == serverId);
                CardsOnHand2.Remove(cardPlayed);
            }

            if(cardPlayed == null)
                throw new CardNotFoundException();

            cardPlayed.PlayCard();
            ServerSendData.SendCreateCard(_playerOnTurn == 0 ? 1 : 0, cardPlayed.CardId, cardPlayed.ServerId, CardPlace.ENEMY_BOARD, boardIndex);
        }

        public static void TurnEnded(long index) {

            if(index != _playerOnTurn) {
                throw new IllegalMessageReceivedException("EndTurn recebido por jogador não permitido.");
            }

            StartTurn();
        }

        private static void StartTurn() {
            _playerOnTurn = _playerOnTurn == 0 ? 1 : 0;

            Console.WriteLine("Starting Player " + _playerOnTurn + " turn.");

            ServerSendData.SendSetTurn(0, _playerOnTurn == 0 ? 1 : 0);
            ServerSendData.SendSetTurn(1, _playerOnTurn == 0 ? 0 : 1);
        }

        public static void DrawCardTo(long index, long qtd) {

            Card toDraw;

            if (index == 0) { 
                if (_cardsOnDeck1.Count == 0)
                    return;

                for (int i = 0; i < qtd; i++) {
                    toDraw = _cardsOnDeck1[0];
                    ServerSendData.SendCreateCard(0, toDraw.CardId, toDraw.ServerId, CardPlace.HAND, -1);
                    _cardsOnDeck1.Remove(toDraw);
                    CardsOnHand1.Add(toDraw);
                    toDraw.ChangePlace(CardPlace.HAND);
                }
            } else {
                if (_cardsOnDeck2.Count == 0)
                    return;

                for (int i = 0; i < qtd; i++) {
                    toDraw = _cardsOnDeck2[0];
                    ServerSendData.SendCreateCard(1, toDraw.CardId, toDraw.ServerId, CardPlace.HAND, -1);
                    _cardsOnDeck2.Remove(toDraw);
                    CardsOnHand2.Add(toDraw);
                    toDraw.ChangePlace(CardPlace.HAND);
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
                Database.GetCard(0, 0, 0),
                Database.GetCard(1, 1, 0),
                Database.GetCard(0, 2, 0),
                Database.GetCard(1, 3, 0)
            };

            _cardsOnDeck2 = new List<Card> {
                Database.GetCard(0, 4, 1),
                Database.GetCard(1, 5, 1),
                Database.GetCard(0, 6, 1),
                Database.GetCard(1, 7, 1)
            };

            DrawCardTo(0, 3);
            DrawCardTo(1, 4);
        }

        #endregion
    }
}
