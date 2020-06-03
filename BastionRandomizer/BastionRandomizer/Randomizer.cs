using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Randomizer
    {
        private List<string> scripts = new List<string>();
        private byte[] mapDataBlock1;
        private byte[] mapDataBlock2;
        private int dataBlock2Length = 0;
        private int[] randomLevelOrder;

        public bool randomizeLevels;
        public bool randomizeWeapons;
        public WeaponRandomization weaponRandomizationType = WeaponRandomization.FullyRandom;


        private List<LevelInfo> levelInfos = new List<LevelInfo>()
        {
            new LevelInfo("ProtoIntro01b", "GUI\\Shell\\level_icons\\map_protointro01b", "Menu_21", "ProtoIntro01b_Completed", 0),
            new LevelInfo("ProtoTown03", "GUI\\Shell\\level_icons\\map_prototown01", "ProtoTown03_154", "", 0),
            new LevelInfo("Crossroads01", "GUI\\Shell\\level_icons\\map_crossroads01", "Menu_33", "Crossroads01_Completed", 99),
            new LevelInfo("Challenge01", "GUI\\Shell\\level_icons\\map_challenge01", "Menu_26", "Challenge01_Completed", 99),
            new LevelInfo("Holdout01", "GUI\\Shell\\level_icons\\map_holdout01", "Menu_31", "Holdout01_Completed", 99),
            new LevelInfo("Falling01", "GUI\\Shell\\level_icons\\map_falling01", "Menu_22", "Falling01_Completed", 99),
            new LevelInfo("Challenge03", "GUI\\Shell\\level_icons\\map_challenge03", "Challenge03_24", "Challenge03_Completed", 99),
            new LevelInfo("Challenge06", "GUI\\Shell\\level_icons\\map_challenge06", "Challenge06_18", "Challenge06_Completed", 99),
            new LevelInfo("Survivor01", "GUI\\Shell\\level_icons\\map_survivor01", "Menu_25", "Survivor01_Completed", 99),
            new LevelInfo("Siege01", "GUI\\Shell\\level_icons\\map_seige01", "Siege01_65", "Siege01_Completed", 99),
            new LevelInfo("Shrine01", "GUI\\Shell\\level_icons\\map_shrine01", "Menu_57", "Shrine01_Completed", 99),
            new LevelInfo("Challenge04", "GUI\\Shell\\level_icons\\map_challenge04", "Challenge04_19", "Challenge04_Completed", 99),
            new LevelInfo("Challenge02", "GUI\\Shell\\level_icons\\map_challenge02", "Challenge02_24", "Challenge02_Completed", 99),
            new LevelInfo("Challenge07", "GUI\\Shell\\level_icons\\map_challenge07", "Challenge07_20", "Challenge07_Completed", 99),
            new LevelInfo("Moving01", "GUI\\Shell\\level_icons\\map_moving01", "Moving01_71", "Moving01_Completed", 99),
            new LevelInfo("Survivor02", "GUI\\Shell\\level_icons\\map_survivor02", "ProtoTown03_144", "Survivor02_Completed", 99),
            new LevelInfo("Crossroads02", "GUI\\Shell\\level_icons\\map_crossroads02", "Menu_58", "Crossroads02_Completed", 99),
            new LevelInfo("Scenes02", "GUI\\Shell\\level_icons\\map_scenes01", "Menu_61", "Scenes02_Completed", 99),
            new LevelInfo("Hunt01", "GUI\\Shell\\level_icons\\map_hunt01", "Menu_60", "Hunt01_Completed", 99),
            new LevelInfo("Challenge10", "GUI\\Shell\\level_icons\\map_challenge10", "ProtoTown03_116", "Challenge10_Completed", 99),
            new LevelInfo("Challenge09", "GUI\\Shell\\level_icons\\map_challenge09", "ProtoTown03_118", "Challenge09_Completed", 99),
            new LevelInfo("Platforms01", "GUI\\Shell\\level_icons\\map_platforms01", "Menu_62", "Platforms01_Completed", 99),
            new LevelInfo("Challenge05", "GUI\\Shell\\level_icons\\map_challenge05", "ProtoTown03_108", "Challenge05_Completed", 99),
            new LevelInfo("Scorched01", "GUI\\Shell\\level_icons\\map_scorched01", "ProtoTown03_73", "Scorched01_Completed", 99),
            new LevelInfo("Challenge08", "GUI\\Shell\\level_icons\\map_challenge08", "ProtoTown03_120", "Challenge08_Completed", 99),
            new LevelInfo("Fortress01", "GUI\\Shell\\level_icons\\map_fortress01", "ProtoTown03_67", "Fortress01_Completed", 99),
            new LevelInfo("Challenge11", "GUI\\Shell\\level_icons\\map_challenge11", "ProtoTown03_117", "Challenge11_Completed", 99),
            new LevelInfo("Gauntlet01", "GUI\\Shell\\level_icons\\map_gauntlet01", "ProtoTown03_74", "Gauntlet01_Completed", 99),
            new LevelInfo("Voyage01", "GUI\\Shell\\level_icons\\map_voyage01", "ProtoTown03_236", "Voyage01_Completed", 99),
            new LevelInfo("Rescue01", "GUI\\Shell\\level_icons\\map_rescue01", "ProtoTown03_159", "Rescue01_Completed", 99),
            new LevelInfo("Challenge12", "GUI\\Shell\\level_icons\\map_challenge12", "ProtoTown03_119", "Challenge12_Completed", 99),
            new LevelInfo("FinalArena01", "GUI\\Shell\\level_icons\\map_finalArena01", "ProtoTown03_101", "", 99)
        };
        private List<WeaponInfo> weaponInfos = new List<WeaponInfo>()
        {
            // Weapons
            new WeaponInfo("Hammer", "Hammer_Kit"),
            new WeaponInfo("ShieldBash", "Shield_Kit"),
            new WeaponInfo("Longbow", "Longbow_Kit"),
            new WeaponInfo("Repeater", "Repeater_Kit"),
            new WeaponInfo("Revolvers", "Revolvers_Kit"),
            new WeaponInfo("Rifle", "Rifle_Kit"),
            new WeaponInfo("Mortar", "Mortar_Kit"),
            new WeaponInfo("Machete", "Machete_Kit"),
            new WeaponInfo("Spear", "Spear_Kit"),
            new WeaponInfo("Flamethrower", "Flamethrower_Kit"),
            new WeaponInfo("Shotgun", "Shotgun_Kit"),
            new WeaponInfo("Cannon", "Cannon_Kit"),
            new WeaponInfo("RamSwipe", "Ram_Kit"),

            // Skills
            new WeaponInfo("Shield_SummonSquirt", "WeaponSkillBook"),
            new WeaponInfo("Mine", "Mine_Kit"),
            new WeaponInfo("Grenade", "Grenade_Kit"),
            new WeaponInfo("PlayerDopplewalk", "WeaponSkillBook"),
            new WeaponInfo("PlayerFullDeflection", "WeaponSkillBook"),
            new WeaponInfo("PlayerAreaRoot", "WeaponSkillBook"),
            new WeaponInfo("SummonPortalTurret", "WeaponSkillBook"),

            // Weapon Skills
            new WeaponInfo("Hammer_Whirlwind", "WeaponSkillBook"),
            new WeaponInfo("Hammer_GroundPound", "WeaponSkillBook"),
            new WeaponInfo("Repeater_TwirlShot", "WeaponSkillBook"),
            new WeaponInfo("Repeater_TranqDart", "WeaponSkillBook"),
            new WeaponInfo("Longbow_BouncingShot", "WeaponSkillBook"),
            new WeaponInfo("Longbow_ArrowStorm", "WeaponSkillBook"),
            new WeaponInfo("Machete_BladeStorm", "WeaponSkillBook"),
            new WeaponInfo("Machete_GhostBlade", "WeaponSkillBook"),
            new WeaponInfo("Rifle_LaserShot", "WeaponSkillBook"),
            new WeaponInfo("Rifle_StickyBomb", "WeaponSkillBook"),
            new WeaponInfo("Flamethrower_Whirlwind", "WeaponSkillBook"),
            new WeaponInfo("Flamethrower_Nova", "WeaponSkillBook"),
            new WeaponInfo("Shotgun_BulletRain", "WeaponSkillBook"),
            new WeaponInfo("Shotgun_FullAuto", "WeaponSkillBook"),
            new WeaponInfo("Revolvers_MagnumShot", "WeaponSkillBook"),
            new WeaponInfo("Revolvers_FullAuto", "WeaponSkillBook"),
            new WeaponInfo("Spear_Jump", "WeaponSkillBook"),
            new WeaponInfo("Spear_Spin", "WeaponSkillBook"),
            new WeaponInfo("Mortar_MultiShot", "WeaponSkillBook"),
            new WeaponInfo("Mortar_SummonTurret", "WeaponSkillBook"),
            new WeaponInfo("Cannon_Bomblettes", "WeaponSkillBook"),
            new WeaponInfo("Cannon_RocketStorm", "WeaponSkillBook"),
            new WeaponInfo("Ram_Earthquake", "WeaponSkillBook"),

            // Hopscotch
            new WeaponInfo("Jump", "Jump_Kit")
        };
        private List<MapData> Maps = new List<MapData>()
        {
            new MapData("ProtoIntro01", 162680, 184473), // done // playerunit 753, hammer_kit 7788
            new MapData("ProtoIntro01a", 5403, 24944), // done
            new MapData("ProtoIntro01b", 347870, 398029), // done // skyway 964
            new MapData("ProtoTown03", 78205, 336088), // done
            new MapData("Crossroads01", 137016, 160204), // done
            new MapData("Falling01", 266414, 294645), // done
            new MapData("Holdout01", 50113, 81129), // done
            new MapData("Survivor01", 246383, 270542), // done
            new MapData("Siege01", 494398, 529044), // done
            new MapData("Shrine01", 62091, 79561), // done
            new MapData("Moving01", 185518, 239299), // done
            new MapData("Survivor02", 192028, 209719), // done
            new MapData("Crossroads02", 119717, 151533), // done
            new MapData("Scenes02", 105982, 124570), // done
            new MapData("Scenes01", 215662, 241492), // done
            new MapData("Hunt01", 136779, 175315), // done
            new MapData("Platforms01", 138589, 166472), // done
            new MapData("Scorched01", 228476, 248326), // done
            new MapData("Fortress01", 125951, 143919), // done
            new MapData("Gauntlet01", 528136, 555335), // done
            new MapData("Attack01", 59356, 83547), // done
            new MapData("Voyage01", 281032, 310044), // done
            new MapData("Rescue01", 302309, 334535), // done
            new MapData("FinalArena01", 222273, 246873), // done
            new MapData("FinalChase01", 292107, 309619), // done
            new MapData("FinalRam01", 101060, 116054), // done
            new MapData("FinalZulf01", 235804, 277706) // done
        };



        public int[] Shuffle(Random rand, int[] array)
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
            // create array for all levels and for levels that will be randomized
            int[] newOrder = Enumerable.Range(0, 32).ToArray();
            int[] tempOrder = new int[17] { 2, 4, 5, 8, 9, 10, 14, 15, 16, 17, 18, 21, 23, 25, 27, 28, 29 };

            randomLevelOrder = Shuffle(rand, tempOrder);

            for (int j = 0; j < tempOrder.Count(); ++j)
            {
                newOrder[tempOrder[j]] = randomLevelOrder[j];
            }
            
            SetMapValues(newOrder, path);
        }

        void SetMapValues(int[] order, string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path + "/Content/Game/MapLayouts.original.xml");

            var levels = doc.GetElementsByTagName("MapNode");

            for (int i = 0; i < order.Length; ++i)
            {
                LevelInfo temp = levelInfos[order[i]];

                levels[i].Attributes["Name"].InnerXml = temp._name;
                levels[i].Attributes["Graphic"].InnerXml = temp._graphic;
                levels[i].Attributes["AudioCue"].InnerXml = temp._audio;

                if (i >= 3)
                    levels[i].Attributes["GoalPiecesRequired"].InnerXml = temp._cores.ToString();

                if (levels[i].Attributes["CompletedTextId"] != null)
                    levels[i].Attributes["CompletedTextId"].InnerXml = temp._completedtext;
            }

            doc.Save(path + "/Content/Game/MapLayouts.xml");
        }
        
        ////////////////////////////
        /// WEAPON RANDOMIZATION ///
        ////////////////////////////

        public void RandomizeWeapons(Random rand, string path)
        {
            int[] newOrder = Enumerable.Range(0, 43).ToArray();

            if(weaponRandomizationType == WeaponRandomization.WeaponAbilitySplit)
            {
                int[] weaponOrder = Enumerable.Range(0, 13).ToArray();
                int[] abilityOrder = Enumerable.Range(13, 30).ToArray();

                weaponOrder = Shuffle(rand, weaponOrder);
                abilityOrder = Shuffle(rand, abilityOrder);
                
                Array.Copy(weaponOrder, newOrder, weaponOrder.Length);
                Array.Copy(abilityOrder, 0, newOrder, weaponOrder.Length, abilityOrder.Length);
            }
            else
               newOrder = Shuffle(rand, newOrder);
            
            SetWeaponValues(newOrder, path);
        }

        public void SetWeaponValues( int[] order, string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path + "/Content/Game/Loot/WeaponKits.original.xml");

            var loot = doc.GetElementsByTagName("Loot");

            for (int i = 0; i < order.Length; ++i)
            {
                WeaponInfo temp = weaponInfos[order[i]];

                loot[i].Attributes["Name"].InnerXml = temp._name;
                loot[i].Attributes["Graphic"].InnerXml = temp._graphic;
            }

            doc.Save(path + "/Content/Game/Loot/WeaponKits.xml");
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
            //ReadScript(path, Maps[0]);
            //WriteScript(path, Maps[0]);

            // Sole Regret
            //ReadScript(path, Maps[1]);
            //WriteScript(path, Maps[1]);

            // Wharf District
            //ReadScript(path, Maps[2]);
            //WriteScript(path, Maps[2]);

            // Bastion
            ReadScript(path, Maps[3]);
            if (randomizeLevels)
            {
                SetLevelOrder();
                UnlockBastion();
            }
            WriteScript(path, Maps[3]);

            // Workmen Ward
            //ReadScript(path, Maps[4]);
            //WriteScript(path, Maps[4]);

            // Sundown Path
            //ReadScript(path, Maps[5]);
            //WriteScript(path, Maps[5]);

            // Melting Pot
            //ReadScript(path, Maps[6]);
            //WriteScript(path, Maps[6]);

            // Hanging Gardens
            //ReadScript(path, Maps[7]);
            //WriteScript(path, Maps[7]);

            // Cinderbrick Fort
            //ReadScript(path, Maps[8]);
            //WriteScript(path, Maps[8]);

            // Pyth Orchard
            //ReadScript(path, Maps[9]);
            //WriteScript(path, Maps[9]);

            // Langston
            ReadScript(path, Maps[10]);
            if (randomizeLevels)
            {
                UncoupleLangston();
            }
            WriteScript(path, Maps[10]);

            // Prosper Bluff
            //ReadScript(path, Maps[11]);
            //WriteScript(path, Maps[11]);

            // Wild Outskirts
            //ReadScript(path, Maps[12]);
            //WriteScript(path, Maps[12]);

            // Jawson Bog
            //ReadScript(path, Maps[13]);
            //WriteScript(path, Maps[13]);

            // Jawson Dream
            //ReadScript(path, Maps[14]);
            //WriteScript(path, Maps[14]);

            // Roathus Lagoon
            //ReadScript(path, Maps[15]);
            //WriteScript(path, Maps[15]);

            // Point Lemaign
            //ReadScript(path, Maps[16]);
            //WriteScript(path, Maps[16]);

            // Colford Cauldron
            //ReadScript(path, Maps[17]);
            //WriteScript(path, Maps[17]);

            // Mount Zand
            //ReadScript(path, Maps[18]);
            //WriteScript(path, Maps[18]);

            // Burstone Quarry
            //ReadScript(path, Maps[19]);
            //WriteScript(path, Maps[19]);

            // Ura Invasion
            //ReadScript(path, Maps[20]);
            //WriteScript(path, Maps[20]);

            // Urzendra Gate
            //ReadScript(path, Maps[21]);
            //WriteScript(path, Maps[21]);

            // Zulten's Hollow
            //ReadScript(path, Maps[22]);
            //WriteScript(path, Maps[22]);

            // Tazal 1
            //ReadScript(path, Maps[23]);
            //WriteScript(path, Maps[23]);

            // Tazal 2
            //ReadScript(path, Maps[24]);
            //WriteScript(path, Maps[24]);

            // Tazal 3
            //ReadScript(path, Maps[25]);
            //WriteScript(path, Maps[25]);

            // Tazal 4
            //ReadScript(path, Maps[26]);
            //WriteScript(path, Maps[26]);
        }

        #region Level Functions

        #region Wharf District
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


            //scripts[3543] = "OnLoad SetFlagTrue MAPS_UNLOCKED " + levelInfos[randomLevelOrder[0]]._name + " ; RequiredFlag = FlagGlobalCompleteProtoIntro01b; SaveStatus = true\r\n";

            // adding randomized level order
            for (int i = 0; i < randomLevelOrder.Length - 1; ++i)
            {
                scripts[3544 + i] = "OnLoad SetFlagTrue MAPS_UNLOCKED " + levelInfos[randomLevelOrder[i + 1]]._name + " ; RequiredFlag = FlagGlobalComplete" + levelInfos[randomLevelOrder[i]]._name + " ; SaveStatus = true\r\n";
            }

            scripts[3544 + randomLevelOrder.Length - 1] = "OnLoad SetFlagTrue MAPS_UNLOCKED FinalArena01 ; RequiredFlag = FlagGlobalComplete" + levelInfos[randomLevelOrder[randomLevelOrder.Length - 1]]._name + " ; SaveStatus = true\r\n";
        }
        #endregion

        #region Workmen Ward
        #endregion

        #region Sundown Path
        #endregion

        #region Melting Pot
        #endregion

        #region Hanging Gardens
        #endregion

        #region Cinderbrick Fort
        #endregion

        #region Pyth Orchard
        #endregion

        #region Langston
        private void UncoupleLangston()
        {
            scripts[1213] = "\tLoadMap ProtoTown03 ; DelaySeconds = 2.0 ;\r\n";
        }
        #endregion

        #region Prosper Bluff
        #endregion

        #region Wild Outskirts
        #endregion

        #region Jawson Bog
        #endregion

        #region Jawson Dream
        #endregion

        #region Roathus Lagoon
        #endregion

        #region Point Lemaign
        #endregion

        #region Colford Cauldron
        #endregion

        #region Mount Zand
        #endregion

        #region Burstone Quarry
        #endregion

        #region Ura Invasion
        #endregion

        #region Urzendra Gate
        #endregion

        #region Zulten's Hollow
        #endregion

        #region Tazal
        #endregion Tazal

        #endregion
    }

    struct MapData
    {
        public MapData(string levelname, int scriptstart, int scriptend)
        {
            levelName = levelname;
            scriptStart = scriptstart;
            scriptEnd = scriptend;
        }

        public string levelName { get; }
        public int scriptStart { get; }
        public int scriptEnd { get; }
    }

    struct LevelInfo
    {
        public LevelInfo(string name, string graphic, string audio, string completedtext, int cores)
        {
            _name = name;
            _graphic = graphic;
            _audio = audio;
            _completedtext = completedtext;
            _cores = cores;
        }

        public string _name;
        public string _graphic;
        public string _audio;
        public string _completedtext;
        public int _cores;
    }

    struct WeaponInfo
    {
        public WeaponInfo(string name, string graphic)
        {
            _name = name;
            _graphic = graphic;
        }

        public string _name;
        public string _graphic;
    }

    enum WeaponRandomization
    {
        FullyRandom,
        WeaponAbilitySplit
    }
}
