using System;
using ADODB;

namespace GameServer {
    public static class MySQL {
        public static Recordset DB_RS = new Recordset();
        public static Connection DB_CONN;

        private static string connectionString = "Driver={MySQL ODBC 5.3 Unicode Driver};Server=localhost;Database=gamedatabase;User=root;Password=;Option=3;";

        public static void MySQLInit() {
            Console.WriteLine("Connecting to MySQL...");
            try {
                DB_RS = new Recordset();
                DB_CONN = new Connection {
                    ConnectionString = connectionString,
                    CursorLocation = CursorLocationEnum.adUseServer
                };

                DB_CONN.Open();
                Console.WriteLine("Connection to MYSQL Server established");

                //DB_RS.Open("Select * from players where login = 'Matheus'", DB_CONN, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic, -1);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public static object[,] GetRows(string str) {

            DB_RS.Open(str, DB_CONN, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic, -1);

            if(DB_RS.EOF) {
                DB_RS.Close();
                throw new EndOfRecordsetException();
            }
                

            DB_RS.MoveFirst();

            object[,] result = DB_RS.GetRows();

            DB_RS.Close();

            return result;

        }

        public static bool RecordExists(string str) {
            DB_RS.Open(str, DB_CONN, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic, -1);

            if (DB_RS.EOF) {
                DB_RS.Close();
                return false;
            }
            else {
                DB_RS.Close();
                return true;
            }
        }

        public static void InsertAccount(string username, string password) {
            DB_RS.Open("select * from players where login = '" + username + "'", DB_CONN, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockOptimistic, -1);

            DB_RS.AddNew();
            DB_RS.Fields[1].Value = username;
            DB_RS.Fields[2].Value = password;
            DB_RS.Fields[3].Value = 0;
            DB_RS.Fields[4].Value = 0;
            DB_RS.Update();

            DB_RS.Close();
        }

        
    }
}
