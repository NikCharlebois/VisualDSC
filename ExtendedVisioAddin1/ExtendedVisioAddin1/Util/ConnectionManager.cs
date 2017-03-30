using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visio = Microsoft.Office.Interop.Visio;
namespace VisualDSC.Util
{
    static class ConnectionManager
    {
        public static string GetDependsOn(Visio.Shape shape)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            var connects = shape.ConnectedShapes(Visio.VisConnectedShapesFlags.visConnectedShapesIncomingNodes, null);
            short customProps = (short)Visio.VisSectionIndices.visSectionProp;
            Visio.Shape connectedShape;
            if(connects.Length > 0)
            {
                sb.Append("            DependsOn = \"");
            }
            foreach (int shapeId in connects)
            {
                connectedShape = shape.ContainingPage.Shapes[shapeId];
                for (short i = 0; i < connectedShape.RowCount[customProps]; i++)
                {
                    if (connectedShape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsLabel].FormulaU.Replace("\"", "").ToLower() == "name")
                    {
                        sb.Append("[" + connectedShape.Master.Name + "]" + connectedShape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsValue].FormulaU.Replace("\"", "") + ";");
                    }
                }
            }
            return sb.ToString() + "\"";
        }
    }
}
