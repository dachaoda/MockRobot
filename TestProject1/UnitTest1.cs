using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockRobotAPI;

namespace TestProject1
{
    [TestClass]
    public class MockRobotTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var Bot1 = new MockRobot();
            Bot1.GetID();
            throw ("hi");
        }
    }
}
