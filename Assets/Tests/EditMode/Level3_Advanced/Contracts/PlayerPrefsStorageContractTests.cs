using TestingLab.Core.Common;
using TestingLab.Gameplay.Infrastructure;

namespace TestingLab.Tests.EditMode.Level3_Advanced.Contracts
{
    /// <summary>
    /// Runs the whole storage contract against the REAL PlayerPrefs. It only ever
    /// touches its own test key and deletes it afterwards, so your save is safe.
    /// </summary>
    public class PlayerPrefsStorageContractTests : KeyValueStorageContract
    {
        protected override IKeyValueStorage CreateStorage() => new PlayerPrefsStorage();
    }
}
