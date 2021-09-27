using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExerciseMethodShareDtNt;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CreateExercises
{
    public class ExerciseDataFeed
    {
        public static List<ExerciseMethodShareDtNt.ResultBase> ResultList()
        {
            List<ExerciseMethodShareDtNt.ResultBase> rb = new List<ResultBase>();
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            //var collection = database.GetCollection<BsonDocument>("Exercises");
            var collection = database.GetCollection<BsonDocument>("ResultBase");
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (BsonDocument bd in documents)
            {
                ResultBase rBase = new ResultBase();
                rBase.Exercise_Name = bd.GetElement("Exercise Name").Value.ToString();
                rBase.Exercise_Type = bd.GetElement("Exercise Type").Value.ToString();
                rBase.Action = bd.GetElement("Action").Value.ToString();
                rBase.Name_Match = bd.GetElement("Name Match").Value.ToString();
                rBase.Searched_exercise = bd.GetElement("Searched Exercise").Value.ToString();
                rBase.Rank = (int)bd.GetElement("Rank").Value.ToInt64();
                rBase.Match = bd.GetElement("Match").Value.ToBoolean();
                rBase.FullMatch = bd.GetElement("Full Match").Value.ToBoolean();
                rb.Add(rBase);
                //res.Add(id, exName);
            }


            return rb;
        }

        public static List<Given_Rule> RuleList()
        {
            List<Given_Rule> res = new List<Given_Rule>();
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            //Dictionary<int, String> res = new Dictionary<int, string>();

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("LawBase");

            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument d in documents)
            {
                Given_Rule exA = new Given_Rule();

                exA.Exercise = d.GetElement("Action").Value.ToString();
                exA.Match_Case = d.GetElement("Match_Case").Value.ToString();
                exA.Rank = (int)d.GetElement("Rank").Value.ToInt64();
                exA.Exercise_ID = (int)d.GetElement("Exercise_Type_ID").Value.ToInt64();
                //res.Add(id, exName);
                res.Add(exA);
            }


            return res;

        }

        public static Dictionary<int, string> Exercise_Types_List()
        {

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            Dictionary<int, String> res = new Dictionary<int, string>();

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("ExerciseType");

            var documents = collection.Find(new BsonDocument()).ToList();

            //int t1 = (int)ExerciseTime.Ten;

            //double p = (double)t1;
            //p = Math.Pow(p, 2.00);

            foreach (BsonDocument d in documents)
            {
                string exName = d.GetElement("Exercise Type").Value.ToString();
                Int64 _Id = d.GetElement("Exercise_Type_ID").Value.ToInt64();

                int id = (int)_Id;
                res.Add(id, exName);
            }

            return res;
        }

        public static Dictionary<int, string> Routine_List(int Type_ID)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            Dictionary<int, String> res = new Dictionary<int, string>();

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercises");

            var filter = Builders<BsonDocument>.Filter.Eq("Exercise_Type_Id", Type_ID);

            var Documents = collection.Find(filter).ToList();

            foreach (BsonDocument d in Documents)
            {
                string exName = d.GetElement("Exercise Name").Value.ToString();
                Int64 _Id = d.GetElement("Exercise_ID").Value.ToInt64();

                int id = (int)_Id;
                res.Add(id, exName);
            }

            return res;
        }

        public static List<WorkOut> WorkOut_Regiment(int ExID)
        {
            List<WorkOut> res = new List<WorkOut>();

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            //Dictionary<int, String> res = new Dictionary<int, string>();

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Routines");

            var filter = Builders<BsonDocument>.Filter.Eq("Exercise_ID", ExID);

            var Documents = collection.Find(filter).ToList();

            foreach (BsonDocument bd in Documents)
            {
                WorkOut w = new WorkOut();


                w.Id = bd.GetElement("Routine_id").Value.ToInt32();
                w.Name = bd.GetElement("Exercise_Name").Value.ToString();
                w.Time = bd.GetElement("Exercise_Time").Value.ToInt32();
                //res.Add(id, exName);
                res.Add(w);
            }



            return res;
        }

        public static List<Food_Log> FoodLogs()
            {

            List<Food_Log> res = new List<Food_Log>();

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            //Dictionary<int, String> res = new Dictionary<int, string>();

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Food_Diary");

            //var filter = Builders<BsonDocument>.Filter.Eq("Exercise_ID", ExID);

            var Documents = collection.Find(new BsonDocument()).ToList(); ;

            foreach (BsonDocument bd in Documents)
            {
                Food_Log f = new Food_Log();


                f.Meal = bd.GetElement("Meal").Value.ToString();
                f.Meal_Description = bd.GetElement("Meal_Description").Value.ToString();
                f.Calorie_Count = bd.GetElement("Calorie_Count").Value.ToInt32();
                f.Date = bd.GetElement("Consumption_Date").Value.ToUniversalTime();
                //res.Add(id, exName);
                res.Add(f);
            }



            return res;


        }

        public static List<Exercise_Log> ExerciseLogs()
        {

            List<Exercise_Log> res = new List<Exercise_Log>();

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            //Dictionary<int, String> res = new Dictionary<int, string>();

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercise_Log");

            //var filter = Builders<BsonDocument>.Filter.Eq("Exercise_ID", ExID);

            var Documents = collection.Find(new BsonDocument()).ToList(); ;

            foreach (BsonDocument bd in Documents)
            {
                Exercise_Log ex = new Exercise_Log();


                ex.Exercise_ID = bd.GetElement("Exercise_ID").Value.ToInt32();
                ex.Exercise_Time = bd.GetElement("Exercise_Time").Value.ToInt32();
                ex.Calorie_Count = bd.GetElement("Calorie_ Count").Value.ToInt32();
                ex.Date = bd.GetElement("Exercise_Date").Value.ToUniversalTime();

                res.Add(ex);
            }



            return res;


        }

        public static void Make_Regiment_Record(int Exercise_ID , WorkOut w)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Routines");
            var collEx = database.GetCollection<BsonDocument>("Exercises");
            var docs = collEx.Find(new BsonDocument()).ToList();
            var documents = collection.Find(new BsonDocument()).ToList();

            var max = collEx.Find(new BsonDocument()).Sort(new BsonDocument("Exercise_ID", -1)).FirstOrDefault();
            Int32 ex_ID = max.GetElement("Exercise_ID").Value.ToInt32();
            

            var document = new BsonDocument { { "Routine_id", w.Id },
                { "Exercise_ID", ex_ID} ,
                { "Exercise_Name", w.Name} ,
                { "Exercise_Time",w.Time} };
            collection.InsertOne(document);
        }

        public static void Make_Exercise_Regiment(int Exercise_Type_ID, string name, int exTime)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercises");

            //var documents = collection.Find(new BsonDocument()).ToList();
            var max = collection.Find(new BsonDocument()).Sort(new BsonDocument("Exercise_ID", -1)).FirstOrDefault();
            Int32 ex_ID = max.GetElement("Exercise_ID").Value.ToInt32();
            ex_ID++;
            var document = new BsonDocument { { "Exercise_Type_Id", Exercise_Type_ID }, 
                { "Exercise_ID", ex_ID} ,
                { "Exercise Name", name} ,
                { "Exercise_Time", exTime} };
            collection.InsertOne(document);
        }

        public static void Delete_Exercise(int Exercise_ID)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercises");
            var collectionA = database.GetCollection<BsonDocument>("Routines");
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("Exercise_ID", Exercise_ID);
            collection.DeleteOne(deleteFilter);
            collectionA.DeleteMany(deleteFilter);
        }

        public static void Make_Result_Base(WorkOut w,int Type_ID ,string ExName)
        {
            List<Given_Rule> gr = RuleList();
            Dictionary<int, string> exTypes = Exercise_Types_List();

            string exType = (from exT in exTypes where exT.Key == Type_ID select exT.Value).FirstOrDefault();

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercises");

            var documents = collection.Find(new BsonDocument()).ToList();
            //var collA = database.GetCollection<BsonDocument>("Routines");
            var collB = database.GetCollection<BsonDocument>("ResultBase");
            //var filter = Builders<BsonDocument>.Filter.Eq("Exercise_ID", 114);
            //var lastScore = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("Exercise_ID", new BsonDocument  {{ "$gte", 114 }  });
            //var docsA = collA.Find(filter).ToList();
            //var docsA = collA.Find(lastScore).ToList();
            var docsB = collB.Find(new BsonDocument()).ToList();

            foreach (Given_Rule g in gr)
            {
                if (w.Name.Contains(g.Exercise) && (g.Exercise_ID == Type_ID || Type_ID == 4 ))
                {
                    ResultBase rb = new ResultBase();
                    rb.Match = true;
                    rb.Name_Match = g.Match_Case;
                    rb.Exercise_Name = w.Name;
                    rb.Action = g.Exercise;
                    rb.Rank = g.Rank;
                    rb.Exercise_Type = exType;
                    rb.Searched_exercise = ExName;
                    rb.FullMatch = true;
                    //res.Add(rb);
                    var docb = new BsonDocument {
                                { "Exercise Name" ,rb.Exercise_Name },
                                {"Exercise Type", rb.Exercise_Type },
                                {"Action", rb.Action},
                                {"Full Match", rb.FullMatch },
                                {"Name Match", rb.Name_Match },
                                {"Match" , rb.Match },
                                {"Rank", rb.Rank },
                                {"Searched Exercise", rb.Searched_exercise }
                            };
                    if (rb.Rank < 2)
                    {
                        collB.InsertOne(docb);
                    }
                }
            }

            
        }

        public static void Make_Log_Entry(int Type_ID, int Ex_Id, double time)
        {
            Dictionary<int, string> exTypes = Exercise_Types_List();

            string exType = (from exT in exTypes where exT.Key == Type_ID select exT.Value).FirstOrDefault();

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            var collB = database.GetCollection<BsonDocument>("Exercise_Log");

            DateTime dt = DateTime.Now;
            DateTime ut = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            var docb = new BsonDocument {
                                { "Exercise_ID" ,Ex_Id},
                                {"Calorie_ Count", fn_SetCalCount(Type_ID, time) },
                                {"Exercise_Date", ut},
                                {"Exercise_Time",time }
                               
                            };
            collB.InsertOne(docb);
        }

        public static void Make_Food_Entry(ExerciseMethodShareDtNt.Food_Log f)
        {
            Dictionary<int, string> exTypes = Exercise_Types_List();

            

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            var collB = database.GetCollection<BsonDocument>("Food_Diary");

           
            var docb = new BsonDocument {
                                { "Meal" ,f.Meal},
                                {"Calorie_ Count", f.Calorie_Count },
                                {"Consumption_Date", f.Date},
                                {"Meal_Description",f.Meal_Description }

                            };
            collB.InsertOne(docb);
        }



        private static int fn_SetCalCount(int type_ID, double time)
        {
            double ratio = 0;
            if (type_ID == 4)
            {
                ratio = Properties.Settings.Default.FB_Cal_Ratio;
                    }
            else
            {
                ratio = Properties.Settings.Default.EX_Cal_Ratio;
            }

            return (int)(time * ratio);
        }
    }
}
