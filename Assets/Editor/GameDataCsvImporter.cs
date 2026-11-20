using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CardBattlerCourse.Enemies;
using CardBattlerCourse.Systems;
using UnityEditor;
using UnityEngine;

namespace CardBattlerCourse.Editor
{
    public static class GameDataCsvImporter
    {
        private const string SourceFolder = "Assets/GameData/Source";
        private const string CardOutputFolder = "Assets/ScriptableObjects/CardData";
        private const string EnemyOutputFolder = "Assets/ScriptableObjects/EnemyData";

        [MenuItem("Tools/Game Data/Import Cards CSV")]
        private static void ImportCardsMenu()
        {
            var path = Path.Combine(SourceFolder, "Cards.csv");
            ImportCards(path);
        }

        [MenuItem("Tools/Game Data/Import Enemies CSV")]
        private static void ImportEnemiesMenu()
        {
            var path = Path.Combine(SourceFolder, "Enemies.csv");
            ImportEnemies(path);
        }

        private static void ImportCards(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"CSV not found: {path}");
                return;
            }
            var errors = new List<string>();
            var rows = ReadRows(path, errors);
            var parsed = new List<CardRow>();
            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var line = i + 2;
                var id = Cell(row, "id");
                if (string.IsNullOrWhiteSpace(id))
                {
                    errors.Add($"Line {line}: id is required.");
                    continue;
                }

                if (!ids.Add(id))
                {
                    errors.Add($"Line {line}: duplicate id '{id}'.");
                }

                var hasValidCost = int.TryParse(Cell(row, "cost"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var cost);
                if (!hasValidCost || cost < 0)
                {
                    errors.Add($"Line {line}: cost must be a non-negative integer.");
                }

                var hasValidPower = int.TryParse(Cell(row, "power"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var power);
                if (!hasValidPower || power < 0)
                {
                    errors.Add($"Line {line}: power must be a non-negative integer.");
                }

                var hasValidBuffUses = int.TryParse(Cell(row, "buffUses"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var buffUses);
                if (!hasValidBuffUses || buffUses < 0)
                {
                    errors.Add($"Line {line}: buffUses must be a non-negative integer.");
                }

                var hasValidPrice = int.TryParse(Cell(row, "buyPrice"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var price);
                if (!hasValidPrice || price < 0)
                {
                    errors.Add($"Line {line}: buyPrice must be a non-negative integer.");
                }

                var hasValidType = Enum.TryParse(Cell(row, "type"), true, out CardType type);
                if (!hasValidType || type == CardType.None || !Enum.IsDefined(typeof(CardType), type))
                {
                    errors.Add($"Line {line}: type must be Attack, Heal, Buff, or Shield.");
                }

                var name = Cell(row, "name");
                if (string.IsNullOrWhiteSpace(name))
                {
                    errors.Add($"Line {line}: name is required.");
                }

                var description = Cell(row, "description");
                var illustrationKey = Cell(row, "illustrationKey");
                var sprite = ResolveSprite(illustrationKey, line, errors);
                parsed.Add(new CardRow(id, name, description, type, cost, power, buffUses, price, sprite, line));
            }
            if (ReportErrors(errors))
            {
                return;
            }

            EnsureFolder(CardOutputFolder);
            var existing = LoadAssets<CardData>(CardOutputFolder);
            if (HasDuplicateAssetIds(existing, errors))
            {
                ReportErrors(errors);
                return;
            }
            foreach (var row in parsed)
            {
                if (FindCardById(existing, row.Id) != null)
                {
                    continue;
                }

                var assetPath = $"{CardOutputFolder}/{SafeFileName(row.Id)}.asset";
                if (AssetDatabase.LoadAssetAtPath<CardData>(assetPath) != null)
                {
                    errors.Add($"Line {row.Line}: {assetPath} exists with a different or empty id.");
                }
            }
            if (ReportErrors(errors))
            {
                return;
            }
            foreach (var row in parsed)
            {
                var asset = FindCardById(existing, row.Id);
                if (asset == null)
                {
                    var fileName = SafeFileName(row.Id) + ".asset";
                    var assetPath = $"{CardOutputFolder}/{fileName}";
                    asset = ScriptableObject.CreateInstance<CardData>();
                    AssetDatabase.CreateAsset(asset, assetPath);
                }
                asset.Id = row.Id;
                asset.CardName = row.Name;
                asset.CardDescription = row.Description;
                asset.Type = row.Type;
                asset.actionCost = row.Cost;
                asset.power = row.Power;
                asset.buffUses = row.BuffUses;
                asset.buyPrice = row.BuyPrice;

                if (row.Sprite != null)
                {
                    asset.Illustration = row.Sprite;
                }

                EditorUtility.SetDirty(asset);
            }
            if (ReportErrors(errors))
            {
                return;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Imported {parsed.Count} card rows from {path}.");
        }

        private static void ImportEnemies(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"CSV not found: {path}");
                return;
            }
            var errors = new List<string>();
            var rows = ReadRows(path, errors);
            var parsed = new List<EnemyRow>();
            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var line = i + 2;
                var id = Cell(row, "id");
                if (string.IsNullOrWhiteSpace(id))
                {
                    errors.Add($"Line {line}: id is required.");
                    continue;
                }

                if (!ids.Add(id))
                {
                    errors.Add($"Line {line}: duplicate id '{id}'.");
                }

                var hasValidHealth = float.TryParse(Cell(row, "maxHealth"), NumberStyles.Float, CultureInfo.InvariantCulture, out var hp);
                if (!hasValidHealth || hp <= 0)
                {
                    errors.Add($"Line {line}: maxHealth must be greater than zero.");
                }

                var hasValidAttackPower = int.TryParse(Cell(row, "attackPower"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var attack);
                if (!hasValidAttackPower || attack < 0)
                {
                    errors.Add($"Line {line}: attackPower must be non-negative.");
                }

                var hasValidEmpowerAmount = int.TryParse(Cell(row, "empowerAmount"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var empower);
                if (!hasValidEmpowerAmount || empower < 0)
                {
                    errors.Add($"Line {line}: empowerAmount must be non-negative.");
                }

                var hasValidBlockAmount = int.TryParse(Cell(row, "blockAmount"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var block);
                if (!hasValidBlockAmount || block < 0)
                {
                    errors.Add($"Line {line}: blockAmount must be non-negative.");
                }

                var name = Cell(row, "name");
                if (string.IsNullOrWhiteSpace(name))
                {
                    errors.Add($"Line {line}: name is required.");
                }

                parsed.Add(new EnemyRow(id, name, hp, attack, empower, block));
            }
            if (ReportErrors(errors))
            {
                return;
            }

            EnsureFolder(EnemyOutputFolder);
            var existing = LoadAssets<EnemyData>(EnemyOutputFolder);
            if (HasDuplicateAssetIds(existing, errors))
            {
                ReportErrors(errors);
                return;
            }
            foreach (var row in parsed)
            {
                if (FindEnemyById(existing, row.Id) != null)
                {
                    continue;
                }

                var assetPath = $"{EnemyOutputFolder}/{SafeFileName(row.Id)}.asset";
                if (AssetDatabase.LoadAssetAtPath<EnemyData>(assetPath) != null)
                {
                    errors.Add($"{assetPath} exists with a different or empty id.");
                }
            }
            if (ReportErrors(errors))
            {
                return;
            }
            foreach (var row in parsed)
            {
                var asset = FindEnemyById(existing, row.Id);
                if (asset == null)
                {
                    var assetPath = $"{EnemyOutputFolder}/{SafeFileName(row.Id)}.asset";
                    asset = ScriptableObject.CreateInstance<EnemyData>();
                    AssetDatabase.CreateAsset(asset, assetPath);
                }
                asset.Id = row.Id;
                asset.name = row.Name;
                asset.maxHealth = row.MaxHealth;
                asset.attackPower = row.AttackPower;
                asset.empowerAmount = row.EmpowerAmount;
                asset.blockAmount = row.BlockAmount;
                EditorUtility.SetDirty(asset);
            }
            if (ReportErrors(errors))
            {
                return;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Imported {parsed.Count} enemy rows from {path}.");
        }

        private static List<Dictionary<string, string>> ReadRows(string path, List<string> errors)
        {
            var lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                errors.Add("CSV is empty.");
                return new List<Dictionary<string, string>>();
            }

            var headers = ParseCsvLine(lines[0]);
            for (var i = 0; i < headers.Count; i++)
            {
                headers[i] = headers[i].Trim();
            }

            var seenHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < headers.Count; i++)
            {
                if (!seenHeaders.Add(headers[i]))
                {
                    errors.Add("CSV contains duplicate column names.");
                    break;
                }
            }

            var result = new List<Dictionary<string, string>>();
            for (var i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                var values = ParseCsvLine(lines[i]);
                if (values.Count != headers.Count)
                {
                    errors.Add($"Line {i + 1}: expected {headers.Count} cells, found {values.Count}.");
                    continue;
                }

                var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                for (var columnIndex = 0; columnIndex < headers.Count; columnIndex++)
                {
                    var header = headers[columnIndex];
                    var value = values[columnIndex].Trim();
                    row.Add(header, value);
                }

                result.Add(row);
            }
            return result;
        }

        // Supports quoted cells, commas inside cells, and escaped quotes.
        private static List<string> ParseCsvLine(string line)
        {
            var cells = new List<string>();
            var cell = new System.Text.StringBuilder();
            var quoted = false;
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '"' && quoted && i + 1 < line.Length && line[i + 1] == '"')
                {
                    cell.Append('"');
                    i++;
                }
                else if (c == '"')
                {
                    quoted = !quoted;
                }
                else if (c == ',' && !quoted)
                {
                    cells.Add(cell.ToString());
                    cell.Clear();
                }
                else
                {
                    cell.Append(c);
                }
            }
            cells.Add(cell.ToString());
            return cells;
        }

        private static string Cell(Dictionary<string, string> row, string key)
        {
            if (row.TryGetValue(key, out var value))
            {
                return value;
            }

            return string.Empty;
        }

        private static Sprite ResolveSprite(string key, int line, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            var matches = new List<Sprite>();
            var spriteGuids = AssetDatabase.FindAssets("t:Sprite");
            for (var i = 0; i < spriteGuids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(spriteGuids[i]);
                var assetsAtPath = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                for (var j = 0; j < assetsAtPath.Length; j++)
                {
                    var sprite = assetsAtPath[j] as Sprite;
                    if (sprite == null)
                    {
                        continue;
                    }

                    if (string.Equals(sprite.name, key, StringComparison.OrdinalIgnoreCase))
                    {
                        matches.Add(sprite);
                    }
                }
            }

            if (matches.Count != 1)
            {
                errors.Add($"Line {line}: illustrationKey '{key}' matched {matches.Count} sprites; use a unique exact Sprite name or leave it blank.");
                return null;
            }

            return matches[0];
        }

        private static List<T> LoadAssets<T>(string folder) where T : ScriptableObject
        {
            var assetGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder });
            var assets = new List<T>();
            for (var i = 0; i < assetGuids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }

        private static bool HasDuplicateAssetIds<T>(List<T> assets, List<string> errors) where T : ScriptableObject
        {
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];
                string id;

                if (asset is CardData card)
                {
                    id = card.Id;
                }
                else
                {
                    var enemy = asset as EnemyData;
                    if (enemy == null)
                    {
                        continue;
                    }

                    id = enemy.Id;
                }

                if (string.IsNullOrWhiteSpace(id))
                {
                    continue;
                }

                if (!seenIds.Add(id))
                {
                    errors.Add($"Existing assets have duplicate id '{id}'.");
                }
            }

            return errors.Count > 0;
        }

        private static bool ReportErrors(List<string> errors)
        {
            if (errors.Count == 0)
            {
                return false;
            }
            Debug.LogError("Game data import failed. No assets were updated:\n- " + string.Join("\n- ", errors));
            return true;
        }

        private static string SafeFileName(string value)
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();
            var safeName = new System.Text.StringBuilder();

            for (var i = 0; i < value.Length; i++)
            {
                var character = value[i];
                var isInvalid = false;
                for (var j = 0; j < invalidCharacters.Length; j++)
                {
                    if (character == invalidCharacters[j])
                    {
                        isInvalid = true;
                        break;
                    }
                }

                if (isInvalid)
                {
                    safeName.Append('_');
                }
                else
                {
                    safeName.Append(character);
                }
            }

            return safeName.ToString();
        }

        private static CardData FindCardById(List<CardData> assets, string id)
        {
            for (var i = 0; i < assets.Count; i++)
            {
                if (string.Equals(assets[i].Id, id, StringComparison.OrdinalIgnoreCase))
                {
                    return assets[i];
                }
            }

            return null;
        }

        private static EnemyData FindEnemyById(List<EnemyData> assets, string id)
        {
            for (var i = 0; i < assets.Count; i++)
            {
                if (string.Equals(assets[i].Id, id, StringComparison.OrdinalIgnoreCase))
                {
                    return assets[i];
                }
            }

            return null;
        }

        private static void EnsureFolder(string path)
        {
            var parts = path.Split('/');
            var current = parts[0];

            for (var i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }

        private sealed class CardRow
        {
            public readonly string Id;
            public readonly string Name;
            public readonly string Description;
            public readonly CardType Type;
            public readonly int Cost;
            public readonly int Power;
            public readonly int BuffUses;
            public readonly int BuyPrice;
            public readonly int Line;
            public readonly Sprite Sprite;

            public CardRow(string id, string name, string description, CardType type, int cost, int power, int buffUses, int buyPrice, Sprite sprite, int line)
            {
                Id = id;
                Name = name;
                Description = description;
                Type = type;
                Cost = cost;
                Power = power;
                BuffUses = buffUses;
                BuyPrice = buyPrice;
                Sprite = sprite;
                Line = line;
            }
        }

        private sealed class EnemyRow
        {
            public readonly string Id;
            public readonly string Name;
            public readonly float MaxHealth;
            public readonly int AttackPower;
            public readonly int EmpowerAmount;
            public readonly int BlockAmount;

            public EnemyRow(string id, string name, float hp, int attack, int empower, int block)
            {
                Id = id;
                Name = name;
                MaxHealth = hp;
                AttackPower = attack;
                EmpowerAmount = empower;
                BlockAmount = block;
            }
        }
    }
}
