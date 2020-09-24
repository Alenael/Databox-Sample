using Databox;
using System;
using System.Linq;

namespace Data.Profile
{
    /// <summary>
    /// Allows use of Reflection to automatically find classes which inherit from it and add them to a databox table
    /// </summary>
    public abstract class BaseProfileObj
    {
        public static void InsertAllProfileData(DataboxObject settingsFile, string tableName)
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            System.Type[] found = (from System.Type type in types
                                   where type.IsSubclassOf(typeof(BaseProfileObj))
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
    public abstract class BaseProfileObj<T, T2> : BaseProfileObj where T : DataboxType
    {
        #region Varibales

        public virtual string EntityName => "";
        public virtual string DataName => "";

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a record in our databox object and sets up an event to it's OnChange
        /// </summary>
        /// <param name="databoxObject">Databox Object we want to insert into</param>
        /// <param name="tableName">Name of table we are inserting data into</param>
        public BaseProfileObj(DataboxObject databoxObject, string tableName)
        {
            //Add Data
            databoxObject.AddData(tableName, EntityName, DataName, GetData());
        }

        #endregion

        #region Overrides

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