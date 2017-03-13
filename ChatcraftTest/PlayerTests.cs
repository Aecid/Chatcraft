//using Chatcraft;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

//namespace ChatcraftTest
//{
//    [TestClass]
//    public class PlayerTests
//    {
//        private MockRepository mockRepository;



//        [TestInitialize]
//        public void TestInitialize()
//        {
//            this.mockRepository = new MockRepository(MockBehavior.Strict);


//        }

//        [TestCleanup]
//        public void TestCleanup()
//        {
//            this.mockRepository.VerifyAll();
//        }

//        [TestMethod]
//        public void TestMethod1()
//        {


//            Player player = this.CreatePlayer();


//        }

//        private Player CreatePlayer()
//        {
//            return new Player();
//        }
//    }
//}