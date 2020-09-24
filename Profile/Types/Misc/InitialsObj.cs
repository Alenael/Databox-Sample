using Databox;

namespace Data.Profile
{
    public class InitialsObj : BaseProfileObj<StringType, InitialsObj>
    {
        public override string EntityName => "Misc";
        public override string DataName => "Name";

        public InitialsObj(DataboxObject databoxObject, string tableName) : base(databoxObject, tableName)
        {
        }

        public override StringType GetData()
        {
            return new StringType();
        }
    }
}