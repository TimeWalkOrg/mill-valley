using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HeurekaGames.AssetHunter
{
    [System.Serializable]
    public class AssetHunterSettings : ScriptableObject
    {
        [SerializeField]
        public List<UnityEngine.Object> m_DirectoryExcludes = new List<UnityEngine.Object>();

        [SerializeField]
        public List<AssetHunterSerializableSystemType> m_AssetTypeExcludes = new List<AssetHunterSerializableSystemType>();

        [SerializeField]
        public List<string> m_AssetSubstringExcludes = new List<string>();

        [SerializeField]
        public bool m_MemoryCleanupActive = false;

        internal static string GetAssetPath()
        {
            return AssetHunterSettingsCreator.GetAssetPath();
        }

        internal bool ValidateDirectory(UnityEngine.Object newDir)
        {
            return !m_DirectoryExcludes.Contains(newDir);
        }

        internal void ExcludeDirectory(UnityEngine.Object newDir)
        {
            m_DirectoryExcludes.Add(newDir);
        }

        internal bool ValidateType(AssetHunterSerializableSystemType newtype)
        {
            return !m_AssetTypeExcludes.Contains(newtype);
        }

        internal void ExcludeType(AssetHunterSerializableSystemType newtype)
        {
            m_AssetTypeExcludes.Add(newtype);
        }

        internal bool ValidateSubstring(string newSubstring)
        {
            return !m_AssetSubstringExcludes.Contains(newSubstring);
        }

        internal void ExcludeSubstring(string newSubstring)
        {
            m_AssetSubstringExcludes.Add(newSubstring);
        }

        internal void RemoveDirectoryAtIndex(int indexer)
        {
            m_DirectoryExcludes.RemoveAt(indexer);
            EditorUtility.SetDirty(this);
        }

        internal void RemoveTypeAtIndex(int indexer)
        {
            m_AssetTypeExcludes.RemoveAt(indexer);
            EditorUtility.SetDirty(this);
        }

        internal void RemoveSubstringAtIndex(int indexer)
        {
            m_AssetSubstringExcludes.RemoveAt(indexer);
            EditorUtility.SetDirty(this);
        }

        internal bool HasExcludes()
        {
            return m_DirectoryExcludes.Count >= 1 || m_AssetTypeExcludes.Count >= 1;
        }

        internal bool HasDirectoryExcludes()
        {
            return m_DirectoryExcludes.Count >= 1;
        }

        internal bool HasTypeExcludes()
        {
            return m_AssetTypeExcludes.Count >= 1;
        }

        internal bool HasSubStringExcludes()
        {
            return m_AssetSubstringExcludes.Count >= 1;
        }
    }
}
