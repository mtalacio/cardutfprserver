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

        private static readonly List<Card>[] CardsOnBoard = {new List<Card>(), new List<Card>()};
        private static readonly List<Card>[] CardsOnHand = { new List<Card>(), new List<Card>() };
        private static readonly List<Card>[] CardsOnDeck = { new List<Card>(), new List<Card>() };

        #region Events

        public static void CardPlayed(long playerIndex, int serverId, int boardIndex)
        {
            if (playerIndex != _playerOnTurn)
                throw new IllegalMessageReceivedException("Carta Jogada por jogador não permitido.");

            Card cardPlayed = CardsOnHand[_playerOnTurn].Find(x => x.ServerId == serverId);
            if (cardPlayed == null)
                throw new CardNotFoundException();

            CardsOnHand[_playerOnTurn].Remove(cardPlayed);
            CardsOnBoard[_playerOnTurn].Add(cardPlayed);

            cardPlayed.PlayCard();

            ServerSendData.SendCreateCard(
                _playerOnTurn == 0 ? 1 : 0, 
                cardPlayed.CardId, 
                cardPlayed.ServerId, 
                CardPlace.ENEMY_BOARD, 
                boardIndex
            );

            SetAvailableMana(_playerOnTurn, PlayerList[_playerOnTurn].ManaRemaining - cardPlayed.Mana);
            AvailablePlaysVerifier.CheckHandPlays(_playerOnTurn, PlayerList[_playerOnTurn], CardsOnHand[_playerOnTurn]);

            if (PlayerList[_playerOnTurn].ManaRemaining < 0)
                throw new IllegalGameEventException("Carta jogada sem ter mana suficiente! Jogador Index: " + _playerOnTurn);
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

            SetTotalMana(_playerOnTurn, (PlayerList[_playerOnTurn].Mana + 1));
            SetAvailableMana(_playerOnTurn, PlayerList[_playerOnTurn].Mana);

            AwakeCards();

            ServerSendData.SendSetTurn(0, _playerOnTurn == 0 ? 1 : 0);
            ServerSendData.SendSetTurn(1, _playerOnTurn == 0 ? 0 : 1);

            AvailablePlaysVerifier.CheckHandPlays(_playerOnTurn, PlayerList[_playerOnTurn], CardsOnHand[_playerOnTurn]);
            AvailablePlaysVerifier.CheckBoardPlays(_playerOnTurn, PlayerList[_playerOnTurn], CardsOnBoard[_playerOnTurn]);
        }

        public static void DrawCardTo(long index, long qtd) {
            if(CardsOnDeck[index].Count == 0) return;

            for (int i = 0; i < qtd; i++) {
                Card toDraw = CardsOnDeck[index][0];
                ServerSendData.SendCreateCard(index, toDraw.CardId, toDraw.ServerId, CardPlace.HAND, -1);
                CardsOnDeck[index].Remove(toDraw);
                CardsOnHand[index].Add(toDraw);
                toDraw.ChangePlace(CardPlace.HAND);
            }

            if(index == _playerOnTurn)
                AvailablePlaysVerifier.CheckHandPlays(_playerOnTurn, PlayerList[_playerOnTurn], CardsOnHand[_playerOnTurn]);
        }

        private static void SetTotalMana(int index, int mana) {
            ServerSendData.SendSetTotalMana(index, mana);
            PlayerList[index].Mana = mana;
        }

        private static void SetAvailableMana(int index, int mana) {
            ServerSendData.SendSetAvailableMana(index, mana);
            PlayerList[index].ManaRemaining = mana;
        }

        private static void AwakeCards() {
            foreach (Card card in CardsOnBoard[_playerOnTurn]) {
                card.WakeUp();
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

            CardsOnDeck[0].Add(Database.GetCard(0, 0, 0));
            CardsOnDeck[0].Add(Database.GetCard(1, 1, 0));
            CardsOnDeck[0].Add(Database.GetCard(0, 2, 0));
            CardsOnDeck[0].Add(Database.GetCard(1, 3, 0));

            CardsOnDeck[1].Add(Database.GetCard(0, 4, 1));
            CardsOnDeck[1].Add(Database.GetCard(1, 5, 1));
            CardsOnDeck[1].Add(Database.GetCard(0, 6, 1));
            CardsOnDeck[1].Add(Database.GetCard(1, 7, 1));

            DrawCardTo(0, 3);
            DrawCardTo(1, 4);
            SetTotalMana(0, 1);
            SetAvailableMana(0, 1);
            SetTotalMana(1, 0);

            AvailablePlaysVerifier.CheckHandPlays(0, PlayerList[0], CardsOnHand[0]);
        }

        #endregion
    }
}
