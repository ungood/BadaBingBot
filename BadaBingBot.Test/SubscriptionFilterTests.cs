using BadaBingBot.Api;
using BadaBingBot.Test.Extensions;
using NUnit.Framework;

namespace BadaBingBot.Test
{
    [TestFixture]
    public class SubscriptionFilterTests
    {
        [Test]
        public void EmptyFilterMatchesAllMessages()
        {
            var filter = new MessageFilter();
            
            filter.AssertMatches(new Message());
            
            filter.AssertMatches(new Message("source"));
            
            filter.AssertMatches(new Message("source") {
                {"dummy", "value"}
            });
        }

        [Test]
        public void FilterBySource()
        {
            var filter = new MessageFilter {
                Source = "jenkins"
            };

            filter.AssertDoesNotMatch(new Message());
           
            filter.AssertMatches(new Message("jenkins"));
            
            filter.AssertDoesNotMatch(new Message("hudson"));
        }

        [Test]
        public void FilterByProperties()
        {
            var filter = new MessageFilter {
                {"hello", "world"}
            };

            filter.AssertDoesNotMatch(new Message());
            
            filter.AssertMatches(new Message {
                {"hello", "world"}
            });
            
            filter.AssertDoesNotMatch(new Message {
                {"hello", "foobar"}
            });
        }

        [Test]
        public void FilterByPropertyRegex()
        {
            var filter = new MessageFilter {
                {"hello", ".*"}
            };

            filter.AssertDoesNotMatch(new Message());
            
            filter.AssertMatches(new Message {
                {"hello", "world"}
            });
            
            filter.AssertMatches(new Message {
                {"hello", "foobar"}
            });
        }

        [Test]
        public void FilterByMultipleProperties()
        {
            var filter = new MessageFilter {
                {"hello", "world"},
                {"anything", ".*"}
            };

            filter.AssertDoesNotMatch(new Message());
            
            filter.AssertDoesNotMatch(new Message {
                {"hello", "world"}
            });

            filter.AssertDoesNotMatch(new Message {
                {"anything", "at all"}  
            });

            filter.AssertMatches(new Message {
                {"hello", "world"},
                {"anything", "at all"}
            });
        }

        [Test]
        public void FilterBySourceAndProperties()
        {
            var filter = new MessageFilter
            {
                {"hello", "world"}
            };
            filter.Source = "jenkins";

            filter.AssertDoesNotMatch(new Message());

            filter.AssertDoesNotMatch(new Message {
                {"hello", "world"}
            });

            filter.AssertDoesNotMatch(new Message("jenkins"));

            filter.AssertMatches(new Message("jenkins") {
                {"hello", "world"},
            });
        }
    }
}
