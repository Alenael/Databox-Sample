using Core;
using Databox;

namespace Data.Profile
{
    public class VersionObj : BaseProfileObj<FloatType, VersionObj>
    {
        public override string EntityName => "Misc";
        public override string DataName => "Version";

        public VersionObj(DataboxObject databoxObject, string tableName) : base(databoxObject, tableName)
        {
        }

        public override FloatType GetData()
        {
            return new FloatType(GameStatics.Version) { InitValue = GameStatics.Version };
        }
    }
}