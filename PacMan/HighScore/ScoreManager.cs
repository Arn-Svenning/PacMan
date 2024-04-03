using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Grid.HighScore
{
    internal class ScoreManager
    {
        private static string fileName = "scores.xml"; // Den kommer i "bin".

        public List<Score> Highscores
        {
            get; private set;
        }
        public List<Score> Scores
        {
            get; private set;
        }

        public ScoreManager() : this(new List<Score>())
        {

        }

        public ScoreManager(List<Score> scores)
        {
            Scores = scores;

            UpdateHighscores();
        }

        public void Add(Score score)
        {
            Scores.Add(score);

            Scores = Scores.OrderByDescending(c => c.Value).ToList(); // Ordnar listan så att de högsta poängen kommer först.

            UpdateHighscores();
        }

        public static ScoreManager Load()
        {
            // Om det inte finns en fil för Highscore så skapas en ny.
            if (!File.Exists(fileName))
                return new ScoreManager();

            // Annars laddas den

            using (var reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                var serilizer = new XmlSerializer(typeof(List<Score>));

                var scores = (List<Score>)serilizer.Deserialize(reader);

                return new ScoreManager(scores);
            }
        }

        public void UpdateHighscores()
        {
            Highscores = Scores.Take(5).ToList(); // Tar dem 5 högsta värdena i score-listan
        }

        public static void Save(ScoreManager scoreManager)
        {
            // Skriver över filen om den redan finns.
            using (var writer = new StreamWriter(new FileStream(fileName, FileMode.Create)))
            {
                var serilizer = new XmlSerializer(typeof(List<Score>));

                serilizer.Serialize(writer, scoreManager.Scores);
            }
        }
    }
}
