using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BastionRandomiztion
{
    class Randomizer
    {
        private List<string> scripts = new List<string>();
        private byte[] mapDataBlock1;
        private byte[] mapDataBlock2;
        private int dataBlock2Length = 0;
        private int[] randomLevelOrder;

        public bool randomizeLevels;
        public bool randomizeLoot;
        public bool noCutscenes;
        public bool noHub;
        public bool weapons;
        public bool abilities;
        public bool loot;
        public bool upgrades;
        public bool cores;
        public bool guaranteeWeapon;
        public bool randomizeHopscotch;

        GameData data = new GameData();


        private int[] Shuffle(Random rand, int[] array)
        {
            int[] randomOrder = new int[array.Length];
            array.CopyTo(randomOrder, 0);

            int n = randomOrder.Count();
            while (n > 1)
            {
                --n;
                int i = rand.Next(n + 1);
                int value = randomOrder[i];
                randomOrder[i] = randomOrder[n];
                randomOrder[n] = value;
            }

            return randomOrder;
        }

        ///////////////////////////
        /// LEVEL RANDOMIZATION ///
        ///////////////////////////

        // Randomize the order of all levels except for challenges, wharf district, the bastion
        public void RandomizeLevelOrder(Random rand, string path)
        {
            // create array all levels and for levels that will be randomized
            int[] newOrder = Enumerable.Range(0, 32).ToArray();
            int[] tempOrder = new int[17] { 2, 4, 5, 8, 9, 10, 14, 15, 16, 17, 18, 21, 23, 25, 27, 28, 29 };

            randomLevelOrder = Shuffle(rand, tempOrder);

            for (int j = 0; j < tempOrder.Count(); ++j)
            {
                newOrder[tempOrder[j]] = randomLevelOrder[j];
            }

            SetMapValues(newOrder, path);
        }

        private void SetMapValues(int[] order, string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path + "/Content/Game/MapLayouts.original.xml");

            var levels = doc.GetElementsByTagName("MapNode");

            for (int i = 0; i < order.Length; ++i)
            {
                LevelInfo temp = data.levelInfos[order[i]];

                levels[i].Attributes["Name"].InnerXml = temp.name;
                levels[i].Attributes["Graphic"].InnerXml = temp.graphic;
                levels[i].Attributes["AudioCue"].InnerXml = temp.audio;

                if (i >= 3)
                    levels[i].Attributes["GoalPiecesRequired"].InnerXml = temp.cores.ToString();

                if (levels[i].Attributes["CompletedTextId"] != null)
                    levels[i].Attributes["CompletedTextId"].InnerXml = temp.completedtext;
            }

            doc.Save(path + "/Content/Game/MapLayouts.xml");
        }

        ////////////////////////////
        /// ITEM RANDOMIZATION ///
        ////////////////////////////
        
        public void RandomizeLoot(Random rand, string path)
        {
            if (!randomizeHopscotch)
                data.loot.Remove(data.loot.Find(x => x.name == "Jump_Kit"));

            if(weapons)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Weapon));
            if (abilities)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Ability));
            if (upgrades)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Upgrade));
            if (loot)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Generic));
            if (cores)
                data.randomizedLoot.AddRange(data.loot.FindAll(x => x.type == LootType.Core));

            int[] newOrder = Enumerable.Range(0, data.randomizedLoot.Count).ToArray();
                        
            newOrder = Shuffle(rand, newOrder);

            SetLootValues(newOrder, path);
        }

        private void SetLootValues(int[] order, string path)
        {
            //FileStream file = new FileStream("test.txt", FileMode.Create);
            //StreamWriter stream = new StreamWriter(file);

            List<Loot> tempLoot = new List<Loot>(data.randomizedLoot.Count);
            foreach (Loot loot in data.randomizedLoot)
            {
                tempLoot.Add(new Loot(loot));
            }

            for (int i = 0; i < order.Length; ++i)
            {
                Loot temp = tempLoot[order[i]];

                // for now this sets hopscotch to be in its original location if it gets randomized to the ram_kit after it is required
                if(data.randomizedLoot[i].name == "Ram_Kit" && temp.name == "Jump_Kit")
                {
                    data.randomizedLoot[i].name = data.randomizedLoot[i - 1].name;
                    data.randomizedLoot[i].type = data.randomizedLoot[i - 1].type;

                    data.randomizedLoot[i - 1].name = temp.name;
                    data.randomizedLoot[i - 1].type = temp.type;

                    continue;
                }

                data.randomizedLoot[i].name = temp.name;
                data.randomizedLoot[i].type = temp.type;
            }

            if(guaranteeWeapon && data.randomizedLoot[0].type != LootType.Weapon)
            {
                for(int i = 0; i < data.randomizedLoot.Count; ++i)
                {
                    if(data.randomizedLoot[i].type == LootType.Weapon)
                    {
                        Loot temp = new Loot(data.randomizedLoot[i]);

                        data.randomizedLoot[i].name = data.randomizedLoot[0].name;
                        data.randomizedLoot[i].type = data.randomizedLoot[0].type;

                        data.randomizedLoot[0].name = temp.name;
                        data.randomizedLoot[0].type = temp.type;

                        break;
                    }
                }
            }

            //for(int j = 0; j < data.randomizedLoot.Count; ++j)
            //{
            //    stream.WriteLine(data.randomizedLoot[j].name + " " + data.randomizedLoot[j].levelIndex);
            //}

            //stream.Close();
            //file.Close();
        }

        ///////////////////////
        /// FILE MANAGEMENT ///
        ///////////////////////
        public void ReadScript(string path, MapData map)
        {
            using (FileStream fileStream = new FileStream(path + "\\Content\\Maps\\" + map.levelName + ".original.map", FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(fileStream);

                mapDataBlock1 = new byte[map.scriptStart];
                reader.Read(mapDataBlock1, 0, map.scriptStart);
                int capacity = reader.ReadInt32();

                scripts.Clear();
                for (int index = 0; index < capacity; ++index)
                    scripts.Add(reader.ReadString());

                dataBlock2Length = (int)fileStream.Length - map.scriptEnd;
                mapDataBlock2 = new byte[(int)fileStream.Length - map.scriptEnd];
                reader.Read(mapDataBlock2, 0, (int)fileStream.Length - map.scriptEnd);

                reader.Close();
            }
        }

        public void WriteScript(string path, MapData map)
        {
            using (FileStream fileStream = new FileStream(path + "\\Content\\Maps\\" + map.levelName + ".map", FileMode.Create))
            {
                BinaryWriter writer = new BinaryWriter(fileStream);

                writer.Write(mapDataBlock1, 0, mapDataBlock1.Length);

                writer.Write(scripts.Count);
                foreach (string str in scripts)
                    writer.Write(str);

                writer.Write(mapDataBlock2, 0, mapDataBlock2.Length);

                writer.Close();
            }
        }

        public void BackupFiles(string folderPath)
        {
            if (File.Exists(folderPath + "/Content/Game/MapLayouts.original.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/MapLayouts.xml", folderPath + "/Content/Game/MapLayouts.original.xml");
            }

            if (File.Exists(folderPath + "/Content/Game/Loot/WeaponKits.original.xml") == false)
            {
                File.Copy(folderPath + "/Content/Game/Loot/WeaponKits.xml", folderPath + "/Content/Game/Loot/WeaponKits.original.xml");
            }

            DirectoryInfo dir = new DirectoryInfo(folderPath + "/Content/Maps/");
            FileInfo[] files = Array.FindAll(dir.GetFiles(), f => f.Extension.Equals(".map"));

            foreach (FileInfo file in files)
            {
                string filename = file.Name.Substring(0, file.Name.IndexOf("."));

                if (File.Exists(file.DirectoryName + "/" + filename + ".original.map"))
                    continue;

                file.CopyTo(file.DirectoryName + "/" + filename + ".original.map");
            }
        }

        public void RestoreFiles(string folderPath)
        {
            if (File.Exists(folderPath + "/Content/Game/MapLayouts.original.xml"))
            {
                File.Delete(folderPath + "/Content/Game/MapLayouts.xml");
                File.Move(folderPath + "/Content/Game/MapLayouts.original.xml", folderPath + "/Content/Game/MapLayouts.xml");
            }

            if (File.Exists(folderPath + "/Content/Game/Loot/WeaponKits.original.xml"))
            {
                File.Delete(folderPath + "/Content/Game/Loot/WeaponKits.xml");
                File.Move(folderPath + "/Content/Game/Loot/WeaponKits.original.xml", folderPath + "/Content/Game/Loot/WeaponKits.xml");
            }

            DirectoryInfo dir = new DirectoryInfo(folderPath + "/Content/Maps/");
            FileInfo[] files = Array.FindAll(dir.GetFiles(), f => f.FullName.Contains("original"));

            foreach (FileInfo file in files)
            {
                string filename = file.Name.Substring(0, file.Name.IndexOf("."));

                File.Delete(file.DirectoryName + "/" + filename + ".map");
                File.Move(file.FullName, file.DirectoryName + "/" + filename + ".map");
            }
        }

        ////////////////////
        /// SCRIPT EDITS ///
        ////////////////////

        public void EditLevelScripts(string path)
        {
            // Rippling Walls
            if (randomizeLoot)
            {
                ReadScript(path, data.Maps[0]);
                SetLevelLoot(0);
                WriteScript(path, data.Maps[0]);
            }

            // Sole Regret
            if (randomizeLoot)
            {
                ReadScript(path, data.Maps[1]);
                SetLevelLoot(1);
                WriteScript(path, data.Maps[1]);
            }

            // Wharf District
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[2]);
                if (noHub)
                {
                    WharfSkipHub();
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(2);
                }
                WriteScript(path, data.Maps[2]);
            }

            // Bastion
            if (randomizeLevels || noCutscenes || randomizeLoot)
            {
                ReadScript(path, data.Maps[3]);
                if (randomizeLevels)
                {
                    SetLevelOrder();
                    UnlockBastion();
                }
                if (noCutscenes)
                {
                    RemoveBastionCutscenes();
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(3);
                }
                WriteScript(path, data.Maps[3]);
            }

            // Workmen Ward
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[4]);
                if (noHub)
                {
                    WorkmenSkipHub(2);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(4);
                }
                WriteScript(path, data.Maps[4]);
            }

            // Sundown Path
            if (noCutscenes || noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[5]);
                if (noCutscenes)
                {
                    RemoveSundownCutscene();
                }
                if (noHub)
                {
                    SundownSkipHub(5);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(5);
                }
                WriteScript(path, data.Maps[5]);
            }

            // Melting Pot
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[6]);
                if (noHub)
                {
                    MeltingSkipHub(4);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(6);
                }
                WriteScript(path, data.Maps[6]);
            }

            // Hanging Gardens
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[7]);
                if (noHub)
                {
                    GardensSkipHub(8);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(7);
                }
                WriteScript(path, data.Maps[7]);
            }

            // Cinderbrick Fort
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[8]);
                if (noHub)
                {
                    CinderbrickSkipHub(9);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(8);
                }
                WriteScript(path, data.Maps[8]);
            }

            // Pyth Orchard
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[9]);
                if (noHub)
                {
                    PythSkipHub(10);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(9);
                }
                WriteScript(path, data.Maps[9]);
            }

            // Langston
            if (randomizeLevels || noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[10]);
                if (randomizeLevels)
                {
                    UncoupleLangston();
                }
                if (noHub)
                {
                    LangstonSkipHub(14);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(10);
                }
                WriteScript(path, data.Maps[10]);
            }

            // Prosper Bluff
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[11]);
                if (noHub)
                {
                    ProsperSkipHub(15);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(11);
                }
                WriteScript(path, data.Maps[11]);
            }

            // Wild Outskirts
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[12]);
                if (noHub)
                {
                    OutskirtsSkipHub(16);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(12);
                }
                WriteScript(path, data.Maps[12]);
            }

            // Jawson Bog
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[13]);
                if (noHub)
                {
                    JawsonSkipHub(17);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(13);
                }
                WriteScript(path, data.Maps[13]);
            }

            // Jawson Dream
            if (randomizeLevels || noCutscenes || randomizeLoot)
            {
                ReadScript(path, data.Maps[14]);
                if(randomizeLevels)
                {
                    RemoveDreamMonument();
                }
                if (noCutscenes)
                {
                    RemoveDreamCutscene();
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(14);
                }
                
                WriteScript(path, data.Maps[14]);
            }

            // Roathus Lagoon
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[15]);
                if (noHub)
                {
                    RoathusSkipHub(18);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(15);
                }
                WriteScript(path, data.Maps[15]);
            }

            // Point Lemaign
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[16]);
                if (noHub)
                {
                    LemaignSkipHub(21);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(16);
                }
                WriteScript(path, data.Maps[16]);
            }

            // Colford Cauldron
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[17]);
                if (noHub)
                {
                    ColfordSkipHub(23);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(17);
                }
                WriteScript(path, data.Maps[17]);
            }

            // Mount Zand
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[18]);
                if (noHub)
                {
                    ZandSkipHub(25);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(18);
                }
                WriteScript(path, data.Maps[18]);
            }

            // Burstone Quarry
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[19]);
                if (noHub)
                {
                    // Invasion skip makes skipping hub not possible so its removed from the mode for now
                    BurstoneSkipHub(27);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(19);
                }
                WriteScript(path, data.Maps[19]);
            }

            // Ura Invasion
            if (randomizeLoot)
            {
                ReadScript(path, data.Maps[20]);
                SetLevelLoot(20);
                WriteScript(path, data.Maps[20]);
            }

            // Urzendra Gate
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[21]);
                if (noHub)
                {
                    UrzendraSkipHub(28);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(21);
                }
                WriteScript(path, data.Maps[21]);
            }

            // Zulten's Hollow
            if (noHub || randomizeLoot)
            {
                ReadScript(path, data.Maps[22]);
                if (noHub)
                {
                    ZultenSkipHub(29);
                }
                if (randomizeLoot)
                {
                    SetLevelLoot(22);
                }
                WriteScript(path, data.Maps[22]);
            }

            // Tazal 1
            if (randomizeLoot)
            {
                ReadScript(path, data.Maps[23]);
                SetLevelLoot(23);
                WriteScript(path, data.Maps[23]);
            }

            // Tazal 2
            if (randomizeLoot)
            {
                ReadScript(path, data.Maps[24]);
                SetLevelLoot(24);
                WriteScript(path, data.Maps[24]);
            }

            // Tazal 3
            if (randomizeLoot)
            {
                ReadScript(path, data.Maps[25]);
                SetLevelLoot(25);
                WriteScript(path, data.Maps[25]);
            }

            // Tazal 4
            if (randomizeLoot)
            {
                ReadScript(path, data.Maps[26]);
                SetLevelLoot(26);
                WriteScript(path, data.Maps[26]);
            }
        }

        #region Level Functions

        private void SetLevelLoot(int index)
        {
            List<Loot> levelLoot = data.randomizedLoot.FindAll(x => x.levelIndex == index);
            levelLoot.Sort((x, y) => y.start.CompareTo(x.start));

            List<byte> tempdata = mapDataBlock2.ToList();

            for (int i = 0; i < levelLoot.Count; ++i)
            {
                int range = levelLoot[i].end - levelLoot[i].start + 1;
                tempdata.RemoveRange(levelLoot[i].start, range);

                // adding the length of the string to the front
                List<byte> lootName = new List<byte>();
                lootName.Add((byte)levelLoot[i].name.Length);
                lootName.AddRange(Encoding.UTF8.GetBytes(levelLoot[i].name));

                tempdata.InsertRange(levelLoot[i].start, lootName);
            }

            mapDataBlock2 = tempdata.ToArray();
        }

        #region Wharf District
        private void WharfSkipHub()
        {
            scripts[18] = "OnFlagTrue HeFell LoadMap " + data.levelInfos[randomLevelOrder[0]].name + " ; DelaySeconds = 1.25\r\n";
            scripts[965] = "OnUsed 964 LoadMap " + data.levelInfos[randomLevelOrder[0]].name + "\r\n";
        }
        #endregion

        #region Bastion
        private void UnlockBastion()
        {
            scripts[2862] = "OnCounter GENERAL GOAL_PIECES_ON_PLAYER >= 1 UseableOn VarOvermap ; \r\n";
            scripts[2863] = "OnCounter GENERAL GOAL_PIECES_ON_PLAYER >= 1 PlayAnimation PlotWorld_Active VarOvermap\r\n";

            scripts[2865] = "OnFlagTrue FlagGlobalPlayerMustBuild UseableOn VarOvermap ; \r\n";
            scripts[2866] = "OnFlagTrue FlagGlobalPlayerMustBuild PlayAnimation PlotWorld_Active VarOvermap ;\r\n";
        }

        private void SetLevelOrder()
        {
            // clearing out original unlocks
            for (int j = 0; j < 53; ++j)
                scripts[3543 + j] = "\r\n";

            // adding randomized level order
            for (int i = 0; i < randomLevelOrder.Length - 1; ++i)
            {
                scripts[3544 + i] = "OnLoad SetFlagTrue MAPS_UNLOCKED " + data.levelInfos[randomLevelOrder[i + 1]].name + " ; RequiredFlag = FlagGlobalComplete" + data.levelInfos[randomLevelOrder[i]].name + " ; SaveStatus = true\r\n";
            }

            scripts[3544 + randomLevelOrder.Length - 1] = "OnLoad SetFlagTrue MAPS_UNLOCKED FinalArena01 ; RequiredFlag = FlagGlobalComplete" + data.levelInfos[randomLevelOrder[randomLevelOrder.Length - 1]].name + " ; SaveStatus = true\r\n";
        }

        private void RemoveBastionCutscenes()
        {
            scripts[50] = "\r\n";
            scripts[51] = "\r\n";
            scripts[52] = "\r\n";
            scripts[53] = "\r\n";
            scripts[54] = "\r\n";
        }
        #endregion

        #region Workmen Ward
        private void WorkmenSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[591] = "OnUsed 9066 LoadMap " + levelname + "\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Sundown Path
        private void RemoveSundownCutscene()
        {
            scripts[605] = "OnFlagTrue SetupEndingPan LoadMap ProtoTown03 ; DelaySeconds = 1.5\r\n";
        }

        private void SundownSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    if (noCutscenes)
                        scripts[605] = "OnFlagTrue SetupEndingPan LoadMap " + levelname + " ; DelaySeconds = 1.5\r\n";
                    else
                        scripts[619] = "OnFlagTrue CommenceEndingPan LoadMap " + levelname + " ; DelaySeconds = 13.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Melting Pot
        private void MeltingSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[587] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Hanging Gardens
        private void GardensSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[513] = "OnFlagTrue SmashCut LoadMap " + levelname + " ; DelaySeconds = 0.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Cinderbrick Fort
        private void CinderbrickSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[821] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Pyth Orchard
        private void PythSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[421] = "OnUsed VarSkyway LoadMap " + levelname + "\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Langston
        private void UncoupleLangston()
        {
            scripts[1213] = "\tLoadMap ProtoTown03 ; DelaySeconds = 2.0 ;\r\n";
        }

        private void LangstonSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[1213] = "\tLoadMap " + levelname + " ; DelaySeconds = 2.0 ;\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Prosper Bluff
        private void ProsperSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[510] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.35 ;\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Wild Outskirts
        private void OutskirtsSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[854] = "\tLoadMap " + levelname + " ; DelaySeconds = 10.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Jawson Bog
        private void JawsonSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[487] = "\tLoadMap " + levelname + " ; DelaySeconds = 7.0\r\n";

                    return;
                }
            }
        }
        #endregion
        
        #region Jawson Dream

        // GoalStructure 151874 151887
        private void RemoveDreamMonument()
        {
            List<byte> tempdata = mapDataBlock2.ToList();
            int range = 14;
            tempdata.RemoveRange(151874, range);

            List<byte> replacement = new List<byte>();
            replacement.Add((byte)"".Length);
            replacement.AddRange(Encoding.UTF8.GetBytes(""));

            tempdata.InsertRange(151874, replacement);

            mapDataBlock2 = tempdata.ToArray();
        }

        private void RemoveDreamCutscene()
        {
            scripts[765] = "\tLoadMap Scenes02 ; DelaySeconds = 3.1\r\n";
            scripts[766] = "\tSetGlobalFlagTrue FlagGlobalCompleteScenes01 ;";
        }
        #endregion

        #region Roathus Lagoon
        private void RoathusSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[959] = "\tLoadMap " + levelname + " ; DelaySeconds = 3.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Point Lemaign
        private void LemaignSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[711] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Colford Cauldron
        private void ColfordSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[552] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.5\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Mount Zand
        private void ZandSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[489] = "\tLoadMap " + levelname + " ; DelaySeconds = 5.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Burstone Quarry
        private void BurstoneSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[770] = "\tLoadMap " + levelname + " ; DelaySeconds = 4.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Ura Invasion
        #endregion

        #region Urzendra Gate
        private void UrzendraSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[974] = "\tLoadMap " + levelname + " ; DelaySeconds = 7.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Zulten's Hollow
        private void ZultenSkipHub(int index)
        {
            for (int i = 0; i < randomLevelOrder.Length; ++i)
            {
                if (randomLevelOrder[i] == index)
                {
                    string levelname;

                    if (i == randomLevelOrder.Length - 1)
                        levelname = "FinalArena01";
                    else
                        levelname = data.levelInfos[randomLevelOrder[i + 1]].name;

                    scripts[941] = "\tLoadMap " + levelname + " ; DelaySeconds = 9.0\r\n";

                    return;
                }
            }
        }
        #endregion

        #region Tazal
        #endregion Tazal

        #endregion
    }
}
