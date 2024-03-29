﻿using System.Runtime.Serialization;
using System.Text;

namespace CsvReader
{
    public class CsvReader
    {
        public static IEnumerable<string[]> Parse(TextReader reader)
        {
            var line = reader.ReadLine();
            if(line is null) yield break;

            for (line = reader.ReadLine(); line is not null; line = reader.ReadLine())
            {
                var cells = new List<string>();
                var cell = new StringBuilder();
                bool isTopOfCell = true;
                bool isEscaped = false;
                for (var i = 0; i < line.Length; i++)
                {
                    var character = line[i];
                    if (isTopOfCell)
                    {
                        if (character == '"')
                        {
                            // 先頭なのでエスケープされたセル。読み飛ばす。
                            isEscaped = true;
                            isTopOfCell = false;
                        }
                        else if (character == ',')
                        {
                            // 空のセル
                            cells.Add(cell.ToString());
                            cell.Clear();
                            isTopOfCell = true;
                        }
                        else 
                        {
                            // セルの1文字目としてあつかう
                            cell.Append(character);
                            isTopOfCell = false;
                        }
                    }
                    else
                    {
                        if (character == ',' && isEscaped)
                        {
                            // エスケープされているので値としてあつかう
                            cell.Append(character);
                            isTopOfCell = false;
                        }
                        else if (character == ',' && isEscaped is false)
                        {
                            // エスケープされていないのでセルの区切りとしてあつかう
                            cells.Add(cell.ToString());
                            cell.Clear();
                            isTopOfCell = true;
                        }
                        else if (character == '"')
                        {
                            if (line.Length - 1 == i)
                            {
                                // 最後のインデックスなので閉じのダブルクォーテーションとして判断し何もしない
                            }
                            else
                            {
                                var nextCharacter = line[i + 1];
                                if (nextCharacter == ',')
                                {
                                    // セルの末尾なのでつぎのセルへ
                                    i++;
                                    cells.Add(cell.ToString());
                                    cell.Clear();
                                    isTopOfCell = true;
                                    isEscaped = false;
                                }
                                else if (nextCharacter == '"')
                                {
                                    // エスケープされたダブルクォーテーション
                                    i++;
                                    cell.Append('"');
                                    isTopOfCell = false;
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        else
                        {
                            // 通常の値
                            cell.Append(character);
                            isTopOfCell = false;
                        }
                    }
                }
                cells.Add(cell.ToString());
                yield return cells.ToArray();
            }
        }
    }
}