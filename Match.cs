using LiteDB;

namespace Tichu_Counter
{
    public class Match
    {
        [BsonId]
        public int MatchID { get; set; }

        public string name_A { get; set; }

        public string name_B { get; set; }

        public int score_A { get; set; }

        public int score_B { get; set; }

        public List<int> rounds_a { get; set; }

        public List<int> rounds_b { get; set; }
        public bool selected { get; set; }

        public DateTime date { get; set; }

        [BsonRef]
        public Boolean isCreated = false;
        [BsonRef]
        public MatchBox box { get; set; }
        [BsonRef]
        DBManager dbManager = new DBManager();

        public Match() {}

        public async void UpdateMatch(int score_a, int score_b, bool remove)
        {
            try
            {
                //Remove a round
                if (remove)
                {
                    //Update Score
                    score_A -= rounds_a[rounds_a.Count - 1];
                    score_B -= rounds_b[rounds_b.Count - 1];

                    //Update Rounds List
                    rounds_a.RemoveAt(rounds_a.Count - 1);
                    rounds_b.RemoveAt(rounds_b.Count - 1);

                    date = DateTime.Now;
                    box.UpdateScores();
                }
                //Add a round
                else
                {
                    //Update Rounds List
                    rounds_a.Add(score_a);
                    rounds_b.Add(score_b);

                    //Update Score
                    score_A += score_a;
                    score_B += score_b;

                    date = DateTime.Now;
                    box.UpdateScores();
                }
            }
            catch(Exception ex) { 
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
