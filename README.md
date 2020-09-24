# Databox Sample
 Small sample project with which shows how to expand Databox to better support Saves, Settings Files, Profile Data, etc. which may change as development continues. 
 
# Why was this made?
One of my largest issues I found in DataBox was that there was no easy ways to migrate from one version of a config, save file, or whatever you need on the Clients machine to the next version of the file. Typically in development (or even in testing) you add new flags or data to a config file and updating one that already exists or using an outdated one can be challenging at times. This system helps to ease that process a bit but doesn't eliminate all issues around the topic so please do not consider it a complete system or the best system you could come up with. 
 
# What does this example help to accomplish?
Provides ability to create profile, save, and user setting files based off actual code. This system allows you to code each feature individually from one another and get them importing in automatically. It can really help distinguish code from one another and not feel like your running in circles as your game grows by keeping code seperated for each thing you need in your database. It also allows all functionality Databox has to offer even the ability to update all these values (and see them as they change!) during run time via the Databox Editor.

# How does it work?
Using SYstem.Reflection and some basic classes you can create each feature which is needed for your database in code using simple code. This is the primary way we create features which get stored into the DataboxObject. In order to create this class it needs an EntityName, DataName, and you must define a GetData() return of the type you are wanting to save to the DataboxObject. This type can be anything as well as DataboxObject is accepting of custom built classes (even if it doesn't render properly in the UI).

This example stores a Version Number in the DataboxObject which in turn is saved into the file. 
```csharp
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
```

Here is an example of a setting which can control the Master Volume. This one is a bit more complicated than the prior one as it also overrides OnValueCHanged which is from the base class BaseSettingsObj and allows you to perform an action when the value changes. Here I am adjusting the AudioManager system which controls the Mixer for my game.
```csharp
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
```

Things get a bit more complicated here as we dive into the BaseSettingsObj. This class has a bit of comments within to help guide you. Unfourtantly I do not have an easy way to build this class up such it could be reused for multiple systems. Instead I opted to just create BaseSettingsObj and BaseProfileObj for this project to design a Profile saving system and a User Settings saving system to function with Databox.

The first class, BaseSettingsObj, is a base class in charge of using System.Reflection to find all classes which inherit from it and automatically insert them into the DataboxObject at run time. This is really quick and I wouldn't expect problems with this unless you had massive amounts of data but I have not tested it extensivly with a large amount of data. 

The next class BaseSettingObj<T, T2> is setup to be able to accept objects generically. This allows you to specify two very important pieces of information about each item you want to insert into Databox.

* T defines the type of object that will be contained within Databox. This is typically a StringType, BoolType, IntType, etc. and can also include custom types you have created.
* T2 defines the class itself you want to be creating and is used by the first class, BaseSettingsObj, to create instances of each of the DatabaseObject types you will be inserting.

```csharp
using Databox;
using System;
using System.Linq;

namespace Data.Setting
{
    /// <summary>
    /// Allows use of Reflection to automatically find classes which inherit from it and add them to a databox table
    /// </summary>
    public abstract class BaseSettingObj
    {
        public static void InsertAllSettingsData(DataboxObject settingsFile, string tableName)
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            System.Type[] found = (from System.Type type in types
                                   where type.IsSubclassOf(typeof(BaseSettingObj))
                                   && !type.IsAbstract
                                   select type).ToArray();

            foreach (var item in found)
                item.BaseType.GetMethod("InsertInto").Invoke(item, new object[] { settingsFile, tableName });
        }
    }

    /// <summary>
    /// Dynamically creates an object and adds data to a databox object based on provided types
    /// </summary>
    /// <typeparam name="T">Something which inherits from DataboxType</typeparam>
    /// <typeparam name="T2">The return object type when calling InsertInto to dynamically create the record. Ususally the class itself.</typeparam>
    public abstract class BaseSettingObj<T, T2> : BaseSettingObj where T : DataboxType
    {
        #region Varibales

        public virtual string EntityName => "";
        public virtual string DataName => "";

        protected UserSettingsItemInfo UserSettingsItemInfo = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a record in our databox object and sets up an event to it's OnChange
        /// </summary>
        /// <param name="databoxObject">Databox Object we want to insert into</param>
        /// <param name="tableName">Name of table we are inserting data into</param>
        public BaseSettingObj(DataboxObject databoxObject, string tableName)
        {
            //Finish setting up DB Info
            UserSettingsItemInfo = new UserSettingsItemInfo
            {
                DataboxObject = databoxObject,
                TableName = tableName,
                DataType = GetData()
            };

            //Add Data
            UserSettingsItemInfo.DataboxObject.AddData(UserSettingsItemInfo.TableName, EntityName, DataName, UserSettingsItemInfo.DataType);

            //Grab our data
            var dataObj = UserSettingsItemInfo.DataboxObject.GetData<T>(UserSettingsItemInfo.TableName, EntityName, DataName);

            //Setup event and execute it once
            dataObj.OnValueChanged = d => OnValueChanged(d as T);
            OnValueChanged(dataObj);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Is triggered every time the value changes in Databox
        /// </summary>
        /// <param name="databoxType">DataboxType Object</param>
        public virtual void OnValueChanged(T databoxType)
        {
        }

        /// <summary>
        /// Used to build the DataboxType Object for this class
        /// </summary>
        /// <returns>DataboxType Object</returns>
        public abstract T GetData();

        #endregion

        #region Static Methods

        /// <summary>
        /// Inserts Data into a DataboxObject
        /// </summary>
        /// <param name="databoxObject">Databox Object we want to insert into</param>
        /// <param name="tableName">Name of table we are inserting data into</param>
        /// <returns>Returns the created class once data has been inserted and event has been registered</returns>
        public static T2 InsertInto(DataboxObject databoxObject, string tableName)
        {
            return (T2)Activator.CreateInstance(typeof(T2), databoxObject, tableName);
        }

        #endregion
    }
}
```

Finally the last bit is actually setting up the Databox object which is done using the script below. All you need to do is attach this to a GameObject and it will ensure the User Settings file gets created and it automatically generates with all User Settings inside it. If the file already exists it is loaded up before hand into the DataboxObject and only items which are missing from it are inserted. 

```csharp
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
```

# What Else is Contained in the Porject?
A profile system built using the same setup as the above user settings system.

# Things I could Improve Upon?
I bet there is plenty here I could improve upon and am happy to hear back about anything you find here or questions you have. Feel free to reach out to me on GitHub or on Discord at Alenael#1801.
