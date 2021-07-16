﻿using System;
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

        public static List<Given_Rule > RuleList()
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

            foreach(BsonDocument bd in Documents)
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
            ex_ID++;

            var document = new BsonDocument { { "Routine_id", w.Id },
                { "Exercise_ID", ex_ID} ,
                { "Exercise_Name", w.Name} ,
                { "Exercise_Time",w.Time} };
            collection.InsertOne(document);
        }

        public static void Make_Exercise_Regiment(int Exercise_Type_ID, string name)
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
                { "Exercise Name", name} };
            collection.InsertOne(document);
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
                if (w.Name.Contains(g.Exercise) && (g.Exercise_ID == Type_ID))
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
    }
}
