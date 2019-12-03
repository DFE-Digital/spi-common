using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;

namespace Dfe.Spi.Common.UnitTesting.Fixtures
{
    public class NonRecursiveAutoDataAttribute : AutoDataAttribute
    {
        public NonRecursiveAutoDataAttribute()
            : base(CreateOmitOnRecursionFixture)
        {
            
        }
        
        private static Fixture CreateOmitOnRecursionFixture()
        { 
            // from https://github.com/AutoFixture/AutoFixture/issues/337
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior()); // recursionDepth

            return fixture;
        }
    }
}