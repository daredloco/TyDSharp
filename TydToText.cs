using System;
using System.Linq;
using System.Text;

namespace Tyd
    {
    public static class TydToText
        {
        /*
            Possible future features:
                - Ability to align string values into a single column
                - Ability to write lists/tables with 0 or 1 children on a single line
                - Some way to better control which strings get quotes and which don't
         */

        ///<summary>
        /// Writes a given TydNode, along with all its descendants, as a string, at a given indent level.
        /// This method is recursive.
        ///</summary>
        public static string Write(TydNode node, int indent = 0, bool whitesmiths = true)
            {
            var braceIndent = whitesmiths ? indent + 1 : indent;
            //It's a string
            var str = node as TydString;
            if (str != null)
                {
                return IndentString(indent) + node.Name + " " + StringContentWriteable(str.Value);
                }

            //It's a table
            var tab = node as TydTable;
            if (tab != null)
                {
                var sb = new StringBuilder();

                //Intro line
                if (AppendNodeIntro(tab, sb, indent) && tab.Count > 0)
                    {
                    sb.AppendLine();
                    }

                if (tab.Count == 0)
                    {
                    sb.Append("{}");
                    }
                else
                    {
                    //Sub-_nodes
                    sb.AppendLine(IndentString(braceIndent) + "{");
                    for (var i = 0; i < tab.Count; i++)
                        {
                        sb.AppendLine(Write(tab[i], indent + 1, whitesmiths));
                        }
                    sb.Append(IndentString(braceIndent) + "}");
                    }

                return sb.ToString();
                }

            //It's a list
            var list = node as TydList;
            if (list != null)
                {
                var sb = new StringBuilder();
                var simple = IsSimpleList(list);
                //Intro line
                if (AppendNodeIntro(list, sb, indent) && list.Count > 0 && !simple)
                    {
                    sb.AppendLine();
                    }

                if (list.Count == 0)
                    {
                    sb.Append(" []");
                    }
                else
                    {
                    if (simple)
                        {
                        sb.Append(" [");
                        for (var i = 0; i < list.Count; i++)
                            {
                            sb.Append(i == 0 ? Write(list[i], 0, whitesmiths) : ";" + Write(list[i], 0, whitesmiths));
                            }
                        sb.Append(" ]");
                        }
                    else
                        {
                        //Sub-_nodes
                        sb.AppendLine(IndentString(braceIndent) + "[");
                        for (var i = 0; i < list.Count; i++)
                            {
                            sb.AppendLine(Write(list[i], indent + 1, whitesmiths));
                            }
                        sb.Append(IndentString(braceIndent) + "]");
                        }

                    }

                return sb.ToString();
                }

            throw new ArgumentException();
            }

        private static bool IsSimpleList(TydList l)
            {
            return l.Nodes.TrueForAll(x => x is TydString);
            }

        private static string StringContentWriteable(string value)
            {
            if (value == "")
                {
                return "\"\"";
                }

            if (value == null)
                {
                return Constants.NullValueString;
                }

            return ShouldWriteWithQuotes(value)
            ? "\"" + EscapeCharsEscapedForQuotedString(value) + "\""
            : value;



            //Returns string content s with escape chars properly escaped according to Tyd rules.
            }

        //This is a set of heuristics to try to determine if we should write a string quoted or naked.
        private static bool ShouldWriteWithQuotes(string value)
            {
            if (value.Length > 40) //String is long
                {
                return true;
                }

            if (value[value.Length - 1] == '.') //String ends with a period. It's probably a sentence
                {
                return true;
                }

            //Check the string character-by-character
            for (var i = 0; i < value.Length; i++)
                {
                var c = value[i];

                //Chars that imply we should use quotes
                //Some of these are heuristics, like space.
                //Some absolutely require quotes, like the double-quote itself. They'll break naked strings if unescaped (and naked strings are always written unescaped).
                //Note that period is not on this list; it commonly appears as a decimal in numbers.
                if (c == ' '
                    || c == '\n'
                    || c == '\t'
                    || c == '"'
                    || c == Constants.CommentChar
                    || c == Constants.RecordEndChar
                    || c == Constants.AttributeStartChar
                    || c == Constants.TableStartChar
                    || c == Constants.TableEndChar
                    || c == Constants.ListStartChar
                    || c == Constants.ListEndChar
                    )
                    {
                    return true;
                    }
                }

            return false;
            }

        private static string EscapeCharsEscapedForQuotedString(string s)
            {
            return s.Replace("\"", "\\\"")
                .Replace("#", "\\#");
            }

        private static bool AppendNodeIntro(TydCollection node, StringBuilder sb, int indent)
            {
            var appendedSomething = false;

            if (node.Name != null)
                {
                AppendWithWhitespace(node.Name, sb, ref appendedSomething, indent);
                }

            foreach (var attribute in node.GetAttributes())
                {
                if (attribute.Value != null)
                    {
                    AppendWithWhitespace(Constants.AttributeStartChar + attribute.Key + " " + StringContentWriteable(attribute.Value), sb, ref appendedSomething, indent);
                    }
                else
                    {
                    AppendWithWhitespace(Constants.AttributeStartChar + attribute.Key, sb, ref appendedSomething, indent);
                    }
                }

            return appendedSomething;

            }

        private static void AppendWithWhitespace(string s, StringBuilder sb, ref bool appendedSomething, int indent)
            {
            sb.Append((appendedSomething ? " " : IndentString(indent)) + s);
            appendedSomething = true;
            }

        private static string IndentString(int indent)
            {
            var s = "";
            for (var i = 0; i < indent; i++)
                {
                s += "    ";
                }
            return s;
            }
        }

    }