using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace Mango.Framework.Core
{
    public static class MethodUtils
    {
        public static void InitMethods(this Dictionary<string, List<MethodInfo>> methodInfoMap,Type type)
        {
            MethodInfo[] _methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (int i = 0; i < _methodInfos.Length; i++)
            {
                string methodName = _methodInfos[i].Name;
                List<MethodInfo> list = null;
                methodInfoMap.TryGetValue(methodName, out list);
                if (list == null)
                {
                    list = new List<MethodInfo>();
                    methodInfoMap.Add(methodName, list);
                }
                list.Add(_methodInfos[i]);
            }
        }

        public static void InvokeMethod(this Dictionary<string, List<MethodInfo>> methodInfoMap,object obj, string command, params object[] paramObject)
        {
            System.Type[] paramTypes = new System.Type[paramObject.Length];
            for(int i = 0; i < paramObject.Length; i++)
            {
                if(paramObject[i] == null)
                {
                    paramTypes[i] = null;
                }
                else
                {
                    paramTypes[i] = paramObject[i].GetType();
                }
            }
            List<MethodInfo> list = null;
            methodInfoMap.TryGetValue(command, out list);
            if (list != null)
            {
                for (int index = 0; index < list.Count; index++)
                {
                    var methodInfo = list[index];
                    // 依次检查返回类型，函数名，参数
                    if (MethodUtils.CheckMethodFullMatch(methodInfo, typeof(void), paramTypes))
                    {
                        methodInfo.Invoke(obj, paramObject);
                        break;
                    }
                }
            }
        }

        public static bool CheckMethodFullMatch(MethodInfo methodInfo, Type returnType, params Type[] matchParamTypes)
        {
            if (methodInfo.ReturnType != returnType) return false;

            var paramInfo = methodInfo.GetParameters();

            if (!IsParamTypeMatch(paramInfo, matchParamTypes)) return false;
            return true;
        }

        private static bool IsParamTypeMatch(ParameterInfo[] paramInfo, Type[] matchParamTypes)
        {
            // 如果传递了{null}这样的匹配类型数组，直接返回空
            if (paramInfo.Length == 0 && matchParamTypes.Length == 0)
            {
                return true;
            }

            if (paramInfo.Length != matchParamTypes.Length)
            {
                return false;
            }

            for (int pIndex = 0; pIndex < paramInfo.Length; pIndex++)
            {
                //如果指定参数类型为null，表示这个参数类型不做检查
                System.Type matchParamType = matchParamTypes[pIndex];
                if (matchParamType == null)
                {
                    continue;
                }
                if ((paramInfo[pIndex].ParameterType != matchParamTypes[pIndex]) &&
                    !(paramInfo[pIndex].ParameterType == typeof(System.Single) && matchParamTypes[pIndex] == typeof(System.Int32)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
