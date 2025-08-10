using System.Collections.Generic;
using System.IO;
using Homey.Net.Dtos;
using Homey.Net.Test.Infrastructure;
using NUnit.Framework;
using ClassicAssert = NUnit.Framework.Legacy.ClassicAssert;

namespace Homey.Net.Test
{
    public class ParserTest
    {
        private ResponseParser _parser;
        private TestDataContext _testData;

        [SetUp]
        public void Setup()
        {
            _testData = new TestDataContext();
            _parser = new ResponseParser();
        }

        [Test]
        public void TestParseZones()
        {
            string data = File.ReadAllText(_testData.GetZonesResponseFile);
            List<Zone> response = _parser.ParseZones(data);
            ClassicAssert.NotNull(response);
        }


        [Test]
        public void TestParseDevices()
        {
            string data = File.ReadAllText(_testData.GetDevicesResponseFile);
            List<Device> response = _parser.ParseDevices(data);
            ClassicAssert.NotNull(response);
        }

        [Test]
        public void TestParseFlow()
        {
            string data = File.ReadAllText(_testData.GetFlowResponseFile);
            Flow response = _parser.ParseFlow(data);
            ClassicAssert.NotNull(response);
        }

        [Test]
        public void TestParseFlows()
        {
            string data = File.ReadAllText(_testData.GetFlowsResponseFile);
            List<Flow> response = _parser.ParseFlows(data);
            ClassicAssert.NotNull(response);
        }

        [Test]
        public void TestParseCapatibilityReport()
        {
            string data = File.ReadAllText(_testData.GetCapatibilityReport);
            CapatibilityReport response = _parser.ParseCapatibilityReport(data);
            ClassicAssert.NotNull(response);
        }

        [Test]
        public void TestParseAlarm()
        {
            string data = File.ReadAllText(_testData.GetAlarmResponse);
            Alarm response = _parser.ParseAlarm(data);
            ClassicAssert.NotNull(response);
        }


        [Test]
        public void TestParseAlarms()
        {
            string data = File.ReadAllText(_testData.GetAlarmsResponse);
            IList<Alarm> response = _parser.ParseAlarms(data);
            ClassicAssert.NotNull(response);
        }

        [Test]
        public void TestParseSetOnOff()
        {
            string data = File.ReadAllText(_testData.SetOnOff);
            TransactionResponse response = _parser.ParseTransactionResponse(data);
            ClassicAssert.NotNull(response);
        }

        [Test]
        public void TestParseSystem()
        {
            string data = File.ReadAllText(_testData.GetSystemResponse);
            HomeySystem response = _parser.ParseSystem(data);
            ClassicAssert.NotNull(response);
        }
    }
}