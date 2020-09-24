using Core;
using Databox;

namespace Data.Setting
{
    public class MasterVolumeObj : BaseSettingObj<FloatType, MasterVolumeObj>
    {
        public override string EntityName => "Audio";
        public override string DataName => "MasterVolume";

        public MasterVolumeObj(DataboxObject databoxObject, string tableName) : base(databoxObject, tableName)
        {
        }

        public override FloatType GetData()
        {
            return new FloatType(1f) { InitValue = 1f };
        }

        public override void OnValueChanged(FloatType databoxType)
        {
            GameStatics.AudioManager?.SetMasterVolume(databoxType.Value);
        }
    }
}