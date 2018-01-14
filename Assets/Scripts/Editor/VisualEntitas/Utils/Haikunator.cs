using System;
using System.Linq;

namespace Atrox
{
    public class Haikunator
    {
        private static Haikunator _instance;

        public static string Random()
        {
            _instance = _instance ?? new Haikunator();
            return _instance.Haikunate();
        }

        private readonly Random _random;

        public string[] Adjectives =
        {
            "aged", "ancient", "autumn", "billowing", "bitter", "black", "blue", "bold",
            "broad", "broken", "calm", "cold", "cool", "crimson", "curly", "damp",
            "dark", "dawn", "delicate", "divine", "dry", "empty", "falling", "fancy",
            "flat", "floral", "fragrant", "frosty", "gentle", "green", "hidden", "holy",
            "icy", "jolly", "late", "lingering", "little", "lively", "long", "lucky",
            "misty", "morning", "muddy", "mute", "nameless", "noisy", "odd", "old",
            "orange", "patient", "plain", "polished", "proud", "purple", "quiet", "rapid",
            "raspy", "red", "restless", "rough", "round", "royal", "shiny", "shrill",
            "shy", "silent", "small", "snowy", "soft", "solitary", "sparkling", "spring",
            "square", "steep", "still", "summer", "super", "sweet", "throbbing", "tight",
            "tiny", "twilight", "wandering", "weathered", "white", "wild", "winter", "wispy",
            "withered", "yellow", "young","nutty","windy","hard","jumpy","humdrum","stormy",
            "intelligent","easy","eager","petite","excited","selfish","swift","evanescent",
            "unnatural","abundant","scintillating","ubiquitous","stimulating",
            "separate","military","thirsty","racial","smiling","future","obnoxious","daily","curvy",
            "faded","verdant","unbecoming","chief","alive","mighty","tan","subdued","political","ablaze",
            "wealthy","wandering","hollow","boundless","threatening","screeching","thoughtless","empty",
            "lucky","psychotic","judicious","scary","heavy","omniscient","nimble","succinct","plant",
            "uppity","acceptable","gamy","lush","worried","public","majestic","didactic","aback","skinny",
            "sneaky","dizzy","better","able","aboard","massive","tiresome","infamous","tart","tacit",
            "optimal","petite","luxuriant","deserted","belligerent","legal","macho","noiseless",
            "ruddy","careful","taboo","tiny","ad hoc","mere","malicious","alcoholic","unwritten",
            "detailed","telling","tangy","scared","feigned","humorous","ambiguous","vast","imported","grotesque",
            "creepy","faithful","great","arrogant","dramatic","ambitious","glistening","glamorous","ratty",
            "wrong","equal","statuesque","zealous","known","spiritual","overjoyed",
            "tedious","nervous","hideous","salty","silky","cluttered","puzzling",
            "brave","certain","harsh","jobless","scandalous","odd","tender",
            "early","kind","jaded","dear","labored","superficial","lively","secret",
            "third","imaginary","disgusting","dusty","annoyed","perfect","joyous",
            "symptomatic","sparkling","rural","wry","dynamic","tidy","breezy","discreet",
            "aquatic","rampant","furry","scientific","lame","impossible","unusual","condemned",
            "gigantic","spiteful","wacky","dependent","fanatical","jolly",
            "wide","mute","victorious","chubby","periodic","responsible",
            "electric","warlike","parched","comfortable","brainy","superb","draconian",
            "impartial","vagabond","synonymous","open","helpless","bashful","glib",
            "smoggy","six","obtainable","successful","ugly","boorish",
            "somber","spurious","handsomely","fearless","quaint","yellow","uneven",
            "straight","painstaking","frail","womanly","adjoining",
            "ajar","productive","gifted","whispering","ten","dead","last",
            "incredible","fertile","neighborly","interesting","used","misty",
            "terrific","many","outgoing","frightening","quirky",
            "hapless","wistful","medical","absent","premium","ragged","hot","quixotic",
            "dangerous","bumpy","wasteful","damaging","hushed","swanky","utopian",
            "direful","tricky","gainful","truculent"};

        public string[] Nouns =
        {
            "art", "band", "bar", "base", "bird", "block", "boat", "bonus",
            "bread", "breeze", "brook", "bush", "butterfly", "cake", "cell", "cherry",
            "cloud", "credit", "darkness", "dawn", "dew", "disk", "dream", "dust",
            "feather", "field", "fire", "firefly", "flower", "fog", "forest", "frog",
            "frost", "glade", "glitter", "grass", "hall", "hat", "haze", "heart",
            "hill", "king", "lab", "lake", "leaf", "limit", "math", "meadow",
            "mode", "moon", "morning", "mountain", "mouse", "mud", "night", "paper",
            "pine", "poetry", "pond", "queen", "rain", "recipe", "resonance", "rice",
            "river", "salad", "scene", "sea", "shadow", "shape", "silence", "sky",
            "smoke", "snow", "snowflake", "sound", "star", "sun", "sun", "sunset",
            "surf", "term", "thunder", "tooth", "tree", "truth", "union", "unit",
            "violet", "voice", "water", "water", "waterfall", "wave", "wildflower", "wind",
            "wood",    "union","police","physics","argument","dinner","importance",
            "recipe","error","client","surgery","procedure","arrival","intention",
            "definition","ad","player","hair","childhood","instance","conclusion",
            "platform","efficiency","photo","honey","breath","hat","bedroom",
            "lake","negotiation","queen","analysis","committee","property",
            "population","contract","government","road","employment","news",
            "debt","movie","energy","attitude","television","night",
            "trainer","meaning","direction","ratio","revenue","inflation","camera",
            "guitar","situation","singer","cell","cheek","product",
            "mud","variety","alcohol","editor","way","girl","understanding",
            "flight","advertising","housing","guidance","departure","security",
            "mode","hotel","speech","friendship","criticism","presence","failure",
            "story","member","grandmother","dad","mall","agreement","tension",
            "law","variation","activity","area","response","recommendation","complaint",
            "investment","preparation","application","resolution","employer","audience",
            "event","supermarket","literature","county","ability","reading","apple",
            "dealer","memory","guest","operation","engineering","nation","sample",
            "anxiety","emotion","king","community","idea","location","estate","family",
            "inspection","quality","economics","truth","statement","measurement","region",
            "introduction","fact","competition","thing","psychology","excitement","membership",
            "software","drawing","championship","chemistry","disease","satisfaction","phone",
            "cabinet","depth","church","bonus","significance","society","hospital",
            "basis","article","heart","setting","collection","analyst","selection",
            "media","woman","airport","assignment","video","piano","city","volume","fortune",
            "responsibility","computer","marriage","worker","magazine","engine",
            "loss","presentation","difficulty","basket","association","university","refrigerator",
            "teaching","philosophy","temperature","emphasis","garbage","profession","lady",
            "vehicle","industry","contribution","addition","disaster","reception","revolution",
            "obligation","explanation","year","poem","reaction","candidate","actor","maintenance","conversation"};

        public Haikunator()
        {
            _random = new Random();
        }

        public Haikunator(int seed)
        {
            _random = new Random(seed);
        }

        /// <summary>
        ///     Generate Heroku-like random names
        /// </summary>
        /// <param name="delimiter">Delimiter</param>
        /// <param name="tokenLength">Token Length</param>
        /// <param name="tokenHex">Token Hex (true/false)</param>
        /// <param name="tokenChars">Token Chars</param>
        /// <returns>Returns heroku-like string</returns>
        public string Haikunate(string delimiter = "", int tokenLength = 0, bool tokenHex = false,
            string tokenChars = "0123456789")
        {
            if (tokenHex) tokenChars = "0123456789abcdef";

            var adjective = RandomString(Adjectives);
            adjective = FirstCharToUpper(adjective);
            var noun = RandomString(Nouns);
            noun = FirstCharToUpper(noun);

            var token = "";
            if (tokenChars.Length > 0)
            {
                for (var i = 0; i < tokenLength; i++)
                {
                    token += tokenChars[_random.Next(tokenChars.Length)];
                }
            }

            string[] sections = { adjective, noun, token };
            return string.Join(delimiter, sections.Where(s => !string.IsNullOrEmpty(s)).ToArray());
        }

        private string RandomString(string[] s)
        {
            var length = s.Length;
            if (length <= 0) return "";

            return s[_random.Next(length)];
        }

        private string FirstCharToUpper(string input)
        {
            switch (input)
            {
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}