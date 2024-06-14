/* This namespace is used to access data, that might be needed in scene.
 * 
 * Planned in the future:
 * - this data would be saved and loaded locally;
 * - this data would be saved and loaded on the server; */
namespace LoadSceneData
{
    /* !! This namespace is not usable yet. !!
     * 
     * In this namespace is planned to be information
     * about player and player's progress,
     * such as levels opened to him. */
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
        // This class contains all needed data about level, while loading it. 
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
