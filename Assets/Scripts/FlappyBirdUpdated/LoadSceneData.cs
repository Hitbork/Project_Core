namespace LoadSceneData
{
    namespace User
    {
        public class UserData
        {
            public string userName = "unknown",
                userPassword = "unknown";

            public UserData() { }

            public UserData(string name, string password)
            {
                this.userName = name;
                this.userPassword = password;
            }
        }
    }

    namespace Level
    {
        public class LevelData
        {
            public LevelName levelName = new LevelName();

            public LevelData() { }

            public LevelData(string levelNameValue)
            {
                this.levelName = new LevelName(levelNameValue);
            }

            public class LevelName
            {
                public string Value
                {
                    get => this.value;
                    set
                    {
                        this.value = value;
                        IsIncorrect();
                    }
                }

                public bool isIncorrect { get; private set; } = false;

                public bool isDefault { get; private set; } = false;

                public bool isEmpty { get; private set; } = false;

                public bool isLengthIncorrect { get; private set; } = false;


                private string value, errorMessage = string.Empty;

                private static int minLength = 4, maxLength = 16;

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

                public void ClearErrorMessage() => errorMessage = string.Empty;

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
                            errorMessage = $"Level name must be at least {minLength-1} digits!";
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

                private bool IsIncorrect()
                {
                    isIncorrect = IsEmpty() || this.IsDefault() || this.IsLengthIncorrect();
                    return isIncorrect;
                }

                public string GetErrorMessage()
                {
                    string err = this.errorMessage;

                    if (!string.IsNullOrEmpty(err))
                    {
                        ClearErrorMessage();
                        return err;
                    }

                    return string.Empty;
                }
            }
        }
    }
}
