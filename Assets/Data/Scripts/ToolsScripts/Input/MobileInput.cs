using System;

namespace Data.Scripts.Tools.Input
{
    public static class MobileInput
    {
        public static Action<bool> OnInputConditionChanged;
        
        public static bool Condition
        {
            get => condition;
            
            set
            {
                if (value == condition)
                    return;
                
                condition = value;
                OnInputConditionChanged?.Invoke(condition);
            }
        }

        static bool condition = true;
    }
}