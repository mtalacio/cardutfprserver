using GameServer.Game_Objects;
using GameServer.Network;
using GameServer.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using static GameServer.Enums;

namespace GameServer {
    public static class GameEngine {

        public delegate void OnCardSelected(long sId);
        public static OnCardSelected OnCardSelectedCallback;

        private static readonly List<Player> PlayerList = new List<Player>();
        public static void AddPlayer(Player player) {
            if(PlayerList.Count > Constants.MAX_PLAYERS) {
                Console.WriteLine("Attempt to add a player beyond MAX_PLAYERS.");
                return;
            }
  
            PlayerList.Add(player);
        }

        private static List<Card> MasterList { get; } = new List<Card>();
        public static List<Card>[] CardsOnBoard { get; } = {new List<Card>(), new List<Card>()};
        public static List<Card>[] CardsOnHand { get; } = { new List<Card>(), new List<Card>() };
        public static List<Card>[] CardsOnDeck { get; } = { new List<Card>(), new List<Card>() };
        public static List<Card>[] CardsOnGraveyard { get; } = { new List<Card>(), new List<Card>() };

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

            SetAvailableMana(_playerOnTurn, PlayerList[_playerOnTurn].ManaRemaining - cardPlayed.CurrentMana);
            cardPlayed.PlayCard();
            CheckAvailableMoves();

            if(cardPlayed.IsSpell)
                ServerSendData.SendDisplaySpell(
                    _playerOnTurn == 0 ? 1 : 0, 
                    cardPlayed.CardId
                );
            else
                ServerSendData.SendCreateCard(
                    _playerOnTurn == 0 ? 1 : 0, 
                    cardPlayed.CardId, 
                    cardPlayed.ServerId, 
                    CardPlace.ENEMY_BOARD, 
                    boardIndex
                );

            if (PlayerList[_playerOnTurn].ManaRemaining < 0)
                throw new IllegalGameEventException("Carta jogada sem ter mana suficiente! Jogador Index: " + _playerOnTurn);
        }

        public static void AddToGame(Card card, int ownerIndex) {
            CardsOnDeck[ownerIndex].Add(card);
            MasterList.Add(card);
        }

        public static void AddToGraveyard(Card card) {
            CardsOnBoard[card.OwnerIndex].Remove(card);
            CardsOnGraveyard[card.OwnerIndex].Add(card);
            card.ChangePlace(CardPlace.GRAVEYARD);
        }

        public static void TurnEnded(long index) {

            if(index != _playerOnTurn)
                throw new IllegalMessageReceivedException("EndTurn recebido por jogador não permitido.");

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

            CheckAvailableMoves();
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
                CheckAvailableMoves();
        }

        private static void SetTotalMana(int index, int mana) {
            ServerSendData.SendSetTotalMana(index, mana);
            PlayerList[index].Mana = mana;
        }

        private static void SetAvailableMana(int index, int mana) {
            ServerSendData.SendSetAvailableMana(index, mana);
            PlayerList[index].ManaRemaining = mana;
        }

        public static void IncrementAvailableMana(int index, int qtd) {
            SetAvailableMana(index, PlayerList[index].ManaRemaining + qtd);
        }

        private static void AwakeCards() {
            foreach (Card card in CardsOnBoard[_playerOnTurn]) {
                card.WakeUp();
            }
        }

        public static void UpdateCardHealthForAll(int sId, int newHealth) {
            ServerSendData.SendUpdateCardHealth(0, sId, newHealth);
            ServerSendData.SendUpdateCardHealth(1, sId, newHealth);
        }

        public static void DestroyCardForAll(int sId) {
            ServerSendData.SendDestroyCard(0, sId);
            ServerSendData.SendDestroyCard(1, sId);
        }

        public static void DestroyCardTo(int index, int sId) {
            ServerSendData.SendDestroyCard(index, sId);
        }

        public static void DamageCard(int sId, int value) {
            Card target = MasterList.Find(x => x.ServerId == sId);
            if(target == null)
                throw new CardNotFoundException();

            target.Damage(value);
        }

        public static void CheckAvailableMoves() {
            AvailablePlaysVerifier.CheckHandPlays(_playerOnTurn, PlayerList[_playerOnTurn], CardsOnHand[_playerOnTurn]);
            AvailablePlaysVerifier.CheckBoardPlays(_playerOnTurn, CardsOnBoard[_playerOnTurn]);
        }

        private static void CreateHeros() {
            Card hero0 = Database.GetCard(0, 0);
            Card hero1 = Database.GetCard(0, 1);

            AddToGame(hero0, 0);
            CardsOnDeck[0].Remove(hero0);
            CardsOnBoard[0].Add(hero0);
            hero0.ChangePlace(CardPlace.BOARD);

            AddToGame(hero1, 1);
            CardsOnDeck[1].Remove(hero1);
            CardsOnBoard[1].Add(hero1);
            hero1.ChangePlace(CardPlace.BOARD);

            ServerSendData.SendHeroPortrait(0, hero0.ServerId);
            ServerSendData.SendEnemyPortrait(0, hero1.ServerId);
            ServerSendData.SendHeroPortrait(1, hero1.ServerId);
            ServerSendData.SendEnemyPortrait(1, hero0.ServerId);
        }

        public static void EndGame(int loserIndex) {
            Console.WriteLine("Game Ended with loser index value = " + loserIndex);

            ServerSendData.SendEndGame(0, loserIndex == 0 ? 0 : 1);
            ServerSendData.SendEndGame(1, loserIndex == 1 ? 0 : 1);

            NetworkSocket.Instance.EndServer();
        }

        public static void StartAttackEvent(long playerIndex, long sId) {
            if(playerIndex != _playerOnTurn)
                throw new IllegalMessageReceivedException("AttackEvent recebido por jogador não permitido.");

            Card attacker = CardsOnBoard[_playerOnTurn].Find(x => x.ServerId == sId);
            if(attacker == null)
                throw new CardNotFoundException();

            if(!attacker.CanAttack())
                throw new IllegalGameEventException("Carta que não pode atacar tentou começar ataque.");

            ServerSendData.SendStartSelectTarget(_playerOnTurn);
            AvailablePlaysVerifier.CheckAttackTargets(_playerOnTurn, CardsOnBoard[_playerOnTurn == 0 ? 1 : 0]);

            _cardSelectCaller = attacker;
            OnCardSelectedCallback += StartAttackResponse;
        }

        public static void PromptTargets(Dictionary<TargetRequirement, int> reqs) {
            ServerSendData.SendStartSelectTarget(_playerOnTurn);

            for (int i = 0; i < 2; i++) {
                foreach (Card card in CardsOnBoard[i]) {
                    bool canTarget = false;

                    if (reqs.TryGetValue(TargetRequirement.ENEMY_MINION, out int extraParam)) {
                        canTarget = card.OwnerIndex != _playerOnTurn;
                    }

                    if (reqs.TryGetValue(TargetRequirement.FRIENDLY_MINION, out extraParam)) {
                        canTarget = card.OwnerIndex == _playerOnTurn;
                    }

                    ServerSendData.SendSetCanTarget(_playerOnTurn, card.ServerId, canTarget ? 1 : 0);
                }
            }
        }

        public static void EndTargetEvent(OnCardSelected associatedEvent) {
            OnCardSelectedCallback -= associatedEvent;
            ServerSendData.SendEndSelectTarget(_playerOnTurn, 1);
            CheckAvailableMoves();
        }

        #endregion

        #region Game State

        private static int _playerOnTurn;    

        public static void InitializeGame() {
            _playerOnTurn = 0;
            ServerSendData.SendSetTurn(0, 1);
            ServerSendData.SendSetTurn(1, 0);
            CreateHeros();

            //FIXME: Temp code for testing purposes

            AddToGame(Database.GetCard(5, 0), 0);
            AddToGame(Database.GetCard(2, 0), 0);
            AddToGame(Database.GetCard(2, 0), 0);
            AddToGame(Database.GetCard(3, 0), 0);
            AddToGame(Database.GetCard(1, 0), 0);
            AddToGame(Database.GetCard(2, 0), 0);
            AddToGame(Database.GetCard(3, 0), 0);
            AddToGame(Database.GetCard(1, 0), 0);

            AddToGame(Database.GetCard(5, 1), 1);
            AddToGame(Database.GetCard(2, 1), 1);
            AddToGame(Database.GetCard(2, 1), 1);
            AddToGame(Database.GetCard(3, 1), 1);
            AddToGame(Database.GetCard(1, 1), 1);
            AddToGame(Database.GetCard(2, 1), 1);
            AddToGame(Database.GetCard(3, 1), 1);
            AddToGame(Database.GetCard(1, 1), 1);

            DrawCardTo(0, 3);
            DrawCardTo(1, 4);
            SetTotalMana(0, 1);
            SetAvailableMana(0, 1);
            SetTotalMana(1, 0);

            AvailablePlaysVerifier.CheckHandPlays(0, PlayerList[0], CardsOnHand[0]);
        }

        #endregion

        #region Card Select Responses

        private static Card _cardSelectCaller;

        private static void StartAttackResponse(long sId) {
            Card attacked = CardsOnBoard[_playerOnTurn == 0 ? 1 : 0].Find(x => x.ServerId == sId);
            if(attacked == null)
                throw new CardNotFoundException();

            _cardSelectCaller.Attack(attacked);
            _cardSelectCaller = null;

            EndTargetEvent(StartAttackResponse);
        }

        #endregion
    }
}
