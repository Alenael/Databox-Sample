using Databox;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Data.Setting
{
    public class UserSettingsFile : MonoBehaviour
    {
        [SerializeField]
        protected DataboxObjectManager databoxManager = null;
        protected DataboxObject SettingsFileDb;

        public const string CONFIG_TABLE_NAME = "Config";

        private void Start()
        {
            SettingsFileDb = databoxManager.GetDataboxObject("UserSettings");
            var settingsExists = File.Exists(SettingsFileDb.savePath);

            if (settingsExists)
                SettingsFileDb.LoadDatabase();

            BaseSettingObj.InsertAllSettingsData(SettingsFileDb, CONFIG_TABLE_NAME);
            SettingsFileDb.SaveDatabase();
        }
    }
}