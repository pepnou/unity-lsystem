using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(ShapeGenerator))]
public class Lsystem : MonoBehaviour
{
    [TextArea(1, 10)] [SerializeField] private string axiomText;
    [TextArea(1, 10)] [SerializeField] private string alphabet;
    [TextArea(1, 10)] [SerializeField] private string rulesText;
    [TextArea(1, 10)] [SerializeField] private string graphic_rulesText;

    [SerializeField] private int iterations = 10;

    Rules rules = new Rules();
    GraphicRules graphicRules = new GraphicRules();
    LsystemSequence lsystemSequence = new LsystemSequence();

    private ShapeGenerator shapeGenerator;

    Drawer drawer;

    public void Generate()
    {
        lsystemSequence.Reset();

        ParseAxiom();
        ParseRules();
        ParseGraphicRules();

        lsystemSequence.Generate(rules, iterations);

        shapeGenerator = GetComponent<ShapeGenerator>();
        shapeGenerator.ResetShape();

        drawer = new Drawer(shapeGenerator);
        lsystemSequence.Draw(graphicRules, drawer);

        float t = Time.realtimeSinceStartup;
        Debug.Log("Generation Shape");
        shapeGenerator.GenerateShape();
        Debug.Log("Done: "+ (Time.realtimeSinceStartup - t) + "s");
    }

    public void ParseRules()
    {
        float t = Time.realtimeSinceStartup;
        Debug.Log("Parsing Rules");

        rules.Reset();

        string[] _rules = rulesText.Split('\n');
        for(int i = 0; i < _rules.Length; i++)
        {
            string[] rule_parts = _rules[i].Split(':');
            
            if(rule_parts.Length != 2)
            {
                Debug.LogError("Rules n°" + i + ", rule must follow format <character>:<string>");
                continue;
            }
            if(rule_parts[0].Length != 1)
            {
                Debug.LogError("Rules n°" + i + ", source must be exactly one character");
                continue;
            }

            rules.Add(rule_parts[0][0], rule_parts[1]);
        }

        Debug.Log("Done: " + (Time.realtimeSinceStartup - t) + "s");
    }

    public void ParseGraphicRules()
    {
        float t = Time.realtimeSinceStartup;
        Debug.Log("Parsing Graphic Rules");
        graphicRules.Reset();

        string[] _rules = graphic_rulesText.Split('\n');
        for (int i = 0; i < _rules.Length; i++)
        {
            string[] rule_parts = _rules[i].Split(':');

            if (rule_parts.Length != 2)
            {
                Debug.LogError("Rules n°" + i + ", rule must follow format <character>:<string>");
                continue;
            }
            if (rule_parts[0].Length != 1)
            {
                Debug.LogError("Rules n°" + i + ", source must be exactly one character");
                continue;
            }

            graphicRules.Add(rule_parts[0][0], rule_parts[1]);
        }
        Debug.Log("Done: " + (Time.realtimeSinceStartup - t) + "s");
    }

    public void ParseAxiom()
    {
        float t = Time.realtimeSinceStartup;
        Debug.Log("Parsing Axiom");
        lsystemSequence.Reset();

        for (int i = 0; i < axiomText.Length; i++)
        {
            if(alphabet.Contains(axiomText[i].ToString()))
            {
                lsystemSequence.Add(axiomText[i]);
            } else
            {
                Debug.LogError("Symbol : " + axiomText[i] + " does not exist in alphabet");
            }
        }
        Debug.Log("Done: " + (Time.realtimeSinceStartup - t) + "s");
    }





    class LsystemSequence
    {
        List<Symbol> symbols, generated, swap;

        public LsystemSequence()
        {
            symbols = new List<Symbol>();
            generated = new List<Symbol>();
        }

        public void Reset()
        {
            symbols.Clear();
            generated.Clear();
        }

        public void Add(char c)
        {
            symbols.Add(new Symbol(c));
        }
        public void Add(Symbol s)
        {
            symbols.Add(new Symbol(s));
        }

        public void Add(List<Symbol> list)
        {
            symbols.AddRange(list);
        }

        public void Generate(Rules rules, int iterations)
        {
            float t = Time.realtimeSinceStartup;
            Debug.Log("Generating Lsystem Sequence");

            swap = generated;
            generated = symbols;
            symbols = swap;

            for (int j = 0; j < iterations; j++)
            {
                for (int i = 0; i < generated.Count; i++)
                {
                    rules.findAndUseRule(generated[i], this);
                }

                swap = generated;
                generated = symbols;
                symbols = swap;

                symbols.Clear();

                Debug.Log("Iteration n° "+j+ " done: "+ ToString());
            }

            Debug.Log("Done: " + (Time.realtimeSinceStartup - t) + "s");
        }

        public void Draw(GraphicRules graphicRules, Drawer drawer)
        {
            float t = Time.realtimeSinceStartup;
            Debug.Log("Adding Shapes");
            for (int i = 0; i < generated.Count; i++)
            {
                graphicRules.findAndUseRule(generated[i], drawer);
            }
            Debug.Log("Done: " + (Time.realtimeSinceStartup - t) + "s");
        }

        public override string ToString()
        {
            string s = "";

            for (int i = 0; i < generated.Count; i++)
            {
                s += generated[i].c;
            }

            return s;
        }
    }


    class Symbol
    {
        public char c;

        public Symbol(char c)
        {
            this.c = c;
        }
        public Symbol(Symbol s)
        {
            this.c = s.c;
        }

        public static bool operator ==(Symbol a, Symbol b)
        {
            return a.c == b.c;
        }
        public static bool operator !=(Symbol a, Symbol b)
        {
            return a.c != b.c;
        }

        public override bool Equals(object o)
        {
            if (o == null)
                return false;

            var second = o as Symbol;

            return second != null && c == second.c;
        }

        public override int GetHashCode()
        {
            return c;
        }
    }


    class Rules
    {
        List<Rule> rules;

        public Rules()
        {
            rules = new List<Rule>();
        }
        public void Reset()
        {
            rules.Clear();
        }
        public void Add(char c, string s)
        {
            rules.Add(new Rule(c, s));
        }

        public void findAndUseRule(Symbol s, LsystemSequence lsystem)
        {
            for(int i = 0; i < rules.Count; i++)
            {
                if(rules[i].findAndUseRule(s, lsystem))
                {
                    return;
                }
            }
            lsystem.Add(s);
        }
    }


    class Rule
    {
        private Symbol source;
        private List<Symbol> target;

        public Rule(char c, string s)
        {
            source = new Symbol(c);
            target = new List<Symbol>();
            for(int i = 0; i < s.Length; i++)
            {
                target.Add(new Symbol(s[i]));
            }
        }

        public bool findAndUseRule(Symbol s, LsystemSequence lsystem)
        {
            if(s == source)
            {
                lsystem.Add(target);
                return true;
            } else
            {
                return false;
            }
        }
    }

    class GraphicRules
    {
        List<GraphicRule> graphicRules;

        public GraphicRules()
        {
            graphicRules = new List<GraphicRule>();
        }
        public void Reset()
        {
            graphicRules.Clear();
        }
        public void Add(char c, string f)
        {
            graphicRules.Add(new GraphicRule(c, f));
        }

        public void findAndUseRule(Symbol s, Drawer d)
        {
            for (int i = 0; i < graphicRules.Count; i++)
            {
                if (graphicRules[i].findAndUseRule(s, d))
                {
                    return;
                }
            }
        }
    }


    class GraphicRule
    {
        private Symbol source;
        private string function;

        public GraphicRule(char c, string f)
        {
            source = new Symbol(c);
            function = string.Copy(f);
        }

        public bool findAndUseRule(Symbol s, Drawer d)
        {
            if (s == source)
            {
                Type type = typeof(Drawer);
                MethodInfo method = type.GetMethod(function);
                method.Invoke(d, null);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Drawer
    {
        Vector3 position;
        Quaternion rotation;
        float length;

        private Stack<Tuple<Vector3, Quaternion, float>> data;

        ShapeGenerator shapeGenerator;

        public Drawer(ShapeGenerator shapeGenerator)
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;

            data = new Stack<Tuple<Vector3, Quaternion, float>>();

            this.shapeGenerator = shapeGenerator;

            length = 5f;
        }

        public void DrawLine()
        {
            //Debug.Log("DrawLine");

            shapeGenerator.AddShape(JoinType.UNION, new RoundCone(position, rotation, 0.06f * length, 0.06f * length, length), 0.1f * length / 5f);
            position += rotation * Vector3.up * length;
        }

        public void PushTransformAndRotateLeft()
        {
            //Debug.Log("PushTransformAndRotateLeft");

            data.Push(new Tuple<Vector3, Quaternion, float>(position, rotation, length));
            rotation = rotation * Quaternion.Euler(45, 0, 0);

            length /= 2;
        }

        public void PopTransformAndRotateRight()
        {
            //Debug.Log("PopTransformAndRotateRight");

            Tuple<Vector3, Quaternion, float> tmp = data.Pop();

            position = tmp.Item1;
            rotation = tmp.Item2;
            length = tmp.Item3 / 2;

            rotation = rotation * Quaternion.Euler(-45, 0, 0);
        }
    }
}
