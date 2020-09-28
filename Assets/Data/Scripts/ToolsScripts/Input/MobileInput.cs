using System;

namespace Data.Scripts.Tools.Input
{
    public class MobileInput
    {
        public static Action<bool> OnInputConditionChanged;
        
        public static bool Condition
        {
            get => condition;
            
            set
            {
                if (value != condition)
                {
                    condition = value;
                    OnInputConditionChanged?.Invoke(condition);
                }
            }
        }

        static bool condition = true;
    }
}