using NUnit.Framework;
using StatSnapShotter;
using StatSnapShotter.DataConnections;
using StatSnapShotter.Entities;
using System;
using System.Collections.Generic;

namespace StatSnapShotterTests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void GetConfiguration_JSON()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\statConfiguration.json";
            TextFileConnection fileConnection = new TextFileConnection(settings);

            StatConfiguration config = new StatConfiguration(fileConnection, "2");
            var expected = 4;
            var actual = (List<StatElement>)config.GetConfiguration<List<StatElement>>();
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public void GetConfiguration_XML()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection fileConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(fileConnection, "2");
            var expected = 1;
            var actual = (List<StatElement>)config.GetConfiguration<List<StatElement>>();
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public void GetConfiguration_If_Interval_Invalid()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, "1sdfsfd");
            var expected = 1;
            var actual = (List<StatElement>)config.GetConfiguration<List<StatElement>>();
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public void GetConfiguration_If_Interval_Is_Zero()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, "0");
            var expected = 1;
            var actual = (List<StatElement>)config.GetConfiguration<List<StatElement>>();
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public void GetConfiguration_If_Interval_Is_Null()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, null);
            var expected = 1;
            var actual = (List<StatElement>)config.GetConfiguration<List<StatElement>>();
            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public void GetConfiguration_If_Path_Does_Not_Exist()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = Guid.NewGuid().ToString();
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, "2");
            List<StatElement> expected = null;
            var actual = (List<StatElement>)config.GetConfiguration<List<StatElement>>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetConfiguration_If_Path_Is_Invalid()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = Guid.NewGuid().ToString();
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, "2");
            List<StatElement> expected = null;
            var actual = (List<StatElement>)config.GetConfiguration<List<StatElement>>();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Check_If_Interval_Is_Right_Based_On_Setup()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, "5");
            var expected = 5;
            var actual = config.Interval;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Check_If_Interval_Is_One_when_Setup_Is_Zero()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, "0");
            var expected = 1;
            var actual = config.Interval;
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Check_If_Interval_Is_One_when_Setup_Is_Null()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, null);
            var expected = 1;
            var actual = config.Interval;
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Check_If_Interval_Is_One_when_Setup_Is_Invalid()
        {
            DataProviderSettings settings = new DataProviderSettings();
            settings.Location = "C:\\Dev\\DevApps\\StatSnapShotter\\StatSnapShotter\\StatConfiguration.xml";
            XMLConnection xmlConnection = new XMLConnection(settings);

            StatConfiguration config = new StatConfiguration(xmlConnection, "sdfdsf");
            var expected = 1;
            var actual = config.Interval;
            Assert.AreEqual(expected, actual);
        }
    }
}
