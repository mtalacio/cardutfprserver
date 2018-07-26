using System;

namespace GameServer.Utils {
    public class AccountExistsException : Exception {
        public AccountExistsException() : base("A conta requerida já existe.") { }
    }

    public class EndOfRecordsetException : Exception {
        public EndOfRecordsetException() : base("O cursor do Recordset já chegou ao final da tabela.") { }
    }

    public class AccountDoesNotExist : Exception {
        public AccountDoesNotExist() : base("A conta acessada não existe.") { }
    }

    public class IncorrectPasswordException : Exception {
        public IncorrectPasswordException() : base("Senha incorreta.") { }
    }
}
