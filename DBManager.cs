using LiteDB;

namespace Tichu_Counter
{
    public class DBManager 
    {
        public DBManager()
        {
        }

        public List<Match> GetMatches()
        {
            try
            {
                using (var db = new LiteDatabase(Path.Combine(FileSystem.AppDataDirectory, "tichu_counter_db.db")))
                {
                    var table = db.GetCollection<Match>("Matches");
                    return table.Query().OrderBy(x => x.date).ToList();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void DeleteAll()
        {
            using (var db = new LiteDatabase(Path.Combine(FileSystem.AppDataDirectory, "tichu_counter_db.db")))
            {
                db.DropCollection("Matches");
            }
        }
        public async Task AddMatch(Match match)
        {
            using (var db = new LiteDatabase(Path.Combine(FileSystem.AppDataDirectory, "tichu_counter_db.db")))
            {
                var table = db.GetCollection<Match>("Matches");
                table.Insert(match);
            }
        }
        public async Task UpdateMatch(Match match)
        {
            using (var db = new LiteDatabase(Path.Combine(FileSystem.AppDataDirectory, "tichu_counter_db.db")))
            {
                var table = db.GetCollection<Match>("Matches");
                table.Update(match);
            }
        }
        public async Task DeleteMatch(Match match)
        {
            using(var db = new LiteDatabase(Path.Combine(FileSystem.AppDataDirectory, "tichu_counter_db.db")))
            {
                var table = db.GetCollection<Match>("Matches");
                table.Delete(match.MatchID);
            }
        }

    }
}
