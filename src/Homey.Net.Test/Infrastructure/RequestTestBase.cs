using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homey.Net.Dtos;
using NUnit.Framework;
using ClassicAssert = NUnit.Framework.Legacy.ClassicAssert;

namespace Homey.Net.Test.Infrastructure
{
    public abstract class RequestTestBase
    {
        private HomeyClient _client;

        [SetUp]
        public void Setup()
        {
            _client = SetupClient();
        }

        public abstract HomeyClient SetupClient();

        [Test]
        public async Task TestRequestDevices()
        {
            IList<Device> devices = await _client.GetDevices();
            ClassicAssert.NotNull(devices);
            ClassicAssert.Greater(devices.Count, 3);
            AssertDevice(devices.First());
        }

        [Test]
        public async Task TestRequestZoneDevices()
        {
            IList<Device> devices = await _client.GetDevices("9919ee1e-ffbc-480b-bc4b-77fb047e9e68");
            ClassicAssert.NotNull(devices);
            ClassicAssert.Greater(devices.Count, 3);
            AssertDevice(devices.First());
        }


        [Test]
        public async Task TestRequestDevice()
        {
            Device device = await _client.GetDevice("447f00f7-64e8-45b4-93c5-346455b2346b");
            AssertDevice(device);
        }
        
        [Test]
        public async Task TestRequestCapability()
        {
            CapatibilityReport report = await _client.GetCapability("447f00f7-64e8-45b4-93c5-346455b2346b", "measure_humidity");
            AssertReport(report);
        }

        [Test]
        public async Task TestRequestZones()
        {
            IList<Zone> zones = await _client.GetZones();
            ClassicAssert.NotNull(zones);
            ClassicAssert.Greater(zones.Count, 3);
            AssertZone(zones.First());
        }
        
        [Test]
        public async Task TestRequestFlows()
        {
            IList<Flow> flows = await _client.GetFlows();
            ClassicAssert.NotNull(flows);
            ClassicAssert.Greater(flows.Count, 3);
            AssertFlow(flows.First());
        }

        [Test]
        public async Task TestRequestFlow()
        {
            string flowId = "d812af08-e413-4b64-991c-7d81e3f35cb7";
            Flow flow = await _client.GetFlow(flowId);
            AssertFlow(flow);
        }

        [Test]
        public async Task TestRequestAlarms()
        {
            IList<Alarm> alarms = await _client.GetAlarms();
            ClassicAssert.NotNull(alarms);
            ClassicAssert.AreEqual(alarms.Count, 1);
            AssertAlarm(alarms.First());
        }

        [Test]
        public async Task TestRequestSystem()
        {
            HomeySystem system = await _client.GetSystem();
            ClassicAssert.NotNull(system);
            AssertSystem(system);
        }
        
        [Test]
        public async Task TestSetSwitchOn()
        {
            string id = "0e87a17f-5995-45ba-810d-37b1710acf46";

            TransactionResponse result = await _client.SetBooleanCapability(id, "onoff", true);
            
            ClassicAssert.NotNull(result);
            ClassicAssert.False(string.IsNullOrEmpty(result.TransactionId));
            ClassicAssert.False(string.IsNullOrEmpty(result.TransactionTime));
        }

        [Test]
        public async Task TestUpdateAlarm()
        {
            string alarmId = "db8f391f-38a6-4e42-81b5-1bf042c87254";
            DayTime time = new DayTime(10, 15);
            Repetition repetition = new Repetition {Friday = true, Monday = true};
            Alarm alarm = await _client.UpdateAlarm(alarmId, true, time, repetition);
            AssertAlarm(alarm);
        }

        [Test]
        public async Task TestEnableFlow()
        {
            string flowId = "d812af08-e413-4b64-991c-7d81e3f35cb7";
            Flow flow = await _client.EnableFlow(flowId, false);
            AssertFlow(flow);
        }


        [Test]
        public async Task TestTriggerFlow()
        {
            string flowId = "d812af08-e413-4b64-991c-7d81e3f35cb7";
            bool result = await _client.TriggerFlow(flowId);
            ClassicAssert.True(result);
        }

        private static void AssertDevice(Device device)
        {
            ClassicAssert.NotNull(device);
            ClassicAssert.False(string.IsNullOrEmpty(device.Id));
            ClassicAssert.False(string.IsNullOrEmpty(device.Name));
            ClassicAssert.NotNull(device.Capabilities);
            ClassicAssert.NotNull(device.CapabilitiesObj);
            ClassicAssert.NotNull(device.Data);
        }

        private void AssertReport(CapatibilityReport report)
        {
            ClassicAssert.NotNull(report);
            ClassicAssert.False(string.IsNullOrEmpty(report.Id));
            ClassicAssert.False(string.IsNullOrEmpty(report.Uri));
            ClassicAssert.Greater(report.Values.Count, 3);
        }

        private void AssertZone(Zone zone)
        {
            ClassicAssert.NotNull(zone);
            ClassicAssert.False(string.IsNullOrEmpty(zone.Name));
            ClassicAssert.False(string.IsNullOrEmpty(zone.Id));
        }

        private void AssertFlow(Flow flow)
        {
            ClassicAssert.NotNull(flow);
            ClassicAssert.False(string.IsNullOrEmpty(flow.Name));
            ClassicAssert.False(string.IsNullOrEmpty(flow.Id));
            ClassicAssert.NotNull(flow.Actions);
            ClassicAssert.NotNull(flow.Conditions);
        }

        private void AssertAlarm(Alarm alarm)
        {
            ClassicAssert.NotNull(alarm);
            ClassicAssert.False(string.IsNullOrEmpty(alarm.Id));
            ClassicAssert.False(string.IsNullOrEmpty(alarm.Name));
            ClassicAssert.False(string.IsNullOrEmpty(alarm.Time));
            ClassicAssert.True(alarm.NextOccurance > DateTime.MinValue);
            ClassicAssert.NotNull(alarm.Repetition);
        }

        private void AssertSystem(HomeySystem system)
        {
            ClassicAssert.False(string.IsNullOrEmpty(system.HomeyVersion));
            ClassicAssert.False(string.IsNullOrEmpty(system.HomeyModelId));
            ClassicAssert.False(string.IsNullOrEmpty(system.Hostname));
            ClassicAssert.False(string.IsNullOrEmpty(system.NodeVersion));
            ClassicAssert.False(string.IsNullOrEmpty(system.WifiAddress));
            ClassicAssert.False(string.IsNullOrEmpty(system.WifiSsid));
        }
    }
}