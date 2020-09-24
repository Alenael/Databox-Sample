using Databox;
using Core;

namespace Data.Setting
{
    public class SFXVolumeObj : BaseSettingObj<FloatType, SFXVolumeObj>
    {
        public override string EntityName => "Audio";
        public override string DataName => "SFXVolume";

        public SFXVolumeObj(DataboxObject databoxObject, string tableName) : base(databoxObject, tableName)
        {
        }

        public override FloatType GetData()
        {
            return new FloatType(1f) { InitValue = 1f };
        }

        public override void OnValueChanged(FloatType databoxType)
        {
            GameStatics.AudioManager.SetSFXVolume(databoxType.Value);
        }
    }
}