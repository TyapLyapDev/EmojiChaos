using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class BaseInitializingBehaviour : MonoBehaviour
{
    private bool _isInitialized;

    protected void BaseInitialize()
    {
        if (_isInitialized)
            throw new InvalidOperationException("Попытка повторной инициализации");

#if UNITY_EDITOR
        ValidateFieldsInInspector();
#endif

        _isInitialized = true;
    }

    protected void ValidateInit([CallerMemberName] string methodName = "")
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Метод {methodName} был вызван до инициализации");
    }

    protected T GetSafeReference<T>(T field, [CallerMemberName] string fieldName = "") where T : class
    {
        ValidateInitForField(fieldName);

        return field ?? throw new InvalidOperationException($"Поле {fieldName} не назначено в инспекторе");
    }

    protected T GetSafeValue<T>(T field, [CallerMemberName] string fieldName = "") where T : struct
    {
        ValidateInitForField(fieldName);

        return field;
    }

    private void ValidateInitForField(string fieldName)
    {
        if (_isInitialized == false)
            throw new InvalidOperationException($"Обращение к полю {fieldName} до инициализации");
    }

#if UNITY_EDITOR
    private void ValidateFieldsInInspector()
    {
        IEnumerable<FieldInfo> fields = GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(f => f.IsDefined(typeof(SerializeField), false));

        foreach (FieldInfo field in fields)
        {
            object value = field.GetValue(this);
            string fieldName = field.Name;

            if (field.FieldType.IsValueType)
                continue;

            if (value == null)
                throw new NullReferenceException($"SerializeField поле {fieldName} не назначено в инспекторе");

            ValidateCollectionElements(value, fieldName, field.FieldType);
        }
    }

    private void ValidateCollectionElements(object collection, string fieldName, Type fieldType)
    {
        if (collection is IEnumerable enumerable && (collection is string) == false)
        {
            int index = 0;

            foreach (object element in enumerable)
            {
                if (element == null)
                {
                    throw new NullReferenceException(
                        $"Элемент с индексом {index} в коллекции {fieldName} ({fieldType.Name}) является null. " +
                        "Все элементы коллекции должны быть назначены в инспекторе.");
                }

                index++;
            }

            switch (collection)
            {
                case IList list when list.Count == 0:
                    Debug.LogWarning($"Коллекция {fieldName} пустая");
                    break;

                case Array array when array.Length == 0:
                    Debug.LogWarning($"Массив {fieldName} пустой");
                    break;

                case ICollection genericCollection when genericCollection.Count == 0:
                    Debug.LogWarning($"Коллекция {fieldName} пустая");
                    break;
            }
        }
    }
#endif
}