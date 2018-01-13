using System.IO;
using UnityEngine;

namespace Entitas {

    public static class EntitasResources {

        public static string GetVersion()
        {
            var projectPath = Application.dataPath;
            var versionPath = Path.Combine(projectPath, "Libs/Entitas/Entitas/version");
            var version = "";
            using(var reader = new StreamReader(versionPath)) {
                version = reader.ReadToEnd();
            }
            return version;
        }
    }
}
