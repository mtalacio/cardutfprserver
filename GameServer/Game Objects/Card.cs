﻿using System;
using System.Collections.Generic;
using GameServer.Network;
using GameServer.Utils;
using static GameServer.Enums;

namespace GameServer.Game_Objects {

    public delegate void Battlecry(Card cardBase);
    public delegate void Deathrattle(Card cardBase);

    public class Card {

        private static int _nextServerId;

        public int ServerId { get; private set; }
        public CardPlace Place { get; private set; }
        public int OwnerIndex { get; private set; }

        public int CurrentHealthPoints { get; private set; }
        public int CurrentAttackPoints { get; private set; }
        public int CurrentMana { get; private set; }

        public int CardId { get; }

        public readonly int HealthPoints;
        public readonly int AttackPoints;
        public readonly int Mana;

        public readonly bool IsTaunt;

        public readonly bool IsSpell;

        private readonly Battlecry _battlecries;
        private readonly Deathrattle _deathrattles;
        private readonly Dictionary<PlayRequirement, bool> _playReqs;

        private bool _justPlayed;
        private bool _justAttacked;
        

        public Card(int cardId, int healthPoints, int attackPoints, int mana, bool isTaunt,Battlecry baseBattlecry, Deathrattle baseDeathrattle, Dictionary<PlayRequirement, bool> playReqs) {
            Place = CardPlace.DECK;
            CardId = cardId;

            HealthPoints = healthPoints;
            AttackPoints = attackPoints;
            Mana = mana;

            CurrentHealthPoints = healthPoints;
            CurrentAttackPoints = attackPoints;
            CurrentMana = mana;

            if(baseBattlecry != null)
                _battlecries += baseBattlecry;

            if(baseDeathrattle != null)
                _deathrattles += baseDeathrattle;

            _playReqs = playReqs;

            IsTaunt = isTaunt;
            IsSpell = false;
        }

        public Card(int cardId, int mana, Battlecry baseBattlecry, Dictionary<PlayRequirement, bool> playReqs) {
            Place = CardPlace.DECK;
            CardId = cardId;

            Mana = mana;

            CurrentMana = mana;

            if (baseBattlecry != null)
                _battlecries += baseBattlecry;

            _playReqs = playReqs;

            IsSpell = true;
        }

        public Card CloneCard() {
            return new Card(CardId, CurrentHealthPoints, CurrentAttackPoints, CurrentMana, IsTaunt,_battlecries, _deathrattles, _playReqs);
        }

        public void AssignCard(int ownerIndex) {
            ServerId = _nextServerId;
            _nextServerId++;
            OwnerIndex = ownerIndex;
        }

        public void ChangePlace(CardPlace place) {
            Console.WriteLine(">> Changing card SID " + ServerId + " from: " + Place + " to: " + place);
            Place = place;
        }

        public void ChangeHealth(int newHealth) {
            CurrentHealthPoints = newHealth;
        }

        public void ChangeAttack(int newAttack) {
            CurrentAttackPoints = newAttack;
        }

        public void ChangeMana(int newMana) {
            CurrentMana = newMana;
        }

        public bool CanAttack() {
            return !_justPlayed && CurrentAttackPoints > 0 && !_justAttacked;
        }

        public void WakeUp() {
            _justPlayed = false;
            _justAttacked = false;
        }

        public void PlayCard() {
            ChangePlace(CardPlace.BOARD);
            _justPlayed = true;
            CallBattlecries();
        }

        private void CallBattlecries() {
            _battlecries?.Invoke(this);

            if (IsSpell) {
                GameEngine.DestroyCardTo(OwnerIndex, ServerId);
                GameEngine.AddToGraveyard(this);
                GameEngine.CheckAvailableMoves();
            }
        }

        private void Die() {
            if (CardId == 0) {
                GameEngine.EndGame(OwnerIndex);
                return;
            }

            Console.WriteLine(">> Card SID = " + ServerId + " died");
            CallDeathrattles();
        }

        private void CallDeathrattles() {
            _deathrattles?.Invoke(this);
            EndDeath();
        }

        private void EndDeath() {
            GameEngine.DestroyCardForAll(ServerId);
            GameEngine.AddToGraveyard(this);
        }

        public void Attack(Card card) {
            Console.WriteLine(">> Card SID: " + ServerId + " attacking SID = " + card.ServerId);
            Damage(card.CurrentAttackPoints);
            card.Damage(CurrentAttackPoints);
            _justAttacked = true;
        }

        public void Damage(int value) {
            Console.WriteLine(">> Card SID: " + ServerId + " taking damage value = " + value);
            ChangeHealth(CurrentHealthPoints - value);
            GameEngine.UpdateCardHealthForAll(ServerId, CurrentHealthPoints);
            if(CurrentHealthPoints <= 0)
                Die();
        }

        public bool CheckRequirement(PlayRequirement req) {
            if (_playReqs.TryGetValue(req, out bool needReq))
                return needReq;
            
            throw new RequirementException("Requerimento não encontrado.");
        }

    }
}
