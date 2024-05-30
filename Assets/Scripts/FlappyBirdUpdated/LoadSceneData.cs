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

            public class LevelName
            {
                //public string Value
                //{
                //    get => this.value;
                //    set
                //    {
                //        this.value = value;
                //        IsIncorrect();
                //    }
                //}

                //public readonly bool isIncorrect = true, isDefault;

                private string value, errorMessage = string.Empty;

                private static int minLength = 3, maxLength = 16;

                private static string defaultLevelName = "unknown";

                public LevelName() 
                {
                    SetDefault();
                }

                public LevelName(string value)
                {
                    this.value = value;
                    IsIncorrect();
                }

                public void SetDefault()
                {
                    this.errorMessage = $"Level name can't be {defaultLevelName}!";
                    this.value = defaultLevelName;
                }

                public void ClearErrorMessage() => errorMessage = string.Empty;

                public bool IsDefault()
                {
                    this.errorMessage = $"Level name can't be {defaultLevelName}!";
                    return this.value == defaultLevelName;
                }

                public bool IsLengthIncorrect()
                {
                    if (this.value.Length < minLength)
                    {
                        errorMessage = $"Level name must be at least {minLength} digits!";
                        return true;
                    }

                    if (this.value.Length > maxLength)
                    {
                        errorMessage = $"Level name must be less or equals to {maxLength} digits!";
                        return true;
                    }

                    return false;
                }

                public bool IsEmpty()
                {
                    if (string.IsNullOrEmpty(this.value) ||
                        string.IsNullOrWhiteSpace(this.value))
                    {
                        this.errorMessage = "Level name is empty!";
                        return true;
                    }

                    return false;
                }

                public bool IsIncorrect()
                {
                    return IsEmpty()
                        || this.IsDefault()
                        || this.IsLengthIncorrect();
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

                public string Get()
                {
                    return value;
                }

                public void Set(string value)
                {
                    this.value = value;
                    IsIncorrect();
                }
            }
        }
    }
}
