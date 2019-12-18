using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Penguin
{
    public static class TypeCollector
    {
        private static List<Type> _allTypes;
        
        public static List<Type> AllTypes
        {
            get
            {
                if (_allTypes == null)
                {
                    _allTypes = new List<Type>();
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                    foreach (var assembly in assemblies)
                    {
                        try
                        {
                            _allTypes.AddRange(assembly.GetTypes());
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.Message);
                        }
                    }
                }


                return _allTypes;
            }
        }

        public static Type[] GetClassesOfInterface(Type type)
        {
            return AllTypes.Where(p => p.GetInterfaces().Any(i => i == type) && !p.IsInterface)
                        .ToArray();
        }

        public static Type[] GetStructsOfInterface(Type type)
        {
            return AllTypes.Where(p => p.GetInterfaces().Any(i => i == type) && p.IsValueType && !p.IsEnum)
                        .ToArray();
        }

        public static Type[] GetClassesOfClass(Type type)
        {
            return AllTypes.Where(p => p.BaseType == type)
                        .ToArray();
        }

        public static Type[] GetClassesOfBase(Type type)
        {
            return AllTypes.Where(p => type.IsAssignableFrom(p))
                        .ToArray();
        }

        public static Type[] GetInterfacesOfInterface(Type type)
        {
            return AllTypes.Where(p => type.IsAssignableFrom(p) && p.IsInterface && p != type)
                        .ToArray();
        }
    }
}