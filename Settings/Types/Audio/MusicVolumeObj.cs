using Databox;
using UnityEngine;
using Core;

namespace Data.Setting
{
    public class MusicVolumeObj : BaseSettingObj<FloatType, MusicVolumeObj>
    {
        public override string EntityName => "Audio";
        public override string DataName => "MusicVolume";

        public MusicVolumeObj(DataboxObject databoxObject, string tableName) : base(databoxObject, tableName)
        {
        }

        public override FloatType GetData()
        {
            return new FloatType(1f) { InitValue = 1f };
        }

        public override void OnValueChanged(FloatType databoxType)
        {
            GameStatics.AudioManager?.SetMusicVolume(databoxType.Value);
        }
    }
}