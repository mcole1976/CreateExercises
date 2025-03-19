using ExerciseMethodShareDtNt;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;

namespace CreateExercises
{
    public class ExerciseDataFeed
    {



        #region List Calls
        public static List<string> MakeContainsList()
        {
            List<string> res = new List<string>();
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Routines");

            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (BsonDocument d in documents)
            {
                res.Add(d.GetElement("Exercise_Name").Value.ToString());
            }

            return res.Distinct().ToList();
        }
        public static List<string> MakeFoodList()
        {
            List<string> res = new List<string>();
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Food_Diary");

            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (BsonDocument d in documents)
            {
                res.Add(d.GetElement("Meal_Description").Value.ToString());
            }

            return res.Distinct().ToList();
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
                try
                {
                    List<string> nonExs = new List<string>();
                    nonExs.Add("Football");
                    nonExs.Add("Cricket");
                    nonExs.Add("Cycling");
                    nonExs.Add("Daily Cycle");
                    nonExs.Add("Golf");
                    nonExs.Add("Walk");
                    nonExs.Add("Walking");


                    string exName = d.GetElement("Exercise Name").Value.ToString();
                    Int64 _Id = d.GetElement("Exercise_ID").Value.ToInt64();
                    bool doesNotExist = nonExs.Contains(exName, StringComparer.OrdinalIgnoreCase);
                    if(!doesNotExist)
                     {
                        int id = (int)_Id;
                        res.Add(id, exName);
                    }

                }
                catch ( Exception ex  )
                {
                    Console.WriteLine(d.GetElement("Exercise_ID").Value.ToInt64().ToString());
                }
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
        public static List<FoodAll> FoodLogsBson(int tFrame)
        {
            List<FoodAll> res = new List<FoodAll>();
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Food_Diary");
            DateTime currentDate = DateTime.UtcNow;
            DateTime oneWeekAgo = currentDate.AddDays(-tFrame);
            var filter = Builders<BsonDocument>.Filter.Gte("Consumption_Date", oneWeekAgo) &
                         Builders<BsonDocument>.Filter.Lte("Consumption_Date", currentDate);
            var lastWeekExercises = collection.Find(filter).ToList();
            foreach (var document in lastWeekExercises)
            {
                FoodAll food = new FoodAll
                {
                    Id = document.GetElement("_id").Value.ToString(),
                    Meal = document.GetElement("Meal").Value.ToString(),
                    MealDescription = document.GetElement("Meal_Description").Value.ToString(),
                    CalorieCount = document.GetElement("Calorie_Count").Value.ToInt32(),
                    ConsumptionDate = document.GetElement("Consumption_Date").Value.ToUniversalTime()
                };
                res.Add(food);
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
                Exercise_Log ex = new Exercise_Log
                {
                    Exercise_ID = bd.GetElement("Exercise_ID").Value.ToInt32(),
                    Exercise_Time = bd.GetElement("Exercise_Time").Value.ToInt32(),
                    Calorie_Count = bd.GetElement("Calorie_Count").Value.ToInt32(),
                    Date = bd.GetElement("Exercise_Date").Value.ToUniversalTime()
                };
                res.Add(ex);
            }

            return res;
        }
        public static List<ExerciseAll> JSONExerciseAll(int tFrame)
        {
            List<ExerciseAll> res = new List<ExerciseAll>();
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercise_Log");

            DateTime currentDate = DateTime.UtcNow;
            DateTime oneWeekAgo = currentDate.AddDays(-tFrame);

            var filter = Builders<BsonDocument>.Filter.Gte("Exercise_Date", oneWeekAgo) &
                         Builders<BsonDocument>.Filter.Lte("Exercise_Date", currentDate);

            var lastWeekExercises = collection.Find(filter).ToList();

            foreach (var document in lastWeekExercises)
            {
                ExerciseAll exercise = new ExerciseAll
                {
                    Id = document.GetElement("_id").Value.ToString(),
                    ExerciseId = document.GetElement("Exercise_ID").Value.ToInt32(),
                    CalorieCount = document.GetElement("Calorie_Count").Value.ToInt32(),
                    ExerciseDate = document.GetElement("Exercise_Date").Value.ToUniversalTime(),
                    ExerciseTime = document.GetElement("Exercise_Time").Value.ToInt32(),
                    Exercise_Name = (ExerciseName(document.GetElement("Exercise_ID").Value.ToInt32()))
                };
                res.Add(exercise);
            }

            return res;
        }
        public static List<ExerciseMethodShareDtNt.Task> Tasks()
        {

            List<ExerciseMethodShareDtNt.Task> r = new List<ExerciseMethodShareDtNt.Task>();
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);


            var database = dbClient.GetDatabase("ExerciseDB");
            //var collection = database.GetCollection<BsonDocument>("Exercises");
            var collection = database.GetCollection<BsonDocument>("Tasks");
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (BsonDocument bd in documents)
            {

                {
                    ExerciseMethodShareDtNt.Task t = new ExerciseMethodShareDtNt.Task();


                    t.Id = bd.GetElement("Task").Value.ToInt32();
                    t.Date = bd.GetElement("Time").Value.ToUniversalTime();
                    r.Add(t);
                };

            }
            return r;
        }
        public static List<ExerciseMethodShareDtNt.Goal> Goals()
        {

            List<ExerciseMethodShareDtNt.Goal> r = new List<ExerciseMethodShareDtNt.Goal>();
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);


            var database = dbClient.GetDatabase("ExerciseDB");
            //var collection = database.GetCollection<BsonDocument>("Exercises");
            var collection = database.GetCollection<BsonDocument>("Goals");
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (BsonDocument bd in documents)
            {

                {
                    ExerciseMethodShareDtNt.Goal g = new ExerciseMethodShareDtNt.Goal();


                    g.Id = bd.GetElement("ID").Value.ToInt32();
                    g.Name = bd.GetElement("Task").Value.ToString();
                    r.Add(g);
                };

            }
            return r;
        }
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
        #endregion
        #region auxillary
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
                Int64 _Id = d.GetElement("Exercise_Type_Id").Value.ToInt64();

                int id = (int)_Id;
                res.Add(id, exName);
            }

            return res;
        }
        public static string ExerciseName(int ID)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            string res = "Not Available/ Deleted";

            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercises");

            var filter = Builders<BsonDocument>.Filter.Eq("Exercise_ID", ID);

            var Documents = collection.Find(filter).ToList();

            foreach (BsonDocument d in Documents)
            {
                res = d.GetElement("Exercise Name").Value.ToString();
            }

            return res;
        }
        #endregion
        #region Amend and Make Data
        public static void Make_Regiment_Record(int Exercise_ID, WorkOut w)
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
        public static void Make_Exercise_Regiment(int Exercise_Type_Id, string name, int exTime)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercises");

            //var documents = collection.Find(new BsonDocument()).ToList();
            var max = collection.Find(new BsonDocument()).Sort(new BsonDocument("Exercise_ID", -1)).FirstOrDefault();
            Int32 ex_ID = max.GetElement("Exercise_ID").Value.ToInt32();
            ex_ID++;
            var document = new BsonDocument { { "Exercise_Type_Id", Exercise_Type_Id },
                { "Exercise_ID", ex_ID} ,
                { "Exercise Name", name} ,
                { "Exercise_Time", exTime} };
            collection.InsertOne(document);
        }
        public static void Make_Exercise_Regiment_Cal(int Exercise_Type_Id, string name, int exTime)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercises");

            //var documents = collection.Find(new BsonDocument()).ToList();
            var max = collection.Find(new BsonDocument()).Sort(new BsonDocument("Exercise_ID", -1)).FirstOrDefault();
            Int32 ex_ID = max.GetElement("Exercise_ID").Value.ToInt32();
            ex_ID++;
            var document = new BsonDocument { { "Exercise_Type_Id", Exercise_Type_Id },
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
        public static void Make_Result_Base(WorkOut w, int Type_ID, string ExName)
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
                if (w.Name.Contains(g.Exercise) && (g.Exercise_ID == Type_ID || Type_ID == 4))
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
        public static void Make_Log_Entry_Names(int calories, string n, int time)
        {
            //Dictionary<int, string> exTypes = Exercise_Types_List();

            //string exType = (from exT in exTypes where exT.Key == Type_ID select exT.Value).FirstOrDefault();

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            var collB = database.GetCollection<BsonDocument>("Exercise_Log");


            var collection = database.GetCollection<BsonDocument>("Exercises");

            var filter = Builders<BsonDocument>.Filter.Eq("Exercise Name", n);

            var Documents = collection.Find(filter).ToList();
            int Ex_Id = -2;
            foreach (BsonDocument d in Documents)
            {
                Ex_Id = d.GetElement("Exercise_ID").Value.ToInt32();
            }

            DateTime dt = DateTime.Now;
            DateTime ut = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            var docb = new BsonDocument {
                                { "Exercise_ID" ,Ex_Id},
                                {"Calorie_Count", calories },
                                {"Exercise_Date", ut},
                                {"Exercise_Time",time }
                            };
            collB.InsertOne(docb);
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
                                {"Calorie_Count", fn_SetCalCount(Type_ID, time) },
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

            DateTime dt = DateTime.Now;
            DateTime ut = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            var docb = new BsonDocument {
                                { "Meal" ,f.Meal},
                                {"Calorie_Count", f.Calorie_Count },
                                {"Consumption_Date", ut},
                                {"Meal_Description",f.Meal_Description }
                            };
            collB.InsertOne(docb);
        }
        public static void Make_Food_Entry_Dated(ExerciseMethodShareDtNt.Food_Log f)
        {
            Dictionary<int, string> exTypes = Exercise_Types_List();

            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);

            var database = dbClient.GetDatabase("ExerciseDB");
            var collB = database.GetCollection<BsonDocument>("Food_Diary");

            DateTime dt = f.Date;
            DateTimeOffset ut = new DateTimeOffset(f.Date, TimeSpan.FromHours(-6));
            dt = ut.DateTime;
            var docb = new BsonDocument {
                                { "Meal" ,f.Meal},
                                {"Calorie_Count", f.Calorie_Count },
                                {"Consumption_Date", dt.ToLocalTime()},
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
        public static bool setTask(Task t)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Tasks"); 
            var document = new BsonDocument { { "Task", t.Id },
                { "Time", t.Date}  };
            collection.InsertOne(document);

            return true;
        }
        public static async void updateFood(FoodAll f)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Food_Diary");
            //var filter = Builders<BsonDocument>.Filter.Eq("Meal", f.Meal);
            var filter = new BsonDocument("_id", new ObjectId(f.Id));
            var update = new BsonDocument
            {
                { "$set", new BsonDocument
                    {
                        { "Meal", f.Meal },
                        { "Meal_Description", f.Meal_Description },
                        { "Calorie_Count", f.Calorie_Count },
                        { "Date", f.Date }
                    }
                }
            };

            var result = await collection.UpdateOneAsync(filter, update);

            
        }
        public static async void updateExercise(ExerciseAll ex)
        {
            MongoClient dbClient = new MongoClient(Properties.Settings.Default.MongoDB);
            var database = dbClient.GetDatabase("ExerciseDB");
            var collection = database.GetCollection<BsonDocument>("Exercise_Log");
            //var filter = Builders<BsonDocument>.Filter.Eq("Meal", f.Meal);
            var filter = new BsonDocument("_id", new ObjectId(ex.Id));
            var update = new BsonDocument
            {
                { "$set", new BsonDocument
                    {
                        
                        { "Calorie_Count", ex.Calorie_Count },
                        { "Date", ex.Date }
                    }
                }
            };

            var result = await collection.UpdateOneAsync(filter, update);


        }

        #endregion
    }
}