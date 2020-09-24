using Databox;
using UnityEngine;

namespace Data.Profile
{
    public class ProfileManager : MonoBehaviour
    {
        [SerializeField]
        private DataboxObjectManager databoxObjectManager = null;

        private DataboxObject databoxObject = null;

        private ProfileSaveFile ProfileSaveFile = null;

        private void Start()
        {
            if (databoxObjectManager != null)
            {
                databoxObject = databoxObjectManager.GetDataboxObject("Profile");
            }
        }

        public void ChangeSelectedProfile(int index)
        {
            if (databoxObject != null)
            {
                databoxObject.fileName = $"Profile{index + 1}.sav";
                ProfileSaveFile = new ProfileSaveFile(databoxObjectManager);
            }
        }

        public DataboxObject GetSelectedProfile()
        {
            return databoxObject;
        }
    }
}