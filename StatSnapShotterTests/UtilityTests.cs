using NUnit.Framework;
using StatSnapShotter.DataConnections;
using StatSnapShotter.Entities;
using System.Collections.Generic;

namespace StatSnapShotterTests
{
    [TestFixture]
    public class UtilityTests
    {        
        [Test]
        public void Deserialize_XML()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            var expected = true;
            var actual = xmlConnection.Deserialize<List<StatElement>>();
            Assert.AreEqual(expected, (actual != null));
        }

        public void Deserialize_XML_If_Path_Invalid()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            List<StatElement> expected = null;
            var actual = xmlConnection.Deserialize<List<StatElement>>();
            Assert.AreEqual(expected, actual);
        }

    }
}
