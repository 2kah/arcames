using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using AssemblyCSharp;

namespace AssemblyCSharp
{
	public enum Direction {None, Up, Right, Down, Left};
    public enum Axis {None, Horizontal, Vertical};
	
	public class Util
	{
        public List<Type> InbuiltRules;
        public List<Type> InbuiltMaps;
        
		public Util ()
		{
            InbuiltRules = new List<Type>() {
                typeof(Hyenas),
                typeof(Rescue),
                typeof(WhackAMole),
                typeof(HyenasPro),
                typeof(Pacu)
            };
            
            InbuiltMaps = new List<Type>() {
                typeof(Equals),
                typeof(Pac)
            };
		}
		
		public Direction Opposite(Direction dir)
		{
			switch(dir)
			{
            case Direction.Up:
                return Direction.Down;
            case Direction.Right:
                return Direction.Left;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            default:
                return Direction.None;
			}
		}
        
        public Direction RightTurn(Direction dir)
        {
            if(dir == Direction.None)
                return Direction.None;
            //TODO: write general purpose function: ((dir + amount of rotation - 1) % 4) + 1
            return (Direction)(((int) dir % 4) + 1);
        }
        
        public Direction LeftTurn(Direction dir)
        {
            if(dir == Direction.None)
                return Direction.None;
            return (Direction)(((int) (dir - 2) % 4) + 1);
        }
        
        public Axis GetAxis(Direction dir)
        {
            if(dir == Direction.Left || dir == Direction.Right)
                return Axis.Horizontal;
            else if(dir == Direction.Up || dir == Direction.Down)
                return Axis.Vertical;
            return Axis.None;
        }
        
        public Vector2 DirectionToVector(Direction dir)
        {
            switch(dir)
            {
            case Direction.Up:
                return new Vector2(0,1);
            case Direction.Right:
                return new Vector2(1,0);
            case Direction.Down:
                return new Vector2(0,-1);
            case Direction.Left:
                return new Vector2(-1,0);
            default:
                return new Vector2(0,0);
            }
        }
        
        public Vector3 EmptySpawnPosition(int mapWidth, int mapHeight)
        {
            for(int i = 0; i < 1000; i++)
            {
                float halfWidth = ((float) mapWidth / 2) - 1;
                float halfHeight = ((float) mapHeight / 2) - 1; 
                //find an empty position that isn't too close to (0,0) which is where the player spawns
                float x = UnityEngine.Random.value < 0.5 ? UnityEngine.Random.Range(-halfWidth,-(halfWidth / 4)) : UnityEngine.Random.Range(halfWidth / 4, halfWidth);
                float z = UnityEngine.Random.value < 0.5 ? UnityEngine.Random.Range(-halfHeight,-(halfHeight / 4)) : UnityEngine.Random.Range(halfHeight / 4, halfHeight);
                Vector3 target = new Vector3(x, 0.5f, z);
                if(PositionEmpty(target))
                    return target;
            }
            return new Vector3(-2, 0.5f, 0);
        }
        
        private bool PositionEmpty(Vector3 pos)
        {
            var checkResult = Physics.OverlapSphere(pos, 1);
            if(checkResult.Length == 0)
                return true;
            return false;
        }
        
        public Vector3 EmptyPosition(int mapWidth, int mapHeight)
        {
            for(int i = 0; i < 1000; i++)
            {
                float halfWidth = ((float) mapWidth / 2) - 1;
                float halfHeight = ((float) mapHeight / 2) - 1;
                Vector3 target = new Vector3(UnityEngine.Random.Range(-halfWidth,halfWidth),0.5f, UnityEngine.Random.Range(-halfHeight,halfHeight));
                if(PositionEmpty(target))
                    return target;
            }
            //we have tried 1000 locations and none are empty
            return new Vector3(0, 0.5f, 0);
        }
        
        public string ExportRuleset()
        {
            Rules rules = CopyToRules();
            StringBuilder builder = new StringBuilder();
            var serializer = new XmlSerializer(typeof(Rules));
            using(TextWriter writer = new StringWriter(builder))
                serializer.Serialize(writer, rules);
            return builder.ToString();
        }
        
        public void ImportRuleset(string xml)
        {
            //TODO: error checking
            Rules rules;
            var serializer = new XmlSerializer(typeof(Rules));
            using (TextReader reader = new StringReader(xml))
                rules = (Rules) serializer.Deserialize(reader);
            CopyFromRules(rules);
        }
        
//        public void SaveRuleset(string path)
//        {
//            //TODO: error checking
//            Rules rules = CopyToRules();
//            var serializer = new XmlSerializer(typeof(Rules));
//            var stream = new FileStream(path, FileMode.Create);
//            serializer.Serialize(stream, rules);
//            stream.Close();
//        }
//        
//        public void LoadRuleset(string path)
//        {
//            //TODO: error checking
//            var serializer = new XmlSerializer(typeof(Rules));
//            var stream = new FileStream(path, FileMode.Open);
//            Rules rules = serializer.Deserialize(stream) as Rules;
//            stream.Close();
//            CopyFromRules(rules);
//        }
        
        public Rules CopyToRules()
        {
            //http://answers.unity3d.com/questions/252903/c-reflection-get-all-public-variables-from-custom.html
            Rules rules = new Rules();
            Ruleset ruleset = GameObject.Find("Ruleset").GetComponent<Ruleset>();
            Type rulesType = rules.GetType();
            foreach (FieldInfo field in typeof(Ruleset).GetFields())
            {
                FieldInfo other = rulesType.GetField(field.Name);
                if (other != null)
                    other.SetValue(rules, field.GetValue(ruleset));
            }
            return rules;
        }
        
        public void CopyFromRules(Rules rules)
        {
            Ruleset ruleset = GameObject.Find("Ruleset").GetComponent<Ruleset>();
            Type rulesetType = typeof(Ruleset);
            foreach (FieldInfo field in typeof(Rules).GetFields())
            {
                FieldInfo other = rulesetType.GetField(field.Name);
                if (other != null)
                    other.SetValue(ruleset, field.GetValue(rules));
            }
        }
	}
}

