using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeTitleDrawer
    {
        private GUIContent _nodeNameContent;
        private GUIContent _nodeSubNameContent;

        public void OnGUI(EditorWindow appView, Rect titleRect, Node node)
        {
            if (_nodeNameContent == null)
            {
                _nodeNameContent = new GUIContent(node.Name);
            }
            if (_nodeSubNameContent == null)
            {
                _nodeSubNameContent = new GUIContent("COMPONENT");
            }

            GUIHelper.DrawQuad(titleRect, StyleProxy.NodeTitleBackdropColor);

            var titleBlockSize = StyleProxy.NodeTitleTextStyle.CalcSize(_nodeNameContent);
            var titlePosition = new Rect(
                titleRect.x + titleRect.width * 0.5f - titleBlockSize.x * 0.5f,
                titleRect.y + 6f,
                titleBlockSize.x,
                16f);

            GUI.Box(titlePosition, node.Name, StyleProxy.NodeTitleTextStyle);

            var subtitleBlockSize = StyleProxy.NodeSubtitleTextStyle.CalcSize(_nodeSubNameContent);
            var subtitlePosition = new Rect(
                titleRect.x + titleRect.width * 0.5f - subtitleBlockSize.x * 0.5f,
                titleRect.y + titleRect.height - subtitleBlockSize.y - 6f,
                subtitleBlockSize.x,
                16f);

            GUI.Box(subtitlePosition, "COMPONENT", StyleProxy.NodeSubtitleTextStyle);
        }
    }
}