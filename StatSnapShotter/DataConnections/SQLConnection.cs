using Seq.Api;
using Seq.Api.Model.Data;
using StatSnapShotter.Entities;
using StatSnapShotter.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.IO;

namespace StatSnapShotter.DataConnections
{
    public class SQLConnection : IDataManipulator
    {
        private string _userID;
        private string _databaseName;
        private string _connectionString;

        public SQLConnection(DataProviderSettings settings)
        {
            url = settings.Location.ToString();
            _databaseName = settings.DatabaseName.ToString();
            _userID = settings.Username.ToString();

            _connectionString = "Data Source=" + url + ";Initial Catalog=" + _databaseName + ";Integrated Security=SSPI;User ID=" + _userID + "";
        }

        public string url { get; set; }

        public object Deserialize<T>()
        {
            throw new NotImplementedException();
        }

        public object Read(QueryParams queryParts = null)
        {
            List<StatRowItem> allStatRows = new List<StatRowItem>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("SELECT DateFrom, DateTo, StatName, StatCount FROM CapturedStats", con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StatRowItem rowItem = new StatRowItem();
                            rowItem.DateFrom = reader.GetValue(0).ToString();
                            rowItem.DateTo = reader.GetValue(1).ToString();
                            rowItem.StatName = reader.GetValue(2).ToString();
                            rowItem.StatCount = int.Parse(reader.GetValue(3).ToString());
                            allStatRows.Add(rowItem);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Log.Error(ee, "Error has occured");
            }

            return allStatRows;
        }

        public bool Write(object dataToWrite, bool overrideExisting)
        {
            return Write(dataToWrite);
        }

        public bool Write(object dataToWrite)
        {

            List<StatRowItem> allStatRows = (List<StatRowItem>)dataToWrite;
            if (allStatRows == null)
            {
                Log.Information("NULL rows provided to write to SQL");
                return true;
            }

            bool writeSuccess = true;
            foreach (StatRowItem row in allStatRows)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string insertStatRow = @"INSERT into CapturedStats (WriteDate, DateFrom, DateTo, StatName, StatCount)
                                    VALUES (@WriteDate, @DateFrom, @DateTo, @StatName, @StatCount)";

                    using (SqlCommand writeCommand = new SqlCommand(insertStatRow))
                    {
                        writeCommand.Connection = connection;
                        writeCommand.Parameters.AddWithValue("@WriteDate", row.WriteDate);
                        writeCommand.Parameters.AddWithValue("@DateFrom", row.DateFrom);
                        writeCommand.Parameters.AddWithValue("@DateTo", row.DateTo);
                        writeCommand.Parameters.AddWithValue("@StatName", row.StatName);
                        writeCommand.Parameters.AddWithValue("@StatCount", row.StatCount);

                        try
                        {
                            connection.Open();
                            writeCommand.ExecuteNonQuery();
                        }
                        catch (Exception ee)
                        {
                            writeSuccess = false;
                            Log.Error(ee, "Error has occured");
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }

            return writeSuccess;
        }
    }

    public class StatRowItem
    {
        public string WriteDate { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string StatName { get; set; }
        public int StatCount { get; set; }
    }
}