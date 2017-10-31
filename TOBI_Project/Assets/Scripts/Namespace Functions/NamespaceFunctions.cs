using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NamespaceFunctions {

    /// <summary>
    /// Checks if the namespace of the given class path exists.
    /// <para>Example: Uses "UnityEngine.UI.Button" to check for the namespace "UnityEngine.UI".</para>
    /// </summary>
    /// <param name="classFromNamespaceToFind"></param>
    /// <returns></returns>
    public static bool NamespaceFound(string classFromNamespaceToFind) {
        string[] s = classFromNamespaceToFind.Split('.');
        if (s.Length > 1)
        {
            string nameSpacePath = "";
            for (int i = 0; i < s.Length - 1; i++)
            {
                nameSpacePath += s[i];
                nameSpacePath += ".";
            }
            if (nameSpacePath.EndsWith("."))
            {
                nameSpacePath = nameSpacePath.Substring(0, nameSpacePath.Length - 1);
            }
            return (System.Type.GetType(classFromNamespaceToFind).Namespace == nameSpacePath);
        }
        else
        {
            return (System.Type.GetType(classFromNamespaceToFind) != null);
        }
    }	

}
