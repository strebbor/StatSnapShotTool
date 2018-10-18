using NUnit.Framework;
using Seq.Api.Model.Data;
using StatSnapShotter.DataConnections;
using StatSnapShotter.Entities;
using System;
using System.Collections.Generic;
using System.Xml;

namespace StatSnapShotterTests
{
    [TestFixture]
    public class ConnectionTests
    {
        SQLConnection sqlConnection;
        TextFileConnection textFileConnection;
        XMLConnection xmlConnection;
        SeqDataConnection seqConnection;

        [Test]
        public void Read_Source_XML_If_File_Exists()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            xmlConnection = new XMLConnection(settings);

            var expected = true;
            var actual = (XmlReader)xmlConnection.Read(null);

            Assert.AreEqual(expected, actual != null);
        }

        [Test]
        public void Should_Fail_Nicely_When_Using_Invalid_SEQ_API_Key()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "http://localhost:5341/";
            settings.APIKey = "InvalidPAIKey";

            seqConnection = new SeqDataConnection(settings);

            QueryParams parts = new QueryParams()
            {
                query = "SELECT * FROM STREAM",
                startDate = new DateTime(2018, 10, 15, 10, 0, 0),
                endDate = new DateTime(2018, 10, 15, 14, 0, 0)
            };

            var expected = true;
            var result = (QueryResultPart)seqConnection.Read(parts);
            var actual = result.Rows.Length > 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Fail_Nicely_When_Using_Invalid_SEQ_Server()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "http://imchelle/";
            settings.APIKey = "MbZ6wyBSuTVaYNfErgkO";

            seqConnection = new SeqDataConnection(settings);

            QueryParams parts = new QueryParams()
            {
                query = "SELECT * FROM STREAM",
                startDate = new DateTime(2018, 9, 13, 16, 0, 0),
                endDate = new DateTime(2018, 9, 14, 17, 0, 0)
            };

            QueryResultPart expected = null;
            var actual = (QueryResultPart)seqConnection.Read(parts);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Read_Source_Seq_If_All_Settings_Is_Fine()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "http://localhost:5341/";
            settings.APIKey = "MbZ6wyBSuTVaYNfErgkO";
            seqConnection = new SeqDataConnection(settings);

            var expected = true;
            QueryParams parts = new QueryParams()
            {
                query = "SELECT * FROM STREAM",
                startDate = new DateTime(2018, 10, 15, 10, 0, 0),
                endDate = new DateTime(2018, 10, 15, 14, 0, 0)
            };
            var actual = (QueryResultPart)seqConnection.Read(parts);
            Assert.AreEqual(expected, actual.Rows.Length >= 2);
        }

        [Test]
        public void Read_Source_XML_If_File_Does_Not_Exist()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "test.txt";
            xmlConnection = new XMLConnection(settings);

            XmlReader expected = null;
            var actual = (string)xmlConnection.Read(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Read_Source_XML_If_File_Is_Invalid()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "sdlkfjsdljf";
            xmlConnection = new XMLConnection(settings);

            XmlReader expected = null;
            var actual = (string)xmlConnection.Read(null);

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Read_Source_TextFile_If_File_Exists()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\storage.txt";
            textFileConnection = new TextFileConnection(settings);

            var expected = true;
            var actual = (string)textFileConnection.Read(null);

            Assert.AreEqual(expected, actual.Length > 0);
        }

        [Test]
        public void Read_Source_TextFile_If_File_Does_Not_Exist()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\nonexistantfile.txt";
            textFileConnection = new TextFileConnection(settings);

            var expected = "";
            var actual = (string)textFileConnection.Read(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Read_Source_TextFile_If_File_Is_Invalid()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\" + DateTime.Now.ToString() + ".txt";
            textFileConnection = new TextFileConnection(settings);

            string expected = null;
            var actual = (string)textFileConnection.Read(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Not_Be_Able_To_Write_Int_To_TextFile()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\storage.txt";
            textFileConnection = new TextFileConnection(settings);

            var expected = false;
            var actual = textFileConnection.Write(123213);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Be_Able_To_Write_String_To_TextFile()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\storage.txt";
            textFileConnection = new TextFileConnection(settings);

            var expected = true;
            var actual = textFileConnection.Write("this is my text");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Can_String_Be_Written_To_TextFile()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\storage.txt";
            textFileConnection = new TextFileConnection(settings);

            var expected = "this is my text";
            textFileConnection.Write("this is my text", true);
            var actual = textFileConnection.Read(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Can_String_Be_Written_To_TextFile_If_File_Invalid()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\" + DateTime.Now.ToString() + ".txt";
            textFileConnection = new TextFileConnection(settings);

            string expected = null;
            textFileConnection.Write("this is my text", true);
            var actual = textFileConnection.Read(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Can_String_Be_Written_To_TextFile_If_File_Does_Not_Exist()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = Guid.NewGuid().ToString() + ".txt";
            textFileConnection = new TextFileConnection(settings);

            var expected = "this is my text";
            textFileConnection.Write("this is my text", true);
            var actual = textFileConnection.Read(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Can_Read_TextFile()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\storage.txt";
            textFileConnection = new TextFileConnection(settings);

            textFileConnection.Write("this is my text", true);

            var expected = "this is my text";
            var actual = textFileConnection.Read(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Can_Read_SQL()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "intradaytest";
            settings.DatabaseName = "STATTRACKER_Test";
            settings.Username = "IUSR_TSTSTATTRACKER";
            sqlConnection = new SQLConnection(settings);

            var expected = true;
            List<StatRowItem> rows = (List<StatRowItem>)sqlConnection.Read(null);
            var actual = rows.Count > 1;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Can_Write_SQL()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "intradaytest";
            settings.DatabaseName = "STATTRACKER_Test";
            settings.Username = "IUSR_TSTSTATTRACKER";
            sqlConnection = new SQLConnection(settings);

            List<StatRowItem> rowsToWrite = new List<StatRowItem>();
            StatRowItem rowItem = new StatRowItem()
            {
                WriteDate = DateTime.Now.ToString(),
                DateFrom = "2018/10/15 10:00:00",
                DateTo = "2018/10/15 14:00:00",
                StatCount = 5,
                StatName = "UnitTest Stat"
            };
            rowsToWrite.Add(rowItem);
            rowsToWrite.Add(rowItem);

            var expected = true;
            var actual = sqlConnection.Write(rowsToWrite);

            Assert.AreEqual(expected, actual);
        }
    }
}
