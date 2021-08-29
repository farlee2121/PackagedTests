using Xunit;
using PackagedTests;

namespace ConsumerTests
{

    public class UnitTest1 : PackagedTests.PackagedTests
    {
        public UnitTest1() : base (5)
        {

        }
        public override ITestApi GetTestApi()
        {
            return new ImplementedApi();
        }

        [Fact]
        public void Test2()
        {

        }

        public class ImplementedApi : ITestApi
        {
            public void DoThing(string str)
            {
                
            }

            public int OtherThing(int a, int b)
            {
                return a;
            }
        }
    }
}