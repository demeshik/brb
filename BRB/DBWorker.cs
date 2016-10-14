using MySql.Data.MySqlClient;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace BRB
{
    public static class DBWorker
    {
        static int clientID;
        static int sclientID;
        static decimal clientCash;
        static decimal sclientCash;
        private async static Task<string> ConnectStringInit()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///app.config"));
            XmlDocument xmlConfiguration = await XmlDocument.LoadFromFileAsync(file);
            IXmlNode node = xmlConfiguration.DocumentElement.SelectSingleNode("./appSettings/add[@key='ConnectionString']/@value");
            return (string)node.NodeValue;
        }
        
        public async static Task<string> CheckClient(string login, string password)
        {
            App.Token = new CancellationTokenSource();
            string ss = "";
            string queryString = $@"select ID, NAME, BLOCK from users where LOGIN='{login}' and PASSWORD='{password}';";
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = await ConnectStringInit();
                MySqlCommand command = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    MySqlDataReader s = command.ExecuteReader();
                    while (s.Read())
                    {
                        App.ID = (int)s[0];
                        ss = s[1].ToString();
                        App.BlockCheck = (bool)s[2];
                    }
                }
                catch (Exception es)
                {
                    Help.Message(es.Message, "ERROR");
                }
            }
            if (ss != string.Empty)
                return ss;
            else
                return null;
        }
        public async static Task<string> CheckManager(string login, string password)
        {
            App.Token = new CancellationTokenSource();
            string ss = "";
            string queryString = $@"select Name from managers where Login='{login}' and Password='{password}';";
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = await ConnectStringInit();
                MySqlCommand command = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    MySqlDataReader s = command.ExecuteReader();
                    while (s.Read())
                    {
                        ss = s[0].ToString();
                    }
                }
                catch (Exception es)
                {
                    Help.Message(es.Message, "ERROR");
                }
            }
            if (ss != string.Empty)
                return ss;
            else
                return null;
        }
        public async static Task<Client> GetInformation(string Name)
        {
            App.Token = new CancellationTokenSource();
            Client person = new Client();
            using (MySqlConnection connect = new MySqlConnection())
            {
                connect.ConnectionString = await ConnectStringInit();
                connect.Open();
                try
                {
                    
                    using (MySqlCommand command = new MySqlCommand($"select LOGIN, ACC_CODE, CASH, BLOCK from users where NAME='{Name}';", connect))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                person.Login = reader[0].ToString();
                                person.AccCode = reader[1].ToString();
                                person.Cash = reader[2].ToString();
                                person.Block = (bool)reader[3];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Token.Cancel();
                    Help.Message(ex.Message, "Error");
                }
            }
            return person;
        }
        public async static Task<List<Client>> GetClients()
        {
            App.Token = new CancellationTokenSource();
            if (App.IdsList == null)
                App.IdsList = new List<int>();
            App.IdsList.Clear();
            List<Client> list = new List<Client>();
            using (MySqlConnection connect = new MySqlConnection())
            {
                connect.ConnectionString = await ConnectStringInit();
                connect.Open();
                try
                {
 
                    using (MySqlCommand command = new MySqlCommand("select ID, NAME, ACC_CODE, CASH, BLOCK from users;", connect)) 
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Client client = new Client();
                                client.ID = (int)reader[0];
                                App.IdsList.Add((int)reader[0]);
                                client.Name = reader[1].ToString();
                                client.AccCode = reader[2].ToString();
                                client.Cash = reader[3].ToString();
                                client.Block = (bool)reader[4];
                                list.Add(client);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    App.Token.Cancel();
                    Help.Message(ex.Message, "Error");
                }
            }
            return list;
        }
        public async static Task<bool> PrepareOperation(string Name , OperationClass operation)
        {
            App.Token = new CancellationTokenSource();
            bool rezult = false;
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = await ConnectStringInit();
                con.Open();
                try
                {
                    using (MySqlCommand command = new MySqlCommand($@"select ID, CASH, ACC_CODE from users where NAME='{Name}' or ACC_CODE='{operation.AccountCode}';", con))
                    {
                        decimal[] cashes = new decimal[2];
                        int[] ids = new int[2];
                        string[] codes = new string[2];
                        using (MySqlDataReader s = command.ExecuteReader())
                        {
                            int i = 0;
                            while (s.Read())
                            {
                                ids[i] = (int)s[0];
                                cashes[i] = (decimal)s[1];
                                codes[i] = s[2].ToString();
                                i++;
                            }
                        }
                        if (codes[0] == operation.AccountCode)
                        {
                            sclientID = ids[0];
                            clientID = ids[1];
                            clientCash = cashes[1];
                            sclientCash = cashes[0];
                        }
                        else
                        {
                            sclientID = ids[1];
                            clientID = ids[0];
                            clientCash = cashes[0];
                            sclientCash = cashes[1];
                        }
                        App.ID = clientID;
                        if (sclientID == 0 && operation.TypeOperation == "Перевод")
                        {
                            App.Token.Cancel();
                            throw new Exception("Проверьте правильность номера счета");
                        }
                        if ((clientCash - Decimal.Parse(operation.Amount)) < 0)
                        {
                            App.Token.Cancel();
                            throw new Exception("Средств на Вашем счете недостаточно.");
                        }
                        else
                        {
                            clientCash -= decimal.Parse(operation.Amount);
                            sclientCash += decimal.Parse(operation.Amount);
                        }
                    }
                    using (MySqlCommand command = new MySqlCommand($@"update USERS set CASH = {clientCash} where ID={clientID};", con))
                    {
                        command.ExecuteNonQuery();
                    }
                    if (sclientID != 0)
                        using (MySqlCommand command = new MySqlCommand($@"update USERS set CASH = {sclientCash} where ID={sclientID};", con))
                        {
                            command.ExecuteNonQuery();
                        }
                    string queryString = $"insert into Operations(ID, TypeOperation, SubType, AccountCode, Amount, Date) values({clientID},'{operation.TypeOperation}',@subType,'{operation.AccountCode}', {operation.Amount}, @date);";
                    MySqlParameter subtype = new MySqlParameter();
                    MySqlParameter date = new MySqlParameter();
                    subtype.ParameterName = "@subType";
                    subtype.Value = operation.SubType;
                    date.ParameterName = "@date";
                    date.Value = operation.Date.ToString("yyyy-MM-dd HH:mm:ss");
                    using (MySqlCommand command = new MySqlCommand(queryString, con))
                    {
                        command.Parameters.Add(subtype);
                        command.Parameters.Add(date);
                        command.ExecuteNonQuery();
                    }
                    rezult = true;

                }
                catch (Exception es)
                {
                    rezult = false;
                    App.Token.Cancel();
                    Help.Message(es.Message, "Ошибка");
                }
            }
            return rezult;
        }
        public async static Task<bool> UpdateInformation(string name, string parametr, string value)
        {
            App.Token = new CancellationTokenSource();
            string query = string.Empty;
            bool rezult = false;
            if (String.Equals(parametr, "Login"))
            {
                query = $"update users set LOGIN='{value}' where NAME='{name}';";
            }
            if (String.Equals(parametr, "Pass"))
            {
                query = $"update users set PASSWORD='{value}' where NAME='{name}';";
            }
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = await ConnectStringInit();
                con.Open();
                try
                {
                    using (MySqlCommand command = new MySqlCommand(query, con))
                    {
                        command.ExecuteNonQuery();
                        rezult = true;
                    }
                }
                catch (Exception es)
                {
                    rezult = false;
                    App.Token.Cancel();
                    Help.Message(es.Message, "Error");
                }
            }
            return rezult;
        }
        public async static Task<bool> UpdateAccountManag(Client person, string oldID)
        {
            App.Token = new CancellationTokenSource();
            bool rezult = false;
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = await ConnectStringInit();
                con.Open();
                try
                {
                    using (MySqlCommand command = new MySqlCommand($"update users set ID={person.ID}, NAME='{person.Name}', ACC_CODE='{person.AccCode}', CASH={decimal.Parse(person.Cash)} where ID='{int.Parse(oldID)}';", con)) 
                    {
                        command.ExecuteNonQuery();
                        rezult = true;
                    }
                }
                catch (Exception es)
                {
                    rezult = false;
                    App.Token.Cancel();
                    Help.Message(es.Message, "Error");
                }
            }
            return rezult;
        }
        public async static Task<bool> AddClient(Client person)
        {
            App.Token = new CancellationTokenSource();
            bool rezult = false;
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = await ConnectStringInit();
                con.Open();
                try
                {
                    App.IdsList.Add(person.ID);
                    using (MySqlCommand command = new MySqlCommand($"insert into users values({person.ID},'{person.Name}','{person.Login}','{person.Password}','{person.AccCode}', {decimal.Parse(person.Cash)},{person.Block});", con)) 
                    {
                        command.ExecuteNonQuery();
                        rezult = true;
                    }
                }
                catch (Exception es)
                {
                    rezult = false;
                    App.Token.Cancel();
                    Help.Message(es.Message, "Error");
                }
            }
            return rezult;
        }
        public async static Task<bool> BlockAccount(string name, bool Block)
        {
            App.Token = new CancellationTokenSource();
            bool rezult = false;

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = await ConnectStringInit();
                con.Open();
                try
                {
                    using (MySqlCommand command = new MySqlCommand($"update users set BLOCK={Block} where NAME='{name}';", con))
                    {
                        command.ExecuteNonQuery();
                        rezult = true;
                    }
                }
                catch (Exception es)
                {
                    rezult = false;
                    App.Token.Cancel();
                    Help.Message(es.Message, "Error");
                }
            }
            return rezult;
        }
        public async static Task<bool> DeleteAccount(string name, int id)
        {
            App.Token = new CancellationTokenSource();
            bool rezult = false;

            App.IdsList.Remove(id);

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = await ConnectStringInit();
                con.Open();
                try
                {
                    using (MySqlCommand command = new MySqlCommand($"delete from users where NAME='{name}';", con))
                    {
                        command.ExecuteNonQuery();
                        rezult = true;
                    }
                }
                catch (Exception es)
                {
                    rezult = false;
                    App.Token.Cancel();
                    Help.Message(es.Message, "Error");
                }
            }
            return rezult;
        }
        public async static Task<List<OperationClass>> GetHistory(string Name)
        {
            App.Token = new CancellationTokenSource();
            List<OperationClass> operations = new List<OperationClass>();

            using (MySqlConnection connect = new MySqlConnection())
            {
                connect.ConnectionString = await ConnectStringInit();
                connect.Open();
                try
                {
                    if (App.ID == 0)
                    {
                        using (MySqlCommand command = new MySqlCommand($"select ID from users where NAME='{Name}';", connect))
                        {
                            using (MySqlDataReader s = command.ExecuteReader())
                            {
                                while (s.Read())
                                {
                                    App.ID = (int)s[0];
                                }
                            }
                        }
                    }
                    using (MySqlCommand command = new MySqlCommand($"select * from operations where ID={App.ID};", connect))
                    {
                        using (MySqlDataReader s = command.ExecuteReader())
                        {
                            while (s.Read())
                            {
                                OperationClass operation = new OperationClass();
                                operation.TypeOperation = s[1].ToString();
                                if (operation.TypeOperation == "Оплата")
                                    operation.SubType = s[2].ToString();
                                else
                                    operation.SubType = null;
                                operation.AccountCode = s[3].ToString();
                                operation.Amount = s[4].ToString();
                                operation.Date = (DateTime)s[5];
                                operations.Add(operation);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Token.Cancel();
                    Help.Message(ex.Message, "Ошибка");
                }
            }
            return operations;
        }
        public async static Task<List<OperationClass>> GetOperations(string parametr, string value)
        {
            App.Token = new CancellationTokenSource();
            List<OperationClass> OperList = new List<OperationClass>();
            using (MySqlConnection connect = new MySqlConnection())
            {
                connect.ConnectionString = await ConnectStringInit();
                connect.Open();
                try
                {
                    string query = string.Empty;
                    MySqlParameter param = null;
                    if (String.Equals(parametr, "Дата")) 
                    {
                        query = $"select * from operations where Date=?date;";
                        param = new MySqlParameter("?date", MySqlDbType.Date);
                        param.Value = DateTime.Parse(value);
                    }
                    if (String.Equals(parametr, "Операция"))
                    {

                        if (String.Equals(value, "Перевод", StringComparison.OrdinalIgnoreCase)) 
                            query = $"select * from operations where upper(TypeOperation)=upper('{value}');";
                        else
                            query = $"select * from operations where upper(SubType)=upper('{value}');";
                    }
                    if(String.Equals(parametr, "ID"))
                    {
                        query = $"select * from operations where ID={Int32.Parse(value)};";
                    }
                    using (MySqlCommand command = new MySqlCommand(query, connect)) 
                    {
                        if (String.Equals(parametr, "Дата")) 
                            command.Parameters.Add(param);
                        using (MySqlDataReader s = command.ExecuteReader())
                        {
                            while (s.Read())
                            {
                                OperationClass operation = new OperationClass();
                                operation.ID = (int)s[0];
                                operation.TypeOperation = s[1].ToString();
                                if (operation.TypeOperation == "Оплата")
                                    operation.SubType = s[2].ToString();
                                else
                                    operation.SubType = null;
                                operation.AccountCode = s[3].ToString();
                                operation.Amount = s[4].ToString();
                                operation.Date = (DateTime)s[5];
                                OperList.Add(operation);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Token.Cancel();
                    Help.Message(ex.Message, "Error");
                }
            }
            return OperList;
        }
        public async static Task<List<OperationClass>> GetOperations(string parametr, List<DateTime> dates)
        {
            App.Token = new CancellationTokenSource();
            List<OperationClass> OperList = new List<OperationClass>();
            using (MySqlConnection connect = new MySqlConnection())
            {
                connect.ConnectionString = await ConnectStringInit();
                connect.Open();
                try
                {
                    string query = $"select * from operations where Date between ?date1 and ?date2;";
                    MySqlParameter param = new MySqlParameter("?date1", MySqlDbType.Date);
                    param.Value = dates[0];
                    MySqlParameter param2 = new MySqlParameter("?date2", MySqlDbType.Date);
                    param2.Value = dates[1];
                    using (MySqlCommand command = new MySqlCommand(query, connect))
                    {
                        command.Parameters.Add(param);
                        command.Parameters.Add(param2);
                        using (MySqlDataReader s = command.ExecuteReader())
                        {
                            while (s.Read())
                            {
                                OperationClass operation = new OperationClass();
                                operation.ID = (int)s[0];
                                operation.TypeOperation = s[1].ToString();
                                if (operation.TypeOperation == "Оплата")
                                    operation.SubType = s[2].ToString();
                                else
                                    operation.SubType = null;
                                operation.AccountCode = s[3].ToString();
                                operation.Amount = s[4].ToString();
                                operation.Date = (DateTime)s[5];
                                OperList.Add(operation);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Token.Cancel();
                    Help.Message(ex.Message, "Error");
                }
            }
            return OperList;
        }
        public async static Task<List<OperationClass>> GetOperations()
        {
            App.Token = new CancellationTokenSource();
            List<OperationClass> OperList = new List<OperationClass>();
            using (MySqlConnection connect = new MySqlConnection())
            {
                connect.ConnectionString = await ConnectStringInit();
                connect.Open();
                try
                {
                    using (MySqlCommand command = new MySqlCommand($"select * from operations;", connect))
                    {
                        using (MySqlDataReader s = command.ExecuteReader())
                        {
                            while (s.Read())
                            {
                                OperationClass operation = new OperationClass();
                                operation.ID = (int)s[0];
                                operation.TypeOperation = s[1].ToString();
                                if (operation.TypeOperation == "Оплата")
                                    operation.SubType = s[2].ToString();
                                else
                                    operation.SubType = null;
                                operation.AccountCode = s[3].ToString();
                                operation.Amount = s[4].ToString();
                                operation.Date = (DateTime)s[5];
                                OperList.Add(operation);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Token.Cancel();
                    Help.Message(ex.Message, "Error");
                }
            }
            return OperList;
        }

    }
}
