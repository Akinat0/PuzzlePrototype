using System;
using System.Reflection;
using UnityEngine;

/**
 * ScriptableObject с возможностью автосохранения
 * всех сериализованных полей в PlayerPrefs.
 *
 * Если поля публичные или с атрибутом [SerializedField] — поле будет сохранено.
 * Если у публичного поля аттрибут [NonSerialized] — поле не будет сохранено.
 *
 * Для работы нужно наслдедовать ScriptableObject от данного класса.
 * При вызове метода SaveSettings сохранятся все сериализованные поля (см. пример в SaveSettingsButton.cs).
 * При вызове метода LoadSettings загрузятся все сериализованные поля.
 *
 * Поддежирвается работа с int, float, string, bool, enum.
 */

    public class SaveableScriptableObject: ScriptableObject
    {
        /// <summary>
        /// Сохранить сериализованные поля в PlayerPrefs
        /// </summary>
        public virtual void SaveSettings()
        {
            var fields = GetType().GetFields(BindingFlags.NonPublic | 
                                             BindingFlags.Public | 
                                             BindingFlags.Instance);

            foreach (var field in fields)
            {
                object val = field.GetValue(this);
                
                // если у полей не указан аттрибут NonSerialized — сохраняем их в PlayerPrefs
                if ((field.Attributes & FieldAttributes.NotSerialized) == 0 )
                {
                    Type type = val.GetType();

                    if (type == typeof(int))
                    {
                        PlayerPrefs.SetInt(field.Name, (int)val);
                    }
                    else if (type == typeof(string))
                    {
                        PlayerPrefs.SetString(field.Name, (string)val);
                    }
                    else if (type == typeof(float))
                    {
                        PlayerPrefs.SetFloat(field.Name, (float)val);
                    }
                    else if (type == typeof(bool))
                    {
                        PlayerPrefs.SetInt(field.Name, (bool)val ? 1 : 0);
                    }
                    else if (type.IsEnum)
                    {
                        PlayerPrefs.SetInt(field.Name, (int)val);
                    }
                }
            }
            
            PlayerPrefs.Save();
        }

        
        /// <summary>
        /// Загрузить сериализованные поля из PlayerPrefs
        /// </summary>
        public virtual void LoadSettings()
        {
            bool someValuesNonExist = false;
            
            var fields = GetType().GetFields(BindingFlags.NonPublic | 
                                             BindingFlags.Public | 
                                             BindingFlags.Instance);

            foreach (var field in fields)
            {
                object val = field.GetValue(this);
                
                // если у полей не указан аттрибут NonSerialized — загружаем его из PlayerPrefs
                if ((field.Attributes & FieldAttributes.NotSerialized) == 0)
                {
                    Type type = val.GetType();

                    if (type == typeof(int))
                    {
                        if (PlayerPrefs.HasKey(field.Name))
                        {
                            int savedValue = PlayerPrefs.GetInt(field.Name);
                            field.SetValue(this, savedValue);
                        }
                        else
                        {
                            someValuesNonExist = true;
                            PlayerPrefs.SetInt(field.Name, (int)val);
                        }
                    }
                    else if (type == typeof(string))
                    {
                        if (PlayerPrefs.HasKey(field.Name))
                        {
                            string savedValue = PlayerPrefs.GetString(field.Name);
                            field.SetValue(this, savedValue);
                        }
                        else
                        {
                            someValuesNonExist = true;
                            PlayerPrefs.SetString(field.Name, (string)val);
                        }
                    }
                    else if (type == typeof(float))
                    {
                        if (PlayerPrefs.HasKey(field.Name))
                        {
                            float savedValue = PlayerPrefs.GetFloat(field.Name);
                            field.SetValue(this, savedValue);
                        }
                        else
                        {
                            someValuesNonExist = true;
                            PlayerPrefs.SetFloat(field.Name, (float)val);
                        }
                    }
                    else if (type == typeof(bool))
                    {
                        if (PlayerPrefs.HasKey(field.Name))
                        {
                            bool savedValue = PlayerPrefs.GetInt(field.Name) == 1;
                            field.SetValue(this, savedValue);
                        }
                        else
                        {
                            someValuesNonExist = true;
                            PlayerPrefs.SetInt(field.Name, (bool)val ? 1 : 0);
                        }
                    }
                    else if (type.IsEnum)
                    {
                        if (PlayerPrefs.HasKey(field.Name))
                        {
                            var loadedValue = PlayerPrefs.GetInt(field.Name);
                            field.SetValue(this, loadedValue);
                        }
                        else
                        {
                            someValuesNonExist = true;
                            PlayerPrefs.SetInt(field.Name, (int)val);
                        }
                    }
                }
            }

            if (someValuesNonExist)
            {
                PlayerPrefs.Save();
            }
        }
    }