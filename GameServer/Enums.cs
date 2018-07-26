
namespace GameServer {
    public class Enums {
        public enum ServerPackets {
            SWelcome = 1,
            SMessage,
            SKick,
            SSendInGame,
            SLoginResponse,
            CreateCard
        }

        public enum ClientPackets {
            CNewAccount = 1,
            CLogin,
            PlayCard
        }

        public enum LoginResponse {
            DOES_NOT_EXISTS = 1,
            INCORRECT_PASSWORD,
            OK
        }

        public enum CardPlace {
            HAND,
            BOARD,
            ENEMY_BOARD
        }
    }
}
