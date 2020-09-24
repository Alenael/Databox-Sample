using Databox;
using Core;

namespace Data.Setting
{
    public class VersionObj : BaseSettingObj<FloatType, VersionObj>
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