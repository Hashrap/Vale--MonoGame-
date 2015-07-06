using System;
using System.Collections.Generic;

namespace Vale.Parsing
{
    internal class Resource
    {
        private static readonly Resource Singleton = new Resource();

        public static Resource Instance { get { return Singleton; } }

        private readonly Dictionary<string, UnitInfo> unitDictionary;
        private readonly Dictionary<string, AbilityInfo> abilityDictionary;

        public Resource()
        {
            unitDictionary = new Dictionary<string, UnitInfo>();
            abilityDictionary = new Dictionary<string, AbilityInfo>();
        }

        /// <summary>
        /// Add the given info to the resource dictionary.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="info">The info</param>
        public void AddInfo(string key, object info)
        {
            if (info is UnitInfo)
            {
                if (unitDictionary.ContainsKey(key))
                    throw new Exception("Duplicate unit info keys.");

                unitDictionary.Add(key, (UnitInfo)info);
            }
            else if (info is AbilityInfo)
            {
                if (abilityDictionary.ContainsKey(key))
                    throw new Exception("Duplicate ability info keys.");

                abilityDictionary.Add(key, (AbilityInfo)info);
            }
        }

        /// <summary>
        ///     Returns the unit information for the specified key.
        /// </summary>
        /// <param name="key">The name of the unit to use as a key.</param>
        /// <returns>Returns the UnitInfo for the specified unit.</returns>
        public UnitInfo GetUnitInfo(string key)
        {
            if (unitDictionary == null || unitDictionary.Count == 0)
                throw new Exception("Unit info has not been parsed.");

            UnitInfo info;

            if (!unitDictionary.TryGetValue(key, out info))
                throw new Exception("Unit info could not be found for key \'" + key + "\'");

            return info;
        }

        /// <summary>
        ///     Returns the ability information for the specified key.
        /// </summary>
        /// <param name="key">The name of the ability to use as a key.</param>
        /// <returns>Returns the AbilityInfo for the specified unit.</returns>
        public AbilityInfo GetAbilityInfo(string key)
        {
            if (abilityDictionary == null || abilityDictionary.Count == 0)
                throw new Exception("Ability info has not been parsed.");

            AbilityInfo info;

            if (!abilityDictionary.TryGetValue(key, out info))
                throw new Exception("Ability info could not be found for key \'" + key + "\'");

            return info;
        }
    }
}