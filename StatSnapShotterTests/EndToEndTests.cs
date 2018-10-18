using NUnit.Framework;
using StatSnapShotter;
using StatSnapShotter.DataConnections;
using StatSnapShotter.Entities;

namespace StatSnapShotterTests
{
    [TestFixture]
    public class EndToEndTests
    {
        private TextFileConnection textFileConnection;
        private SeqDataConnection seqConnection;
        private StatConfiguration statConfiguration;

        [Test]
        public void DoesAllStatsGetProcessed()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            DataProviderSettings textfileSettings = new DataProviderSettings();
            textfileSettings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\storage.txt";
            textFileConnection = new TextFileConnection(textfileSettings);

            DataProviderSettings seqSettings = new DataProviderSettings();
            seqSettings.Location = "http://localhost:5341/";
            seqSettings.APIKey = "MbZ6wyBSuTVaYNfErgkO";
            seqConnection = new SeqDataConnection(seqSettings);

            statConfiguration = new StatConfiguration(xmlConnection, "5");

            StatProcessor statProcessor = new StatProcessor(statConfiguration, seqConnection, textFileConnection);
            var actual = statProcessor.Run();
            var expected = 1;
            Assert.AreEqual(expected, actual);
        }
    }
}
