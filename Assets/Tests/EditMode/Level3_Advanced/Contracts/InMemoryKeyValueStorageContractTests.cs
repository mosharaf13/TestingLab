using TestingLab.Core.Common;
using TestingLab.Tests.Shared.Fakes;

namespace TestingLab.Tests.EditMode.Level3_Advanced.Contracts
{
    /// <summary>Runs the whole storage contract against the test fake.</summary>
    public class InMemoryKeyValueStorageContractTests : KeyValueStorageContract
    {
        protected override IKeyValueStorage CreateStorage() => new InMemoryKeyValueStorage();
    }
}
