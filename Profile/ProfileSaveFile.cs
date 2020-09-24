using Databox;
using System.IO;

namespace Data.Profile
{
    public class ProfileSaveFile
    {
        private DataboxObject DataboxObject;

        public const string CONFIG_TABLE_NAME = "Profile";

        public ProfileSaveFile(DataboxObjectManager DataboxManager)
        {
            DataboxObject = DataboxManager.GetDataboxObject(CONFIG_TABLE_NAME);

            if (DataboxObject != null)
            {
                var profileExists = File.Exists(DataboxObject.savePath);

                if (profileExists)
                    DataboxObject.LoadDatabase();

                BaseProfileObj.InsertAllProfileData(DataboxObject, CONFIG_TABLE_NAME);
                DataboxObject.SaveDatabase();
            }
        }
    }
}