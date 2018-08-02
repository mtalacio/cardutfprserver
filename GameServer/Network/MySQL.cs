using System;
using ADODB;
using GameServer.Utils;

namespace GameServer.Network {
    public static class MySql {
        private static Recordset _dbRs = new Recordset();
        private static Connection _dbConn;

        private const string CONNECTION_STRING = "Driver={MySQL ODBC 5.3 Unicode Driver};Server=localhost;Database=gamedatabase;User=root;Password=;Option=3;";

        public static void MySqlInit() {
            Console.WriteLine("Connecting to MySQL...");
            try {
                _dbRs = new Recordset();
                _dbConn = new Connection {
                    ConnectionString = CONNECTION_STRING,
                    CursorLocation = CursorLocationEnum.adUseServer
                };

                _dbConn.Open();
                Console.WriteLine("Connection to MYSQL Server established");
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public static object[,] GetRows(string str) {

            _dbRs.Open(str, _dbConn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic);

            if(_dbRs.EOF) {
                _dbRs.Close();
                throw new EndOfRecordsetException();
            }
                

            _dbRs.MoveFirst();

            object[,] result = _dbRs.GetRows();

            _dbRs.Close();

            return result;

        }

        public static bool RecordExists(string str) {
            _dbRs.Open(str, _dbConn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic);

            if (_dbRs.EOF) {
                _dbRs.Close();
                return false;
            }

            _dbRs.Close();
            return true;
        }

        public static void InsertAccount(string username, string password) {
            _dbRs.Open("select * from players where login = '" + username + "'", _dbConn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic);

            _dbRs.AddNew();
            _dbRs.Fields[1].Value = username;
            _dbRs.Fields[2].Value = password;
            _dbRs.Fields[3].Value = 0;
            _dbRs.Fields[4].Value = 0;
            _dbRs.Update();

            _dbRs.Close();
        }

        
    }
}
