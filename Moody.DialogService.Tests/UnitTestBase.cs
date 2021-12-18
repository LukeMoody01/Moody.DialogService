using Moq.AutoMock;

namespace Moody.Tests
{
    public class UnitTestBase<TSut> where TSut : class
    {
        public UnitTestBase()
        {
            Mocker = new AutoMocker(Moq.MockBehavior.Default, Moq.DefaultValue.Mock);
            Sut = Mocker.CreateInstance<TSut>();
        }

        public AutoMocker Mocker { get; }

        public TSut Sut { get; set; }
    }
}
