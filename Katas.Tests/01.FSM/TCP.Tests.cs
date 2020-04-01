using Katas.Console._01.FSM;
using Xunit;

namespace Katas.Tests._01.FSM
{
    public class TCPTests
    {
        [Fact]
        public void TraverseStatesTest()
        {
            Assert.Equal("CLOSE_WAIT", TCP.TraverseStates(new[] { "APP_ACTIVE_OPEN", "RCV_SYN_ACK", "RCV_FIN" }));
            Assert.Equal("ESTABLISHED", TCP.TraverseStates(new[] { "APP_PASSIVE_OPEN", "RCV_SYN", "RCV_ACK" }));
            Assert.Equal("LAST_ACK", TCP.TraverseStates(new[] { "APP_ACTIVE_OPEN", "RCV_SYN_ACK", "RCV_FIN", "APP_CLOSE" }));
            Assert.Equal("SYN_SENT", TCP.TraverseStates(new[] { "APP_ACTIVE_OPEN" }));
            Assert.Equal("ERROR", TCP.TraverseStates(new[] { "APP_PASSIVE_OPEN", "RCV_SYN", "RCV_ACK", "APP_CLOSE", "APP_SEND" }));
        }
    }
}
