using BadaBingBot.Api;
using NUnit.Framework;

namespace BadaBingBot.Test.Extensions
{
    public static class SubscriptionFilterExtensions
    {
        public static void AssertMatches(this MessageFilter filter, Message message)
        {
            Assert.IsTrue(filter.Matches(message));
        }

        public static void AssertDoesNotMatch(this MessageFilter filter, Message message)
        {
            Assert.IsFalse(filter.Matches(message));
        }
    }
}
