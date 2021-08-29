using Xunit;

namespace PackagedTests
{

    public abstract class PackagedTests
    {
        public abstract ITestApi GetTestApi();

        protected ITestApi _testApi;

        public PackagedTests(int baseline)
        {
            _testApi = GetTestApi();
        }

        [Fact]
        public void Test1()
        {
            Assert.Contains(_testApi.OtherThing(1, 2), new[] { 1, 2 });
        }

        public interface ITestApi
        {
            void DoThing(string str);
            int OtherThing(int a, int b);
        }
    }
}