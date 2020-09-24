using Databox;
using UnityEngine;
using Core;

namespace Data.Setting
{
    public class MuteSettingObj : BaseSettingObj<BoolType, MuteSettingObj>
    {
        public override string EntityName => "Audio";
        public override string DataName => "Mute";

        public MuteSettingObj(DataboxObject databoxObject, string tableName) : base(databoxObject, tableName)
        {
        }

        public override BoolType GetData()
        {
            return new BoolType(false);
        }

        public override void OnValueChanged(BoolType databoxType)
        {
            if (databoxType.Value)
            {
                AudioListener.volume = 0f;
            }
            else
            {
                var masterVolumeData = UserSettingsItemInfo.DataboxObject.GetData<FloatType>(UserSettingsItemInfo.TableName, "Audio", "MasterVolume");
                AudioListener.volume = masterVolumeData.Value;
            }
        }
    }
}