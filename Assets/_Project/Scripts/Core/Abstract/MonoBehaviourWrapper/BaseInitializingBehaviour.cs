using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EmojiChaos.Core.Abstract.MonoBehaviourWrapper
{
    public abstract class BaseInitializingBehaviour : MonoBehaviour
    {
        private bool _isInitialized;

        protected bool IsInitialized => _isInitialized;

        protected void BaseInitialize()
        {
            if (_isInitialized)
                throw new InvalidOperationException($"{gameObject.name}: ������� ��������� �������������");

#if UNITY_EDITOR
            ValidateFieldsInInspector();
#endif

            _isInitialized = true;
        }

        protected void ValidateInit([CallerMemberName] string methodName = "")
        {
            if (_isInitialized == false)
                throw new InvalidOperationException($"{gameObject.name}: ����� {methodName} ��� ������ �� �������������");
        }

        protected T GetSafeReference<T>(T field, [CallerMemberName] string fieldName = "")
            where T : class
        {
            ValidateInitForField(fieldName);

            return field ?? throw new InvalidOperationException($"{gameObject.name}: ���� {fieldName} �� ��������� � ����������");
        }

        protected T GetSafeValue<T>(T field, [CallerMemberName] string fieldName = "")
            where T : struct
        {
            ValidateInitForField(fieldName);

            return field;
        }

        private void ValidateInitForField(string fieldName)
        {
            if (_isInitialized == false)
                throw new InvalidOperationException($"{gameObject.name}: ��������� � ���� {fieldName} �� �������������");
        }

#if UNITY_EDITOR
        private void ValidateFieldsInInspector()
        {
            IEnumerable<FieldInfo> fields = GetAllFieldsInHierarchy();

            foreach (FieldInfo field in fields)
            {
                if (!field.IsDefined(typeof(SerializeField), false))
                    continue;

                object value = field.GetValue(this);
                string fieldName = field.Name;

                if (field.FieldType.IsValueType && IsUnityStruct(field.FieldType) == false)
                    continue;

                if (IsUnityObjectNull(value))
                    throw new NullReferenceException($"{gameObject.name}: SerializeField ���� {fieldName} �� ��������� � ����������");

                ValidateCollectionElements(value, fieldName, field.FieldType);
            }
        }

        private IEnumerable<FieldInfo> GetAllFieldsInHierarchy()
        {
            List<FieldInfo> allFields = new();
            Type currentType = GetType();

            while (currentType != null && currentType != typeof(BaseInitializingBehaviour) && currentType != typeof(object))
            {
                FieldInfo[] typeFields = currentType.GetFields(
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);

                allFields.AddRange(typeFields);
                currentType = currentType.BaseType;
            }

            return allFields;
        }

        private bool IsUnityStruct(Type type)
        {
            if (type == null) return false;

            string namespaceName = type.Namespace;
            return namespaceName == "UnityEngine" ||
                   namespaceName == "UnityEngine.AI" ||
                   namespaceName == "UnityEngine.Rendering" ||
                   type == typeof(Vector3) || type == typeof(Vector2) ||
                   type == typeof(Color) || type == typeof(Quaternion);
        }

        private bool IsUnityObjectNull(object obj)
        {
            if (obj == null)
                return true;

            if (obj is UnityEngine.Object unityObj)
                return unityObj == null;

            return false;
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
                            $"{gameObject.name}: ������� � �������� {index} � ��������� {fieldName} ({fieldType.Name}) �������� null. " +
                            "��� �������� ��������� ������ ���� ��������� � ����������.");
                    }

                    index++;
                }

                switch (collection)
                {
                    case IList list when list.Count == 0:
                        Debug.LogWarning($"{gameObject.name}: ��������� {fieldName} ������");
                        break;

                    case Array array when array.Length == 0:
                        Debug.LogWarning($"{gameObject.name}: ������ {fieldName} ������");
                        break;

                    case ICollection genericCollection when genericCollection.Count == 0:
                        Debug.LogWarning($"{gameObject.name}: ��������� {fieldName} ������");
                        break;
                }
            }
        }
#endif
    }
}