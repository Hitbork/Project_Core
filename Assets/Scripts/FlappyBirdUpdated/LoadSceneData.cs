/* This namespace is used to access data, that might be needed in scene.
 * 
 * Planned in the future:
 * - this data would be saved and loaded locally;
 * - this data would be saved and loaded on the server; */
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LoadSceneData
{
    public abstract class Info
    {
        public static int amountOfLevels = 9;
    }

    public abstract class SavingClass
    {
        public string nameOfSavingFile = "SavingClass";

        public SavingData savingData = new SavingData();

        public abstract void ChangeNameOfSavingFile();

        public abstract void AddSavingInfo();

        public abstract void ReadSavingInfo();

        public virtual void Load()
        {
            ClearSavingData();
            if (!File.Exists(Application.persistentDataPath + $"/{nameOfSavingFile}.dat"))
                Save();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + $"/{nameOfSavingFile}.dat", FileMode.Open);
            this.savingData = (SavingData)bf.Deserialize(file);
            file.Close();
            Debug.Log($"File {nameOfSavingFile}.dat was loaded!");

            ReadSavingInfo();
        }

        public virtual void Save()
        {
            ClearSavingData();
            AddSavingInfo();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + $"/{nameOfSavingFile}.dat");
            bf.Serialize(file, this.savingData);
            file.Close();
            Debug.Log($"File {nameOfSavingFile}.dat was saved!");
        }

        public void ClearSavingData() => savingData = new SavingData();

        public SavingClass()
        {
            ChangeNameOfSavingFile();
        }

        [Serializable]
        public class SavingData
        {
            public List<bool> bools = new List<bool>();
            public List<int> ints = new List<int>();
            public List<string> strings = new List<string>();
            public List<double[]> doubleArrays = new List<double[]>();
        }
    }

    /* !! This namespace is not usable yet. !!
     * 
     * In this namespace is planned to be information
     * about player and player's progress,
     * such as levels opened to him. */
    namespace User
    {
        public class UserData : SavingClass
        {
            public string userName = "unknown",
                userPassword = "unknown";

            public bool isCheatSaving { get; private set; } = false; 

            public bool isNewRecordSetted { get; private set; } = false;

            public int indexOfLastUncoveredLevel { get; private set; } = 0;

            public double[] timeRecordsInLevels = new double[Info.amountOfLevels];

            public UserData() : base() { }

            public UserData(bool creatingCheatSaving) : this()
            {
                this.isCheatSaving = creatingCheatSaving;
                
                if (this.isCheatSaving)
                {
                    this.indexOfLastUncoveredLevel = Info.amountOfLevels;
                    this.userName = "cheat";
                    this.userPassword = "cheat";
                }
            }

            public UserData(string name, string password) : this()
            {
                this.userName = name;
                this.userPassword = password;
            }
            private void CheckForNewRecord(double currentTime, int indexOfLevel)
            {
                this.isNewRecordSetted = this.timeRecordsInLevels[indexOfLevel] > currentTime || this.timeRecordsInLevels[indexOfLevel] == 0;

                if (isNewRecordSetted) this.timeRecordsInLevels[indexOfLevel] = currentTime;
            }

            private void CheckForOpeningNewLevel(int indexOfLevel)
            {
                if (this.indexOfLastUncoveredLevel == indexOfLevel)
                    this.indexOfLastUncoveredLevel++;
            }


            public override void AddSavingInfo()
            {
                this.savingData.bools.Add(this.isCheatSaving);
                this.savingData.strings.Add(this.userName);
                this.savingData.strings.Add(this.userPassword);
                this.savingData.ints.Add(this.indexOfLastUncoveredLevel);
                this.savingData.doubleArrays.Add(this.timeRecordsInLevels);
            }

            public override void ChangeNameOfSavingFile()
            {
                this.nameOfSavingFile = "UserData";
            }

            public override void ReadSavingInfo()
            {
                this.isCheatSaving = this.savingData.bools[0];
                this.userName = this.savingData.strings[0];
                this.userPassword = this.savingData.strings[1];
                this.indexOfLastUncoveredLevel = this.savingData.ints[0];
                this.timeRecordsInLevels = this.savingData.doubleArrays[0];
            }

            public void LevelFinished(int finishedLevelIndex)
            {
                CheckForOpeningNewLevel(finishedLevelIndex);
                this.Save();
            }

            public void LevelFinished(int finishedLevelIndex, double currentTime)
            {
                LevelFinished(finishedLevelIndex);
                CheckForNewRecord(currentTime, finishedLevelIndex);
                this.Save();
            }
        }
    }

    namespace Level
    {
        // This class contains all needed data about level, while loading it. 
        public class LevelData : SavingClass
        {
            public LevelName levelName = new LevelName();

            public LevelData() : base() { }

            public LevelData(string levelNameValue) : this()
            {
                this.levelName = new LevelName(levelNameValue);
            }

            public override void AddSavingInfo()
            {
                this.savingData.strings.Add(this.levelName.Value);
            }
            
            public override void ChangeNameOfSavingFile()
            {
                this.nameOfSavingFile = "LevelData";
            }

            public override void ReadSavingInfo()
            {
                this.levelName.Value = this.savingData.strings[0];
            }

            public class LevelName
            {
                private static int minLength = 3, maxLength = 16;

                private static string defaultLevelName = "unknown";

                private string value;

                public bool isDefault { get; private set; } = false;

                public bool isEmpty { get; private set; } = false;

                public bool isIncorrect { get; private set; } = false;

                public bool isLengthIncorrect { get; private set; } = false;

                public string errorMessage { get; private set; } = string.Empty;

                public string Value
                {
                    get => this.value;
                    set
                    {
                        this.value = value;
                        CheckForCorrectness();

                        if (!isIncorrect)
                            ClearErrorMessage();
                    }
                }

                public LevelName() 
                {
                    SetDefault();
                }

                public LevelName(string strValue)
                {
                    this.Value = strValue;
                }

                private bool IsDefault()
                {
                    if (this.value == defaultLevelName)
                    {
                        isDefault = true;
                        this.errorMessage = $"Level name can't be {defaultLevelName}!";
                    }
                    else
                        isDefault = false;

                    return isDefault;
                }

                private bool IsEmpty()
                {
                    if (string.IsNullOrEmpty(this.value) ||
                        string.IsNullOrWhiteSpace(this.value))
                    {
                        isEmpty = true;
                        this.errorMessage = "Level name is empty!";
                    }
                    else
                        isEmpty = false;

                    return isEmpty;
                }

                private bool IsLengthIncorrect()
                {
                    if (this.value.Length < minLength || this.value.Length > maxLength)
                    {
                        isLengthIncorrect = true;

                        if (this.value.Length < minLength)
                            errorMessage = $"Level name must be at least {minLength} digits!";
                        else
                            errorMessage = $"Level name must be less or equals to {maxLength} digits!";
                    }
                    else
                        isLengthIncorrect = false;

                    return isLengthIncorrect;
                }

                private void CheckForCorrectness() => isIncorrect = IsEmpty() || this.IsDefault() || this.IsLengthIncorrect();

                private void ClearErrorMessage() => errorMessage = string.Empty;

                public string GetErrorMessage()
                {
                    return this.errorMessage;
                }

                public void SetDefault()
                {
                    isIncorrect = true;
                    isDefault = true;
                    this.errorMessage = $"Level name can't be {defaultLevelName}!";
                    this.value = defaultLevelName;
                }
            }
        }
    }
}
