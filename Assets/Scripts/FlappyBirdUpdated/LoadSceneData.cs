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
    public abstract class SavingClass
    {
        public string nameOfSavingFile = "SavingClass";

        public SavingData savingData = new SavingData();

        public abstract void ChangeNameOfSavingFile();

        public abstract void AddSavingInfo();

        public abstract void ReadSavingInfo();

        public void ClearSavingData() => savingData = new SavingData();

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
        
        public SavingClass()
        {
            ChangeNameOfSavingFile();
        }

        [Serializable]
        public class SavingData
        {
            public List<string> strings = new List<string>();
            public List<int> ints = new List<int>();
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

            public int indexOfLastUncoveredLevel = 0;

            public UserData() : base() { }

            public UserData(string name, string password) : this()
            {
                this.userName = name;
                this.userPassword = password;
                ClearSavingData();
            }

            public override void AddSavingInfo()
            {
                this.savingData.strings.Add(this.userName);
                this.savingData.strings.Add(this.userPassword);
                this.savingData.ints.Add(this.indexOfLastUncoveredLevel);
            }

            public override void ChangeNameOfSavingFile()
            {
                nameOfSavingFile = "UserData";
            }

            public override void ReadSavingInfo()
            {
                this.userName = this.savingData.strings[0];
                this.userPassword = this.savingData.strings[1];
                this.indexOfLastUncoveredLevel = this.savingData.ints[0];
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
                nameOfSavingFile = "LevelData";
            }

            public override void ReadSavingInfo()
            {
                this.levelName.Value = this.savingData.strings[0];
            }

            public class LevelName
            {
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

                public bool isIncorrect { get; private set; } = false;

                public bool isDefault { get; private set; } = false;

                public bool isEmpty { get; private set; } = false;

                public bool isLengthIncorrect { get; private set; } = false;

                private string value;
                public string errorMessage { get; private set; } = string.Empty;

                private static int minLength = 3, maxLength = 16;

                private static string defaultLevelName = "unknown";

                public LevelName() 
                {
                    SetDefault();
                }

                public LevelName(string strValue)
                {
                    this.Value = strValue;
                }

                public void SetDefault()
                {
                    isIncorrect = true;
                    isDefault = true;
                    this.errorMessage = $"Level name can't be {defaultLevelName}!";
                    this.value = defaultLevelName;
                }

                private void ClearErrorMessage() => errorMessage = string.Empty;

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

                private void CheckForCorrectness()
                {
                    isIncorrect = IsEmpty() || this.IsDefault() || this.IsLengthIncorrect();
                }

                public string GetErrorMessage()
                {
                    return this.errorMessage;
                }
            }
        }
    }
}
