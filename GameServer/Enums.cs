
namespace GameServer {
    public static class Enums {
        public enum ServerPackets {
            SWelcome = 1,
            SMessage,
            SKick,
            SSendInGame,
            SLoginResponse,
            CreateCard,
            SetTurn
        }

        public enum ClientPackets {
            CNewAccount = 1,
            CLogin,
            PlayCard,
            EndTurn,
            PlayerReady,
            DrawCards
        }

        public enum LoginResponse {
            DOES_NOT_EXISTS = 1,
            INCORRECT_PASSWORD,
            OK
        }

        public enum CardPlace {
            HAND,
            BOARD,
            ENEMY_BOARD,
            DECK
        }
    }
}
